using AMishnahADay.Views;
using GalaSoft.MvvmLight.Command;

namespace AMishnahADay.ViewModels;

public class NotifyIconViewModel : ViewModelBase {
  public NotifyIconViewModel() =>
    LoadCommands();

  private void LoadCommands() {
    ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindowCommandExecute);
    OpenMainWindowCommand = new RelayCommand(OpenMainWindowCommandExecute);
    ExitApplicationCommand = new RelayCommand(ExitApplicationCommandExecute);
  }

  #region ShowSettingsWindowCommand

  public RelayCommand ShowSettingsWindowCommand { get; set; }

  private void ShowSettingsWindowCommandExecute() =>
    new Settings().ShowDialog();

  #endregion

  #region OpenMainWindowCommand
  public RelayCommand OpenMainWindowCommand { get; set; }

  private void OpenMainWindowCommandExecute() =>
    new MainWindow().ShowDialog();

  #endregion

  #region ExitApplicationCommand
  public RelayCommand ExitApplicationCommand { get; set; }

  private void ExitApplicationCommandExecute() =>
    System.Windows.Application.Current.Shutdown();

  #endregion
}
