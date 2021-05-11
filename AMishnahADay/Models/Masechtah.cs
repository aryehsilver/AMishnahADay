using System.Collections.Generic;

namespace AMishnahADay.Models {
  public class Masechtah {
    public int ID { get; set; }
    public string Name { get; set; }
    public List<Perek> Perekim { get; set; }
    public Masechtas Masechet { get; set; }
  }

  public enum Masechtas {
    // Zeraim
    Berakhot = 1,
    Peah = 2,
    Demai = 3,
    Kilayim = 4,
    Sheviit = 5,
    Terumot = 6,
    Maasrot = 7,
    Maaser_Sheni = 8,
    Challah = 9,
    Orlah = 10,
    Bikkurim = 11,

    // Moed
    Shabbat = 12,
    Eruvin = 13,
    Pesachim = 14,
    Shekalim = 15,
    Yoma = 16,
    Sukkah = 17,
    Beitzah = 18,
    Rosh_Hashanah = 19,
    Taanit = 20,
    Megillah = 21,
    Moed_Katan = 22,
    Chagigah = 23,

    // Nashim
    Yevamot = 24,
    Ketubot = 25,
    Nedarim = 26,
    Nazir = 27,
    Sotah = 28,
    Gittin = 29,
    Kiddushin = 30,

    // Nezikin
    Bava_Kamma = 31,
    Bava_Metzia = 32,
    Bava_Batra = 33,
    Sanhedrin = 34,
    Makkot = 35,
    Shevuot = 36,
    Eduyot = 37,
    Avodah_Zarah = 38,
    Avot = 39,
    Horayot = 40,

    // Kodashim
    Zevachim = 41,
    Menachot = 42,
    Chullin = 43,
    Bekhorot = 44,
    Arakhin = 45,
    Temurah = 46,
    Keritot = 47,
    Meilah = 48,
    Tamid = 49,
    Middot = 50,
    Kinnim = 51,

    // Tahorot
    Kelim = 52,
    Oholot = 53,
    Negaim = 54,
    Parah = 55,
    Tahorot = 56,
    Mikvaot = 57,
    Niddah = 58,
    Makhshirin = 59,
    Zavim = 60,
    Tevul_Yom = 61,
    Yadayim = 62,
    Oktzin = 63
  }
}
