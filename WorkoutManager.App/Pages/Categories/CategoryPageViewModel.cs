using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PubSub.Core;
using WorkoutManager.App.Events;
using WorkoutManager.App.Pages.Categories.Dialogs.CategoryDialog;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Structures.Collections.Common;
using WorkoutManager.App.Structures.Dialogs;
using WorkoutManager.App.Structures.ViewModels;
using WorkoutManager.Contract.Models.Base;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Repository;
using WorkoutManager.Service.Services;

namespace WorkoutManager.App.Pages.Categories
{
    internal class CategoryPageViewModel : ViewModelBase
    {
        public static string DialogsIdentifier => "CategoryPageDialogs";
        
        private readonly Repository<Category> _categoryRepository;
        private readonly CategoryOptionsService _categoryOptionsService;
        private readonly DialogFactory _dialogs;

        private CategoryOptions _options;
        
        
        #region Commands

        public ICommand DeleteCommand { get; }
        
        public ICommand OpenAddCategoryDialogCommand { get; }
        
        public ICommand OpenEditCategoryDialogCommand { get; }

        #endregion


        #region UI Properties

        public ObservableRangeCollection<Category> Categories { get; } = new WpfObservableRangeCollection<Category>();

        #endregion


        #region CategoryDialog

        private IEntity GetOption(int id, Type type) => _options.GetOptions(type).FirstOrDefault(x => x.Id == id);
        
        private async void OpenAddCategoryDialogAsync()
        {
            var category = new Category();
                    
            var dialog = _dialogs.For<CategoryDialogViewModel>(DialogsIdentifier);
            dialog.Data.DialogTitle = "New category";
            dialog.Data.SubmitButtonTitle = "Create";
            //    Order required ..
            dialog.Data.CategoryOptions = _options;
            dialog.Data.Category = CategoryViewModel.FromModel(category, GetOption);  
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            category = dialog.Data.Category.ToModel();
            Categories.Add(category);
            CreateCategory(category);
        }

        private async void OpenEditCategoryDialogAsync(Category category)
        {
            var categoryClone = category.Clone();

            var dialog = _dialogs.For<CategoryDialogViewModel>(DialogsIdentifier);               
            dialog.Data.DialogTitle = "Modified category";
            dialog.Data.SubmitButtonTitle = "Save";
            //    Order required ..
            dialog.Data.CategoryOptions = _options;
            dialog.Data.Category = CategoryViewModel.FromModel(categoryClone, GetOption);  
            
            var dialogResult = await dialog.Show();

            if (dialogResult != DialogResult.Ok)
            {
                return;
            }

            categoryClone = dialog.Data.Category.ToModel();
            Categories.Replace(category, categoryClone);
            UpdateCategory(categoryClone);
        }

        #endregion
        
        private void LoadData()
        {
            Categories.AddRange(_categoryRepository.GetAll());
            _options = _categoryOptionsService.GetOptions();
        }

        private void CreateCategory(Category category) => _categoryRepository.Create(category);

        private void UpdateCategory(Category category) => _categoryRepository.Update(category);

        private void DeleteCategory(Category category)
        {
            Categories.Remove(category);

            Task.Run(() => _categoryRepository.Delete(category));
        }

        private void NotifyMusclesChanged(MusclesChangedEvent ev)
        {
            _options.Muscles = ev.ActualMuscles;
        }

        private void NotifyExercisesChanged(ExercisesChangedEvent ev)
        {
            _options.Exercises = ev.ActualExercises;
        }
        
        public CategoryPageViewModel(Repository<Category> categoryRepository, DialogFactory dialogs, CategoryOptionsService categoryOptionsService, Hub hub)
        {
            _categoryRepository = categoryRepository;
            _dialogs = dialogs;
            _categoryOptionsService = categoryOptionsService;

            OpenAddCategoryDialogCommand = new Command(OpenAddCategoryDialogAsync);
            OpenEditCategoryDialogCommand = new Command<Category>(OpenEditCategoryDialogAsync);
            DeleteCommand = new Command<Category>(DeleteCategory);
            
            hub.Subscribe<MusclesChangedEvent>(NotifyMusclesChanged);
            hub.Subscribe<ExercisesChangedEvent>(NotifyExercisesChanged);
            
            Task.Run(LoadData);
        }
    }
}