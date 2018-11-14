using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkoutManager.App.Pages.Progress.Dialogs;
using WorkoutManager.App.Pages.Progress.Structures;
using WorkoutManager.App.Pages.Progress.Structures.Calculators;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Contract.Extensions;
using WorkoutManager.Contract.Models.Base;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.Progress;
using WorkoutManager.Contract.Models.Sessions;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.Progress
{
    internal class ProgressPageViewModel : ViewModelBase
    {
        public static string DialogsIdentifier => "ProgressPageDialogs";

        private readonly DialogFactory _dialogs;
        
        private readonly Repository<Exercise> _exerciseRepository;
        private readonly Repository<TrainingSession> _trainingSessionRepository;
        private readonly Repository<Muscle> _muscleRepository;
        private readonly Repository<Category> _categoryRepository;
        private readonly Repository<ProgressFilter> _progressFilterRepository;
        
        private readonly UserPreferencesService _userPreferencesService;
        

        #region Commands

        public ICommand OpenAddFilterDialogCommand { get; }
        
        public ICommand RefreshCommand { get; }
        
        public ICommand RemoveFilterCommand { get; }
        
        public ICommand SelectFilterCommand { get; }

        #endregion


        #region UI Properties

        public ObservableRangeCollection<ProgressFilter> Filters { get; } = new WpfObservableRangeCollection<ProgressFilter>();
        
        public IEnumerable<FilterBy> FilterByOptions { get; } = Enum.GetValues(typeof(FilterBy)).Cast<FilterBy>();
        public IEnumerable<GroupBy> GroupByOptions { get; } = Enum.GetValues(typeof(GroupBy)).Cast<GroupBy>();
        public IEnumerable<FilterMetric> MetricOptions { get; } = Enum.GetValues(typeof(FilterMetric)).Cast<FilterMetric>();
        public IEnumerable<DayOfWeek> DayOfWeekOptions { get; } = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
        
        private IEntity _filteringValue;
        
        private FilterBy _filterBy = FilterBy.Exercise;
        private GroupBy _groupBy = GroupBy.Day ;
        private FilterMetric _metric = FilterMetric.Volume;
        
        private IEnumerable<ProgressResult> _results;
        
        private IEnumerable<IEntity> _filteringValueOptions;
        private bool _shouldFilterFrom;
        private bool _shouldFilterTill;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private DayOfWeek _startingDayOfWeek = DayOfWeek.Monday;
        
        public DayOfWeek StartingDayOfWeek
        {
            get => _startingDayOfWeek;
            set => SetField(ref _startingDayOfWeek, value);
        }
        
        public IEnumerable<ProgressResult> Results
        {
            get => _results;
            set => SetField(ref _results, value);
        }

        public IEntity SelectedFilteringValue
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

        public IEnumerable<IEntity> FilteringValueOptions
        {
            get => _filteringValueOptions;
            set => SetField(ref _filteringValueOptions, value);
        }
        
        #endregion
  
        
        #region ProgressFilterDialog

        private async void OpenAddFilterDialogAsync()
        {
            var progressFilterForDialog = new ProgressFilterViewModel();

            var dialog = _dialogs.For<ProgressFilterDialogViewModel>(DialogsIdentifier);
            dialog.Data.ProgressFilter = progressFilterForDialog;
            dialog.Data.DialogTitle = "New filter";
            dialog.Data.SubmitButtonTitle = "Create";
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            var progressFilter = new ProgressFilter
            {
                Name = progressFilterForDialog.Name
            };
                    
            if (progressFilterForDialog.RememberFilterBy)
            {
                progressFilter.FilterBy = _filterBy;
                progressFilter.FilteringValue = SelectedFilteringValue;
            }

            if (progressFilterForDialog.RememberGroupBy)
            {
                progressFilter.GroupBy = _groupBy;
            }

            if (progressFilterForDialog.RememberMetric)
            {
                progressFilter.Metric = _metric;
            }

            _progressFilterRepository.Create(progressFilter);
                    
            Filters.Add(progressFilter);
        }

        #endregion
        
        
        private static readonly IEnumerable<string> FilterProperties = new[]
        {
            nameof(SelectedFilteringValue), nameof(GroupBy), nameof(Metric), nameof(ShouldFilterFrom),
            nameof(ShouldFilterTill), nameof(DateFrom), nameof(DateTo), nameof(StartingDayOfWeek), string.Empty
        };

        private IEnumerable<TrainingSession> _trainingSessions = new List<TrainingSession>();
        private IEnumerable<Exercise> _exercises = new List<Exercise>();
        private IEnumerable<Muscle> _muscles = new List<Muscle>(); 
        private IEnumerable<Category> _categories = new List<Category>();   
        private readonly CategoryOptions _options = new CategoryOptions(new List<Muscle>(), new List<Exercise>());
        
        private void LoadData()
        {
            _exercises = _exerciseRepository.GetAll().OrderBy(x => x.Name);
            _muscles = _muscleRepository.GetAll().OrderBy(x => x.Name);
            _categories = _categoryRepository.GetAll().OrderBy(x => x.Name);
            _options.Muscles = _muscles;
            _options.Exercises = _exercises;
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

                    return $"{volume:F2} {weightUnits}";
                default:

                    throw new ArgumentException("Invalid metric");
            }
        }

        private IEnumerable<TItem> GetCategoryItems<TItem>(Category category)
            where TItem : IEntity => _options.GetOptions(category.ItemType)
            .Where(option => category.Items.Select(x => x.Id).Contains(option.Id))
            .Cast<TItem>();
        
        private IMetricsCalculator GetMetricsCalculator()
        {
            switch (SelectedFilteringValue)
            {
                case Exercise exercise:

                    return new ExerciseMetricsCalculator(new [] { exercise });
                case Muscle muscle:

                    return new MuscleMetricCalculator(new [] { muscle });
                case Category category:
                    var itemType = category.ItemType;

                    if (itemType.FullName == typeof(Exercise).FullName)
                    {
                        var items = GetCategoryItems<Exercise>(category);
                        
                        return new ExerciseMetricsCalculator(items);    
                        
                    } else if (itemType.FullName == typeof(Muscle).FullName)
                    {
                        var items = GetCategoryItems<Muscle>(category);
                        
                        return new MuscleMetricCalculator(items);
                    }
                    
                    throw new ArgumentException($"Invalid type: {itemType}");
                default:

                    throw new ArgumentException("Invalid filtering value type");
            }
        }
        
        private IEnumerable<IEntity> GetFilteringValueOptions()
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
                _startingDayOfWeek
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

        #region Filters

        private void SelectFilter(ProgressFilter filter)
        {
            if (filter.Metric.HasValue)
            {
                Metric = filter.Metric.Value;
            }

            if (filter.GroupBy.HasValue)
            {
                GroupBy = filter.GroupBy.Value;
            }

            if (filter.FilterBy.HasValue)
            {
                FilterBy = filter.FilterBy.Value;
                SelectedFilteringValue = filter.FilteringValue;
            }
        }

        private void RemoveFilter(ProgressFilter filter)
        {
            Filters.Remove(filter);
            
            Task.Run(() => _progressFilterRepository.Delete(filter));
        }

        #endregion
        
        
        private void Refresh()
        {
            void Action()
            {
                LoadData();

                _filteringValueOptions = GetFilteringValueOptions();
                _filteringValue = _filteringValueOptions.FirstOrDefault();

                OnPropertyChanged(string.Empty);
            }

            Task.Run(Action);
        }

        private void InitializeData()
        {
            LoadData();
            Filters.AddRange(_progressFilterRepository.GetAll());
            OnPropertyChanged(nameof(FilterBy));
        }
     
        public ProgressPageViewModel(Repository<Exercise> exerciseRepository, Repository<TrainingSession> trainingSessionRepository, Repository<Muscle> muscleRepository, UserPreferencesService userPreferencesService, Repository<Category> categoryRepository, DialogFactory dialogs, Repository<ProgressFilter> progressFilterRepository)
        {
            _userPreferencesService = userPreferencesService;
            _categoryRepository = categoryRepository;
            _dialogs = dialogs;
            _progressFilterRepository = progressFilterRepository;
            _exerciseRepository = exerciseRepository;
            _trainingSessionRepository = trainingSessionRepository;
            _muscleRepository = muscleRepository;
   
            RemoveFilterCommand = new Command<ProgressFilter>(RemoveFilter);
            SelectFilterCommand = new Command<ProgressFilter>(SelectFilter);
            OpenAddFilterDialogCommand = new Command(OpenAddFilterDialogAsync);
            RefreshCommand = new Command(Refresh);
            
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
            
            Task.Run(InitializeData);
        }
    }
}