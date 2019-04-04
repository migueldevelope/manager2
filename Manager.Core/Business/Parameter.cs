using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  ///  Objeto persiste no banco de dados
  /// </summary>
  public class Parameter : BaseEntity
  {
    public string Name { get; set; }
    public string Content { get; set; }
    public string Key { get; set; }
    public string Help { get; set; }
  }

  /*
   * Parêmetros existentes
   * 
   * DeadlineAdm - Total de dias do contrato de experiência
   * mailcheckpoint - E-mail do RH para enviar aviso de Decisão de Efetivação | Checkpoint
   * servicemailmessage - Ativar monitoramento por e-mail - 0=Não, 1=Sim
   * typeregisterperson - Tipo do cadastro da pessoa - 0=Cadastro Normal, 1=Multi contratos
   * viewlo - Mostrar as linhas de oportunidade - 0=Esconde, 1=Mostra
   * 
   */
}
