using AMishnahADay.Models.Models;
using AMishnahADay.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using NotifyIcon;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace AMishnahADay {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {

    private readonly AppDbContext Context = new();
    private TaskbarIcon notifyIcon;
    public DateTime TimeForToast { get; set; }
    public bool IsStarted { get; set; }

    protected override void OnStartup(StartupEventArgs e) {
      using AppDbContext context = new();
      context.Database.Migrate();

      // Listen to notification activation
      ToastNotificationManagerCompat.OnActivated += toastArgs =>
        Current.Dispatcher.Invoke(delegate {
          new MainWindow().ShowDialog();
        });

      if (!IsStarted) {
        base.OnStartup(e);
        // Create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
        notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

        StyleManager.ApplicationTheme = new FluentTheme();
        if (Context.Settings.SingleOrDefault().DarkMode) {
          FluentPalette.LoadPreset(FluentPalette.ColorVariation.Dark);
        } else {
          FluentPalette.LoadPreset(FluentPalette.ColorVariation.Light);
        }
        //ThemeEffectsHelper.IsAcrylicEnabled = false;

        // To be removed since going to setup popping the toast in another app
        // DateTime timeForToast = new AppDbContext().Settings.SingleOrDefault().TimeForToast;
        // TimeForToast = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, timeForToast.Hour, timeForToast.Minute, 0);
        // if (TimeForToast <= DateTime.Now) {
        //   TimeForToast = TimeForToast.AddDays(1);
        // }

        IsStarted = true;
      }
    }

    public void PopTheToast() =>
      new ToastContentBuilder()
          .SetToastScenario(ToastScenario.Reminder)
          .AddArgument("masechtah", "berachot")
          .AddArgument("mishnah", 1)
          .AddArgument("perek", 1)
          .AddText("Learn a Mishnah!")
          .AddText("Click to view the Mishnah of the day.")
          //.AddHeroImage(new Uri("file:///"))
          .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath("Data/AMishnahADay.png")), ToastGenericAppLogoCrop.Circle)
          .AddAttributionText("Via AMAD")
          .AddToastInput(new ToastSelectionBox("snoozeTime") {
            DefaultSelectionBoxItemId = "15",
            Items = {
              new ToastSelectionBoxItem("5", "5 minutes"),
              new ToastSelectionBoxItem("15", "15 minutes"),
              new ToastSelectionBoxItem("60", "1 hour"),
              new ToastSelectionBoxItem("240", "4 hours"),
              new ToastSelectionBoxItem("1440", "1 day")
            }
          })
          .AddButton(new ToastButtonSnooze() { SelectionBoxId = "snoozeTime" })
          .AddButton(new ToastButtonDismiss())
          .Schedule(TimeForToast, toast => toast.ExpirationTime = TimeForToast.AddDays(1));

    protected override void OnExit(ExitEventArgs e) {
      // The icon would clean up automatically, but this is cleaner
      notifyIcon.Dispose();
      base.OnExit(e);
    }
  }
}
