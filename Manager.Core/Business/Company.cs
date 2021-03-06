﻿using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Company : BaseEntity
  {
    public string Name { get; set; }
    public string Logo { get; set; }
    public List<ViewListSkill> Skills { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string Template  { get; set; }
    public ViewListCompany GetViewList()
    {
      return new ViewListCompany()
      {
        _id = _id,
        Name = Name
      };
    }
    public ViewCrudCompany GetViewCrud()
    {
      return new ViewCrudCompany()
      {
        _id = _id,
        Name = Name,
        Logo = Logo
      };
    }
  }
}
