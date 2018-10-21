using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WorkoutManager.App.Structures;
using WorkoutManager.Contract.Models.Misc;

namespace WorkoutManager.App.Pages.Categories.Models
{
    internal class AddCategoryOptionsDialogViewModel : DialogModelBase
    {
        private IEnumerable<IEntity> _selectedOptions = new List<IEntity>();
        
        public IEnumerable<IEntity> Options { get; set; }

        public IEnumerable<IEntity> SelectedOptions => _selectedOptions;

        public ICommand SelectionChanged { get; }

        public AddCategoryOptionsDialogViewModel()
        {
            SelectionChanged = new Command<IList>(
                selectedItems => _selectedOptions = selectedItems.Cast<IEntity>()
            );
        }
    }
}