using Manager.Core.Base;
using Manager.Core.Enumns;
using System;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados - treinamentos obrigatórios por funcionário
  /// </summary>
  public class PersonMandatory : BaseEntity
  {
    public Course Course { get; set; }
    public Person Person { get; set; }
    public DateTime? BeginDate { get; set; }
    public EnumTypeMandatoryTraining TypeMandatoryTraining { get; set; }
  }
}
