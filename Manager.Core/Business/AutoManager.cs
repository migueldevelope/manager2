using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados para auto gestão
  /// </summary>
  public class AutoManager : BaseEntity
  {
    public Person Person { get; set; }
    public Person Requestor { get; set; }
    public List<Workflow> Workflow { get; set; }
    public EnumStatusAutoManager StatusAutoManager { get; set; }
    public DateTime? OpenDate { get; set; }
  }
}
