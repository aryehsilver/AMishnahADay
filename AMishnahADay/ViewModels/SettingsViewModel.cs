using AMishnahADay.Models.Models;
using IWshRuntimeLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using File = System.IO.File;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace AMishnahADay.ViewModels {
  public class SettingsViewModel : ViewModelBase {
    private readonly AppDbContext context = new();
    private static readonly string Startup_Folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

    public SettingsViewModel() =>
      _ = LoadData();

    private async Task LoadData() {
      DarkMode = context.Settings.SingleOrDefault().DarkMode;
      DarkModeToolTip = context.Settings.SingleOrDefault().DarkMode ? "Switch to light mode" : "Switch to dark mode";
      Startup = context.Settings.SingleOrDefault().StartOnSystemStartup;
      TimeForToast = context.Settings.SingleOrDefault().TimeForToast;
      Mishnah = context.Settings
        .Include(s => s.Mishnah)
          .ThenInclude(m => m.Perek)
        .Include(s => s.Mishnah)
          .ThenInclude(m => m.Masechtah)
        .SingleOrDefault().Mishnah;
      Masechtas = await context.Masechtahs.ToListAsync();
      Perakim = await context.Perakim.Where(p => p.Masechtah == Mishnah.Masechtah).ToListAsync();
      Mishnayos = await context.Mishnayos.Where(m => m.Perek == Mishnah.Perek).ToListAsync();
      Masechtah = Mishnah.Masechtah;
      Perek = Mishnah.Perek;
    }

    private async Task SetMishnah(Masechtah masechtah = null, Perek perek = null) {
      if (masechtah != null) {
        Perakim = await context.Perakim.Where(p => p.Masechtah == masechtah).ToListAsync();
        Perek = masechtah.Perekim.FirstOrDefault();
        Mishnayos = await context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
        Mishnah = masechtah.Perekim.FirstOrDefault().Mishnayos.FirstOrDefault();
      }
      if (perek != null) {
        Mishnayos = await context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
        Mishnah = perek.Mishnayos.FirstOrDefault();
      }
    }

    #region Save
    public async Task Save() =>
      await Task.Run(() => {
        IsBusy = true;
        Settings settings = context.Settings.SingleOrDefault();
        settings.StartOnSystemStartup = Startup;
        settings.TimeForToast = TimeForToast;
        settings.Mishnah = Mishnah;
        context.Update(settings);
        context.SaveChanges();
        SetStartUp();
        IsBusy = false;
      });

    private void SetStartUp() {
      if (Startup) {
        WshShell wsh = new();
        IWshShortcut shortcut = wsh.CreateShortcut(Startup_Folder + "\\A Mishnah A Day.lnk") as IWshShortcut;
        shortcut.TargetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "A Mishnah A Day.exe");
        shortcut.Description = "Shortcut to 'A Mishnah A Day'";
        shortcut.Save();
      } else {
        File.Delete(Path.Combine(Startup_Folder, "A Mishnah A Day.lnk"));
      }
    }

    #endregion

    #region Startup
    private bool _Startup;
    public bool Startup {
      get => _Startup;
      set {
        if (_Startup != value) {
          _Startup = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region TimeForToast
    private DateTime _TimeForToast = new(1970, 1, 1, 14, 30, 0, DateTimeKind.Local);
    public DateTime TimeForToast {
      get => _TimeForToast;
      set {
        if (_TimeForToast != value) {
          _TimeForToast = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region Masechtas
    private List<Masechtah> _Masechtas = new();
    public List<Masechtah> Masechtas {
      get => _Masechtas;
      set {
        if (_Masechtas != value) {
          _Masechtas = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region Perakim
    private List<Perek> _Perakim = new();
    public List<Perek> Perakim {
      get => _Perakim;
      set {
        if (_Perakim != value) {
          _Perakim = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region Mishnayos
    private List<Mishnah> _Mishnayos = new();
    public List<Mishnah> Mishnayos {
      get => _Mishnayos;
      set {
        if (_Mishnayos != value) {
          _Mishnayos = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region Masechtah
    private Masechtah _Masechtah = new();
    public Masechtah Masechtah {
      get => _Masechtah;
      set {
        if (_Masechtah != value) {
          _Masechtah = value;
          _ = SetMishnah(value);
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region Perek
    private Perek _Perek = new();
    public Perek Perek {
      get => _Perek;
      set {
        if (_Perek != value) {
          _Perek = value;
          _ = SetMishnah(null, value);
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region Mishnah
    private Mishnah _Mishnah = null;
    public Mishnah Mishnah {
      get => _Mishnah;
      set {
        if (_Mishnah != value) {
          _Mishnah = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region IsBusy
    private bool _IsBusy = false;
    public bool IsBusy {
      get => _IsBusy;
      set {
        if (_IsBusy != value) {
          _IsBusy = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region SetTheme

    public async Task SetTheme() {
      DarkMode = !DarkMode;
      DarkModeToolTip = DarkMode ? "Switch to light mode" : "Switch to dark mode";
      FluentPalette.LoadPreset(DarkMode ? FluentPalette.ColorVariation.Dark : FluentPalette.ColorVariation.Light);
      context.Settings.SingleOrDefault().DarkMode = DarkMode;
      await context.SaveChangesAsync();
    }

    #endregion

    #region DarkMode
    private bool _DarkMode = false;
    public bool DarkMode {
      get => _DarkMode;
      set {
        if (_DarkMode != value) {
          _DarkMode = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region DarkModeToolTip
    private string _DarkModeToolTip = "Switch to dark mode";
    public string DarkModeToolTip {
      get => _DarkModeToolTip;
      set {
        if (_DarkModeToolTip != value) {
          _DarkModeToolTip = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion
  }
}
