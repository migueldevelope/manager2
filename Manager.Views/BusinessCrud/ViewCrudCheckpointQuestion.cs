using System.Collections.Generic;

namespace Manager.Views.BusinessCrud
{
  public class ViewCrudCheckpointQuestion : _ViewCrud
  {
    public ViewCrudQuestions Question { get; set; }
    public byte Mark { get; set; }
    public List<ViewCrudCheckpointQuestion> Itens { get; set; }
  }
}
