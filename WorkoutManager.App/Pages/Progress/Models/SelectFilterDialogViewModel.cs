using System.Threading.Tasks;
using System.Windows.Input;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Progress;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Progress.Models
{
    internal class SelectFilterDialogViewModel : ViewModelBase
    {
        private readonly Repository<ProgressFilter> _progressFilterRepository;
        
        public ICommand RemoveFilter { get; }
        
        public ICommand SelectFilter { get; }
        
        public BulkObservableCollection<ProgressFilter> Filters { get; } = new BulkObservableCollection<ProgressFilter>();

        private void LoadData()
        {
            var s = _progressFilterRepository.GetAll();
            Filters.AddRange(s);
        }
        
        public SelectFilterDialogViewModel(Repository<ProgressFilter> progressFilterRepository, Hub eventAggregator)
        {
            _progressFilterRepository = progressFilterRepository;
            
            Filters.ShapeView().OrderBy(filter => filter.Name);

            SelectFilter = new Command<ProgressFilter>(
                filter => eventAggregator.Publish(new ProgressFilterSelectedEvent(filter))
            );
            
            RemoveFilter = new Command<ProgressFilter>(
                filter =>
                {
                    Filters.Remove(filter);
                    Task.Run(() => _progressFilterRepository.Delete(filter));
                });
            
            Task.Run(LoadData);
        }
    }
}