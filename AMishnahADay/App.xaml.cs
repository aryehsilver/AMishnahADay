using AMishnahADay.Views;
using Microsoft.Toolkit.Uwp.Notifications;
using NotifyIcon;
using System;
using System.IO;
using System.Timers;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.MaterialControls;
using Windows.Foundation.Collections;

namespace AMishnahADay {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    #region Props, Fields & consts
    private const string APP_ID = "A Mishnah A Day";
    private static readonly string App_Folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AMAD_App";
    private static readonly string Startup_Folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
    private TaskbarIcon notifyIcon;
    private Timer timer;
    public DateTime LastRead { get; set; }
    public bool RunsOnStartup { get; set; } = true;
    public bool SplashScreen { get; set; } = true;
    public double Interval { get; set; } = 30;
    public DateTime TimeForToast { get; set; }
    public int MishnahNumber { get; set; }
    public string Perek { get; set; }
    public string Mesechtah { get; set; }
    public bool IsStarted { get; set; } = false;
    #endregion

    protected override void OnStartup(StartupEventArgs e) {
      // Listen to notification activation
      ToastNotificationManagerCompat.OnActivated += toastArgs => {
        // Obtain the arguments from the notification
        ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

        // Obtain any user input (text boxes, menu selections) from the notification
        ValueSet userInput = toastArgs.UserInput;

        // Need to dispatch to UI thread if performing UI operations
        Current.Dispatcher.Invoke(delegate {
          // TODO: Show the corresponding content
          MessageBox.Show("Toast activated. Args: " + toastArgs.Argument);

          // TODO: In this window show the Mishnah
          new MainWindow().ShowDialog();
        });
      };

      if (!IsStarted) {
        base.OnStartup(e);
        //create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
        notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

        StyleManager.ApplicationTheme = new FluentTheme();
        FluentPalette.LoadPreset(FluentPalette.ColorVariation.Dark);
        ThemeEffectsHelper.IsAcrylicEnabled = false;

        Start();
      }
    }

    public void Start() {
      if (!Directory.Exists(App_Folder)) {
        Directory.CreateDirectory(App_Folder);
      }

      ReadFromXml();
      RunTimer();

      IsStarted = true;
    }

    private void ReadFromXml() {
      bool success;
      Exception exception = new();

      try {
        if (File.Exists(Path.Combine(App_Folder, "Settings.xml"))) {
          System.Xml.XmlDocument readFile = new();
          readFile.Load(Path.Combine(App_Folder, "Settings.xml"));

          System.Xml.XmlNode startupNode = readFile.SelectSingleNode("/Settings/Startup");
          RunsOnStartup = startupNode.InnerText == "True";

          System.Xml.XmlNode intervalNode = readFile.SelectSingleNode("/Settings/Interval");
          Interval = double.TryParse(intervalNode.InnerText, out double outInterval) ? outInterval : 30;

          System.Xml.XmlNode notifTextNode = readFile.SelectSingleNode("/Settings/NotifText");
        } else {
          RunsOnStartup = true;
          SplashScreen = true;
        }

        LastRead = DateTime.Now;

        success = true;
      } catch (Exception ex) {
        success = false;
        exception = ex;
      }

      if (!success) {
        RadWindow.Alert(new DialogParameters {
          Header = "A Mishnah A Day App - Error",
          Content = "Error reading the settings" + Environment.NewLine + exception.Message
        });
      }
    }

    private void RunTimer() {
      timer = new Timer {
        // [1 min = 60,000 | 5 min = 300,000 | 30 min = 1,800,000 | 1 hr = 3,600,000]
        Interval = Interval * 60000
      };
      timer.Start();
      timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
      // ISSUE: This will only get hit the next time the timer finishes it's elapsed time.
      // What if the user has chosen a smaller interval, say from 30 min down to 5,
      // how will the app know to change it until another 30 min passes and we hit the elapsed?
      // Settings need to raise an event which will be picked up here...
      CheckTimeStamps();
      PopTheToast();
    }

    private void CheckTimeStamps() {
      DateTime time = File.GetLastWriteTime(Path.Combine(App_Folder, "Settings.xml"));
      if (LastRead < time) {
        ReadFromXml();

        LastRead = DateTime.Now;
      }
    }

    public static void PopTheToast() =>
      new ToastContentBuilder()
          .AddArgument("action", "viewConversation")
          .AddArgument("conversationId", 9813)
          .AddText("Learn a Mishnah!")
          .AddText("Click to view the Mishnah of the day.")
          .AddAppLogoOverride(new Uri("file:///" + Path.GetFullPath("Data/AMishnahADay.png")), ToastGenericAppLogoCrop.Circle)
          .AddAttributionText("Via AMAD")
          .Show();

    protected override void OnExit(ExitEventArgs e) {
      // The icon would clean up automatically, but this is cleaner
      notifyIcon.Dispose();
      base.OnExit(e);
    }
  }
}
