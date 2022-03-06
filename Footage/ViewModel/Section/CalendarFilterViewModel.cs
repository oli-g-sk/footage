namespace Footage.ViewModel.Section
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Footage.Presentation;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class CalendarFilterViewModel : ViewModelBase
    {
        public ObservableCollection<DateTime> SelectedDates { get; set; }
        
        public RelayCommand ClearCommand { get; }

        public CalendarFilterViewModel()
        {
            SelectedDates = new ObservableCollection<DateTime>();
            ClearCommand = new RelayCommand(Clear, CanClear);
        }

        private void Clear()
        {
            SelectedDates.Clear();
        }

        private bool CanClear()
        {
            return SelectedDates?.Any() ?? false;
        }
    }
}