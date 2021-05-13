using System;

namespace AMishnahADay.Models {
  public class Settings {
    public int ID { get; set; }
    public int MishnahID { get; set; }
    public Mishnah Mishnah { get; set; }
    public bool StartOnSystemStartup { get; set; }
    public DateTime TimeForToast { get; set; }
  }
}
