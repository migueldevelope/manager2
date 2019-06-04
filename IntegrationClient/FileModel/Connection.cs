using System;

namespace IntegrationClient.FileModel
{
  [Serializable]
  class Connection
  {
    public string BancoDados { get; set; }
    public string ConnectionString { get; set; }
  }
}
