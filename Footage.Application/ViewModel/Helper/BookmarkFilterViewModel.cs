namespace Footage.Application.ViewModel.Helper
{
    using System;
    using Footage.Application.ViewModel.Base;
    using Footage.Model.ModelHelper;

    public class BookmarkFilterViewModel : ViewModelBase
    {
        private BookmarkFilter filter;

        private bool enabledSetterExecuting;

        public event EventHandler FilterChanged;
        
        public bool Enabled
        {
            get => filter.Enabled;
            set
            {
                enabledSetterExecuting = true;
                
                filter.Enabled = value;

                filter.IncludeLow = value;
                filter.IncludeMedium = value;
                filter.IncludeHigh = value;
                RaisePropertyChanged(string.Empty); // all properties changed

                enabledSetterExecuting = false;
                
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public bool IncludeLow
        {
            get => filter.IncludeLow;
            set
            {
                filter.IncludeLow = value;
                RaisePropertyChanged(nameof(IncludeLow));
                ToggleEnabledIfNeeded();
                
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public bool IncludeMedium
        {
            get => filter.IncludeMedium;
            set
            {
                filter.IncludeMedium = value;
                RaisePropertyChanged(nameof(IncludeMedium));
                ToggleEnabledIfNeeded();
                
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public bool IncludeHigh
        {
            get => filter.IncludeHigh;
            set
            {
                filter.IncludeHigh = value;
                RaisePropertyChanged(nameof(IncludeHigh));
                ToggleEnabledIfNeeded();
                
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public BookmarkFilterViewModel(BookmarkFilter filter)
        {
            this.filter = filter;
        }

        private void ToggleEnabledIfNeeded()
        {
            if (enabledSetterExecuting)
            {
                return;
            }
            
            if (Any())
            {
                filter.Enabled = true;
                RaisePropertyChanged(nameof(Enabled));
            }
            else if (!Any())
            {
                filter.Enabled = false;
                RaisePropertyChanged(nameof(Enabled));
            }
            
            bool Any() => IncludeLow || IncludeMedium || IncludeHigh;
        }
    }
}