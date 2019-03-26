using Manager.Core.Base;
using Manager.Views.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - treinamentos obrigatórios do cargo
  /// </summary>
  public class OccupationMandatory : BaseEntity
  {
    public Course Course { get; set; }
    public Occupation Occupation { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
