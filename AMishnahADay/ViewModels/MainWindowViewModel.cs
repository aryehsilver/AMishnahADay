using AMishnahADay.Models.Models;

namespace AMishnahADay.ViewModels;

public class MainWindowViewModel : ViewModelBase {
  private AppDbContext _context;

  public MainWindowViewModel() =>
    _ = LoadData();

  public async Task LoadData() {
    _context = new();
    DarkMode = _context.Settings.SingleOrDefault().DarkMode;
    DarkModeToolTip = _context.Settings.SingleOrDefault().DarkMode ? "Switch to light mode" : "Switch to dark mode";
    Mishnah = _context.Settings
      .Include(s => s.Mishnah)
        .ThenInclude(m => m.Perek)
      .Include(s => s.Mishnah)
        .ThenInclude(m => m.Masechtah)
      .SingleOrDefault().Mishnah;
    Masechtas = await _context.Masechtahs.ToListAsync();
    Perakim = await _context.Perakim.Where(p => p.Masechtah == Mishnah.Masechtah).ToListAsync();
    Mishnayos = await _context.Mishnayos.Where(m => m.Perek == Mishnah.Perek).ToListAsync();
    Masechtah = Mishnah.Masechtah;
    Perek = Mishnah.Perek;
    EnglishText = Mishnah.EnglishText;
    HebrewText = Mishnah.HebrewText;
  }

  private async Task SetMishnah(Masechtah masechtah = null, Perek perek = null, Mishnah mishnah = null) {
    if (masechtah != null) {
      Perakim = await _context.Perakim.Where(p => p.Masechtah == masechtah).ToListAsync();
      Perek = masechtah.Perekim.FirstOrDefault();
      Mishnayos = await _context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
      Mishnah = masechtah.Perekim.FirstOrDefault().Mishnayos.FirstOrDefault();
      EnglishText = Mishnah.EnglishText;
      HebrewText = Mishnah.HebrewText;
    }
    if (perek != null) {
      Mishnayos = await _context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
      Mishnah = perek.Mishnayos.FirstOrDefault();
      EnglishText = Mishnah.EnglishText;
      HebrewText = Mishnah.HebrewText;
    }
    if (mishnah != null) {
      Masechtah = await _context.Masechtahs.FirstOrDefaultAsync(m => m == mishnah.Masechtah);
      Perakim = await _context.Perakim.Where(p => p.Masechtah == Masechtah).ToListAsync();
      Perek = mishnah.Perek;
      Mishnayos = await _context.Mishnayos.Where(m => m.Perek == Perek).ToListAsync();
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

  #region Next or Previous Mishnah

  public void NextOrPreviousMishnah(int number) =>
    Mishnah = Mishnah.ID == 1 && number == -1
        ? _context.Mishnayos
              .Include(m => m.Masechtah)
              .Include(m => m.Perek)
              .SingleOrDefault(m => m.ID == 4192)
        : Mishnah.ID == 4192 && number == 1
          ? _context.Mishnayos
                .Include(m => m.Masechtah)
                .Include(m => m.Perek)
                .SingleOrDefault(m => m.ID == 1)
          : _context.Mishnayos
                .Include(m => m.Masechtah)
                .Include(m => m.Perek)
                .SingleOrDefault(m => m.ID == Mishnah.ID + number);

  #endregion

  #region Save
  public async Task Save() =>
    await Task.Run(() => {
      Settings settings = _context.Settings.SingleOrDefault();
      settings.MishnahID = Mishnah.ID;
      _context.Update(settings);
      _context.SaveChanges();
    });
  #endregion

  #region SetTheme

  public async Task SetTheme() {
    DarkMode = !DarkMode;
    DarkModeToolTip = DarkMode ? "Switch to light mode" : "Switch to dark mode";
    Windows11Palette.LoadPreset(DarkMode ? Windows11Palette.ColorVariation.Dark : Windows11Palette.ColorVariation.Light);
    _context.Settings.SingleOrDefault().DarkMode = DarkMode;
    await _context.SaveChangesAsync();
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
