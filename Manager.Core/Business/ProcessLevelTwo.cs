using Manager.Core.Base;
using Manager.Views.BusinessList;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados e coleção
  /// </summary>
  public class ProcessLevelTwo : BaseEntity
  {
    public string Name { get; set; }
    public ProcessLevelOne ProcessLevelOne { get; set; }
    public string Comments { get; set; }
    public long Order { get; set; }
    public ViewListProcessLevelTwo GetViewList()
    {
      return new ViewListProcessLevelTwo()
      {
        _id = _id,
        Name = Name,
        Order = Order,
        ProcessLevelOne = ProcessLevelOne.GetViewList()
      };
    }
  }
}
