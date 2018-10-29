using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkoutManager.App.Annotations;

namespace WorkoutManager.App.Pages.Progress.Structures
{
    internal class ProgressFilterForDialog : INotifyPropertyChanged
    {
        private bool _rememberFilterBy;
        private bool _rememberGroupBy;
        private bool _rememberMetric;

        public string Name { get; set; }
        
        public bool RememberFilterBy
        {
            get => _rememberFilterBy;
            set
            {
                if (SetField(ref _rememberFilterBy, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool RememberGroupBy
        {
            get => _rememberGroupBy;
            set
            {
                if (SetField(ref _rememberGroupBy, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool RememberMetric
        {
            get => _rememberMetric;
            set
            {
                if (SetField(ref _rememberMetric, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool IsValid
            => RememberFilterBy || RememberGroupBy || RememberMetric;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}