using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - tabela salarial
  /// </summary>
  public class SalaryScale : BaseEntity
  {
    public ViewListCompany Company { get; set; }
    public string Name { get; set; }
    public List<Grade> Grades { get; set; }
    public DateTime? Date {get;set;}
    [BsonRepresentation(BsonType.ObjectId)]
    public string _idSalaryScalePrevious { get; set; }
  }
}
