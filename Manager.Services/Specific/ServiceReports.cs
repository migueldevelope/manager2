using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.BusinessModel;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.BusinessView;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Azure.ServiceBus;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 4014
    public class ServiceReports : Repository<Monitoring>, IServiceReports
    {
        private readonly ServiceGeneric<Monitoring> serviceMonitoring;
        private readonly ServiceGeneric<OnBoarding> serviceOnboarding;
        private readonly ServiceGeneric<Occupation> serviceOccupation;
        private readonly ServiceGeneric<Area> serviceArea;
        private readonly ServiceGeneric<Checkpoint> serviceCheckpoint;
        private readonly ServiceGeneric<Workflow> serviceWorkflow;
        private readonly ServiceGeneric<Person> servicePerson;
        private readonly ServiceGeneric<Plan> servicePlan;
        private readonly ServiceGeneric<SalaryScale> serviceSalaryScale;
        private readonly ServiceLog serviceLog;
        private readonly ServiceMailModel serviceMailModel;
        private readonly ServiceGeneric<MailLog> serviceMail;
        private readonly ServiceGeneric<Account> serviceAccount;
        private readonly ServiceGeneric<Certification> serviceCertification;
        private readonly ServiceGeneric<CertificationPerson> serviceCertificationPerson;
        private readonly ServiceGeneric<Recommendation> serviceRecommendation;
        private readonly ServiceGeneric<RecommendationPerson> serviceRecommendationPerson;
        private readonly ServiceGeneric<Parameter> serviceParameter;
        private readonly ServiceGeneric<Reports> serviceReport;
        private readonly ServiceGeneric<Event> serviceEvent;
        private readonly ServiceGeneric<EventHistoric> serviceEventHistoric;
        private readonly ServiceGeneric<Group> serviceGroup;
        private readonly ServiceGeneric<MyAwareness> serviceMyAwareness;
        private readonly IServicePerson serviceIPerson;

        public string path;
        private readonly IQueueClient queueClient;
        private readonly IQueueClient queueClientReturn;

        #region construtctor
        public ServiceReports(DataContext context, DataContext contextLog, string pathToken, IServicePerson _serviceIPerson, string serviceBusConnectionString)
          : base(context)
        {
            try
            {
                serviceMonitoring = new ServiceGeneric<Monitoring>(context);
                serviceOnboarding = new ServiceGeneric<OnBoarding>(context);
                servicePerson = new ServiceGeneric<Person>(context);
                servicePlan = new ServiceGeneric<Plan>(context);
                serviceCheckpoint = new ServiceGeneric<Checkpoint>(context);
                serviceLog = new ServiceLog(context, contextLog);
                serviceGroup = new ServiceGeneric<Group>(context);
                serviceMailModel = new ServiceMailModel(context);
                serviceMail = new ServiceGeneric<MailLog>(contextLog);
                serviceWorkflow = new ServiceGeneric<Workflow>(context);
                serviceAccount = new ServiceGeneric<Account>(context);
                serviceCertification = new ServiceGeneric<Certification>(context);
                serviceCertificationPerson = new ServiceGeneric<CertificationPerson>(context);
                serviceRecommendation = new ServiceGeneric<Recommendation>(context);
                serviceRecommendationPerson = new ServiceGeneric<RecommendationPerson>(context);
                serviceParameter = new ServiceGeneric<Parameter>(context);
                serviceSalaryScale = new ServiceGeneric<SalaryScale>(context);
                serviceOccupation = new ServiceGeneric<Occupation>(context);
                serviceArea = new ServiceGeneric<Area>(context);
                serviceReport = new ServiceGeneric<Reports>(context);
                serviceEvent = new ServiceGeneric<Event>(context);
                serviceEventHistoric = new ServiceGeneric<EventHistoric>(context);
                serviceMyAwareness = new ServiceGeneric<MyAwareness>(context);
                serviceIPerson = _serviceIPerson;
                queueClient = new QueueClient(serviceBusConnectionString, "reports");
                queueClientReturn = new QueueClient(serviceBusConnectionString, "reportsreturn");
                path = pathToken;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetUser(IHttpContextAccessor contextAccessor)
        {
            User(contextAccessor);
            servicePerson._user = _user;
            serviceOnboarding._user = _user;
            serviceMonitoring._user = _user;
            servicePlan._user = _user;
            serviceLog._user = _user;
            serviceMailModel._user = _user;
            serviceWorkflow._user = _user;
            serviceMail._user = _user;
            serviceReport._user = _user;
            serviceCertification._user = _user;
            serviceCheckpoint._user = _user;
            serviceRecommendation._user = _user;
            serviceRecommendationPerson._user = _user;
            serviceParameter._user = _user;
            serviceSalaryScale._user = _user;
            serviceOccupation._user = _user;
            serviceArea._user = _user;
            serviceMyAwareness._user = _user;
            serviceEvent._user = _user;
            serviceEventHistoric._user = _user;
            serviceGroup._user = _user;
            serviceCertificationPerson._user = _user;
            serviceIPerson.SetUser(_user);
        }

        public void SetUser(BaseUser baseUser)
        {
            _user = baseUser;
            servicePerson._user = _user;
            serviceOnboarding._user = _user;
            serviceMonitoring._user = _user;
            servicePlan._user = _user;
            serviceLog._user = _user;
            serviceMailModel._user = _user;
            serviceWorkflow._user = _user;
            serviceMail._user = _user;
            serviceReport._user = _user;
            serviceCertification._user = _user;
            serviceCheckpoint._user = _user;
            serviceRecommendation._user = _user;
            serviceRecommendationPerson._user = _user;
            serviceParameter._user = _user;
            serviceSalaryScale._user = _user;
            serviceMyAwareness._user = _user;
            serviceOccupation._user = _user;
            serviceArea._user = _user;
            serviceGroup._user = _user;
            serviceEvent._user = _user;
            serviceEventHistoric._user = _user;
            serviceCertificationPerson._user = _user;
            serviceIPerson.SetUser(_user);
        }
        #endregion


        #region reports

        public string ListMyAwareness(string idperson)
        {
            try
            {
                var data = new List<ViewMyAwareness>();
                var viewMA = new ViewMyAwareness();
                var result = serviceMyAwareness.GetAllNewVersion(p => p._idPerson == idperson).Result.LastOrDefault()?.GetViewCrud();

                if (result != null)
                {
                    viewMA._idPerson = result._idPerson;
                    viewMA.NamePerson = result.NamePerson;
                    viewMA.Date = result.Date == null ? "" : result.Date.Value.ToString("dd/MM/yyyy hh:mm");
                    viewMA.RealitySelfImage = result.Reality?.SelfImage;
                    viewMA.RealityWorker = result.Reality?.Worker;
                    viewMA.RealityPersonalRelationships = result.Reality?.PersonalRelationships;
                    viewMA.RealityPersonalInterest = result.Reality?.PersonalInterest;
                    viewMA.RealityHealth = result.Reality?.Health;
                    viewMA.RealityPurposeOfLife = result.Reality?.PurposeOfLife;
                    viewMA.ImpedimentSelfImage = result.Impediment?.SelfImage;
                    viewMA.ImpedimentWorker = result.Impediment?.Worker;
                    viewMA.ImpedimentPersonalRelationships = result.Impediment?.PersonalRelationships;
                    viewMA.ImpedimentPersonalInterest = result.Impediment?.PersonalInterest;
                    viewMA.ImpedimentHealth = result.Impediment?.Health;
                    viewMA.ImpedimentPurposeOfLife = result.Impediment?.PurposeOfLife;
                    viewMA.FutureVisionSelfImage = result.FutureVision?.SelfImage;
                    viewMA.FutureVisionWorker = result.FutureVision?.Worker;
                    viewMA.FutureVisionPersonalRelationships = result.FutureVision?.PersonalRelationships;
                    viewMA.FutureVisionPersonalInterest = result.FutureVision?.PersonalInterest;
                    viewMA.FutureVisionHealth = result.FutureVision?.Health;
                    viewMA.FutureVisionPurposeOfLife = result.FutureVision?.PurposeOfLife;
                    viewMA.PlanningSelfImage = result.Planning?.SelfImage;
                    viewMA.PlanningWorker = result.Planning?.Worker;
                    viewMA.PlanningPersonalRelationships = result.Planning?.PersonalRelationships;
                    viewMA.PlanningPersonalInterest = result.Planning?.PersonalInterest;
                    viewMA.PlanningHealth = result.Planning?.Health;
                    viewMA.PlanningPurposeOfLife = result.Planning?.PurposeOfLife;
                }

                data.Add(viewMA);

                var view = new ViewReport()
                {
                    Data = data,
                    Name = "listmyawareness",
                    _idReport = NewReport("listmyawareness"),
                    _idAccount = _user._idAccount
                };
                SendMessageAsync(view);
                var report = new ViewCrudReport();

                while (report.StatusReport == EnumStatusReport.Open)
                {
                    var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
                    report.StatusReport = rest.StatusReport;
                    report.Link = rest.Link;
                    //Thread.Sleep(1000);
                }

                return report.Link;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string ListOpportunityLine(string idcompany, string idarea)
        {
            try
            {
                var occupations = serviceOccupation.GetAllNewVersion(p => p.Group.Company._id == idcompany).Result;

                var list = new List<ViewListOpportunityLine>();

                foreach (var item in occupations)
                {
                    var group = serviceGroup.GetNewVersion(p => p._id == item.Group._id).Result;
                    foreach (var proc in item.Process)
                    {
                        var view = new ViewListOpportunityLine();
                        view.Occupation = item.Name;
                        view.Group = item.Group.Name;
                        view.LineGroup = item.Group.Line;
                        view.Shepre = group.Sphere.Name;
                        view.TypeShepre = group.Sphere.TypeSphere;
                        view.Axis = group.Axis.Name;
                        view.TypeAxis = group.Axis.TypeAxis;
                        int countarea = 1;
                        if (idarea != "")
                            if (idarea != proc.ProcessLevelOne?.Area?._id)
                                countarea = 0;

                        if (countarea > 0)
                        {
                            view.ProcessLevelOne = proc.ProcessLevelOne?.Name;
                            view.Area = proc.ProcessLevelOne?.Area?.Name;
                            view.ProcessLevelTwo = proc.Name;
                            list.Add(view);
                        }

                    }
                }

                var data = list.OrderBy(p => p.Area).ThenBy(p => p.TypeShepre).ThenBy(p => p.TypeAxis)
                  .ThenBy(p => p.LineGroup).ThenBy(p => p.ProcessLevelOne)
                  .ThenBy(p => p.ProcessLevelTwo).ThenBy(p => p.Occupation).ToList();

                var viewReport = new ViewReport()
                {
                    Data = data,
                    Name = "listopportunityline",
                    _idReport = NewReport("listopportunityline"),
                    _idAccount = _user._idAccount
                };
                SendMessageAsync(viewReport);
                var report = new ViewCrudReport();

                while (report.StatusReport == EnumStatusReport.Open)
                {
                    var rest = serviceReport.GetNewVersion(p => p._id == viewReport._idReport).Result;
                    report.StatusReport = rest.StatusReport;
                    report.Link = rest.Link;
                    //Thread.Sleep(1000);
                }

                return report.Link;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ListPersons()
        {
            try
            {
                var data = servicePerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result
                  .Select(p => new _ViewListBase() { _id = p._id, Name = p.User.Name }).ToList();

                var view = new ViewReport()
                {
                    Data = data,
                    Name = "listpersons",
                    _idReport = NewReport("listpersons"),
                    _idAccount = _user._idAccount
                };
                SendMessageAsync(view);
                var report = new ViewCrudReport();

                while (report.StatusReport == EnumStatusReport.Open)
                {
                    var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
                    report.StatusReport = rest.StatusReport;
                    report.Link = rest.Link;
                    //Thread.Sleep(1000);
                }

                return report.Link;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ListTraining(string idevent)
        {
            try
            {
                var events = serviceEvent.GetNewVersion(p => p._id == idevent).Result;

                List<ViewListTraining> data = new List<ViewListTraining>();
                List<string> dates = new List<string>();
                string instructors = "";

                if (events.Instructors != null)
                {
                    foreach (var item in events.Instructors)
                    {
                        instructors += item.Name + "\n";
                    }
                }

                if (events.Days != null)
                {
                    foreach (var item in events.Days)
                    {
                        dates.Add(item.Begin.ToString("dd/MM/yyyy") + "  " +
                          item.Begin.ToString("HH:mm") + " - " + item.End.ToString("HH:mm"));
                    }
                }

                if (events.Participants != null)
                {
                    foreach (var item in events.Participants)
                    {
                        var viewEvent = new ViewListTraining()
                        {
                            NameEvent = events.Name,
                            NameCourse = events.Course?.Name,
                            Workload = Math.Round(events.Workload / 60, 2),
                            Registration = item.Person?.Registration,
                            Content = events.Content,
                            DateBegin = events.Begin,
                            DateEnd = events.End,
                            NameEntity = events.Entity?.Name,
                            NameParticipant = item.Name,
                            Instructor = instructors,
                        };
                        if (dates.Count >= 1)
                            viewEvent.Day1 = dates[0].ToString();
                        if (dates.Count >= 2)
                            viewEvent.Day2 = dates[1].ToString();
                        if (dates.Count >= 3)
                            viewEvent.Day3 = dates[2].ToString();
                        if (dates.Count >= 4)
                            viewEvent.Day4 = dates[3].ToString();
                        if (dates.Count >= 5)
                            viewEvent.Day5 = dates[4].ToString();
                        if (dates.Count >= 6)
                            viewEvent.Day6 = dates[5].ToString();
                        if (dates.Count >= 7)
                            viewEvent.Day7 = dates[6].ToString();
                        if (dates.Count >= 8)
                            viewEvent.Day8 = dates[7].ToString();
                        if (dates.Count >= 9)
                            viewEvent.Day9 = dates[8].ToString();
                        if (dates.Count >= 10)
                            viewEvent.Day10 = dates[9].ToString();
                        if (dates.Count >= 11)
                            viewEvent.Day11 = dates[10].ToString();
                        if (dates.Count >= 12)
                            viewEvent.Day12 = dates[11].ToString();
                        if (dates.Count >= 13)
                            viewEvent.Day13 = dates[12].ToString();
                        if (dates.Count >= 14)
                            viewEvent.Day14 = dates[13].ToString();
                        if (dates.Count >= 15)
                            viewEvent.Day15 = dates[14].ToString();

                        data.Add(viewEvent);
                    };
                }

                var view = new ViewReport()
                {
                    Data = data,
                    Name = "listtraining",
                    _idReport = NewReport("listtraining"),
                    _idAccount = _user._idAccount
                };
                SendMessageAsync(view);
                var report = new ViewCrudReport();

                while (report.StatusReport == EnumStatusReport.Open)
                {
                    var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
                    report.StatusReport = rest.StatusReport;
                    report.Link = rest.Link;
                    //Thread.Sleep(1000);
                }

                return report.Link;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ListCertificate(string idevent, string idperson)
        {
            try
            {
                var events = serviceEvent.GetNewVersion(p => p._id == idevent).Result;

                List<ViewCertificate> data = new List<ViewCertificate>();
                string instructors = "";

                if (events != null)
                {
                    if (events.Instructors != null)
                    {
                        foreach (var item in events.Instructors)
                        {
                            instructors += item.Name + "\n";
                        }
                    }

                    if (idperson != string.Empty)
                        events.Participants = events.Participants.Where(p => p.Person._id == idperson).ToList();

                    if (events.Participants != null)
                    {
                        foreach (var item in events.Participants)
                        {
                            var viewEvent = new ViewCertificate()
                            {
                                NameEvent = events.Name,
                                NameCourse = events.Course?.Name,
                                Content = events.Content,
                                DateBegin = events.Begin,
                                DateEnd = events.End,
                                NameEntity = events.Entity?.Name,
                                NameParticipant = item.Name,
                                Workload = Math.Round(events.Workload / 60, 2),
                                Instructor = instructors,
                                Code = events.Code
                            };
                            data.Add(viewEvent);
                        };
                    }
                }
                else
                {
                    var eventsHistoric = serviceEventHistoric.GetFreeNewVersion(p => p.Event._id == idevent).Result;

                    var viewEvent = new ViewCertificate()
                    {
                        NameEvent = eventsHistoric.Name,
                        NameCourse = eventsHistoric.Course?.Name,
                        Content = "",
                        DateBegin = eventsHistoric.Begin,
                        DateEnd = eventsHistoric.End,
                        NameEntity = eventsHistoric.Entity?.Name,
                        NameParticipant = eventsHistoric.Person.Name,
                        Workload = Math.Round(eventsHistoric.Workload / 60, 2),
                        Instructor = "",
                        Code = ""
                    };
                    data.Add(viewEvent);
                }

                if (data.Count() == 0)
                    return "empty";

                var view = new ViewReport()
                {
                    Data = data,
                    Name = "listcertificate",
                    _idReport = NewReport("listcertificate"),
                    _idAccount = _user._idAccount
                };
                SendMessageAsync(view);
                var report = new ViewCrudReport();

                while (report.StatusReport == EnumStatusReport.Open)
                {
                    var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
                    report.StatusReport = rest.StatusReport;
                    report.Link = rest.Link;
                    //Thread.Sleep(1000);
                }

                return report.Link;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string ListHistoricTraining(ViewFilterDate date, string idperson)
        {
            try
            {
                List<EventHistoric> events = null;
                List<Event> listevent = null;

                if ((date == null) & (idperson == ""))
                {
                    events = serviceEventHistoric.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;
                    listevent = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized).Result;
                }
                else if ((date == null) & (idperson != ""))
                {
                    events = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson).Result;
                    listevent = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized).Result;
                }
                else if ((date != null) & (idperson == ""))
                {
                    events = serviceEventHistoric.GetAllNewVersion(p => p.Begin >= date.Begin && p.End <= date.End).Result;
                    listevent = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized && p.Begin >= date.Begin && p.End <= date.End).Result;
                }
                else
                {
                    events = serviceEventHistoric.GetAllNewVersion(p => p.Begin >= date.Begin && p.End <= date.End && p.Person._id == idperson).Result;
                    listevent = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized && p.Begin >= date.Begin && p.End <= date.End).Result;
                }


                var data = new List<ViewListHistoricTraining>();
                List<Person> persons = new List<Person>();

                if (idperson != "")
                    persons = servicePerson.GetAllNewVersion(p => p._id == idperson).Result;
                else
                    persons = servicePerson.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

                foreach (var item in events)
                {
                    var person = persons.Where(p => p._id == item.Person._id).FirstOrDefault();
                    if (person != null)
                    {
                        var viewL = new ViewListHistoricTraining()
                        {
                            Name = person.User?.Name,
                            Manager = person.Manager?.Name,
                            Course = item.Course?.Name,
                            Occupation = person.Occupation?.Name,
                            DateBegin = item.Begin,
                            DateEnd = item.End,
                            Entity = item.Entity?.Name,
                            Schooling = person.User.Schooling?.Name,
                            Workload = item.Workload,
                            _id = person._id,
                            Type = EnumTypeHistoricTraining.Realized
                        };
                        data.Add(viewL);
                    }

                }

                //instructors
                if (idperson != "")
                {
                    foreach (var item in listevent)
                    {
                        if (item.Instructors != null)
                        {
                            if (item.Instructors.Where(p => p.Person?._id == idperson).Count() > 0)
                            {
                                var person = persons.Where(p => p._id == idperson).FirstOrDefault();
                                if (person != null)
                                {
                                    var viewL = new ViewListHistoricTraining()
                                    {
                                        Name = person.User?.Name,
                                        Manager = person.Manager?.Name,
                                        Course = item.Course?.Name,
                                        Occupation = person.Occupation?.Name,
                                        DateBegin = item.Begin,
                                        DateEnd = item.End,
                                        Entity = item.Entity?.Name,
                                        Schooling = person.User.Schooling?.Name,
                                        Workload = item.Workload,
                                        _id = person._id,
                                        Type = EnumTypeHistoricTraining.Instructor
                                    };
                                    data.Add(viewL);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in listevent)
                    {
                        if (item.Instructors != null)
                        {
                            foreach (var inst in item.Instructors)
                            {
                                if (inst.Person != null)
                                {
                                    var person = persons.Where(p => p._id == inst.Person._id).FirstOrDefault();
                                    if (person != null)
                                    {
                                        var viewL = new ViewListHistoricTraining()
                                        {
                                            Name = person.User?.Name,
                                            Manager = person.Manager?.Name,
                                            Course = item.Course?.Name,
                                            Occupation = person.Occupation?.Name,
                                            DateBegin = item.Begin,
                                            DateEnd = item.End,
                                            Entity = item.Entity?.Name,
                                            Schooling = person.User.Schooling?.Name,
                                            Workload = item.Workload,
                                            _id = person._id,
                                            Type = EnumTypeHistoricTraining.Instructor
                                        };
                                        data.Add(viewL);
                                    }
                                }

                            }
                        }
                    }
                }


                var view = new ViewReport()
                {
                    Data = data,
                    Name = "listhistorictraining",
                    _idReport = NewReport("listhistorictraining"),
                    _idAccount = _user._idAccount
                };
                SendMessageAsync(view);
                var report = new ViewCrudReport();

                while (report.StatusReport == EnumStatusReport.Open)
                {
                    var rest = serviceReport.GetNewVersion(p => p._id == view._idReport).Result;
                    report.StatusReport = rest.StatusReport;
                    report.Link = rest.Link;
                    //Thread.Sleep(1000);
                }

                return report.Link;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region ... private ...

        private string NewReport(string name)
        {
            try
            {
                var report = new Reports()
                {
                    StatusReport = EnumStatusReport.Open,
                    Date = DateTime.Now,
                    Name = name
                };

                return serviceReport.InsertNewVersion(report).Result._id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void SendMessageAsync(ViewReport view)
        {
            try
            {
                dynamic result = view;
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)));
                queueClient.SendAsync(message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void RegisterOnMessageHandlerAndReceiveMesssages()
        {
            try
            {
                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };

                queueClientReturn.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                var view = JsonConvert.DeserializeObject<ViewCrudReport>(Encoding.UTF8.GetString(message.Body));
                SetUser(new BaseUser()
                {
                    _idAccount = view._idAccount
                });

                if (view.StatusReport != EnumStatusReport.Open)
                {
                    Reports report = serviceReport.GetFreeNewVersion(p => p._id == view._id).Result;
                    report.StatusReport = view.StatusReport;
                    report.Link = view.Link;
                    serviceReport.UpdateAccount(report, null).Wait();

                    await queueClientReturn.CompleteAsync(message.SystemProperties.LockToken);
                }
            }
            catch (Exception e)
            {
                var error = e.Message;
                queueClientReturn.CompleteAsync(message.SystemProperties.LockToken);
            }

        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            return Task.CompletedTask;
        }

        #endregion

    }

}

#pragma warning restore 4014

