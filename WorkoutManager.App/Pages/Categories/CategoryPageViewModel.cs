using System.Threading.Tasks;
using System.Windows.Input;
using Force.DeepCloner;
using WorkoutManager.App.Pages.Categories.Models;
using WorkoutManager.App.Structures;
using WorkoutManager.App.Utils.Dialogs;
using WorkoutManager.Contract.Models.Categories;
using WorkoutManager.Repository;

namespace WorkoutManager.App.Pages.Categories
{
    internal class CategoryPageViewModel : ViewModelBase
    {
        private readonly Repository<Category> _categoryRepository;

        public string CategoryDialogIdentifier => "CategoryDialog";
        
        public ObservableRangeCollection<Category> Categories { get; } = new WpfObservableRangeCollection<Category>();

        public ICommand Delete { get; }
        
        public ICommand OpenAddCategoryDialog { get; }
        
        public ICommand OpenEditCategoryDialog { get; }
        
        private void LoadCategories() => Categories.AddRange(_categoryRepository.GetAll());

        private void CreateCategory(Category category) => _categoryRepository.Create(category);

        private void UpdateCategory(Category category) => _categoryRepository.Update(category);
        
        private void DeleteCategory(Category category) => _categoryRepository.Delete(category);
        
        public CategoryPageViewModel(Repository<Category> categoryRepository, DialogViewer dialogViewer)
        {
            _categoryRepository = categoryRepository;

            OpenAddCategoryDialog = new Command(
                async () =>
                {
                    var category = new Category();
                    
                    var dialog = dialogViewer.For<CategoryDialogViewModel>(CategoryDialogIdentifier);
                    dialog.Data.Category = category;
                    dialog.Data.SubmitButtonTitle = "Create";
                    dialog.Data.DialogTitle = "New category";
                    
                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Categories.Add(category);
                    CreateCategory(category);
                });
            
            OpenEditCategoryDialog = new Command<Category>(
                async category =>
                {
                    var categoryClone = category.DeepClone();
                    
                    var dialog = dialogViewer.For<CategoryDialogViewModel>(CategoryDialogIdentifier);
                    dialog.Data.Category = categoryClone;
                    dialog.Data.SubmitButtonTitle = "Save";
                    dialog.Data.DialogTitle = "Modified category";
                    
                    var dialogResult = await dialog.Show();

                    if (dialogResult != DialogResult.Ok)
                    {
                        return;
                    }

                    Categories.Replace(category, categoryClone);
                    UpdateCategory(categoryClone);
                });
            
            Delete = new Command<Category>(
                category =>
                {
                    Categories.Remove(category);
                    Task.Run(() => DeleteCategory(category));
                });
            
            Task.Run(LoadCategories);
        }
    }
}