using Manager.Core.Base;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business
{
  public class SalaryScaleScore: BaseEntity
  {
    public EnumSteps Step { get; set; }
    public byte Ranking { get; set; }
    public decimal Value { get; set; }
  }
}
