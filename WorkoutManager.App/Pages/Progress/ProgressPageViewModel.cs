using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Progress;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.Progress
{
    internal delegate bool FilteringPredicate(SessionExercise sessionExercise);
    
    internal enum FilterBy
    {
        [Description("Primary muscle group")]
        PrimaryMuscleGroup,
            
        [Description("Secondary muscle group")]
        SecondaryMuscleGroup,
        
        [Description("Motion")]
        Motion,
        
        [Description("Exercise")]
        Exercise,
    }

    internal enum FilterMetric
    {
        Volume,
        Sets,
    }
    
    internal enum GroupBy
    {
        [Description("Day")]
        Day,
            
        [Description("Week")]
        Week,
            
        [Description("1 Month")]
        Month1,
            
        [Description("3 Months")]
        Month3,
            
        [Description("6 Months")]
        Month6,
            
        [Description("Year")]
        Year,
    }
    
    internal class ProgressPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<TrainingSession> _trainingSessionRepository;
        private readonly Repository<MuscleGroup> _muscleGroupRepository;
        private readonly Repository<JointMotion> _motionsRepository;
        private readonly UserPreferencesService _userPreferencesService;
        
        private object _selectedFilteringValue;
        private FilterBy _filterBy = FilterBy.Exercise;
        private IEnumerable<ProgressResult> _results;
        private GroupBy _groupBy = GroupBy.Day ;
        private FilterMetric _metric = FilterMetric.Volume;

        private static readonly IEnumerable<string> FilterProperties = new[]
            { nameof(SelectedFilteringValue), nameof(GroupBy), nameof(Metric) };

        private IEnumerable<TrainingSession> _trainingSessions = new List<TrainingSession>();
        private IEnumerable<Exercise> _exercises = new List<Exercise>();
        private IEnumerable<MuscleGroup> _muscleGroups = new List<MuscleGroup>(); 
        private IEnumerable<JointMotion> _motions = new List<JointMotion>();
        private IEnumerable<object> _filteringValueOptions;

        public IEnumerable<FilterBy> FilterByOptions { get; } = Enum.GetValues(typeof(FilterBy)).Cast<FilterBy>();
        public IEnumerable<GroupBy> GroupByOptions { get; } = Enum.GetValues(typeof(GroupBy)).Cast<GroupBy>();
        public IEnumerable<FilterMetric> MetricOptions { get; } = Enum.GetValues(typeof(FilterMetric)).Cast<FilterMetric>();
        
        public IEnumerable<ProgressResult> Results
        {
            get => _results;
            set => SetField(ref _results, value);
        }

        public object SelectedFilteringValue
        {
            get => _selectedFilteringValue;
            set => SetField(ref _selectedFilteringValue, value);
        }

        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateTo { get; set; }

        public FilterBy FilterBy
        {
            get => _filterBy;
            set => SetField(ref _filterBy, value);
        }

        public GroupBy GroupBy
        {
            get => _groupBy;
            set => SetField(ref _groupBy, value);
        }

        public FilterMetric Metric
        {
            get => _metric;
            set => SetField(ref _metric, value);
        }

        public IEnumerable<object> FilteringValueOptions
        {
            get => _filteringValueOptions;
            set => SetField(ref _filteringValueOptions, value);
        }

        public ICommand Refresh { get; }

        private void LoadData()
        {
            _exercises = _exerciseRepository.GetAll();
            _muscleGroups = _muscleGroupRepository.GetAll();
            _motions = _motionsRepository.GetAll();
            _trainingSessions = _trainingSessionRepository.GetAll().OrderByDescending(session => session.Date);
        }

        private FilteringPredicate GetFilteringPredicate()
        {
            switch (FilterBy)
            {
                case FilterBy.Exercise:

                    return exercise => exercise.Exercise.Equals(SelectedFilteringValue);
                case FilterBy.Motion:

                    return exercise => exercise.Exercise.Motions.Contains(SelectedFilteringValue);
                case FilterBy.PrimaryMuscleGroup:

                    return exercise => exercise.Exercise.PrimaryMuscles.Any(
                        muscle => muscle.MuscleGroup.Equals(SelectedFilteringValue)
                    );
                case FilterBy.SecondaryMuscleGroup:

                    return exercise => exercise.Exercise.SecondaryMuscles.Any(
                        muscle => muscle.MuscleGroup.Equals(SelectedFilteringValue)
                    );
                default:

                    throw new ArgumentException("Invalid filter by");
            }
        }
        
        private int GetSetCount(TrainingSession session, FilteringPredicate predicate)
        {
            return session.Exercises.Aggregate(
                0,
                (totalSets, exercise) =>
                    !predicate(exercise)
                        ? totalSets
                        : totalSets + exercise.Sets.Length
            );
        }

        private double GetVolume(TrainingSession session, FilteringPredicate predicate)
        {
            double GetExerciseVolume(SessionExercise exercise, double bodyweightVolume) => exercise.Sets
                .OfType<DynamicExerciseSet>()
                .Aggregate(
                    0d,
                    (exerciseVolume, set) => exerciseVolume + set.Reps * (set.Weight + bodyweightVolume)
                );
            
            var bodyweight = session.Bodyweight;

            return session.Exercises.Aggregate(
                0d,
                (sessionVolume, exercise) =>
                {
                    if (!predicate(exercise))
                    {
                        return sessionVolume;
                    }
                    
                    var bodyweightVolume = exercise.Exercise.RelativeBodyweight / 100 * bodyweight;
                    var exerciseVolume = GetExerciseVolume(exercise, bodyweightVolume);

                    return sessionVolume + exerciseVolume;
                }
            );
        }

        private string CalculateProgressValue(IEnumerable<TrainingSession> sessions, FilteringPredicate predicate)
        {       
            
            switch (Metric)
            {
                case FilterMetric.Sets:

                    var sets = sessions.Aggregate(
                        0,
                        (totalSets, session) => totalSets + GetSetCount(session, predicate)
                    );

                    return $"{sets}";
                case FilterMetric.Volume:
                    var weightUnits = _userPreferencesService.Load().WeightUnits.GetDescription();

                    var volume = sessions.Aggregate(
                        0d,
                        (totalVolume, session) => totalVolume + GetVolume(session, predicate)
                    );

                    return $"{volume} {weightUnits}";
                default:

                    throw new ArgumentException("Invalid metric");
            }
        }

        private IEnumerable<object> GetFilteringValueOptions()
        {
            switch (FilterBy)
            {
                case FilterBy.Exercise:

                    return _exercises;
                
                case FilterBy.Motion:

                    return _motions;
                case FilterBy.PrimaryMuscleGroup:
                case FilterBy.SecondaryMuscleGroup:

                    return _muscleGroups;
                default:
                    throw new ArgumentException("Invalid filter by option");
            }
        }

        private static int GetWeek(DateTime date) => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
            date,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday
        );

        private string GetDateUnitFromDate(DateTime date)
        {
            switch (GroupBy) {
                case GroupBy.Day:

                    return date.ToShortDateString();
                case GroupBy.Week:

                    return $"{GetWeek(date)}/{date.Year}";
                case GroupBy.Month1:

                    return $"{date.Month}/{date.Year}";
                case GroupBy.Month3:

                    return $"{Math.Floor(date.Month / 3d) + 1}/{date.Year}";
                case GroupBy.Month6: 
                    
                    return $"{Math.Floor(date.Month / 6d) + 1}/{date.Year}";
                case GroupBy.Year:

                    return date.Year.ToString();
                default:

                    throw new ArgumentException("Invalid group by.");
            }
        }
        
        private IEnumerable<TrainingSession> GetFilteredSessions()
        {
            var filteredSessions = _trainingSessions.AsEnumerable();
            
            if (DateFrom != null)
            {
                filteredSessions = filteredSessions.Where(session => session.Date >= DateFrom);
            }
            
            if (DateTo != null)
            {
                filteredSessions = filteredSessions.Where(session => session.Date <= DateTo);
            }

            return filteredSessions;
        }
        
        public ProgressPageViewModel(Repository<Exercise> exerciseRepository, Repository<TrainingSession> trainingSessionRepository, Repository<MuscleGroup> muscleGroupRepository, Repository<JointMotion> motionsRepository, UserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
            _exerciseRepository = exerciseRepository;
            _trainingSessionRepository = trainingSessionRepository;
            _muscleGroupRepository = muscleGroupRepository;
            _motionsRepository = motionsRepository;

            Task.Run(() =>
                {
                    LoadData();
                    OnPropertyChanged(nameof(FilterBy));
                }
            );
            
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(FilterBy))
                {
                    _filteringValueOptions = GetFilteringValueOptions();
                    OnPropertyChanged(nameof(FilteringValueOptions));
                    SelectedFilteringValue = _filteringValueOptions.FirstOrDefault();
                    
                    return;
                }
                
                if (!FilterProperties.Contains(args.PropertyName))
                {
                    return;
                }

                var filteringPredicate = GetFilteringPredicate();

                var results = GetFilteredSessions()
                    .GroupBy(session => GetDateUnitFromDate(session.Date))
                    .Select(
                        sessions => new ProgressResult
                        {
                            DateUnit = sessions.Key,
                            Value = CalculateProgressValue(sessions, filteringPredicate)
                        }
                    );
                
                Results = results;
            };
            
            Refresh = new Command(
                () =>
                {
                    Task.Run(
                        () =>
                        {
                            var previousSelection = SelectedFilteringValue;
                            LoadData();

                            if (!_filteringValueOptions.Contains(previousSelection))
                            {
                                return;
                            }

                            IEnumerable options;

                            switch (FilterBy)
                            {
                                case FilterBy.Exercise:
                                    options = _exercises;

                                    break;
                                case FilterBy.Motion:
                                    options = _motions;

                                    break;
                                case FilterBy.PrimaryMuscleGroup:
                                    options = _muscleGroups;

                                    break;
                                case FilterBy.SecondaryMuscleGroup:
                                    options = _muscleGroups;

                                    break;
                                default:

                                    throw new ArgumentException("Invalid filter by");
                            }

                            SelectedFilteringValue =
                                options.Cast<object>().First(motion => motion.Equals(previousSelection));

                            OnPropertyChanged(string.Empty);
                        }
                    );
                }
            );
        }
    }
}