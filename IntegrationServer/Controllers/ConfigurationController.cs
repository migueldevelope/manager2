using Manager.Core.Business.Integration;
using Manager.Core.Enumns;
using Manager.Core.Interfaces;
using Manager.Views.Enumns;
using Manager.Views.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationServer.Controllers
{
  [Produces("application/json")]
  [Route("configuration")]
  public class ConfigurationController : Controller
  {
    private readonly IServiceIntegration service;

    public ConfigurationController(IServiceIntegration _service, IHttpContextAccessor contextAccessor)
    {
      try
      {
        service = _service;
        service.SetUser(contextAccessor);
      }
      catch (Exception)
      {
        throw;
      }
    }
    [Authorize]
    [HttpGet]
    public IActionResult GetParameter()
    {
      try
      {
        IntegrationParameter param = service.GetIntegrationParameter();
        if (param == null)
          return Ok(new ViewIntegrationParameter()
          {
            ConnectionString = string.Empty,
            CriticalError = string.Empty,
            CustomVersionExecution = string.Empty,
            FilePathLocal = string.Empty,
            SheetName = string.Empty,
            LastExecution = null,
            LinkPackCustom = string.Empty,
            LinkPackProgram = string.Empty,
            MachineIdentity = string.Empty,
            MessageAtualization = string.Empty,
            Process = EnumIntegrationProcess.Manual,
            Mode  = EnumIntegrationMode.DataBaseV1,
            ProgramVersionExecution = string.Empty,
            SqlCommand = string.Empty,
            StatusExecution = string.Empty,
            Type = EnumIntegrationType.Basic,
            UploadNextLog = false,
            VersionPackCustom = string.Empty,
            VersionPackProgram = string.Empty,
            _id = string.Empty
          });
        return Ok(new ViewIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          CustomVersionExecution = param.CustomVersionExecution,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          LinkPackCustom = param.LinkPackCustom,
          LinkPackProgram = param.LinkPackProgram,
          MachineIdentity = param.MachineIdentity,
          MessageAtualization = param.MessageAtualization,
          Process = param.Process,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Type = param.Type,
          UploadNextLog = param.UploadNextLog,
          VersionPackCustom = param.VersionPackCustom,
          VersionPackProgram = param.VersionPackProgram,
          _id = param._id
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    [Authorize]
    [HttpPost]
    [Route("mode")]
    public IActionResult SetParameterMode([FromBody]ViewIntegrationParameterMode view)
    {
      try
      {
        IntegrationParameter param = service.SetIntegrationParameter(view);
        return Ok(new ViewIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          CustomVersionExecution = param.CustomVersionExecution,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          LinkPackCustom = param.LinkPackCustom,
          LinkPackProgram = param.LinkPackProgram,
          MachineIdentity = param.MachineIdentity,
          MessageAtualization = param.MessageAtualization,
          Process = param.Process,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Type = param.Type,
          UploadNextLog = param.UploadNextLog,
          VersionPackCustom = param.VersionPackCustom,
          VersionPackProgram = param.VersionPackProgram,
          _id = param._id
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    [Authorize]
    [HttpPost]
    [Route("pack")]
    public IActionResult SetParameterPack([FromBody]ViewIntegrationParameterPack view)
    {
      try
      {
        IntegrationParameter param = service.SetIntegrationParameter(view);
        return Ok(new ViewIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          CustomVersionExecution = param.CustomVersionExecution,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          LinkPackCustom = param.LinkPackCustom,
          LinkPackProgram = param.LinkPackProgram,
          MachineIdentity = param.MachineIdentity,
          MessageAtualization = param.MessageAtualization,
          Process = param.Process,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Type = param.Type,
          UploadNextLog = param.UploadNextLog,
          VersionPackCustom = param.VersionPackCustom,
          VersionPackProgram = param.VersionPackProgram,
          _id = param._id
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpPost]
    [Route("execution")]
    public IActionResult SetParameterMode([FromBody]ViewIntegrationParameterExecution view)
    {
      try
      {
        IntegrationParameter param = service.SetIntegrationParameter(view);
        return Ok(new ViewIntegrationParameter()
        {
          ConnectionString = param.ConnectionString,
          CriticalError = param.CriticalError,
          CustomVersionExecution = param.CustomVersionExecution,
          FilePathLocal = param.FilePathLocal,
          SheetName = param.SheetName,
          LastExecution = param.LastExecution,
          LinkPackCustom = param.LinkPackCustom,
          LinkPackProgram = param.LinkPackProgram,
          MachineIdentity = param.MachineIdentity,
          MessageAtualization = param.MessageAtualization,
          Process = param.Process,
          Mode = param.Mode,
          ProgramVersionExecution = param.ProgramVersionExecution,
          SqlCommand = param.SqlCommand,
          StatusExecution = param.StatusExecution,
          Type = param.Type,
          UploadNextLog = param.UploadNextLog,
          VersionPackCustom = param.VersionPackCustom,
          VersionPackProgram = param.VersionPackProgram,
          _id = param._id
        });
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
