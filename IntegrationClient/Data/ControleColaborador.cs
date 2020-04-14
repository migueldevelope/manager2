using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manager.Views.Enumns;
using Manager.Views.Integration;

namespace IntegrationClient.Data
{
  public class ControleColaborador
  {
    public string ChaveColaborador { get; set; }
    public ColaboradorImportar Colaborador { get; private set; }
    public ColaboradorImportar ColaboradorAnterior { get; private set; }
    public List<string> CamposAlterados { get; set; }
    public EnumColaboradorSituacao Situacao { get; set; }
    public string Message { get; set; }
    public string IdPerson { get; set; }
    public string IdContract { get; set; }

    #region Constructor Colaborador Novo
    public ControleColaborador(ColaboradorImportar colaborador)
    {
      ChaveColaborador = colaborador.ChaveColaborador;
      Colaborador = colaborador;
      CamposAlterados = new List<string>();
      Situacao = EnumColaboradorSituacao.SendServer;
      Message = string.Empty;
    }
    #endregion

    #region Atualizar Colaborador
    public void SetColaborador(ColaboradorImportar colaborador)
    {
      if (Situacao == EnumColaboradorSituacao.Atualized)
      {
        Message = string.Empty;
        Situacao = EnumColaboradorSituacao.NoChange;
        ColaboradorAnterior = Colaborador;
        Colaborador = colaborador;
        if (string.IsNullOrEmpty(Message) && ColaboradorAnterior != null)
          TestarMudanca();
      }
      else
      {
        Colaborador = colaborador;
        Situacao = EnumColaboradorSituacao.SendServer;
      }
    }
    #endregion

    #region Testar mudança de propriedades
    public void TestarMudanca()
    {
      try
      {
        CamposAlterados = new List<string>();
        if (this != null && Colaborador != null)
        {
          var type = typeof(ColaboradorImportar);
          var unequalProperties =
              from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
              let selfValue = type.GetProperty(pi.Name).GetValue(ColaboradorAnterior, null)
              let toValue = type.GetProperty(pi.Name).GetValue(Colaborador, null)
              where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
              select new ViewColaboradorMudanca() { Campo = pi.Name, ValorAntigo = selfValue.ToString(), ValorNovo = toValue.ToString() };
          if (unequalProperties.Count() != 0)
          {
            Situacao = EnumColaboradorSituacao.SendServer;
            foreach (var item in unequalProperties.ToList())
              CamposAlterados.Add(item.Campo);
          }
        }
      }
      catch (Exception)
      {
        
      }
    }
    #endregion

  }
}
