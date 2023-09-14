namespace AMishnahADay.Views;

public partial class Settings : RadWindow {
  public Settings() =>
    InitializeComponent();

  private void Save_Click(object sender, RoutedEventArgs e) {
    _ = ((SettingsViewModel)DataContext).Save();
    Close();
  }

  private void Cancel_Click(object sender, RoutedEventArgs e) =>
    Close();

  private void Theme_Click(object sender, RoutedEventArgs e) =>
    _ = ((SettingsViewModel)DataContext).SetTheme();

  private void Hyperlink_Click(object sender, RoutedEventArgs e) =>
    Process.Start(new ProcessStartInfo("https://alldev.co.uk") { UseShellExecute = true });
}
