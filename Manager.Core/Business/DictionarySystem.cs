using Manager.Core.Base;

namespace Manager.Core.Business
{
  /// <summary>
  /// Objeto persiste no sistema Dicionário para quem não quer utilizaro padrão do sistema
  /// </summary>
  public class DictionarySystem: BaseEntity
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
