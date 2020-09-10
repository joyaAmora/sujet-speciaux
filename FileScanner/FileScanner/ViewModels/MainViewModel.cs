using FileScanner.Commands;
using FileScanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileScanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string selectedFolder;
        private ObservableCollection<string> folderItems = new ObservableCollection<string>();

        private ObservableCollection<ItemType> listItems = new ObservableCollection<ItemType>();
        public DelegateCommand<string> OpenFolderCommand { get; private set; }
        public DelegateCommand<string> ScanFolderCommand { get; private set; }

        public ObservableCollection<string> FolderItems
        {
            get => folderItems;
            set
            {
                folderItems = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ItemType> ListItems
        {
            get => listItems;
            set
            {
                listItems = value;
                OnPropertyChanged();
            }
        }
        public string SelectedFolder
        {
            get => selectedFolder;
            set
            {
                selectedFolder = value;
                OnPropertyChanged();
                ScanFolderCommand.RaiseCanExecuteChanged();
            }
        }

        public MainViewModel()
        {
            OpenFolderCommand = new DelegateCommand<string>(OpenFolder);
            ScanFolderCommand = new DelegateCommand<string>(ScanFolderAsync, CanExecuteScanFolder);
        }

        private bool CanExecuteScanFolder(string obj)
        {
            return !string.IsNullOrEmpty(SelectedFolder);
        }

        private void OpenFolder(string obj)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SelectedFolder = fbd.SelectedPath;
                }
            }
        }

        private async void ScanFolderAsync(string dir)
        {
            await Task.Run(() =>
            {
                try
                {
                    FolderItems = new ObservableCollection<string>(GetDirFiles(dir));
                    foreach (var item in Directory.EnumerateDirectories(dir, "*"))
                    {
                        var newItem = new ItemType(item, "/images/folder.jpg");

                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        (Action)delegate ()
                        {
                            ListItems.Add(newItem);
                        });
                       
                    }

                    foreach (var item in Directory.EnumerateFiles(dir, "*"))
                    {
                        var newItem = new ItemType(item, "/images/file.png");
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        (Action)delegate ()
                        {
                            ListItems.Add(newItem);
                        });
                    }
                }
                catch (System.UnauthorizedAccessException)
                {

                    MessageBox.Show("You don't have access to all the files in here!");
                }
            });

        }
        IEnumerable<string> GetDirFiles(string dir)
        {
            foreach (var d in Directory.EnumerateDirectories(dir, "*"))
            {
                yield return d;

                foreach (var f in Directory.EnumerateFiles(d, "*"))
                {
                    yield return f;
                }
            }
        }


    }
}