using Manager.Core.Base;
using Manager.Core.BaseV2;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.BusinessV2
{
  public class PersonV2 : BasePrivate
  {
    [BsonRepresentation(BsonType.ObjectId)]
#pragma warning disable IDE1006 // Estilos de Nomenclatura
    public string _idUser { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idAuth { get; set; }
#pragma warning restore IDE1006 // Estilos de Nomenclatura
    public string Name { get; set; }
    public string Document { get; set; }
    public string Mail { get; set; }
    public ViewListSchooling Schooling { get; set; }
    public bool Active { get; set; }
    public EnumStatusUser StatusUser { get; set; }
    public bool UserAdmin { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public ViewListCompany Company { get; set; }
    public ViewListEstablishment Establishment { get; set; }
    public string Registration { get; set; }
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime? DateAdm { get; set; }
    [BsonDateTimeOptions(DateOnly = true)] 
    public DateTime? DateResignation { get; set; }
    public BaseFields Manager { get; set; }
    public ViewListOccupationResume Occupation { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public int Workload { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public SalaryScalePerson SalaryScales { get; set; }
    public bool ShowSalary { get; set; }
  }
}
