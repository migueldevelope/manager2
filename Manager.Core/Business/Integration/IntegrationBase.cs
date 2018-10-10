using Manager.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Core.Business.Integration
{
  public class IntegrationBase : BaseEntity
  {
    public string Code { get; set; }
    public string Name { get; set; }
    public string _idCompany { get; set; }
  }
}
