using AMishnahADay.Models;
using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace AMishnahADay.ViewModels {
  public class MainWindowViewModel : ViewModelBase {
    private readonly AppDbContext context = new();
    public MainWindowViewModel() {
      _ = LoadData();
      LoadCommands();
    }

    private void LoadCommands() {
      NextCommand = new RelayCommand(NextCommandExecute);
      PreviousCommand = new RelayCommand(PreviousCommandExecute);
    }

    public async Task LoadData() {
      DarkMode = context.Settings.SingleOrDefault().DarkMode;
      DarkModeToolTip = context.Settings.SingleOrDefault().DarkMode ? "Switch to light mode" : "Switch to dark mode";
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
      EnglishText = Mishnah.EnglishText;
      HebrewText = Mishnah.HebrewText;
    }

    private async Task SetMishnah(Masechtah masechtah = null, Perek perek = null, Mishnah mishnah = null) {
      if (masechtah != null) {
        Perakim = await context.Perakim.Where(p => p.Masechtah == masechtah).ToListAsync();
        Perek = masechtah.Perekim.FirstOrDefault();
        Mishnayos = await context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
        Mishnah = masechtah.Perekim.FirstOrDefault().Mishnayos.FirstOrDefault();
        EnglishText = Mishnah.EnglishText;
        HebrewText = Mishnah.HebrewText;
      }
      if (perek != null) {
        Mishnayos = await context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
        Mishnah = perek.Mishnayos.FirstOrDefault();
        EnglishText = Mishnah.EnglishText;
        HebrewText = Mishnah.HebrewText;
      }
      if (mishnah != null) {
        Masechtah = await context.Masechtahs.FirstOrDefaultAsync(m => m == mishnah.Masechtah);
        Perakim = await context.Perakim.Where(p => p.Masechtah == Masechtah).ToListAsync();
        Perek = mishnah.Perek;
        Mishnayos = await context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
        EnglishText = Mishnah.EnglishText;
        HebrewText = Mishnah.HebrewText;
      }
    }

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
          _ = SetMishnah(null, null, value);
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region EnglishText
    private string _EnglishText = "";
    public string EnglishText {
      get => _EnglishText;
      set {
        if (_EnglishText != value) {
          _EnglishText = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region HebrewText
    private string _HebrewText = "";
    public string HebrewText {
      get => _HebrewText;
      set {
        if (_HebrewText != value) {
          _HebrewText = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion

    #region NextCommand
    public RelayCommand NextCommand { get; set; }

    private void NextCommandExecute() =>
      NextOrPreviousMishnah(1);

    #endregion

    private void NextOrPreviousMishnah(int number) {
      if (Mishnah.ID == 1 && number == -1) {
        Mishnah = context.Mishnayos
          .Include(m => m.Masechtah)
          .Include(m => m.Perek)
          .SingleOrDefault(m => m.ID == 4192);
      } else if (Mishnah.ID == 4192 && number == 1) {
        Mishnah = context.Mishnayos
          .Include(m => m.Masechtah)
          .Include(m => m.Perek)
          .SingleOrDefault(m => m.ID == 1);
      } else {
        Mishnah = context.Mishnayos
          .Include(m => m.Masechtah)
          .Include(m => m.Perek)
          .SingleOrDefault(m => m.ID == Mishnah.ID + number);
      }
    }

    #region PreviousCommand
    public RelayCommand PreviousCommand { get; set; }

    private void PreviousCommandExecute() =>
      NextOrPreviousMishnah(-1);

    #endregion

    #region Save
    public async Task Save() =>
      await Task.Run(() => {
        Settings settings = context.Settings.SingleOrDefault();
        // TODO: If not keeping for tomorrow then add 1 to show the next mishnah...
        settings.MishnahID = Mishnah.ID;
        context.Update(settings);
        context.SaveChanges();
      });
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
    private bool _DarkMode;
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
