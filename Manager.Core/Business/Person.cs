using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.Business
{
  public class Person : BaseEntity
  {
    public string Name { get; set; }
    public string Document { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public EnumTypeUser TypeUser { get; set; }
    public EnumStatusUser StatusUser { get; set; }
    public Company Company { get; set; }
    public Occupation Occupation { get; set; }
    public long Registration { get; set; }
    public Person Manager { get; set; }
    public DateTime? DateBirth { get; set; }
    public DateTime? DateAdm { get; set; }
    public string DocumentManager { get; set; }
    public Schooling Schooling { get; set; }
    //public Object._id Photo { get; set; }
    public string PhotoUrl { get; set; }
    public long Coins { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public string ForeignForgotPassword { get; set; }
    public EnumTypeJourney TypeJourney { get; set; }

  }
}
