using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no banco de dados
  /// </summary>
  public class Checkpoint : BaseEntity
  {
    public ViewListPersonInfo Person { get; set; }
    public ViewListOccupation Occupation { get; set; }
    public DateTime? DateBegin { get; set; }
    public DateTime? DateEnd { get; set; }
    public string Comments { get; set; }
    public string TextDefault { get; set; }
    public List<CheckpointQuestions> Questions { get; set; }
    public EnumStatusCheckpoint StatusCheckpoint { get; set; }
    public DateTime? DataAccess { get; set; }
    public EnumCheckpoint TypeCheckpoint { get; set; }
  }
}
