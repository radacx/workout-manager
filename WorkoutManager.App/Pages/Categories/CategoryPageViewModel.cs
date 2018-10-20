using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Categories.Dialogs;
using WorkoutManager.App.Pages.Categories.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Categories
{
    internal class CategoryPageViewModel : ViewModelBase
    {
        private readonly Repository<Category> _categoryRepository;
        private readonly DialogFactory<CategoryDialog, CategoryDialogViewModel> _categoryDialogFactory;
        
        public BulkObservableCollection<Category> Categories { get; } = new BulkObservableCollection<Category>();

        public ICommand Delete { get; }
        
        public ICommand OpenAddCategoryDialog { get; }
        
        public ICommand OpenEditCategoryDialog { get; }
        
        private void LoadCategories() => Categories.AddRange(_categoryRepository.GetAll());

        private void CreateCategory(Category category) => _categoryRepository.Create(category);

        private void UpdateCategory(Category category) => _categoryRepository.Update(category);
        
        private void DeleteCategory(Category category) => _categoryRepository.Delete(category);
        
        public CategoryPageViewModel(Repository<Category> categoryRepository, DialogFactory<CategoryDialog, CategoryDialogViewModel> categoryDialogFactory)
        {
            _categoryRepository = categoryRepository;
            _categoryDialogFactory = categoryDialogFactory;

            OpenAddCategoryDialog = new Command(
                () =>
                {
                    var category = new Category();
                    var dialog = _categoryDialogFactory.Get();
                    dialog.Data.Category = category;
                    dialog.Data.SubmitButtonTitle = "Create";
                    
                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Categories.Add(category);
                    Task.Run(() => CreateCategory(category));
                });
            
            OpenEditCategoryDialog = new Command<Category>(
                category =>
                {
                    var categoryClone = category.DeepClone();
                    var dialog = _categoryDialogFactory.Get();
                    dialog.Data.Category = categoryClone;
                    dialog.Data.SubmitButtonTitle = "Save";
                    
                    var dialogResult = dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Categories.Replace(category, categoryClone);
                    Task.Run(() => UpdateCategory(categoryClone));
                });
            
            Delete = new Command<Category>(
                category =>
                {
                    Categories.Remove(category);
                    Task.Run(() => DeleteCategory(category));
                });
            
            Task.Run(() => LoadCategories());
        }
    }
}