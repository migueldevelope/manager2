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
    [BsonIgnoreIfNull]
    public string Name { get; set; }
    [BsonIgnoreIfNull]
    public string Document { get; set; }
    public string Mail { get; set; }
    [BsonIgnoreIfNull]
    public string Phone { get; set; }
    [BsonIgnoreIfNull]
    public string Password { get; set; }
    [BsonIgnoreIfNull]
    public DateTime? DateBirth { get; set; }
    [BsonIgnoreIfNull]
    public DateTime? DateAdm { get; set; }
    public ViewListSchooling Schooling { get; set; }
    [BsonIgnoreIfNull]
    public string PhotoUrl { get; set; }
    public long Coins { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    [BsonIgnoreIfNull]
    public string ForeignForgotPassword { get; set; }
    [BsonIgnoreIfNull]
    public string PhoneFixed { get; set; }
    [BsonIgnoreIfNull]
    public string DocumentID { get; set; }
    [BsonIgnoreIfNull]
    public string DocumentCTPF { get; set; }
    public EnumSex Sex { get; set; }
    [BsonIgnoreIfNull]
    public string Nickname { get; set; }
    [BsonIgnoreIfNull]
    public bool UserAdmin { get; set; }
    [BsonIgnoreIfNull]
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
