<Query Kind="Statements">
  <Connection>
    <ID>7c290e33-d337-48d4-b207-e171c4912b87</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>SynopticClaims</Database>
  </Connection>
  <Output>DataGrids</Output>
  <NuGetReference>HtmlAgilityPack</NuGetReference>
  <NuGetReference>Microsoft.AspNetCore.Mvc.NewtonsoftJson</NuGetReference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

int MasechtahID = 1;
int perekID = 1;
int mishnahID = 1;

foreach (var m in Enum.GetValues(typeof(Masechtas))) {
	Console.WriteLine(@"modelBuilder.Entity<Masechtah>().HasData(new Masechtah { ID = " + MasechtahID + ", Name = \"" + m.ToString() + "\" });");
	//Console.WriteLine(m.ToString());
	for (int i = 1; i < 31; i++) {
		try {
			WebClient client = new WebClient();
			string url = $"https://www.sefaria.org/api/texts/Mishnah_{m.ToString()}.{i}?lang=bi";
			string jsonStr = client.DownloadString(url);
			if (jsonStr.StartsWith("{\"error\": ")) {
				break;
				throw new Exception(jsonStr);
			}
			JObject jObject = JObject.Parse(jsonStr);
			Array eng = jObject["text"].ToArray();
			Array he = jObject["he"].ToArray();

			Console.WriteLine(@"modelBuilder.Entity<Perek>().HasData(new Perek { ID = " + perekID + ", PerekNumber = " + i + ", MasechtahID = " + MasechtahID + " });");
			//Console.WriteLine("Perek: " + i);

			int syncM = 0;
			foreach (var mishnah in eng) {
				Console.WriteLine(@"modelBuilder.Entity<Mishnah>().HasData(new Mishnah { ID = " + mishnahID + $", MishnahNumber = {syncM + 1}, HebrewText = @\"{he.GetValue(syncM)}\", EnglishText = @\"{mishnah}\", PerekID = " + perekID + ", MasechtahID = " + MasechtahID + " });");
				//Console.WriteLine("Mishnah: " + (syncM + 1));
				syncM++;
				mishnahID++;
			}
		} catch (Exception ex) {
			//Console.WriteLine(ex.Message);
		}
		perekID++;
	}

	Console.WriteLine(Environment.NewLine);
	MasechtahID++;
}

enum Masechtas {
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
