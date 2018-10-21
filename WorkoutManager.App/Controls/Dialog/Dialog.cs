using System.Windows;
using System.Windows.Controls;

namespace WorkoutManager.App.Controls.Dialog
{
    internal class Dialog : ContentControl
    {
        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register(
            "Result",
            typeof(object),
            typeof(Dialog),
            new PropertyMetadata(default(object))
        );

        public object Result
        {
            get => (object) GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
        }
        
        public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(
            "DialogTitle",
            typeof(string),
            typeof(Dialog),
            new PropertyMetadata(default(string))
        );

        public string DialogTitle
        {
            get => (string) GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }
        
        public static readonly DependencyProperty CanSubmitProperty = DependencyProperty.Register(
            "CanSubmit",
            typeof(bool),
            typeof(Dialog),
            new PropertyMetadata(true)
        );

        public bool CanSubmit
        {
            get => (bool) GetValue(CanSubmitProperty);
            set => SetValue(CanSubmitProperty, value);
        }
        
        public static readonly DependencyProperty SubmitButtonTitleProperty = DependencyProperty.Register(
            "SubmitButtonTitle",
            typeof(string),
            typeof(Dialog),
            new PropertyMetadata(default(string))
        );

        public string SubmitButtonTitle
        {
            get => (string) GetValue(SubmitButtonTitleProperty);
            set => SetValue(SubmitButtonTitleProperty, value);
        }
    }
}
