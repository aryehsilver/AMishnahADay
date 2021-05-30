using AMishnahADay.ViewModels;
using System.Windows;
using Telerik.Windows.Controls;

namespace AMishnahADay.Views {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : RadWindow {

    public MainWindow() =>
      InitializeComponent();
    //HtmlFormatProvider provider = new();
    //RadDocument engDocument = provider.Import(mishnah.EnglishText);
    //englishMishnah.Document = engDocument;
    //RadDocument hebDocument = provider.Import(mishnah.HebrewText);
    //hebrewMishnah.Document = hebDocument;

    private void Settings_Click(object sender, RoutedEventArgs e) {
      new Settings().ShowDialog();
      // TODO: Since context hasn't changed the mishnah is still the old one not the changed...
      _ = ((MainWindowViewModel)DataContext).LoadData();
    }

    protected override void OnClosed() =>
      _ = ((MainWindowViewModel)DataContext).Save();

    private void Theme_Click(object sender, RoutedEventArgs e) =>
      _ = ((MainWindowViewModel)DataContext).SetTheme();
  }
}
