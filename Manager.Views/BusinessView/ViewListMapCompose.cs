namespace Manager.Views.BusinessView
{
  public class ViewListMapCompose
  {

    public ViewListSubId _id { get; set; }
    public double value { get; set; }

    public class ViewListSubId
    {
      public string manager { get; set; }
      //0 - comments, 1 - plans, 2 - praise
      public double type { get; set; }
    }
  }
}
