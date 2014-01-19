using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postolego {
    public class PostolegoPivotHeader : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _headerText;
        private bool _isLoading;

        public string HeaderText {
            get {
                return _headerText;
            }

            set {
                _headerText = value;
                OnPropertyChanged("HeaderText");
            }
        }

        public bool IsLoading {
            get {
                return _isLoading;
            }

            set {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private void OnPropertyChanged(string changedProperty) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(changedProperty));
            }
        }
    }
}
