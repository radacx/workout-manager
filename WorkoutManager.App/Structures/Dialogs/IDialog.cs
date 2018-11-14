using System.Threading.Tasks;

namespace WorkoutManager.App.Structures.Dialogs
{
    internal interface IDialog<out TDataContext>
    {
        TDataContext Data { get; }

        Task<DialogResult> Show();
    }
}