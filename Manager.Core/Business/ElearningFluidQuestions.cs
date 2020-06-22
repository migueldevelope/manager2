using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using System;

namespace Manager.Core.Business
{
  public class ElearningFluidQuestions : BaseEntity
  {
    public string Theme { get; set; }
    public string Question { get; set; }
    public string ChoiceA { get; set; }
    public string ChoiceB { get; set; }
    public string ChoiceC { get; set; }
    public string Correct { get; set; }

    public ViewCrudElearningFluidQuestions GetViewCrud()
    {
      return new ViewCrudElearningFluidQuestions()
      {
        _id = _id,
        Theme  = Theme,
        Question = Question,
        ChoiceA = ChoiceA,
        ChoiceB = ChoiceB,
        ChoiceC = ChoiceC,
        Correct= Correct

      };
    }

    public ViewListElearningFluidQuestions GetViewList()
    {
      return new ViewListElearningFluidQuestions()
      {
        _id = _id,
        Question = Question
      };
    }

  }
}
