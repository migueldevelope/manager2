using Manager.Views.BusinessList;

namespace Manager.Core.BusinessModel
{
  public class MyAwarenessQuestions
  {
    public string SelfImage { get; set; }
    public string Worker { get; set; }
    public string PersonalRelationships { get; set; }
    public string PersonalInterest { get; set; }
    public string Health { get; set; }
    public string PurposeOfLife { get; set; }
    public ViewListMyAwarenessQuestions GetViewList()
    {
      return new ViewListMyAwarenessQuestions()
      {
        SelfImage = SelfImage,
        Worker = Worker,
        PersonalRelationships = PersonalRelationships,
        PersonalInterest = PersonalInterest,
        Health = Health,
        PurposeOfLife = PurposeOfLife
      };
    }

  }
}
