// Copyright (C) 2026 blyssmew - Licensed under the GNU GPL v3.
// See the LICENSE file in the project root for full license details.

using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SmallSteps.Desktop.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private object? _currentPage;

    public object? CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (ReferenceEquals(_currentPage, value))
                return;

            _currentPage = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
        }
    }

    public ICommand ShowHomeCommand { get; }
    public ICommand ShowTrackersCommand { get; }
    public ICommand ShowTrackerDetailCommand { get; }
    public ICommand ShowSettingsCommand { get; }

    public MainWindowViewModel()
    {
        ShowHomeCommand = new RelayCommand(() => CurrentPage = new HomeViewModel());
        ShowTrackersCommand = new RelayCommand(() => CurrentPage = new TrackersViewModel());
        ShowTrackerDetailCommand = new RelayCommand(() => CurrentPage = new TrackerDetailViewModel());
        ShowSettingsCommand = new RelayCommand(() => CurrentPage = new SettingsViewModel());
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private sealed class RelayCommand(Action execute) : ICommand
    {
        private readonly Action _execute = execute;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
