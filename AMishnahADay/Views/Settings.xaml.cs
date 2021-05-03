using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace AMishnahADay.Views {
  /// <summary>
  /// Interaction logic for Settings.xaml
  /// </summary>
  public partial class Settings : RadWindow {
    #region Props, Fields & Consts
    private const string APP_ID = "AMAD_App";
    private static readonly string App_Folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\";
    private static readonly string Startup_Folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
    public bool RunsOnStartup { get; set; } = true;
    public bool Success { get; set; } = false;
    #endregion

    public Settings() {
      InitializeComponent();
      Success = true;
    }

    private void Test_Click(object sender, RoutedEventArgs e) {

    }

    private void Save_Click(object sender, RoutedEventArgs e) {

    }

    private void Cancel_Click(object sender, RoutedEventArgs e) {

    }
  }
}
