using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class User : BaseEntity
  {
    
    public string Name { get; set; }
    
    public string Document { get; set; }
    public string Mail { get; set; }
    
    public string Phone { get; set; }
    
    public string Password { get; set; }
    
    public DateTime? DateBirth { get; set; }
    
    public DateTime? DateAdm { get; set; }
    public ViewListSchooling Schooling { get; set; }
    
    public string PhotoUrl { get; set; }
    public long Coins { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    
    public string ForeignForgotPassword { get; set; }
    
    public string PhoneFixed { get; set; }
    
    public string DocumentID { get; set; }
    
    public string DocumentCTPF { get; set; }
    public EnumSex Sex { get; set; }
    public string Nickname { get; set; }
    
    public bool UserAdmin { get; set; }
    
    public List<UserTermOfService> UserTermOfServices { get; set; }
    public ViewCrudUser GetViewList()
    {
      return new ViewCrudUser()
      {
        _id = _id,
        Document = Document,
        Mail = Mail,
        Name = Name,
        Phone = Phone
      };
    }
    public ViewCrudUser GetViewCrud()
    {
      return new ViewCrudUser()
      {
        _id = _id,
        Document = Document,
        Mail = Mail,
        Name = Name,
        Phone = Phone,
        DateAdm = DateAdm,
        DateBirth = DateBirth,
        DocumentCTPF = DocumentCTPF,
        DocumentID = DocumentID,
        Nickname = Nickname,
        Password = Password,
        PhoneFixed = PhoneFixed,
        PhotoUrl = PhotoUrl,
        Schooling = Schooling,
        Sex = Sex
      };
    }
  }
}
