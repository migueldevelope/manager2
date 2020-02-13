using Manager.Views.BusinessCrud;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Views.BusinessView
{
  public class ViewMyAwareness
  {
    public string _idPerson { get; set; }
    public string NamePerson { get; set; }
    public DateTime? Date { get; set; }

    public string RealitySelfImage { get; set; }
    public string RealityWorker { get; set; }
    public string RealityPersonalRelationships { get; set; }
    public string RealityPersonalInterest { get; set; }
    public string RealityHealth { get; set; }
    public string RealityPurposeOfLife { get; set; }

    public string ImpedimentSelfImage { get; set; }
    public string ImpedimentWorker { get; set; }
    public string ImpedimentPersonalRelationships { get; set; }
    public string ImpedimentPersonalInterest { get; set; }
    public string ImpedimentHealth { get; set; }
    public string ImpedimentPurposeOfLife { get; set; }

    public string FutureVisionSelfImage { get; set; }
    public string FutureVisionWorker { get; set; }
    public string FutureVisionPersonalRelationships { get; set; }
    public string FutureVisionPersonalInterest { get; set; }
    public string FutureVisionHealth { get; set; }
    public string FutureVisionPurposeOfLife { get; set; }

    public string PlanningSelfImage { get; set; }
    public string PlanningWorker { get; set; }
    public string PlanningPersonalRelationships { get; set; }
    public string PlanningPersonalInterest { get; set; }
    public string PlanningHealth { get; set; }
    public string PlanningPurposeOfLife { get; set; }

  }
}
