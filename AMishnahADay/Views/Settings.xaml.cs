using AMishnahADay.ViewModels;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.MaterialControls;

namespace AMishnahADay.Views {
  /// <summary>
  /// Interaction logic for Settings.xaml
  /// </summary>
  public partial class Settings : RadWindow {
    public Settings() {
      InitializeComponent();
      ThemeEffectsHelper.IsAcrylicEnabled = false;
    }

    private void Save_Click(object sender, RoutedEventArgs e) {
      _ = ((SettingsViewModel)DataContext).Save();
      Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => Close();

    private void Theme_Click(object sender, RoutedEventArgs e) =>
      _ = ((SettingsViewModel)DataContext).SetTheme();
  }
}
