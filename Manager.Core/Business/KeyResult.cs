using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  public class KeyResult : BaseEntity
  {
    public string Name { get; set; }
    public EnumTypeKeyResult TypeKeyResult { get; set; }
    public decimal QuantityGoal { get; set; }
    public string QualityGoal { get; set; }
    public decimal BeginProgressGoal { get; set; }
    public decimal EndProgressGoal { get; set; }
    public EnumSense Sense { get; set; }
    public string Description { get; set; }
    public byte Weight { get; set; }
    public ViewListObjective Objective { get; set; }
    public List<ViewCrudParticipantKeyResult> Participants { get; set; }
    public decimal Achievement { get; set; }
    public decimal QuantityResult { get; set; }
    public string QualityResult { get; set; }
    public bool Reached { get; set; }
    public EnumTypeCheckin TypeCheckin { get; set; }

    public ViewCrudKeyResult GetViewCrud()
    {
      return new ViewCrudKeyResult
      {
        _id = _id,
        Name = Name,
        TypeKeyResult = TypeKeyResult,
        QuantityGoal = QuantityGoal,
        QualityGoal = QualityGoal,
        BeginProgressGoal = BeginProgressGoal,
        EndProgressGoal = EndProgressGoal,
        Sense = Sense,
        Description = Description,
        Weight = Weight,
        Objective = Objective,
        TypeCheckin = TypeCheckin
      };
    }

    public ViewListKeyResult GetViewList()
    {
      return new ViewListKeyResult
      {
        _id = _id,
        Name = Name
      };
    }
  }
}
