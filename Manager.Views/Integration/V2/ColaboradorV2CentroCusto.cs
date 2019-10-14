﻿using System;

namespace Manager.Views.Integration.V2
{
  public class ColaboradorV2CentroCusto : ColaboradorV2Base, IColaboradorV2
  {
    public string CentroCusto { get; set; }
    public string NomeCentroCusto { get; set; }
    public DateTime? DataTrocaCentroCusto { get; set; }
    // Chaves de cálculo interno
    public string ChaveCentroCusto => string.Format("{0};{1}", Empresa, CentroCusto);
  }
}