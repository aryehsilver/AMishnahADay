namespace AMishnahADay.Views;

  public partial class MainWindow : RadWindow {
    public MainWindow() {
      InitializeComponent();
      SetStyleOnHebrewDoc();
    SetDocColours();
    }

    private void Settings_Click(object sender, RoutedEventArgs e) {
      new Settings().ShowDialog();
      _ = ((MainWindowViewModel)DataContext).LoadData();
    }

    protected override void OnClosed() =>
      _ = ((MainWindowViewModel)DataContext).Save();

  private void Theme_Click(object sender, RoutedEventArgs e) {
      _ = ((MainWindowViewModel)DataContext).SetTheme();
    SetDocColours();
  }

  private void SetDocColours() {
    hebrewMishnah.Document.Selection.SelectAll();
    englishMishnah.Document.Selection.SelectAll();
    hebrewMishnah.ChangeFontSize(18);
    englishMishnah.ChangeFontSize(18);
    if (((MainWindowViewModel)DataContext).DarkMode) {
      SetDarkColours(hebrewMishnah);
      SetDarkColours(englishMishnah);
    } else {
      SetLightColours(hebrewMishnah);
      SetLightColours(englishMishnah);
    }
    hebrewMishnah.Document.Selection.Clear();
    englishMishnah.Document.Selection.Clear();
  }

  private static void SetDarkColours(RadRichTextBox textBox) {
    textBox.ChangeTextForeColor((Media.Color)Media.ColorConverter.ConvertFromString("#fbfbfb"));
    textBox.Background = new SolidColorBrush {
      Color = (Media.Color)Media.ColorConverter.ConvertFromString("#2d2d2d")
    };
  }

  private static void SetLightColours(RadRichTextBox textBox) {
    textBox.ChangeTextForeColor((Media.Color)Media.ColorConverter.ConvertFromString("#2d2d2d"));
    textBox.Background = new SolidColorBrush {
      Color = (Media.Color)Media.ColorConverter.ConvertFromString("#fbfbfb")
    };
  }

    private void HebrewMishnah_DocumentChanged(object sender, EventArgs e) =>
      SetStyleOnHebrewDoc();

    private void SetStyleOnHebrewDoc() {
    hebrewMishnah.Document.StyleRepository[RadDocumentDefaultStyles.NormalStyleName].ParagraphProperties.FlowDirection = System.Windows.FlowDirection.RightToLeft;
      hebrewMishnah.Document.Selection.SelectAll();
    hebrewMishnah.ChangeParagraphFlowDirection(System.Windows.FlowDirection.RightToLeft);
    hebrewMishnah.ChangeFontFamily(new Media.FontFamily("Narkisim"));
      hebrewMishnah.Document.Selection.Clear();
    }
  private void Hyperlink_Click(object sender, RoutedEventArgs e) =>
    Process.Start(new ProcessStartInfo("https://www.sefaria.org") { UseShellExecute = true });
  }
}
