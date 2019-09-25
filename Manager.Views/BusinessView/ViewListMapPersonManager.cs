namespace Manager.Views.BusinessView
{
  public class ViewListMapPersonManager
  {
    public ViewListSubId _id { get; set; }
    public double value { get; set; }

    public class ViewListSubId
    {
      public string manager { get; set; }
      public string person { get; set; }
    }
  }
}
