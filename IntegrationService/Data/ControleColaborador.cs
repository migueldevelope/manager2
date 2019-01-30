using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manager.Views.Enumns;

namespace IntegrationService.Data
{
  public class ControleColaborador
  {
    public string ChaveColaborador { get; set; }
    public ColaboradorImportar Colaborador { get; private set; }
    public ColaboradorImportar ColaboradorAnterior { get; private set; }
    public List<string> CamposAlterados { get; set; }
    public EnumColaboradorAcao Acao { get; set; }
    public EnumColaboradorSituacao Situacao { get; set; }
    public string Message { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }

    public ControleColaborador(ColaboradorImportar colaborador)
    {
      ChaveColaborador = colaborador.ChaveColaborador;
      Colaborador = colaborador;
      CamposAlterados = new List<string>();
      Acao = EnumColaboradorAcao.Insert;
      Situacao = EnumColaboradorSituacao.Pendent;
      Message = string.Empty;
    }

    public void SetColaborador(ColaboradorImportar colaborador)
    {
      Message = string.Empty;
      if (Situacao == EnumColaboradorSituacao.Atualized)
      {
        Acao = EnumColaboradorAcao.Update;
        Situacao = EnumColaboradorSituacao.Pendent;
        ColaboradorAnterior = Colaborador;
      }
      Colaborador = colaborador;
      if (string.IsNullOrEmpty(Message) && ColaboradorAnterior != null)
        TestarMudanca();
    }

    #region Testar mudança de propriedades
    public string TestarMudanca()
    {
      try
      {
        if (this != null && Colaborador != null)
        {
          var type = typeof(ColaboradorImportar);
          var unequalProperties =
              from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
              let selfValue = type.GetProperty(pi.Name).GetValue(ColaboradorAnterior, null)
              let toValue = type.GetProperty(pi.Name).GetValue(Colaborador, null)
              where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
              select new MudancaColaborador() { Campo = pi.Name, ValorAntigo = selfValue.ToString(), ValorNovo = toValue.ToString() };
          if (unequalProperties.Count() == 0)
          {
            CamposAlterados = new List<string>();
            Acao = EnumColaboradorAcao.Passed;
            Situacao = EnumColaboradorSituacao.Atualized;
          }
          else
          {
            foreach (var item in unequalProperties.ToList<MudancaColaborador>())
              CamposAlterados.Add(item.Campo);
          }
        }
        return string.Empty;
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }
    #endregion

  }
}
