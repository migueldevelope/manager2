using Manager.Core.BaseV2;
using Manager.Core.BusinessModel;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.BusinessV2
{
#pragma warning disable IDE1006 // Estilos de Nomenclatura
  public class AuthV2 : BasePublic
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idUser { get; set; }
    public EnumTypeAuth TypeAuth { get; set; }
    public string Mail { get; set; }
    public string Document { get; set; }
    public string NickName { get; set; }
    public string Password { get; set; }
    public EnumChangePassword ChangePassword { get; set; }
    public string ForeignForgotPassword { get; set; }
    public List<UserTermOfService> UserTermOfServices { get; set; }
    public bool Active { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idPerson { get; set; }
  }
#pragma warning restore IDE1006 // Estilos de Nomenclatura
}
