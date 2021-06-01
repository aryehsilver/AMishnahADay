using AMishnahADay.ViewModels;
using System;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Model;

namespace AMishnahADay.Views {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : RadWindow {

    public MainWindow() {
      InitializeComponent();
      //hebrewMishnah.Document.StyleRepository[RadDocumentDefaultStyles.NormalStyleName].ParagraphProperties.FlowDirection = FlowDirection.RightToLeft;
      hebrewMishnah.Document.Selection.SelectAll();
      hebrewMishnah.ChangeParagraphFlowDirection(FlowDirection.RightToLeft);
      hebrewMishnah.Document.Selection.Clear();
    }

    private void Settings_Click(object sender, RoutedEventArgs e) {
      new Settings().ShowDialog();
      // TODO: Since context hasn't changed the mishnah is still the old one not the changed...
      _ = ((MainWindowViewModel)DataContext).LoadData();
    }

    protected override void OnClosed() =>
      _ = ((MainWindowViewModel)DataContext).Save();

    private void Theme_Click(object sender, RoutedEventArgs e) =>
      _ = ((MainWindowViewModel)DataContext).SetTheme();

    private void HebrewMishnah_DocumentChanged(object sender, EventArgs e) {
      //hebrewMishnah.Document.StyleRepository[RadDocumentDefaultStyles.NormalStyleName].ParagraphProperties.FlowDirection = FlowDirection.RightToLeft;
      hebrewMishnah.Document.Selection.SelectAll();
      hebrewMishnah.ChangeParagraphFlowDirection(FlowDirection.RightToLeft);
      hebrewMishnah.Document.Selection.Clear();
    }
  }
}
