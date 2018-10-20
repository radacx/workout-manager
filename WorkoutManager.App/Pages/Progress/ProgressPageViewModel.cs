using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.Progress.Structures;
using WorkoutManager.App.Pages.Progress.Structures.Calculators;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.Progress
{
    internal class ProgressPageViewModel : ViewModelBase
    {
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<TrainingSession> _trainingSessionRepository;
        private readonly Repository<Muscle> _muscleRepository;
        private readonly Repository<Category> _categoryRepository;
        
        private readonly UserPreferencesService _userPreferencesService;
        
        private object _filteringValue;
        
        private FilterBy _filterBy = FilterBy.Exercise;
        private GroupBy _groupBy = GroupBy.Day ;
        private FilterMetric _metric = FilterMetric.Volume;

        private IEnumerable<ProgressResult> _results;

        private static readonly IEnumerable<string> FilterProperties = new[]
        {
            nameof(SelectedFilteringValue), nameof(GroupBy), nameof(Metric), nameof(ShouldFilterFrom),
            nameof(ShouldFilterTill), nameof(DateFrom), nameof(DateTo), string.Empty
        };

        private IEnumerable<TrainingSession> _trainingSessions = new List<TrainingSession>();
        private IEnumerable<Exercise> _exercises = new List<Exercise>();
        private IEnumerable<Muscle> _muscles = new List<Muscle>(); 
        private IEnumerable<Category> _categories = new List<Category>();
        
        private IEnumerable<object> _filteringValueOptions;
        private bool _shouldFilterFrom;
        private bool _shouldFilterTill;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;

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
            get => _filteringValue;
            set => SetField(ref _filteringValue, value);
        }

        public bool ShouldFilterFrom
        {
            get => _shouldFilterFrom;
            set => SetField(ref _shouldFilterFrom, value);
        }

        public bool ShouldFilterTill
        {
            get => _shouldFilterTill;
            set => SetField(ref _shouldFilterTill, value);
        }

        public DateTime? DateFrom
        {
            get => _dateFrom;
            set => SetField(ref _dateFrom, value);
        }

        public DateTime? DateTo
        {
            get => _dateTo;
            set => SetField(ref _dateTo, value);
        }

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
            _muscles = _muscleRepository.GetAll();
            _categories = _categoryRepository.GetAll();
            _trainingSessions = _trainingSessionRepository.GetAll().OrderByDescending(session => session.Date);
        }
        
        private string CalculateProgressValue(IEnumerable<TrainingSession> sessions)
        {
            var metricsCalculator = GetMetricsCalculator();

            switch (Metric)
            {
                case FilterMetric.Sets:

                    var sets = sessions.Aggregate(
                        0d,
                        (totalSets, session) => totalSets + metricsCalculator.GetSetCount(session)
                    );

                    return $"{sets}";
                case FilterMetric.Volume:
                    var weightUnits = _userPreferencesService.Load().WeightUnits.GetDescription();

                    var volume = sessions.Aggregate(
                        0d,
                        (totalVolume, session) => totalVolume + metricsCalculator.GetLoadVolume(session)
                    );

                    return $"{volume} {weightUnits}";
                default:

                    throw new ArgumentException("Invalid metric");
            }
        }

        private IMetricsCalculator GetMetricsCalculator()
        {
            switch (SelectedFilteringValue)
            {
                case Exercise exercise:

                    return new ExerciseMetricsCalculator(new [] { exercise });
                case Muscle muscle:

                    return new MuscleMetricCalculator(new [] { muscle });
                case Category category:

                    if (category.ItemType == typeof(Exercise))
                    {
                        return new ExerciseMetricsCalculator(category.Items.Cast<Exercise>());
                    }
                    else if (category.ItemType == typeof(Muscle))
                    {
                        return new MuscleMetricCalculator(category.Items.Cast<Muscle>());
                    }
                    else
                    {
                        throw new ArgumentException("Invalid category item type");
                    }
                default:

                    throw new ArgumentException("Invalid filtering value type");
            }
        }
        
        private IEnumerable<object> GetFilteringValueOptions()
        {
            switch (FilterBy)
            {
                case FilterBy.Exercise:

                    return _exercises;
                case FilterBy.Muscle:

                    return _muscles;
                case FilterBy.Category:

                    return _categories;
                default:
                    throw new ArgumentException("Invalid filter by option");
            }
        }

        private string GetDateUnitFromDate(DateTime date)
        {
            int GetWeek() => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday
            );
            
            switch (GroupBy) {
                case GroupBy.Day:

                    return date.ToShortDateString();
                case GroupBy.Week:

                    return $"{GetWeek()}/{date.Year}";
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
            
            if (ShouldFilterFrom && DateFrom != null)
            {
                filteredSessions = filteredSessions.Where(session => session.Date >= DateFrom);
            }
            
            if (ShouldFilterTill && DateTo != null)
            {
                filteredSessions = filteredSessions.Where(session => session.Date <= DateTo);
            }

            return filteredSessions;
        }
        
        public ProgressPageViewModel(Repository<Exercise> exerciseRepository, Repository<TrainingSession> trainingSessionRepository, Repository<Muscle> muscleRepository, UserPreferencesService userPreferencesService, Repository<Category> categoryRepository)
        {
            _userPreferencesService = userPreferencesService;
            _categoryRepository = categoryRepository;
            _exerciseRepository = exerciseRepository;
            _trainingSessionRepository = trainingSessionRepository;
            _muscleRepository = muscleRepository;

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

                var results = GetFilteredSessions()
                    .GroupBy(session => GetDateUnitFromDate(session.Date))
                    .Select(
                        sessions => new ProgressResult
                        {
                            DateUnit = sessions.Key,
                            Value = CalculateProgressValue(sessions)
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
                            LoadData();
                            
                            _filteringValueOptions = GetFilteringValueOptions();
                            _filteringValue = _filteringValueOptions.FirstOrDefault();
                            
                            OnPropertyChanged(string.Empty);
                        }
                    );
                }
            );
        }
    }
}