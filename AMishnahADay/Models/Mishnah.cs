namespace AMishnahADay.Models {
  public class Mishnah {
    public int ID { get; set; }
    public int MishnahNumber { get; set; }
    public string HebrewText { get; set; }
    public string EnglishText { get; set; }
    public int PerekID { get; set; }
    public Perek Perek { get; set; }
    public int MasechtahID { get; set; }
    public Masechtah Masechtah { get; set; }
  }
}
