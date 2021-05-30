﻿using AMishnahADay.Models;
using AMishnahADay.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Notifications;
using NotifyIcon;
using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using Telerik.Windows.Controls;

namespace AMishnahADay {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    #region Props, Fields & consts
    private static readonly string App_Folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AMAD_App";
    private TaskbarIcon notifyIcon;
    private Timer timer;
    public DateTime LastRead { get; set; }
    public bool RunsOnStartup { get; set; } = true;
    public double Interval { get; set; } = 30;
    public DateTime TimeForToast { get; set; }
    public int MishnahNumber { get; set; }
    public string Perek { get; set; }
    public string Mesechtah { get; set; }
    public bool IsStarted { get; set; } = false;
    #endregion

    protected override void OnStartup(StartupEventArgs e) {
      // Listen to notification activation
      ToastNotificationManagerCompat.OnActivated += toastArgs =>
        Current.Dispatcher.Invoke(delegate {
          new MainWindow().ShowDialog();
        });

      if (!IsStarted) {
        base.OnStartup(e);
        //create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
        notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

        StyleManager.ApplicationTheme = new FluentTheme();
        SetThemeMode(new AppDbContext());
        //ThemeEffectsHelper.IsAcrylicEnabled = false;

        Start();
        //PopTheToast();
      }

      using AppDbContext context = new();
      context.Database.Migrate();
    }

    private void SetThemeMode(AppDbContext context) =>
      FluentPalette.LoadPreset(context.Settings.SingleOrDefault().DarkMode ? FluentPalette.ColorVariation.Dark : FluentPalette.ColorVariation.Dark);

    public void Start() {
      if (!Directory.Exists(App_Folder)) {
        Directory.CreateDirectory(App_Folder);
      }

      DateTime timeForToast = new AppDbContext().Settings.SingleOrDefault().TimeForToast;
      TimeForToast = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, timeForToast.Hour, timeForToast.Minute, 0);

      //RunTimer();

      IsStarted = true;
    }

    private void RunTimer() {
      timer = new Timer {
        // [1 min = 60,000 | 5 min = 300,000 | 30 min = 1,800,000 | 1 hr = 3,600,000]
        Interval = Interval * 60000
      };
      timer.Start();
      timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e) =>
      // TODO: Use Ninject Messaging to update timer...

      // ISSUE: This will only get hit the next time the timer finishes it's elapsed time.
      // What if the user has chosen a smaller interval, say from 30 min down to 5,
      // how will the app know to change it until another 30 min passes and we hit the elapsed?
      // Settings need to raise an event which will be picked up here...

      // CheckTimeStamps();
      PopTheToast();

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
          // TODO: If time has passed an exception will be thrown, need to use tommorow in that case...
          .Schedule(TimeForToast, toast => toast.ExpirationTime = TimeForToast.AddMinutes(1));

    protected override void OnExit(ExitEventArgs e) {
      // The icon would clean up automatically, but this is cleaner
      notifyIcon.Dispose();
      base.OnExit(e);
    }
  }
}
