using AMishnahADay.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace AMishnahADay.ViewModels {
  public class NotifyIconViewModel {
    public ICommand ShowSettingsWindowCommand =>
      new DelegateCommand {
        CommandAction = () => new Settings().ShowDialog()
      };

    public ICommand OpenMainWindowCommand =>
      new DelegateCommand {
        CommandAction = () => new MainWindow().ShowDialog()
      };

    public ICommand ExitApplicationCommand =>
      new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
  }

  public class DelegateCommand : ICommand {
    public Action CommandAction { get; set; }
    public Func<bool> CanExecuteFunc { get; set; }

    public void Execute(object parameter) =>
      CommandAction();

    public bool CanExecute(object parameter) =>
      CanExecuteFunc == null || CanExecuteFunc();

    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
  }
}
