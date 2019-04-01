using Manager.Core.Base;
using Manager.Core.Enumns;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Manager.Core.BusinessModel
{
  /// <summary>
  /// Coleção para acreditação porém também persiste no banco de dados
  /// </summary>
  public class CertificationPerson: BaseEntity
  {
    public ViewListPerson Person { get; set; }
    public string TextDefault { get; set; }
    public string TextDefaultEnd { get; set; }
    public EnumStatusCertificationPerson StatusCertificationPerson { get; set; }
    public string Comments { get; set; }
    public DateTime? DateApprovation { get; set; }

  }
}
