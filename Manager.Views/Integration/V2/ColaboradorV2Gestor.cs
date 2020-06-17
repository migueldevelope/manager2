using Manager.Views.Enumns;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2Gestor
  {
    public ColaboradorV2Base Colaborador { get; set; }
    public string IdColaborador { get; set; }
    public ColaboradorV2Base Gestor { get; set; }
    public string IdGestor { get; set; }
    public EnumTypeUser? TypeUserGestor { get; set; }
    public bool Erro { get; set; }
  }
}
