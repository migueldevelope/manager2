using Manager.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  public class CertificationQuestions: BaseEntity
  {
    public Questions Question { get; set; }
    public string Answer { get; set; }
  }
}
