using Manager.Core.Base;
using Manager.Views.Enumns;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class PersonOld : BaseEntity
  {
    public EnumStatusUser StatusUser { get; set; }
    public Company Company { get; set; }
    public Occupation Occupation { get; set; }
    public PersonOld Manager { get; set; }
    public string DocumentManager { get; set; }
    public DateTime? DateLastOccupation { get; set; }
    public decimal Salary { get; set; }
    public DateTime? DateLastReadjust { get; set; }
    public DateTime? DateResignation { get; set; }
    [BsonIgnore]
    public decimal SalaryScale { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }
    public Establishment Establishment { get; set; }
    public DateTime? HolidayReturn { get; set; }
    public string MotiveAside { get; set; }
    public string Name { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public User User { get; set; }


    public long Registration { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public Schooling Schooling { get; set; }
    public string PhotoUrl { get; set; }
    public long Coins { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public string ForeignForgotPassword { get; set; }
    public string PhoneFixed { get; set; }
    public string DocumentID { get; set; }
    public string DocumentCTPF { get; set; }
    public EnumSex Sex { get; set; }
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }

  }
}
