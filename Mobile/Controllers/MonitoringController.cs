using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NAudio.Wave;
using Tools;
using Tools.Data;

namespace Mobile.Controllers
{
  /// <summary>
  /// Controlador para acompanhamento
  /// </summary>
  [Produces("application/json")]
  [Route("monitoring")]
  public class MonitoringController : DefaultController
  {
    private readonly IServiceMonitoring service;
    private readonly ServiceGeneric<Attachments> serviceAttachment;
    private readonly DataContext context;
    private readonly string blobKey;

    #region Constructor
    /// <summary>
    /// Construtor do controlador
    /// </summary>
    /// <param name="_service">Serviço de acompanhamento</param>
    /// <param name="contextAccessor">Token de segurança</param>
    public MonitoringController(IServiceMonitoring _service, IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
      Config conn = XmlConnection.ReadVariablesSystem();
      context = new DataContext(conn.Server, conn.DataBase);
      blobKey = conn.BlobKey;
      serviceAttachment = new ServiceGeneric<Attachments>(context);

      service = _service;

      service.SetUser(contextAccessor);
      serviceAttachment.User(contextAccessor);
    }
    #endregion

    #region Monitoring
    /// <summary>
    /// Remover todos os monitoramentos de uma pessoa
    /// </summary>
    /// <param name="idperson">Identificador da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteall/{idperson}")]
    public async Task<string> RemoveAllMonitoring(string idperson)
    {
      return await Task.Run(() => service.RemoveAllMonitoring(idperson));
    }
    /// <summary>
    /// Exclusão de um monitoramento
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("delete/{idmonitoring}")]
    public async Task<string> RemoveMonitoring(string idmonitoring)
    {
      return await Task.Run(() => service.RemoveMonitoring(idmonitoring));
    }
    /// <summary>
    /// Exclusão do último monitoramento
    /// </summary>
    /// <param name="idperson">Identificação da pessoa</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletelast/{idperson}")]
    public async Task<string> RemoveLastMonitoring(string idperson)
    {
      return await Task.Run(() => service.RemoveLastMonitoring(idperson));
    }
    /// <summary>
    /// Exclusão de atividade do monitoramento
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <param name="idactivitie">Identificador da atividade</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("removemonitoringactivities/{idmonitoring}/{idactivitie}")]
    public async Task<string> RemoveMonitoringActivities(string idmonitoring, string idactivitie)
    {
      return await Task.Run(() => service.RemoveMonitoringActivities(idmonitoring, idactivitie));
    }
    /// <summary>
    /// Exclusão de compentário
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <param name="iditem">Identificador do item</param>
    /// <param name="idcomments">Identificador do comentário</param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deletecomments/{idmonitoring}/{iditem}/{idcomments}")]
    public async Task<string> DeleteComments(string idmonitoring, string iditem, string idcomments)
    {
      return await Task.Run(() => service.DeleteComments(idmonitoring, iditem, idcomments));
    }
    /// <summary>
    /// Alteração de comentário
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <param name="iditem">Identificador do item</param>
    /// <param name="usercomment">Tipo de usuário do comentário</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecommentsview/{idmonitoring}/{iditem}/{usercomment}")]
    public async Task<string> UpdateCommentsView(string idmonitoring, string iditem, EnumUserComment usercomment)
    {
      return await Task.Run(() => service.UpdateCommentsView(idmonitoring, iditem, usercomment));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="idmonitoring"></param>
    /// <param name="iditem"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addpraise/{idmonitoring}/{iditem}")]
    public async Task<string> AddPraise([FromBody]ViewText text, string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.AddPraise(idmonitoring, iditem, text, "mobile"));
    }
    /// <summary>
    /// Alteração do comentário
    /// </summary>
    /// <param name="idmonitoring">Identificador do monitoramento</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("validcomments/{idmonitoring}")]
    public async Task<bool> UpdateCommentsView(string idmonitoring)
    {
      return await Task.Run(() => service.ValidComments(idmonitoring));
    }
    /// <summary>
    /// Inclusão monitoring
    /// </summary>
    /// <param name="idperson">Identificador contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("new/{idperson}")]
    public async Task<ViewListMonitoring> NewMonitoring(string idperson)
    {
      return await Task.Run(() => service.NewMonitoring(idperson, "mobile"));
    }
    /// <summary>
    /// Atualiza informações monitogin
    /// </summary>
    /// <param name="monitoring">Objeto Crud</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("update")]
    public async Task<string> UpdateMonitoring([FromBody]ViewCrudMonitoring monitoring)
    {
      return await Task.Run(() => service.UpdateMonitoring(monitoring, "mobile"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmonitoring"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatestatus/{idmonitoring}/{status}")]
    public async Task<string> UpdateStatusMonitoring(string idmonitoring, EnumStatusMonitoring status)
    {
      return await Task.Run(() => service.UpdateStatusMonitoring(idmonitoring, status, "mobile"));
    }

    /// <summary>
    /// Lista monitoring finalizado para gestor
    /// </summary>
    /// <param name="idmanager">Identificador Gestor</param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listend/{idmanager}")]
    public async Task<List<ViewListMonitoring>> ListMonitoringsEnd(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsEnd(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Lista monitoring para exclusão
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getlistexclud")]
    public async Task<List<ViewListMonitoring>> GetListExclud(int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.GetListExclud(ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }
    /// <summary>
    /// Lista monitoring em andamento para gestor
    /// </summary>
    /// <param name="idmanager"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("list/{idmanager}")]
    public async Task<List<ViewListMonitoring>> ListMonitoringsWait(string idmanager, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsWait(idmanager, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }

    /// <summary>
    /// Lista monitoring em andamento para gestor
    /// </summary>
    /// <param name="persons"></param>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("v2/list")]
    public async Task<List<ViewListMonitoring>> ListMonitoringsWait_V2([FromBody]List<ViewListIdIndicators> persons, int count = 10, int page = 1, string filter = "")
    {
      long total = 0;
      var result = service.ListMonitoringsWait_V2(persons, ref total, filter, count, page);
      Response.Headers.Add("x-total-count", total.ToString());
      return await Task.Run(() => result);
    }


    /// <summary>
    /// Lista monitoring para pessoa
    /// </summary>
    /// <param name="idmanager">Identificador contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("personend/{idmanager}")]
    public async Task<List<ViewListMonitoring>> PersonMonitoringsEnd(string idmanager)
    {
      return await Task.Run(() => service.PersonMonitoringsEnd(idmanager));
    }
    /// <summary>
    /// Lista monitoring para pessoa
    /// </summary>
    /// <param name="idmanager">Identificador do contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("personwait/{idmanager}")]
    public async Task<ViewListMonitoring> PersonMonitoringsWait(string idmanager)
    {
      return await Task.Run(() => service.PersonMonitoringsWait(idmanager));
    }
    /// <summary>
    /// Lista skills
    /// </summary>
    /// <param name="idperson">Identificador do contrato</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getskills/{idperson}")]
    public async Task<List<ViewListSkill>> GetSkills(string idperson)
    {
      return await Task.Run(() => service.GetSkills(idperson));
    }
    /// <summary>
    /// Busca informações para editar entrega
    /// </summary>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="idactivitie">Identificador entrega</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("getmonitoringactivities/{idmonitoring}/{idactivitie}")]
    public async Task<ViewCrudMonitoringActivities> GetMonitoringActivities(string idmonitoring, string idactivitie)
    {
      return await Task.Run(() => service.GetMonitoringActivities(idmonitoring, idactivitie));
    }
    /// <summary>
    /// Atualiza entrega monitoring
    /// </summary>
    /// <param name="activitie">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatemonitoringactivities/{idmonitoring}")]
    public async Task<string> UpdateMonitoringActivities([FromBody]ViewCrudMonitoringActivities activitie, string idmonitoring)
    {
      return await Task.Run(() => service.UpdateMonitoringActivities(idmonitoring, activitie));
    }
    /// <summary>
    /// Adiciona um entrega no monitoring
    /// </summary>
    /// <param name="activitie">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addmonitoringactivities/{idmonitoring}")]
    public async Task<string> AddMonitoringActivities([FromBody] ViewCrudActivities activitie, string idmonitoring)
    {
      return await Task.Run(() => service.AddMonitoringActivities(idmonitoring, activitie));
    }
    /// <summary>
    /// Lista comentarios de um item do monitoring
    /// </summary>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador Item</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listcomments/{idmonitoring}/{iditem}")]
    public async Task<List<ViewCrudComment>> GetListComments(string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.GetListComments(idmonitoring, iditem));
    }

    /// <summary>
    /// Inclusão comentario
    /// </summary>
    /// <param name="comments">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador Monitoring</param>
    /// <param name="iditem">Identificador item monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addcomments/{idmonitoring}/{iditem}")]
    public async Task<List<ViewCrudComment>> AddComments([FromBody]ViewCrudComment comments, string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.AddComments(idmonitoring, iditem, comments, "mobile"));
    }
    /// <summary>
    /// Atualiza comentario item do monitoring
    /// </summary>
    /// <param name="comments">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador item do monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecomments/{idmonitoring}/{iditem}")]
    public async Task<List<ViewCrudComment>> UpdateComments([FromBody]ViewCrudComment comments, string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.UpdateComments(idmonitoring, iditem, comments));
    }
    /// <summary>
    /// Adiciona um plano
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador item</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("addplan/{idmonitoring}/{iditem}")]
    public async Task<List<ViewCrudPlan>> AddPlan([FromBody]ViewCrudPlan plan, string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.AddPlan(idmonitoring, iditem, plan, "mobile"));
    }
    /// <summary>
    /// Atualiza informações do plano dentro de um item do monitoring
    /// </summary>
    /// <param name="plan">Objeto Crud</param>
    /// <param name="idmonitoring">Identificador monitoring</param>
    /// <param name="iditem">Identificador item do monitoring</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updateplan/{idmonitoring}/{iditem}")]
    public async Task<List<ViewCrudPlan>> UpdatePlan([FromBody]ViewCrudPlan plan, string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.UpdatePlan(idmonitoring, iditem, plan));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmonitoring"></param>
    /// <param name="iditem"></param>
    /// <param name="idplan"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    [Route("deleteplan/{idmonitoring}/{iditem}/{idplan}")]
    public async Task<string> DeletePlan(string idmonitoring, string iditem, string idplan)
    {
      return await Task.Run(() => service.DeletePlan(idmonitoring, iditem, idplan));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="persons"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("exportmonitoring")]
    public async Task<List<ViewExportStatusMonitoringGeral>> ExportStatusMonitoring([FromBody] List<ViewListIdIndicators> persons)
    {
      return await Task.Run(() => service.ExportStatusMonitoring(persons));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [Route("exportmonitoringcomments")]
    public async Task<List<ViewExportMonitoringComments>> ExportMonitoringComments([FromBody] ViewFilterIdAndDate filter)
    {
      return await Task.Run(() => service.ExportMonitoringComments(filter));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idperson"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("exportmonitoring/{idperson}")]
    public async Task<List<ViewExportStatusMonitoring>> ExportStatusMonitoring(string idperson)
    {
      return await Task.Run(() => service.ExportStatusMonitoring(idperson));
    }

    #endregion

    #region Mobile

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmonitoring"></param>
    /// <param name="iditem"></param>
    /// <param name="typeuser"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("{idmonitoring}/speech/{iditem}/{typeuser}/monitoring")]
    public async Task<string> PostSpeechRecognitionMonitoring([FromBody]ViewTime time, string idmonitoring, string iditem, EnumUserComment typeuser)
    {
      try
      {
        foreach (var file in HttpContext.Request.Form.Files)
        {
          var ext = Path.GetExtension(file.FileName).ToLower();
          if (ext == ".exe" || ext == ".msi" || ext == ".bat" || ext == ".jar")
            return "Bad file type.";
        }

        List<Attachments> listAttachments = new List<Attachments>();
        var url = "";
        foreach (var file in HttpContext.Request.Form.Files)
        {
          Attachments attachment = new Attachments()
          {
            Extension = ".mp3",
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

            blockBlob.Properties.ContentType = "audio/mpeg";


            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            url = blockBlob.Uri.ToString();
          }
          catch (Exception e)
          {
            attachment.Saved = false;
            await serviceAttachment.Update(attachment, null);
            throw e;
          }
          service.AddCommentsSpeech(idmonitoring, iditem, url, typeuser, time.TotalTime, "mobile");
          var i = Task.Run(() => SendCommentsSpeech(idmonitoring, iditem, typeuser, url));
          listAttachments.Add(attachment);
        }
        return url;
      }
      catch (Exception e)
      {
        throw e;
      }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("get/{id}")]
    public async Task<ViewCrudMonitoringMobile> GetMonitoringMobile(string id)
    {
      return await Task.Run(() => service.GetMonitoringsMobile(id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="comments"></param>
    /// <param name="idonboarding"></param>
    /// <param name="usercomment"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [Route("updatecommentsgeral/{idonboarding}/{usercomment}")]
    public async Task<IActionResult> UpdateComments([FromBody]ViewCrudCommentEnd comments, string idonboarding, EnumUserComment usercomment)
    {
      return await Task.Run(() => Ok(service.UpdateCommentsEndMobile(idonboarding, usercomment, comments)));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idmonitoring"></param>
    /// <param name="iditem"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("listplan/{idmonitoring}/{iditem}")]
    public async Task<List<ViewCrudPlan>> ListPlansMobile(string idmonitoring, string iditem)
    {
      return await Task.Run(() => service.ListPlansMobile(idmonitoring, iditem));
    }

    #endregion

    #region audio

    private void SendCommentsSpeech(string idonboarding, string iditem, EnumUserComment user, string link)
    {
      try
      {

        var pathspeech = "http://10.0.0.16:5400/";
        service.UpdateCommentsSpeech(idonboarding, iditem, user, pathspeech, link);
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    private async void ConvertMp3ToWav(Stream _inPath_, string _outPath_)
    {
      try
      {
        using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
        {
          using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
          {
            WaveFileWriter.CreateWaveFile(_outPath_, pcm);
          }
        }
        Stream stream = new StreamReader(_outPath_).BaseStream;

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
        CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(_outPath_.Replace(".wav", ""));

        blockBlob.Properties.ContentType = "audio/wav";

        await blockBlob.UploadFromStreamAsync(stream);
        var url = blockBlob.Uri.ToString();
      }
      catch (Exception e)
      {
        throw e;
      }
    }
    #endregion
  }
}