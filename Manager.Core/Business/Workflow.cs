using Manager.Core.Base;
using Manager.Core.Enumns;
using MongoDB.Bson;
using System;

namespace Manager.Core.Business
{
  public class Workflow : BaseEntity
  {
    public Person Requestor { get; set; }
    public EnumWorkflow StatusWorkflow { get; set; }
    public string Commetns { get; set; }
    public DateTime? Date { get; set; }
    public int Sequence { get; set; }
  }
}
