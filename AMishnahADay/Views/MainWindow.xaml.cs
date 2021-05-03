using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.IO;
using System.Windows;

namespace AMishnahADay.Views {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() =>
      InitializeComponent();

    private void ShowToast_Click(object sender, RoutedEventArgs e) =>
      new ToastContentBuilder()
          .AddArgument("masechtah", "berachot")
          .AddArgument("mishnah", 1)
          .AddArgument("perek", 1)
          .AddText("Learn a Mishnah!")
          .AddText("Click to view the Mishnah of the day.")
          //.AddHeroImage(new Uri("file:///"))
          .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath("Data/AMishnahADay.png")), ToastGenericAppLogoCrop.Circle)
          .AddAttributionText("Via AMAD")
          .Schedule(DateTime.Now.AddSeconds(10)/*, toast => toast.ExpirationTime = DateTime.Now.AddMinutes(2)*/);
          //.Show(toast => toast.ExpirationTime = DateTime.Now.AddMinutes(2));
  }
}
