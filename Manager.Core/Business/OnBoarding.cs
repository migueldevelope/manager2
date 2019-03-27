using Manager.Core.Base;
using Manager.Core.BusinessModel;
using Manager.Views.Enumns;
using System;
using System.Collections.Generic;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class OnBoarding : BaseEntity
  {
    public Person Person { get; set; }
    public DateTime? DateBeginPerson { get; set; }
    public DateTime? DateBeginManager { get; set; }
    public DateTime? DateBeginEnd { get; set; }
    public DateTime? DateEndPerson { get; set; }
    public DateTime? DateEndManager { get; set; }
    public DateTime? DateEndEnd { get; set; }
    public string CommentsPerson { get; set; }
    public string CommentsManager { get; set; }
    public string CommentsEnd { get; set; }
    public List<OnBoardingSkills> SkillsCompany { get; set; }
    public List<OnBoardingSkills> SkillsGroup { get; set; }
    public List<OnBoardingSkills> SkillsOccupation { get; set; }
    public List<OnBoardingScope> Scopes { get; set; }
    public List<OnBoardingSchooling> Schoolings { get; set; }
    public List<OnBoardingActivities> Activities { get; set; }
    public EnumStatusOnBoarding StatusOnBoarding { get; set; }
  }
}
