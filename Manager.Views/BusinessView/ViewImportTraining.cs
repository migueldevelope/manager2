using System;

namespace Manager.Views.BusinessView
{
  public class ViewImportTraining
  {
    public string Cpf { get; set; }
    public string NameCourse { get; set; }
    public string Content { get; set; }
    public byte Peridiocity { get; set; }
    public string NameEvent { get; set; }
    public decimal Workload { get; set; }
    public string NameEntity { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
  }
}
