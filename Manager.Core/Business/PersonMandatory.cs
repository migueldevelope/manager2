using Manager.Core.Base;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - treinamentos obrigatórios por funcionário
  /// </summary>
  public class PersonMandatory : BaseEntity
  {
    public ViewListCourse Course { get; set; }
    public ViewListPersonBase Person { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
    public ViewCrudPersonMandatory GetViewCrud()
    {
      return new ViewCrudPersonMandatory()
      {
        _id = _id,
       BeginDate = BeginDate,
       Course = Course,
       Name = Person.Name,
       Person = Person,
       TypeMandatoryTraining  = TypeMandatoryTraining
      };
    }
  }
}
