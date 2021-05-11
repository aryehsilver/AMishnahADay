using System.Collections.Generic;

namespace AMishnahADay.Models {
  public class Perek {
    public int ID { get; set; }
    public int PerekNumber { get; set; }
    public int MasechtahID { get; set; }
    public Masechtah Masechtah { get; set; }
    public List<Mishnah> Mishnayos { get; set; }
  }
}
