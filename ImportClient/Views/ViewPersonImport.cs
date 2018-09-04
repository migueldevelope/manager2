using System;

namespace ImportClient.Views
{
  public class ViewPersonImport
  {
    public string Name { get; set; }
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public byte TypeUser { get; set; }
    public byte StatusUser { get; set; }
    public string NameCompany { get; set; }
    public string NameOccupation { get; set; }
    public string NameOccupationGroup { get; set; }
    public long Registration { get; set; }
    public string NameManager { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public string DocumentManager { get; set; }
    public string NameArea { get; set; }
    public string NameSchooling { get; set; }
  }
}
