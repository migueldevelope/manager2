using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Commons;
using Tools;
using Manager.Core.Base;
using Manager.Core.Interfaces;
using Manager.Views.Enumns;
using Tools.Data;

namespace EdeskIntegration.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  [Route("upload")]
  public class UploadController : Controller
  {
    private readonly ServiceGeneric<Attachments> service;
    private readonly IServicePerson personService;
    private readonly IServiceCompany companyService;
    private readonly IServicePlan planService;
    private readonly IServiceCertification certificationService;
    private readonly IServiceEvent eventService;
    private readonly IServiceUser serviceUser;
    private readonly IServiceBaseHelp serviceBaseHelp;
    private readonly IServiceSalaryScale serviceSalaryScale;
    private readonly IServiceRecommendation serviceRecommendation;
    private readonly DataContext context;
    private readonly string blobKey;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contextAccessor"></param>
    /// <param name="_companyService"></param>
    /// <param name="_personService"></param>
    /// <param name="_planService"></param>
    /// <param name="_serviceEvent"></param>
    /// <param name="_serviceCertification"></param>
    /// <param name="_serviceBaseHelp"></param>
    /// <param name="_serviceRecommendation"></param>
    /// <param name="_serviceSalaryScale"></param>
    /// <param name="_serviceUser"></param>
    public UploadController(IHttpContextAccessor contextAccessor, IServiceCompany _companyService, IServicePerson _personService, IServicePlan _planService,
      IServiceEvent _serviceEvent, IServiceCertification _serviceCertification, IServiceBaseHelp _serviceBaseHelp, IServiceRecommendation _serviceRecommendation,
      IServiceSalaryScale _serviceSalaryScale, IServiceUser _serviceUser)
    {
      BaseUser baseUser = new BaseUser();
      var user = contextAccessor.HttpContext.User;
      foreach (Claim ci in user.Claims)
      {
        if (ci.Type == ClaimTypes.Name)
          baseUser.NamePerson = ci.Value;

        if (ci.Type == ClaimTypes.Hash)
          baseUser._idAccount = ci.Value;

        if (ci.Type == ClaimTypes.Email)
          baseUser.Mail = ci.Value;

        if (ci.Type == ClaimTypes.NameIdentifier)
          baseUser.NameAccount = ci.Value;

        if (ci.Type == ClaimTypes.UserData)
          baseUser._idUser = ci.Value;
        if (ci.Type == ClaimTypes.Actor)
          baseUser._idPerson = ci.Value;

      }
      Config conn = XmlConnection.ReadVariablesSystem();
      context = new DataContext(conn.Server, conn.DataBase);
      blobKey = conn.BlobKey;
      service = new ServiceGeneric<Attachments>(context);
      service.User(contextAccessor);
      personService = _personService;
      companyService = _companyService;
      planService = _planService;
      service._user = baseUser;
      eventService = _serviceEvent;
      certificationService = _serviceCertification;
      serviceBaseHelp = _serviceBaseHelp;
      serviceRecommendation = _serviceRecommendation;
      serviceSalaryScale = _serviceSalaryScale;
      serviceUser = _serviceUser;

      serviceSalaryScale.SetUser(contextAccessor);
      certificationService.SetUser(contextAccessor);
      eventService.SetUser(baseUser);
      eventService.SetUser(contextAccessor);
      personService.SetUser(contextAccessor);
      companyService.SetUser(contextAccessor);
      planService.SetUser(contextAccessor);
      serviceBaseHelp.SetUser(contextAccessor);
      serviceRecommendation.SetUser(contextAccessor);
      serviceUser.SetUser(contextAccessor);
      serviceUser.SetUser(baseUser);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<ObjectResult> Post()
    {
      foreach (var file in HttpContext.Request.Form.Files)
      {
        var ext = Path.GetExtension(file.FileName).ToLower();
        if (ext == ".exe" || ext == ".msi" || ext == ".bat" || ext == ".jar")
          return BadRequest("Bad file type.");
      }
      List<Attachments> listAttachments = new List<Attachments>();
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
        }
        catch (Exception)
        {
          attachment.Saved = false;
          await service.Update(attachment, null);
          throw;
        }
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("link")]
    public async Task<string> PostLink()
    {
      foreach (var file in HttpContext.Request.Form.Files)
      {
        var ext = Path.GetExtension(file.FileName).ToLower();
        if (ext == ".exe" || ext == ".msi" || ext == ".bat" || ext == ".jar")
          return "Bad file type.";
      }
      List<Attachments> listAttachments = new List<Attachments>();
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
          if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
          {
            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
              PublicAccess = BlobContainerPublicAccessType.Blob
            });
          }
          CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}{1}", attachment._id.ToString(), attachment.Extension));
          blockBlob.Properties.ContentType = file.ContentType;
          await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
          return blockBlob.Uri.ToString();
        }
        catch (Exception)
        {
          attachment.Saved = false;
          await service.Update(attachment, null);
          throw;
        }
        //listAttachments.Add(attachment);
      }
      return "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcompany"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{idcompany}/logocompany")]
    public async Task<ObjectResult> PostLogo(string idcompany)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
          await service.Update(attachment, null);
          throw;
        }

        companyService.SetLogo(idcompany, url);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idbasehelp"></param>
    /// <returns></returns>
    [HttpPost("{idbasehelp}/basehelp")]
    public async Task<ObjectResult> PostBaseHelp(string idbasehelp)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
          await service.Update(attachment, null);
          throw;
        }

        serviceBaseHelp.SetAttachment(idbasehelp, url, attachment.LocalName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idsalaryscale"></param>
    /// <returns></returns>
    [HttpPost("{idsalaryscale}/salaryscale")]
    public string PostSalaryScale(string idsalaryscale)
    {
      foreach (var file in HttpContext.Request.Form.Files)
      {
        //var ext = Path.GetExtension(file.FileName).ToLower();
        //if (ext == ".exe" || ext == ".msi" || ext == ".bat" || ext == ".jar")
        if(file.FileName != "SALARYSCALE.xlsx")
          return "bad_file_type";
      }
      foreach (var file in HttpContext.Request.Form.Files)
      {
        var result = serviceSalaryScale.ImportSalaryScale(idsalaryscale, file.OpenReadStream());
        return result;
      }
      return "not_file";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idrecommendation"></param>
    /// <returns></returns>
    [HttpPost("{idrecommendation}/recommendation")]
    public async Task<ObjectResult> PostRecommendation(string idrecommendation)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
          await service.Update(attachment, null);
          throw;
        }

        serviceRecommendation.SetImage(idrecommendation, url, attachment.LocalName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idevent"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{idevent}/event")]
    public async Task<ObjectResult> PostEvent(string idevent)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
        catch (Exception e)
        {
          attachment.Saved = false;
          await service.Update(attachment, null);
          throw e;
        }

        eventService.SetAttachment(idevent, url, file.FileName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ideventhistoric"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{ideventhistoric}/eventhistoric")]
    public async Task<ObjectResult> PostEventHistoric(string ideventhistoric)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
        catch (Exception e)
        {
          attachment.Saved = false;
          await service.Update(attachment, null);
          throw e;
        }

        eventService.SetAttachmentHistoric(ideventhistoric, url, file.FileName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmonitoring"></param>
    /// <param name="idplan"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{idmonitoring}/plan/{idplan}")]
    public async Task<ObjectResult> PostPlan(string idmonitoring, string idplan)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
        catch (Exception e)
        {
          attachment.Saved = false;
          await service.Update(attachment, null);
          throw e;
        }

        planService.SetAttachment(idplan, idmonitoring, url, file.FileName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="idcertification"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("certification/{idcertification}")]
    public async Task<ObjectResult> PostCertification(string idcertification)
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
        catch (Exception e)
        {
          attachment.Saved = false;
          await service.Update(attachment, null);
          throw e;
        }

        certificationService.SetAttachment(idcertification, url, file.FileName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
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
        await this.service.InsertNewVersion(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
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
          await service.Update(attachment, null);
          throw;
        }

        serviceUser.SetPhoto(idperson, url);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    //[Authorize]
    //[HttpDelete("plan/{iddp}/delete/{id}")]
    //public async Task<ObjectResult> DeleteLogbbook(string id, string iddp)
    //{
    //  var attachment = this.service.GetAllNewVersion(p => p._id == id).FirstOrDefault();
    //  CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
    //  CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
    //  CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
    //  CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}.{1}", attachment._id.ToString(), attachment.Extension));
    //  await blockBlob.DeleteIfExistsAsync();
    //  attachment.Status = EnumStatus.Disabled;
    //  service.Update(attachment, null);
    //  planService.DeleteAttachment(iddp, id);

    //  return Ok("Attachment deleted!");
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ObjectResult> Delete(string id)
    {
      var attachment = this.service.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
      CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
      CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(service._user._idAccount);
      CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}.{1}", attachment._id.ToString(), attachment.Extension));
      await blockBlob.DeleteIfExistsAsync();
      attachment.Status = EnumStatus.Disabled;
      await service.Update(attachment, null);
      return Ok("Attachment deleted!");
    }
  }
}
