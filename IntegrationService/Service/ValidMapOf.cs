﻿using IntegrationService.Api;
using IntegrationService.Core;
using Manager.Views.Integration;
using System;

namespace IntegrationService.Service
{
  public class ValidMapOf
  {
    public ViewIntegrationMapOfV1 Map { get; private set; }
    public ValidMapOf(ViewPersonLogin person, EnumValidKey key, string code, string name, string idCompany)
    {
      try
      {
        Map = new ViewIntegrationMapOfV1()
        {
          Name = name,
          Code = code,
          IdCompany = idCompany
        };
        PersonIntegration personIntegration = new PersonIntegration(person);
        Map = personIntegration.GetByName(key, Map);
      }
      catch (Exception)
      {

      }
    }
  }
}
