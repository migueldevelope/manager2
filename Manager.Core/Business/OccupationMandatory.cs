using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - treinamentos obrigatórios do cargo
  /// </summary>
  public class OccupationMandatory : BaseEntity
  {
    public ViewListCourse Course { get; set; }
    public ViewListOccupation Occupation { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
    public ViewCrudOccupationMandatory GetViewCrud()
    {
      return new ViewCrudOccupationMandatory()
      {
        BeginDate = BeginDate,
        Course = Course,
        Occupation = Occupation,
        TypeMandatoryTraining = TypeMandatoryTraining,
        Name = Occupation.Name,
        _id = _id
      };
    }
  }
}
