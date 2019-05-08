using Manager.Core.Base;
using System;

namespace Manager.Core.Business
{
  public class TermsOfService : BaseEntity
  {
    public string Text { get; set; }
    public DateTime? Date { get; set; }
  }
}
