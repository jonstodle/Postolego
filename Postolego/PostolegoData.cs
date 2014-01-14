﻿using PocketInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postolego {
    public class PostolegoData : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public Pocket PocketSession {
            get {
                if(!settings.Contains("PocketSession")) {
                    settings["PocketSession"] = null;
                }
                return (Pocket)settings["PocketSession"];
            }

            set {
                settings["PocketSession"] = value;
                OnPropertyChanged("PocketSession");
            }
        }

        public ObservableCollection<PocketItem> UnreadList {
            get {
                if(!settings.Contains("UnreadList")) {
                    settings["UnreadList"] = null;
                }
                return (ObservableCollection<PocketItem>)settings["UnreadList"];
            }

            set {
                settings["UnreadList"] = value;
                OnPropertyChanged("UnreadList");
            }
        }

        public ObservableCollection<PocketItem> FavoritesList {
            get {
                if(!settings.Contains("FavoritesList")) {
                    settings["FavoritesList"] = null;
                }
                return (ObservableCollection<PocketItem>)settings["FavoritesList"];
            }

            set {
                settings["FavoritesList"] = value;
                OnPropertyChanged("FavoritesList");
            }
        }

        public ObservableCollection<PocketItem> ArchiveList {
            get {
                if(!settings.Contains("ArchiveList")) {
                    settings["ArchiveList"] = null;
                }
                return (ObservableCollection<PocketItem>)settings["ArchiveList"];
            }

            set {
                settings["ArchiveList"] = value;
                OnPropertyChanged("ArchiveList");
            }
        }

        private void OnPropertyChanged(string changedProperty) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(changedProperty));
            }
        }
    }
}