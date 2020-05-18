using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Tools;
using Tools.Data;

namespace Mobile.Controllers
{
  /// <summary>
  /// Controlador do Onboarding
  /// </summary>
  [Produces("application/json")]
  [Route("panel")]
  public class PanelController : DefaultController
  {
    private readonly ServiceGeneric<Attachments> serviceAttachment;
    private readonly IServiceOnBoarding serviceOnborading;
    private readonly IServiceIndicators serviceIndicators;
    private readonly IServicePerson servicePerson;
    private readonly IServiceUser serviceUser;
    private readonly IServiceFeelingDay serviceFeelingDay;
    private readonly DataContext context;
    private readonly string blobKey;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_serviceOnborading">Serviço de Onboarding</param>
    /// <param name="_serviceIndicators">Serviço de Indicadores</param>
    /// <param name="_servicePerson">Serviço de Contratos</param>
    /// <param name="_serviceUser">Serviço de Usuários</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public PanelController(IServiceOnBoarding _serviceOnborading, IServiceIndicators _serviceIndicators,
      IServiceUser _serviceUser, IServicePerson _servicePerson, IServiceFeelingDay _serviceFeelingDay, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      Config conn = XmlConnection.ReadVariablesSystem();
      context = new DataContext(conn.Server, conn.DataBase);
      blobKey = conn.BlobKey;
      serviceAttachment = new ServiceGeneric<Attachments>(context);

      serviceOnborading = _serviceOnborading;
      serviceIndicators = _serviceIndicators;
      servicePerson = _servicePerson;
      serviceUser = _serviceUser;
      serviceFeelingDay = _serviceFeelingDay;
      serviceOnborading.SetUser(contextAccessor);
      serviceIndicators.SetUser(contextAccessor);
      servicePerson.SetUser(contextAccessor);
      serviceUser.SetUser(contextAccessor);
      serviceFeelingDay.SetUser(contextAccessor);
    }
    #endregion

    #region Panel

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <param name="idUser"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("alterpass/{idUser}")]
    public async Task<string> AlterPassword([FromBody]ViewAlterPass view, string idUser)
    {
      return await Task.Run(() => serviceUser.AlterPassword(view, idUser));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="feeling"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("new/{feeling}")]
    public async Task<IActionResult> Post(EnumFeeling feeling)
    {
      return await Task.Run(() => Ok(serviceFeelingDay.New(feeling, "mobile")));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getfeelingday")]
    public async Task<ViewCrudFeelingDay> GetFeelingDay()
    {
      return await Task.Run(() => serviceFeelingDay.GetFeelingDay());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getnotesperson/{idperson}")]
    public async Task<List<ViewIndicatorsNotes>> GetNotesPerson(string idperson)
    {
      return await Task.Run(() => serviceIndicators.GetNotesPerson(idperson));
    }

    /// <summary>
    /// Buscar informações da pessoa para alteração
    /// </summary>
    /// <param name="id">Identificador da pessoa</param>
    /// <returns>Objeto de alteração da pessoa</returns>
    [Authorize]
    [HttpGet]
    [Route("edit/{id}")]
    public async Task<ViewCrudPerson> Get(string id)
    {
      return await Task.Run(() => servicePerson.Get(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{idperson}/photoperson")]
    public async Task<ObjectResult> PostPhoto(string idperson)
    {
      foreach (var file in HttpContext.Request.Form.Files)
      {
        var ext = Path.GetExtension(file.FileName).ToLower();
        if (ext == ".exe" || ext == ".msi" || ext == ".bat" || ext == ".jar")
          return BadRequest("Bad file type.");
      }
      List<Attachments> listAttachments = new List<Attachments>();
      var url = "";
      foreach (var file in HttpContext.Request.Form.Files)
      {
        Attachments attachment = new Attachments()
        {
          Extension = Path.GetExtension(file.FileName).ToLower(),
          LocalName = file.FileName,
          Lenght = file.Length,
          Status = EnumStatus.Enabled,
          Saved = true
        };
        await this.serviceAttachment.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(serviceAttachment._user._idAccount);
          if (await cloudBlobContainer.CreateIfNotExistsAsync())
          {
            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
              PublicAccess = BlobContainerPublicAccessType.Blob
            });
          }
          CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}{1}", attachment._id.ToString(), attachment.Extension));
          blockBlob.Properties.ContentType = file.ContentType;
          await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
          url = blockBlob.Uri.ToString();
        }
        catch (Exception)
        {
          attachment.Saved = false;
          await serviceAttachment.Update(attachment, null);
          throw;
        }

        serviceUser.SetPhoto(idperson, url);
        listAttachments.Add(attachment);
      }
      return Ok(new { Link = url });
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getnotes/{idperson}")]
    public async Task<List<ViewIndicatorsNotes>> GetNotes(string idperson)
    {
      return await Task.Run(() => serviceIndicators.GetNotes(idperson));
    }


    #endregion
  }
}