﻿using System;
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
using MongoDB.Bson;
using Manager.Core.Business;
using Manager.Data;
using Manager.Services.Specific;
using Manager.Services.Commons;
using Manager.Core;
using Manager.Core.Enumns;
using Tools;
using Manager.Core.Base;
using Manager.Core.Interfaces;

namespace EdeskIntegration.Controllers
{
  [Route("upload")]
  public class UploadController : Controller
  {
    private readonly ServiceGeneric<Attachments> service;
    private readonly IServicePerson personService;
    private readonly IServiceCompany companyService;
    private readonly IServicePlan planService;
    private readonly DataContext context;
    private readonly ObjectId account;
    private readonly string blobKey;

    public UploadController(IHttpContextAccessor contextAccessor, IServiceCompany _companyService, IServicePerson _personService, IServicePlan _planService)
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
          baseUser._idPerson = ci.Value;

      }
      var conn = ConnectionNoSqlService.GetConnetionServer();
      context = new DataContext(conn.Server, conn.DataBase);
      blobKey = conn.BlobKey;
      service = new ServiceGeneric<Attachments>(context, baseUser);
      personService = _personService;
      companyService = _companyService;
      planService = _planService;
      personService.SetUser(contextAccessor);
      companyService.SetUser(contextAccessor);
      planService.SetUser(contextAccessor);
    }

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
        this.service.Insert(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
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
          service.Update(attachment, null);
          throw;
        }
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }
    

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
        this.service.Insert(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
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
          service.Update(attachment, null);
          throw;
        }

        companyService.SetLogo(idcompany, url);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }
    

    [Authorize]
    [HttpPost("{id}/plan")]
    public async Task<ObjectResult> PostPlan(string id)
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
        this.service.Insert(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
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
          service.Update(attachment, null);
          throw;
        }

        planService.SetAttachment(id, url, file.FileName, attachment._id);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }
    
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
        this.service.Insert(attachment);
        try
        {
          CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
          CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
          CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
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
          service.Update(attachment, null);
          throw;
        }

        personService.SetPhoto(idperson, url);
        listAttachments.Add(attachment);
      }
      return Ok(listAttachments);
    }

    //[Authorize]
    //[HttpDelete("plan/{iddp}/delete/{id}")]
    //public async Task<ObjectResult> DeleteLogbbook(string id, string iddp)
    //{
    //  var attachment = this.service.GetAll(p => p._id == id).FirstOrDefault();
    //  CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
    //  CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
    //  CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
    //  CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}.{1}", attachment._id.ToString(), attachment.Extension));
    //  await blockBlob.DeleteIfExistsAsync();
    //  attachment.Status = EnumStatus.Disabled;
    //  service.Update(attachment, null);
    //  planService.DeleteAttachment(iddp, id);

    //  return Ok("Attachment deleted!");
    //}

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ObjectResult> Delete(string id)
    {
      var attachment = this.service.GetAll(p => p._id == id).FirstOrDefault();
      CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobKey);
      CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
      CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(account.ToString());
      CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}.{1}", attachment._id.ToString(), attachment.Extension));
      await blockBlob.DeleteIfExistsAsync();
      attachment.Status = EnumStatus.Disabled;
      service.Update(attachment, null);
      return Ok("Attachment deleted!");
    }
  }
}