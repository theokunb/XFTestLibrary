using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace XFTestLibrary.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected bool isBusy;


        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                OnPropertyChnaged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChnaged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
