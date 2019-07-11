using Manager.Core.Base;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Workflow : BaseEntity
  {
    public ViewListPersonBaseManager Requestor { get; set; }
    public EnumWorkflow StatusWorkflow { get; set; }
    public string Commetns { get; set; }
    public DateTime? Date { get; set; }
    public int Sequence { get; set; }
  }
}
