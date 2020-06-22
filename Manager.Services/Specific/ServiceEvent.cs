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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
#pragma warning disable 1998
    public class ServiceEvent : Repository<Event>, IServiceEvent
    {
        private readonly ServiceGeneric<Course> serviceCourse;
        private readonly ServiceGeneric<CourseESocial> serviceCourseESocial;
        private readonly ServiceGeneric<Entity> serviceEntity;
        private readonly ServiceGeneric<Event> serviceEvent;
        private readonly ServiceGeneric<EventHistoric> serviceEventHistoric;
        private readonly ServiceGeneric<EventHistoricTemp> serviceEventHistoricTemp;
        private readonly ServiceLog serviceLog;
        private readonly ServiceGeneric<Person> servicePerson;
        private readonly ServiceGeneric<TrainingPlan> serviceTrainingPlan;
        private readonly string Path;
        private readonly string blobkey;

        #region Constructor
        public ServiceEvent(DataContext context, DataContext contextLog, string pathToken, string _blobkey) : base(context)
        {
            try
            {
                serviceCourse = new ServiceGeneric<Course>(context);
                serviceCourseESocial = new ServiceGeneric<CourseESocial>(context);
                serviceEntity = new ServiceGeneric<Entity>(context);
                serviceEvent = new ServiceGeneric<Event>(context);
                serviceEventHistoric = new ServiceGeneric<EventHistoric>(context);
                serviceEventHistoricTemp = new ServiceGeneric<EventHistoricTemp>(context);
                serviceLog = new ServiceLog(context, contextLog);
                servicePerson = new ServiceGeneric<Person>(context);
                serviceTrainingPlan = new ServiceGeneric<TrainingPlan>(context);
                Path = pathToken;
                blobkey = _blobkey;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetUser(IHttpContextAccessor contextAccessor)
        {
            User(contextAccessor);
            serviceCourse._user = _user;
            serviceCourseESocial._user = _user;
            serviceEntity._user = _user;
            serviceEvent._user = _user;
            serviceEventHistoric._user = _user;
            serviceEventHistoricTemp._user = _user;
            serviceLog.SetUser(_user);
            servicePerson._user = _user;
            serviceTrainingPlan._user = _user;
        }

        public void SetUser(BaseUser user)
        {
            _user = user;
            serviceCourse._user = user;
            serviceCourseESocial._user = user;
            serviceEntity._user = user;
            serviceEvent._user = user;
            serviceEventHistoric._user = user;
            serviceEventHistoricTemp._user = user;
            serviceLog.SetUser(user);
            servicePerson._user = user;
            serviceTrainingPlan._user = user;
        }
        #endregion

        #region Event
        public string RemoveDays(string idevent, string iddays)
        {
            try
            {



                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                Task.Run(() => LogSave(_user._idPerson, "Remove Days Event: " + idevent + " | day: " + iddays));

                foreach (var item in events.Days)
                {
                    if (item._id == iddays)
                    {
                        events.Days.Remove(item);
                        UpdateAddDaysParticipant(ref events, item);
                        MathWorkload(ref events);
                        serviceEvent.Update(events, null).Wait();
                        return "remove success";
                    }
                }
                return "remove success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string RemoveParticipant(string idevent, string idperson)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                foreach (var item in events.Participants)
                {
                    if (item._id == idperson)
                    {
                        events.Participants.Remove(item);
                        serviceEvent.Update(events, null).Wait();
                        return "remove success";
                    }

                }

                return "remove success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string RemoveInstructor(string idevent, string id)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                foreach (var item in events.Instructors)
                {
                    if (item._id == id)
                    {
                        events.Instructors.Remove(item);
                        serviceEvent.Update(events, null).Wait();
                        return "remove success";
                    }

                }

                return "remove success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Remove(string id)
        {
            try
            {


                var item = serviceEvent.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
                Task.Run(() => LogSave(_user._idPerson, "Delete Event " + id));
                item.Status = EnumStatus.Disabled;
                serviceEvent.Update(item, null).Wait();
                return "deleted";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ReopeningEvent(string idevent)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                foreach (var item in serviceEventHistoric.GetAllNewVersion(p => p.Event._id == events._id).Result.ToList())
                {
                    serviceEventHistoric.Delete(item._id);

                }

                var plans = serviceTrainingPlan.GetAllNewVersion(p => p.Event._id == events._id & p.StatusTrainingPlan == EnumStatusTrainingPlan.Realized).Result.ToList();
                foreach (var traningplan in plans)
                {
                    traningplan.StatusTrainingPlan = EnumStatusTrainingPlan.Open;
                    serviceTrainingPlan.Update(traningplan, null).Wait();
                }

                events.StatusEvent = EnumStatusEvent.Open;
                serviceEvent.Update(events, null).Wait();

                return "reopening";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetAttachment(string idevent, string url, string fileName, string attachmentid)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();

                if (events.Attachments == null)
                {
                    events.Attachments = new List<ViewCrudAttachmentField>();
                }
                events.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
                serviceEvent.Update(events, null).Wait();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string SetGrade(string idevent, string idparticipant, decimal grade)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();

                foreach (var participant in events.Participants)
                {
                    if (participant._id == idparticipant)
                    {
                        participant.Grade = grade;
                        if (participant.Grade < events.Grade)
                            participant.ApprovedGrade = false;
                        else
                            participant.ApprovedGrade = true;

                        serviceEvent.Update(events, null).Wait();
                    }
                }

                return "success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Present(string idevent, string idparticipant, string idday, bool present)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                decimal total = 0;
                decimal count = 0;

                foreach (var participant in events.Participants)
                {
                    if (participant._id == idparticipant)
                    {
                        foreach (var freq in participant.FrequencyEvent)
                        {
                            if (freq._id == idday)
                            {
                                freq.Present = present;
                            }
                            if (freq.Present)
                                count += 1;

                            total += 1;
                        }

                        if (((count * 100) / total) > events.MinimumFrequency)
                            participant.Approved = true;
                        else
                            participant.Approved = false;

                        serviceEvent.Update(events, null).Wait();
                    }
                }

                return "success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public ViewCrudEvent Get(string id)
        {
            try
            {

                var events = serviceEvent.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
                Task.Run(() => LogSave(_user._idPerson, "Get Event by ID"));

                return new ViewCrudEvent()
                {
                    _id = events._id,
                    Course = events.Course,
                    Name = events.Name,
                    Content = events.Content,
                    Entity = events.Entity,
                    MinimumFrequency = events.MinimumFrequency,
                    LimitParticipants = events.LimitParticipants,
                    Grade = events.Grade,
                    OpenSubscription = events.OpenSubscription,
                    DaysSubscription = events.DaysSubscription,
                    Workload = events.Workload,
                    Begin = events.Begin,
                    End = events.End,
                    Instructors = events.Instructors,
                    Days = events.Days,
                    Participants = events.Participants,
                    StatusEvent = events.StatusEvent,
                    Observation = events.Observation,
                    Evalution = events.Evalution,
                    Attachments = events.Attachments,
                    DateEnd = events.DateEnd,
                    Modality = events.Modality,
                    TypeESocial = events.TypeESocial,
                    Code = events.Code
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListPersonResume> ListPersonParticipants(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                int skip = (count * (page - 1));
                var detail = new List<ViewListPersonResume>();
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                var participants = events.Participants.Select(p => p.Person).ToList();
                var list = servicePerson.GetAllNewVersion(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result.ToList();
                foreach (var item in list)
                {
                    if (!participants.Contains(item.GetViewListResume()))
                        detail.Add(
                          new ViewListPersonResume()
                          {
                              _id = item._id,
                              Name = item.User.Name,
                              Document = item.User.Document,
                              Registration = item.Registration,
                              //Person = item.GetViewList(),
                              Cbo = item.Occupation == null ? null : (item.Occupation.Cbo == null) ? null : new ViewListCbo()
                              {
                                  _id = item.Occupation.Cbo._id,
                                  Name = item.Occupation.Cbo.Name,
                                  Code = item.Occupation.Cbo.Code
                              }
                          });
                }

                total = detail.Count();

                return detail.Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListPersonResume> ListPersonInstructor(string idevent, string idcompany, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                int skip = (count * (page - 1));
                var detail = new List<ViewListPersonResume>();
                var events = serviceEvent.GetNewVersion(p => p._id == idevent).Result;
                var instructors = events.Instructors.Select(p => p.Person).ToList();
                var list = servicePerson.GetAllNewVersion(p => p.Company._id == idcompany & p.StatusUser != EnumStatusUser.Disabled & p.TypeUser != EnumTypeUser.Administrator & p.User.Name.ToUpper().Contains(filter.ToUpper())).Result;

                foreach (var item in list)
                {
                    if (!instructors.Contains(item.GetViewListResume()))
                        detail.Add(new ViewListPersonResume()
                        {
                            _id = item._id,
                            Name = item.User.Name,
                            Document = item.User.Document,
                            //Person = item.GetViewList(),
                            Cbo = item.Occupation == null ? null : (item.Occupation.Cbo == null) ? null : new ViewListCbo()
                            {
                                _id = item.Occupation.Cbo?._id,
                                Name = item.Occupation.Cbo?.Name,
                                Code = item.Occupation.Cbo?.Code
                            }
                        });
                }

                total = detail.Count();

                return detail.ToList().Skip(skip).Take(count).OrderBy(p => p.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEventDetail> List(EnumTypeEvent type, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {

                int skip = (count * (page - 1));
                List<Event> detail = new List<Event>();

                if (type == EnumTypeEvent.All)
                {
                    detail = serviceEvent.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;
                }
                else if (type == EnumTypeEvent.Open)
                {
                    detail = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Open && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
                }
                else if (type == EnumTypeEvent.End)
                {
                    detail = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized && p.Name.ToUpper().Contains(filter.ToUpper())).Result;
                }

                detail = detail.Where(p => !(p.Participants.Count() == 0 && p.Days.Count() == 0 && p.StatusEvent == EnumStatusEvent.Realized)).ToList().OrderBy(p => p.StatusEvent).ThenByDescending(p => p.End).Skip(skip).Take(count).ToList();
                total = detail.Count();

                return detail.Select(p => new ViewListEventDetail()
                {
                    _id = p._id,
                    Name = p.Name,
                    _idCourse = p.Course?._id,
                    NameCourse = p.Course?.Name,
                    Begin = p.Begin,
                    End = p.End,
                    StatusEvent = p.StatusEvent
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEvent> ListEventOpen(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {

                int skip = (count * (page - 1));
                var detail = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceEvent.CountNewVersion(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewListEvent()
                {
                    _id = p._id,
                    Name = p.Name,
                    _idCourse = p.Course._id,
                    NameCourse = p.Course.Name
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEventSubscription> ListEventInstructor(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                //LogSave(_user._idPerson, "List Open Events subscrive");
                DateTime? date = DateTime.Now;
                int skip = (count * (page - 1));
                var detail = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.End).ToList();

                var result = new List<Event>();
                foreach (var item in detail)
                {
                    var instructors = item.Instructors.Where(p => p.Person != null).ToList();
                    if (instructors.Where(p => p.Person._id == idperson).Count() > 0)
                        result.Add(item);

                }
                total = result.Count();

                return result.OrderBy(p => p.End).Skip(skip).Take(count).Select(p => new ViewListEventSubscription()
                {
                    _id = p._id,
                    NameEvent = p.Name,
                    Attachments = p.Attachments,
                    Days = p.Days,
                    Entity = p.Entity?.Name,
                    Instructors = p.Instructors,
                    Observation = p.Observation,
                    Workload = p.Workload,
                    Content = p.Content
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEventSubscription> ListEventOpenSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                //LogSave(_user._idPerson, "List Open Events subscrive");
                DateTime? date = DateTime.Now;
                int skip = (count * (page - 1));
                var detail = serviceEvent.GetAllNewVersion(p => p.OpenSubscription == true &
                p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.End).ToList();

                var result = new List<Event>();
                foreach (var item in detail)
                {
                    if (item.Begin != null)
                    {
                        if (date.Value.Date < item.Begin.Value.AddDays(item.DaysSubscription * -1).Date)
                        {
                            var participants = item.Participants.Where(p => p.Person != null).ToList();
                            if (participants.Where(p => p.Person._id == idperson).Count() == 0)
                                if (item.LimitParticipants > participants.Count())
                                    result.Add(item);

                        }
                    }
                }
                total = result.Count();

                return result.Skip(skip).Take(count).Select(p => new ViewListEventSubscription()
                {
                    _id = p._id,
                    NameEvent = p.Name,
                    Attachments = p.Attachments,
                    Days = p.Days,
                    Entity = p.Entity?.Name,
                    Instructors = p.Instructors,
                    Observation = p.Observation,
                    Workload = p.Workload,
                    Content = p.Content
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEventSubscription> ListEventSubscription(string idperson, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                //LogSave(_user._idPerson, "List Open Events subscrive");
                DateTime? date = DateTime.Now;
                int skip = (count * (page - 1));
                var detail = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Open & p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.End).ToList();

                var result = new List<Event>();
                foreach (var item in detail)
                {
                    if (item.Participants != null)
                    {
                        try
                        {
                            var participants = item.Participants.Where(p => p.Person != null).ToList();
                            if (participants.Where(p => p.Person._id == idperson).Count() > 0)
                                result.Add(item);
                        }
                        catch (Exception)
                        {
                            //person null
                        }
                    }
                }
                total = result.Count();

                return result.Skip(skip).Take(count).Select(p => new ViewListEventSubscription()
                {
                    _id = p._id,
                    NameEvent = p.Name,
                    Attachments = p.Attachments,
                    Days = p.Days,
                    Entity = p.Entity?.Name,
                    Instructors = p.Instructors,
                    Observation = p.Observation,
                    Workload = p.Workload,
                    Content = p.Content
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEvent> ListEventEnd(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {

                int skip = (count * (page - 1));
                var detail = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized & p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceEvent.CountNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized & p.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewListEvent()
                {
                    _id = p._id,
                    Name = p.Name,
                    _idCourse = p.Course._id,
                    NameCourse = p.Course.Name
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ViewListEvent New(ViewCrudEvent view)
        {
            try
            {
                var person = servicePerson.GetAllNewVersion(p => p.User._id == _user._idUser).Result.FirstOrDefault();
                var events = new Event()
                {
                    Participants = new List<ViewCrudParticipant>(),
                    Instructors = new List<ViewCrudInstructor>(),
                    Attachments = new List<ViewCrudAttachmentField>(),
                    UserInclude = person.GetViewListBase(),
                    DateInclude = DateTime.Now,
                    Days = new List<ViewCrudDaysEvent>(),
                    Entity = AddEntity(view.Entity.Name),
                    Course = serviceCourse.GetAllNewVersion(p => p._id == view.Course._id).Result.Select(p => new ViewListCourse() { _id = p._id, Name = p.Name }).FirstOrDefault(),
                    Name = view.Name,
                    Begin = view.Begin,
                    Content = view.Content,
                    DateEnd = view.DateEnd,
                    DaysSubscription = view.DaysSubscription,
                    End = view.End,
                    Evalution = view.Evalution,
                    Grade = view.Grade,
                    LimitParticipants = view.LimitParticipants,
                    MinimumFrequency = view.MinimumFrequency,
                    Modality = view.Modality,
                    Observation = view.Observation,
                    OpenSubscription = view.OpenSubscription,
                    Status = EnumStatus.Enabled,
                    StatusEvent = view.StatusEvent,
                    TypeESocial = view.TypeESocial,
                    Workload = view.Workload,
                    Code = view.Code
                };

                serviceEvent.InsertNewVersion(events).Wait();

                return new ViewListEvent()
                {
                    _id = events._id,
                    Name = events.Name,
                    NameCourse = events.Course.Name,
                    _idCourse = events.Course._id
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string AddDays(string idevent, ViewCrudDaysEvent view)
        {
            try
            {

                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                var days = new ViewCrudDaysEvent()
                {
                    Begin = view.Begin,
                    End = view.End,
                    _id = ObjectId.GenerateNewId().ToString()
                };

                if (events.Days == null)
                    events.Days = new List<ViewCrudDaysEvent>();

                events.Days.Add(days);
                MathWorkload(ref events);
                UpdateAddDaysParticipant(ref events, days);
                serviceEvent.Update(events, null).Wait();


                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void UpdateRemoveDaysParticipant(string idevent, ViewCrudDaysEvent days)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                foreach (var item in events.Participants)
                {

                }
                serviceEvent.Update(events, null).Wait();

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string AddInstructor(string idevent, ViewCrudInstructor view)
        {
            try
            {
                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                var instructor = new ViewCrudInstructor()
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    Name = view.Name,
                    Content = view.Content,
                    Document = view.Document,
                    TypeInstructor = view.TypeInstructor,
                    Schooling = view.Schooling,
                    Person = view.Person == null ? null : servicePerson.GetAllNewVersion(p => p._id == view.Person._id).Result.FirstOrDefault().GetViewListResume(),
                    Cbo = view.Cbo ?? null
                };

                events.Instructors.Add(instructor);
                serviceEvent.Update(events, null).Wait();
                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string AddParticipant(string idevent, ViewCrudParticipant view)
        {
            try
            {

                var events = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();
                var participant = new ViewCrudParticipant()
                {
                    _id = ObjectId.GenerateNewId().ToString(),
                    FrequencyEvent = new List<ViewCrudFrequencyEvent>(),
                    Approved = true,
                    Name = view.Name,
                    Person = view.Person,
                    TypeParticipant = view.TypeParticipant,
                    ApprovedGrade = view.ApprovedGrade,
                    Grade = view.Grade
                };



                foreach (var days in events.Days)
                {
                    participant.FrequencyEvent.Add(new ViewCrudFrequencyEvent()
                    {
                        _id = ObjectId.GenerateNewId().ToString(),
                        DaysEvent = new ViewCrudDaysEvent()
                        {
                            Begin = days.Begin,
                            End = days.End,
                            _id = ObjectId.GenerateNewId().ToString()
                        },
                        Present = true
                    });
                }

                if (events.Grade > 0)
                    participant.ApprovedGrade = false;
                else
                    participant.ApprovedGrade = true;

                events.Participants.Add(participant);
                serviceEvent.Update(events, null).Wait();
                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewCrudParticipant> ListParticipants(string idevent, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                int skip = (count * (page - 1));
                var detail = serviceEvent.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                //var total = serviceEvent.CountNewVersion(p => p._id == idevent).Result.FirstOrDefault().Participants.Where(p => p.Name.ToUpper().Contains(filter.ToUpper()));

                return detail.Select(p => new ViewCrudParticipant()
                {
                    _id = p._id,
                    Person = p.Person,
                    FrequencyEvent = p.FrequencyEvent?.OrderBy(k => k.DaysEvent.Begin).Select
                    (y => new ViewCrudFrequencyEvent()
                    {
                        _id = y._id,
                        Present = y.Present,
                        DaysEvent = (y.DaysEvent == null) ? null : new ViewCrudDaysEvent() { _id = y.DaysEvent._id, Begin = y.DaysEvent.Begin, End = y.DaysEvent.End }
                    }).ToList(),
                    Approved = p.Approved,
                    Grade = p.Grade,
                    Name = p.Name,
                    TypeParticipant = p.TypeParticipant
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ViewListEvent Update(ViewCrudEvent view)
        {
            try
            {
                var person = servicePerson.GetAllNewVersion(p => p.User._id == _user._idUser).Result.FirstOrDefault();
                var events = serviceEvent.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
                events.Course = serviceCourse.GetAllNewVersion(p => p._id == view.Course._id).Result.Select(p => new ViewListCourse()
                {
                    _id = p._id,
                    Name = p.Name
                }).FirstOrDefault();
                events.Name = view.Name;
                events.Begin = view.Begin;
                events.Content = view.Content;
                events.DateEnd = view.DateEnd;
                events.DaysSubscription = view.DaysSubscription;
                events.End = view.End;
                events.Evalution = view.Evalution;
                events.Grade = view.Grade;
                events.LimitParticipants = view.LimitParticipants;
                events.MinimumFrequency = view.MinimumFrequency;
                events.Modality = view.Modality;
                events.Observation = view.Observation;
                events.OpenSubscription = view.OpenSubscription;
                events.StatusEvent = view.StatusEvent;
                events.TypeESocial = view.TypeESocial;
                events.Workload = view.Workload;
                events.Code = view.Code;

                events.UserEdit = person.GetViewListBase();
                events.Entity = AddEntity(view.Entity.Name);
                if (view.StatusEvent == EnumStatusEvent.Realized)
                {
                    view.DateEnd = DateTime.Now;
                    GenerateHistoric(events);
                }
                serviceEvent.Update(events, null).Wait();
                return new ViewListEvent()
                {
                    _id = view._id,
                    Name = view.Name
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

        #region CourseESocial
        public ViewCrudCourseESocial GetCourseESocial(string id)
        {
            try
            {
                var course = serviceCourseESocial.GetFreeNewVersion(p => p._id == id).Result;
                return new ViewCrudCourseESocial()
                {
                    _id = course._idAccount,
                    Name = course.Name,
                    Code = course.Code
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string UpdateCourseESocial(ViewCrudCourseESocial view)
        {
            try
            {
                var esocial = serviceCourseESocial.GetFreeNewVersion(p => p._id == view._id).Result;
                esocial.Name = view.Name;
                esocial.Code = view.Code;

                serviceCourseESocial.UpdateAccount(esocial, null).Wait();
                return "update";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string NewCourseESocial(ViewCrudCourseESocial view)
        {
            try
            {
                serviceCourseESocial.InsertFreeNewVersion(new CourseESocial()
                {
                    Name = view.Name,
                    Code = view.Code,
                    Status = EnumStatus.Enabled
                }).Wait();
                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewCrudCourseESocial> ListCourseESocial(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                int skip = (count * (page - 1));
                var detail = serviceCourseESocial.GetAllFreeNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceCourseESocial.CountFreeNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewCrudCourseESocial()
                {
                    _id = p._id,
                    Name = p.Name,
                    Code = p.Code
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string RemoveCourseESocial(string id)
        {
            try
            {
                var item = serviceCourseESocial.GetFreeNewVersion(p => p._id == id).Result;
                item.Status = EnumStatus.Disabled;
                serviceCourseESocial.UpdateAccount(item, null).Wait();
                return "deleted";
            }
            catch (Exception e)
            {
                throw e;
            }
            throw new NotImplementedException();
        }
        #endregion

        #region Course
        public string RemoveCourse(string id)
        {
            try
            {
                var item = serviceCourse.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

                var exists = serviceEvent.CountNewVersion(p => p.Course._id == item._id & p.StatusEvent == EnumStatusEvent.Open).Result;
                if (exists > 0)
                    return "error_exists";

                item.Status = EnumStatus.Disabled;
                serviceCourse.Update(item, null).Wait();
                return "deleted";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ViewCrudCourse GetCourse(string id)
        {
            try
            {

                var course = serviceCourse.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();

                return new ViewCrudCourse()
                {
                    _id = course._id,
                    Name = course.Name,
                    Content = course.Content,
                    CourseESocial = (course.CourseESocial == null) ? null : new ViewCrudCourseESocial() { _id = course.CourseESocial._id, Name = course.CourseESocial.Name, Code = course.CourseESocial.Code },
                    Deadline = course.Deadline,
                    Equivalents = course.Equivalents?.Select(p => new ViewListCourse()
                    {
                        _id = p._id,
                        Name = p.Name
                    }).ToList(),
                    Prerequisites = course.Prerequisites?.Select(p => new ViewListCourse()
                    {
                        _id = p._id,
                        Name = p.Name
                    }).ToList(),
                    Periodicity = course.Periodicity,
                    Wordkload = course.Wordkload
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ViewListCourse> ListCourse(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {


                int skip = (count * (page - 1));
                var detail = serviceCourse.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceCourse.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewListCourse()
                {
                    _id = p._id,
                    Name = p.Name
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string NewCourse(ViewCrudCourse view)
        {
            try
            {
                var course = serviceCourse.InsertNewVersion(new Course()
                {
                    Name = view.Name,
                    Wordkload = view.Wordkload,
                    Content = view.Content,
                    Deadline = view.Deadline,
                    Periodicity = view.Periodicity,
                    CourseESocial = view.CourseESocial,
                    Status = EnumStatus.Enabled,
                    Prerequisites = view.Prerequisites,
                    Equivalents = view.Equivalents
                }).Result;

                //VerifyEquivalent(course);

                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string UpdateCourse(ViewCrudCourse view)
        {
            try
            {
                var course = serviceCourse.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
                var modifymatriz = false;
                if (course.Periodicity != view.Periodicity)
                    modifymatriz = true;

                course.Content = view.Content;

                course.Periodicity = view.Periodicity;

                course.Deadline = view.Deadline;
                course.Wordkload = view.Wordkload;
                course.CourseESocial = view.CourseESocial;
                course.Equivalents = view.Equivalents;
                course.Prerequisites = view.Prerequisites;

                serviceCourse.Update(course, null).Wait();

                VerifyEquivalent(course);
                return "update";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region EventHistoric

        //nome colaborador, nome cargo, nome curso, inicio, fim, carga horária
        public List<ViewListHistoric> ListHistoric(string idperson, string idcourse, ViewFilterDate date)
        {
            try
            {
                List<EventHistoric> list = new List<EventHistoric>();

                if ((idperson != string.Empty) && (idcourse != string.Empty) && (date != null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson
                    && p.Course._id == idcourse &&
                    p.End >= date.Begin && p.End <= date.End).Result;
                else if ((idperson != string.Empty) && (idcourse != string.Empty) && (date != null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson
                    && p.Course._id == idcourse
                    && p.End >= date.Begin && p.End <= date.End).Result;
                else if ((idperson != string.Empty) && (idcourse == string.Empty) && (date != null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson
                    && p.End >= date.Begin && p.End <= date.End).Result;
                else if ((idperson != string.Empty) && (idcourse != string.Empty) && (date == null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson
                    && p.Course._id == idcourse).Result;
                else if ((idperson != string.Empty) && (idcourse == string.Empty) && (date == null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == idperson).Result;
                else if ((idperson == string.Empty) && (idcourse != string.Empty) && (date != null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Course._id == idcourse
                    && p.End >= date.Begin && p.End <= date.End).Result;
                else if ((idperson == string.Empty) && (idcourse == string.Empty) && (date != null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.End >= date.Begin && p.End <= date.End).Result;
                else if ((idperson == string.Empty) && (idcourse != string.Empty) && (date == null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Status == EnumStatus.Enabled
                    && p.Course._id == idcourse).Result;
                else if ((idperson == string.Empty) && (idcourse == string.Empty) && (date == null))
                    list = serviceEventHistoric.GetAllNewVersion(p => p.Status == EnumStatus.Enabled).Result;

                var result = new List<ViewListHistoric>();

                foreach (var item in list)
                {
                    var view = new ViewListHistoric()
                    {
                        Person = item.Person?.Name,
                        Event = item.Name,
                        Occupation = servicePerson.GetNewVersion(p => p._id == item.Person._id).Result.Occupation?.Name,
                        Begin = item.Begin.Value.ToString("dd/MM/yyyy"),
                        End = item.End.Value.ToString("dd/MM/yyyy"),
                        Wordload = item.Workload / 60,
                        WorkloadMin = item.Workload
                    };
                    result.Add(view);
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string UpdateEventHistoricFrontEnd(ViewCrudEventHistoric view)
        {
            try
            {
                var eventHistoric = serviceEventHistoric.GetAllNewVersion(p => p._id == view._id).Result.FirstOrDefault();
                eventHistoric.Name = view.Name;
                eventHistoric.Person = servicePerson.GetAllNewVersion(p => p._id == view._idPerson).Result.FirstOrDefault().GetViewListBase();
                eventHistoric.Course = (view.Course == null) ? null : serviceCourse.GetAllNewVersion(p => p._id == view.Course._id).Result.Select(p => new ViewListCourse()
                {
                    _id = p._id,
                    Name = p.Name
                }).FirstOrDefault();
                eventHistoric.Event = (view.Event == null) ? null : serviceEvent.GetAllNewVersion(p => p._id == view.Event._id).Result.Select(p => new ViewListEvent()
                {
                    _id = p._id,
                    Name = p.Name
                }).FirstOrDefault();
                eventHistoric.Workload = view.Workload;
                eventHistoric.Begin = view.Begin;
                eventHistoric.End = view.End;
                eventHistoric.Attachments = view.Attachments;


                eventHistoric.Entity = AddEntity(view.Entity.Name);
                serviceEventHistoric.Update(eventHistoric, null).Wait();
                return "update";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string NewEventHistoricFrontEnd(ViewCrudEventHistoric view)
        {
            try
            {
                var eventhistoric = new EventHistoric()
                {
                    Entity = AddEntity(view.Entity.Name),
                    Person = servicePerson.GetAllNewVersion(p => p._id == view._idPerson).Result.FirstOrDefault().GetViewListBase(),
                    Course = (view.Course == null) ? null : serviceCourse.GetAllNewVersion(p => p._id == view.Course._id)
                  .Result.Select(p => new ViewListCourse()
                  {
                      _id = p._id,
                      Name = p.Name
                  }).FirstOrDefault(),
                    Event = (view.Event == null) ? null : serviceEvent.GetAllNewVersion(p => p._id == view.Event._id).Result.Select(p => new ViewListEvent()
                    {
                        _id = p._id,
                        Name = p.Name,
                        NameCourse = p.Course.Name,
                        _idCourse = p.Course._id
                    }).FirstOrDefault(),
                    Workload = view.Workload,
                    Begin = view.Begin,
                    End = view.End,
                    Name = view.Name,
                    Attachments = view.Attachments
                };


                if (eventhistoric.Workload.ToString().Contains(","))
                    eventhistoric.Workload = decimal.Parse(TimeSpan.Parse(view.Workload.ToString().Split(",")[0].PadLeft(2, '0') + ":" + view.Workload.ToString().Split(",")[1].PadRight(2, '0')).TotalMinutes.ToString());
                else
                    eventhistoric.Workload = view.Workload * 60;

                //TimeSpan span = TimeSpan.FromHours(double.Parse(view.Workload.ToString()));
                //view.Workload = decimal.Parse(span.TotalMinutes.ToString());
                //string time = view.Workload.ToString().Replace(",",":");
                //string[] pieces = time.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //TimeSpan difference2 = new TimeSpan(Convert.ToInt32(pieces[0]), Convert.ToInt32(pieces[1]), 0);
                //double minutes2 = difference2.TotalMinutes; 
                //view.Workload = decimal.Parse(minutes2.ToString());

                var events = serviceEventHistoric.InsertNewVersion(eventhistoric).Result;
                var plan = serviceTrainingPlan.GetAllNewVersion(p => p.Person._id == eventhistoric.Person._id & p.Course._id == view.Course._id
                & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).Result.FirstOrDefault();
                if (plan != null)
                {
                    plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
                    plan.Observartion = "Realized Event: " + events.Name + ", ID_Historic: " + events._id;
                    serviceTrainingPlan.Update(plan, null).Wait();
                }
                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewCrudEventHistoric> ListEventHistoricPerson(string id, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {

                int skip = (count * (page - 1));
                var listperson = servicePerson.GetAllNewVersion(p => p.User._id == id).Result;
                foreach (var person in listperson)
                {
                    var detail = serviceEventHistoric.GetAllNewVersion(p => p.Person._id == person._id & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderByDescending(p => p.End).ThenBy(p => p.Name).Skip(skip).Take(count).ToList();
                    total = serviceEventHistoric.CountNewVersion(p => p.Person._id == person._id & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result;
                    return detail.Select(p => new ViewCrudEventHistoric()
                    {
                        _id = p._id,
                        Name = p.Course.Name,
                        _idPerson = p.Person._id,
                        NamePerson = p.Person.Name,
                        Begin = p.Begin,
                        End = p.End,
                        Course = p.Course,
                        Entity = p.Entity,
                        Event = p.Event,
                        Workload = p.Workload,
                        Attachments = p.Attachments
                    }).ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewCrudEventHistoric> ListEventHistoricInstructor(string id, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {

                int skip = (count * (page - 1));
                var listperson = servicePerson.GetAllNewVersion(p => p.User._id == id).Result.Select(p => p._id).ToList();
                foreach (var person in listperson)
                {
                    List<Event> detail = new List<Event>();
                    var list = serviceEvent.GetAllNewVersion(p => p.StatusEvent == EnumStatusEvent.Realized & p.Course.Name.ToUpper().Contains(filter.ToUpper())).Result.ToList();
                    foreach (var item in list)
                    {
                        if (item.Instructors != null)
                            if (item.Instructors.Where(x => x.Person?._id == person).Count() > 0)
                                detail.Add(item);
                    }

                    total = detail.Count();
                    return detail.Select(p => new ViewCrudEventHistoric()
                    {
                        _id = p._id,
                        Name = p.Course.Name,
                        _idPerson = p.Instructors?.Where(x => x.Person._id == person).FirstOrDefault()?._id,
                        NamePerson = p.Instructors?.Where(x => x.Person._id == person).FirstOrDefault()?.Name,
                        Begin = p.Begin,
                        End = p.End,
                        Course = p.Course,
                        Entity = p.Entity,
                        Event = null,
                        Workload = p.Workload,
                        Attachments = p.Attachments
                    }).OrderByDescending(p => p.End).ThenBy(p => p.Name).Skip(skip).Take(count).ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListEventHistoric> ListEventHistoric(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {


                int skip = (count * (page - 1));
                var detail = serviceEventHistoric.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceEventHistoric.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewListEventHistoric()
                {
                    _id = p._id,
                    Name = p.Name,
                    _idPerson = p.Person._id,
                    NamePerson = p.Person.Name
                }).OrderBy(p => p.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string NewEventHistoric(EventHistoric view)
        {
            try
            {
                view.Entity = AddEntity(view.Entity.Name);
                var events = serviceEventHistoric.InsertNewVersion(view).Result;
                var plan = serviceTrainingPlan.GetAllNewVersion(p => p.Person._id == view.Person._id & p.Course._id == view.Course._id
                & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).Result.FirstOrDefault();
                if (plan != null)
                {
                    plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
                    plan.Observartion = "Realized Event: " + events.Name + ", ID_Historic: " + events._id;
                    plan.Event = view.Event;
                    serviceTrainingPlan.Update(plan, null).Wait();
                }
                return "add success";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UpdateEventHistoric(EventHistoric view)
        {
            try
            {


                if (view.Workload.ToString().Contains(","))
                    view.Workload = decimal.Parse(TimeSpan.Parse(view.Workload.ToString().Split(",")[0].PadLeft(2, '0') + ":" + view.Workload.ToString().Split(",")[1].PadRight(2, '0')).TotalMinutes.ToString());
                else
                    view.Workload *= 60;

                view.Entity = AddEntity(view.Entity.Name);
                serviceEventHistoric.Update(view, null).Wait();
                return "update";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ViewCrudEventHistoric GetEventHistoric(string id)
        {
            try
            {

                var eventhistoric = serviceEventHistoric.GetFreeNewVersion(p => p._id == id).Result;
                Task.Run(() => LogSave(_user._idPerson, "Get Historic by ID"));

                return new ViewCrudEventHistoric()
                {
                    _id = eventhistoric._id,
                    Begin = eventhistoric.Begin,
                    Course = eventhistoric.Course,
                    Name = eventhistoric.Name,
                    End = eventhistoric.End,
                    Workload = eventhistoric.Workload,
                    _idPerson = eventhistoric.Person._id,
                    NamePerson = eventhistoric.Person.Name,
                    Entity = eventhistoric.Entity,
                    Event = eventhistoric.Event,
                    Attachments = eventhistoric.Attachments
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string RemoveEventHistoric(string id)
        {
            try
            {
                var item = serviceEventHistoric.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
                Task.Run(() => LogSave(_user._idPerson, "Delete Event Historic " + id));
                var obs = "Realized Event: " + item.Name + ", ID_Historic: " + item._id;
                var trainingplan = serviceTrainingPlan.GetAllNewVersion(p => p.Person._id == item.Person._id
                & p.Course._id == item.Course._id & p.StatusTrainingPlan == EnumStatusTrainingPlan.Realized
                & p.Observartion == obs).Result.FirstOrDefault();
                if (trainingplan != null)
                {
                    trainingplan.StatusTrainingPlan = EnumStatusTrainingPlan.Open;
                    serviceTrainingPlan.Update(trainingplan, null).Wait();
                }
                item.Status = EnumStatus.Disabled;
                serviceEventHistoric.Update(item, null).Wait();
                return "deleted";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetAttachmentHistoric(string idevent, string url, string fileName, string attachmentid)
        {
            try
            {
                var eventsHistoric = serviceEventHistoric.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();

                if (eventsHistoric.Attachments == null)
                {
                    eventsHistoric.Attachments = new List<ViewCrudAttachmentField>();
                }
                eventsHistoric.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
                serviceEventHistoric.Update(eventsHistoric, null).Wait();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Entity

        public List<ViewCrudEntity> ListEntity(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                int skip = (count * (page - 1));
                var detail = serviceEntity.GetAllNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceEntity.CountNewVersion(p => p.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewCrudEntity()
                {
                    _id = p._id,
                    Name = p.Name
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region EventHistoricTemp
        public List<ViewCrudEventHistoricTemp> ListEventHistoricTempPerson(string id, ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {

                int skip = (count * (page - 1));
                var listperson = servicePerson.GetAllNewVersion(p => p.User._id == id).Result;
                foreach (var person in listperson)
                {
                    var detail = serviceEventHistoricTemp.GetAllNewVersion(p => p.StatusEventHistoricTemp == EnumStatusEventHistoricTemp.Wait && p.Person._id == person._id & p.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderByDescending(p => p.End).ThenBy(p => p.Name).Skip(skip).Take(count).ToList();
                    total = serviceEventHistoricTemp.CountNewVersion(p => p.StatusEventHistoricTemp == EnumStatusEventHistoricTemp.Wait && p.Person._id == person._id & p.Name.ToUpper().Contains(filter.ToUpper())).Result;
                    return detail.Select(p => new ViewCrudEventHistoricTemp()
                    {
                        _id = p._id,
                        Name = p.Name,
                        _idPerson = p.Person._id,
                        NamePerson = p.Person.Name,
                        Begin = p.Begin,
                        End = p.End,
                        Entity = p.Entity,
                        Event = p.Event,
                        Workload = p.Workload,
                        Attachments = p.Attachments
                    }).ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewCrudEventHistoricTemp> ListEventHistoricTemp(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {


                int skip = (count * (page - 1));
                var detail = serviceEventHistoricTemp.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceEventHistoricTemp.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewCrudEventHistoricTemp()
                {
                    _id = p._id,
                    Name = p.Name,
                    _idPerson = p.Person._id,
                    NamePerson = p.Person.Name,
                    StatusEventHistoricTemp = p.StatusEventHistoricTemp,
                    Attachments = p.Attachments,
                    Begin = p.Begin,
                    End = p.End,
                    Entity = p.Entity,
                    Observation = p.Observation,
                    Workload = p.Workload
                }).OrderBy(p => p.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewCrudEventHistoricTemp> ListEventHistoricTempWait(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {


                int skip = (count * (page - 1));
                var detail = serviceEventHistoricTemp.GetAllNewVersion(p => p.StatusEventHistoricTemp == EnumStatusEventHistoricTemp.Wait
                & p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result.OrderBy(p => p.Person.Name).ThenBy(p => p.Name).Skip(skip).Take(count).ToList();
                total = serviceEventHistoricTemp.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;

                return detail.Select(p => new ViewCrudEventHistoricTemp()
                {
                    _id = p._id,
                    Name = p.Name,
                    _idPerson = p.Person._id,
                    NamePerson = p.Person.Name,
                    StatusEventHistoricTemp = p.StatusEventHistoricTemp,
                    Attachments = p.Attachments,
                    Begin = p.Begin,
                    End = p.End,
                    Entity = p.Entity,
                    Observation = p.Observation,
                    Workload = p.Workload
                }).OrderBy(p => p.NamePerson).ThenBy(p => p.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ViewCrudEventHistoricTemp NewEventHistoricTemp(ViewCrudEventHistoricTemp view)
        {
            try
            {

                view.Entity = AddEntity(view.Entity.Name);
                var model = new EventHistoricTemp()
                {
                    Begin = view.Begin,
                    End = view.End,
                    Attachments = view.Attachments,
                    Entity = view.Entity,
                    Name = view.Name,
                    Workload = view.Workload * 60,
                    StatusEventHistoricTemp = EnumStatusEventHistoricTemp.Wait,
                    Observation = view.Observation,
                    Person = new ViewListPersonBase() { _id = view._idPerson, Name = view.NamePerson }
                };
                var eventhistoric = serviceEventHistoricTemp.InsertNewVersion(model).Result;
                return new ViewCrudEventHistoricTemp()
                {
                    _id = eventhistoric._id,
                    Begin = eventhistoric.Begin,
                    Name = eventhistoric.Name,
                    End = eventhistoric.End,
                    Workload = eventhistoric.Workload,
                    _idPerson = eventhistoric.Person._id,
                    NamePerson = eventhistoric.Person.Name,
                    Entity = eventhistoric.Entity,
                    Event = eventhistoric.Event,
                    StatusEventHistoricTemp = eventhistoric.StatusEventHistoricTemp,
                    Attachments = eventhistoric.Attachments,
                    Observation = eventhistoric.Observation
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UpdateEventHistoricTemp(ViewCrudEventHistoricTemp view)
        {
            try
            {
                if (view.Workload.ToString().Contains(","))
                    view.Workload = decimal.Parse(TimeSpan.Parse(view.Workload.ToString().Split(",")[0].PadLeft(2, '0') + ":" + view.Workload.ToString().Split(",")[1].PadRight(2, '0')).TotalMinutes.ToString());
                else
                    view.Workload *= 60;

                view.Entity = AddEntity(view.Entity.Name);
                var model = serviceEventHistoricTemp.GetNewVersion(p => p._id == view._id).Result;
                model.Begin = view.Begin;
                model.End = view.End;
                model.Attachments = view.Attachments;
                model.Entity = view.Entity;
                model.Name = view.Name;
                model.Workload = view.Workload;
                model.Observation = view.Observation;

                serviceEventHistoricTemp.Update(model, null).Wait();
                return "update";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ViewCrudEventHistoricTemp GetEventHistoricTemp(string id)
        {
            try
            {

                var eventhistoric = serviceEventHistoricTemp.GetFreeNewVersion(p => p._id == id).Result;
                Task.Run(() => LogSave(_user._idPerson, "Get Historic by ID"));

                return new ViewCrudEventHistoricTemp()
                {
                    _id = eventhistoric._id,
                    Begin = eventhistoric.Begin,
                    Name = eventhistoric.Name,
                    End = eventhistoric.End,
                    Workload = eventhistoric.Workload,
                    _idPerson = eventhistoric.Person._id,
                    NamePerson = eventhistoric.Person.Name,
                    Entity = eventhistoric.Entity,
                    Event = eventhistoric.Event,
                    StatusEventHistoricTemp = eventhistoric.StatusEventHistoricTemp,
                    Attachments = eventhistoric.Attachments,
                    Observation = eventhistoric.Observation
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string SetStatusEventHistoricTemp(ViewObs view, EnumStatusEventHistoricTemp status, string id, string idcourse)
        {
            try
            {

                var course = new ViewListCourse();
                var model = serviceEventHistoricTemp.GetNewVersion(p => p._id == id).Result;
                if (idcourse != "")
                    course = serviceCourse.GetNewVersion(p => p._id == idcourse).Result.GetViewList();

                model.StatusEventHistoricTemp = status;
                model.Observation = view.Observation;
                var x = serviceEventHistoricTemp.Update(model, null);

                if (status == EnumStatusEventHistoricTemp.Approved)
                {
                    NewEventHistoric(new EventHistoric()
                    {
                        Name = model.Name,
                        Course = course,
                        Entity = model.Entity,
                        Workload = model.Workload,
                        Person = model.Person,
                        Begin = model.Begin,
                        End = model.End,
                        Attachments = model.Attachments
                    });
                }

                return "ok";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetAttachmentHistoricTemp(string idevent, string url, string fileName, string attachmentid)
        {
            try
            {
                var eventsHistoricTemp = serviceEventHistoricTemp.GetAllNewVersion(p => p._id == idevent).Result.FirstOrDefault();

                if (eventsHistoricTemp.Attachments == null)
                {
                    eventsHistoricTemp.Attachments = new List<ViewCrudAttachmentField>();
                }
                eventsHistoricTemp.Attachments.Add(new ViewCrudAttachmentField { Url = url, Name = fileName, _idAttachment = attachmentid });
                serviceEventHistoricTemp.Update(eventsHistoricTemp, null).Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string RemoveEventHistoricTemp(string id)
        {
            try
            {
                var item = serviceEventHistoricTemp.GetAllNewVersion(p => p._id == id).Result.FirstOrDefault();
                item.Status = EnumStatus.Disabled;
                serviceEventHistoricTemp.Update(item, null).Wait();
                return "deleted";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


        #region private

        private async Task<string> GenerateQRCode(string idevent, string idparticipant)
        {
            try
            {
                var data = idevent + ";" + idparticipant;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data,
                QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20);
                var stream = new MemoryStream();
                qrCodeImage.Save(stream, ImageFormat.Bmp);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobkey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_user._idAccount);
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }
                CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(string.Format("{0}{1}", data.Replace(";", ""), ".bmp"));
                blockBlob.Properties.ContentType = "image/bmp";
                await blockBlob.UploadFromStreamAsync(stream);

                return blockBlob.Uri.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void UpdateAddDaysParticipant(ref Event events, ViewCrudDaysEvent days)
        {
            try
            {
                foreach (var item in events.Participants)
                {
                    item.FrequencyEvent.Add(new ViewCrudFrequencyEvent()
                    {
                        DaysEvent = days,
                        Present = true,
                        _id = ObjectId.GenerateNewId().ToString()
                    });
                }
                //serviceEvent.Update(events, null);

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void GenerateHistoric(Event view)
        {
            try
            {
                foreach (var item in view.Participants)
                {
                    if (item.Approved & (item.Grade >= view.Grade))
                    {
                        NewEventHistoric(new EventHistoric()
                        {
                            Name = view.Name,
                            Event = new ViewListEvent()
                            {
                                _id = view._id,
                                Name = view.Name,
                                NameCourse = view.Course?.Name,
                                _idCourse = view.Course?._id
                            },
                            Course = view.Course,
                            Entity = view.Entity,
                            Workload = view.Workload,
                            Person = new ViewListPersonBase() { _id = item.Person?._id, Name = item.Person?.Name },
                            Status = EnumStatus.Enabled,
                            Begin = DateTime.Parse(view.Begin.ToString()),
                            End = DateTime.Parse(view.End.ToString()),
                            Attachments = view.Attachments
                        });
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void MathWorkload(ref Event events)
        {
            try
            {
                if (events.Days.Count() > 0)
                {
                    events.Begin = events.Days.Min(p => p.Begin);
                    events.End = events.Days.Max(p => p.End);
                }
                else
                {
                    events.Begin = null;
                    events.End = null;
                }
                decimal workload = 0;
                foreach (var item in events.Days)
                {
                    workload += decimal.Parse((item.End - item.Begin).TotalMinutes.ToString());
                }
                events.Workload = workload;
                //serviceEvent.Update(events, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private ViewCrudEntity AddEntity(string name)
        {
            try
            {
                var entity = serviceEntity.GetFreeNewVersion(p => p.Name.ToUpper().Contains(name.ToUpper())).Result;
                if (entity == null)
                    entity = serviceEntity.InsertNewVersion(new Entity()
                    {
                        Status = EnumStatus.Enabled,
                        Name = name
                    }).Result;

                return new ViewCrudEntity()
                {
                    _id = entity._id,
                    Name = entity.Name
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void VerifyEquivalent(Course course)
        {
            try
            {
                if (course.Equivalents != null)
                {
                    foreach (var item in course.Equivalents)
                    {

                        var list = serviceTrainingPlan.GetAllNewVersion(p => p.Course._id == item._id & p.StatusTrainingPlan == EnumStatusTrainingPlan.Open).Result.ToList();
                        foreach (var plan in list)
                        {
                            var eventsHis = serviceEventHistoric.GetAllNewVersion(p => p.Course._id == course._id & p.Person._id == plan.Person._id).Result;
                            if (eventsHis.Count() > 0)
                            {
                                plan.StatusTrainingPlan = EnumStatusTrainingPlan.Realized;
                                plan.Observartion = "Realized Event: " + eventsHis.LastOrDefault().Name + ", ID_Historic: " + eventsHis.LastOrDefault()._id;
                                serviceTrainingPlan.Update(plan, null).Wait();
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void LogSave(string iduser, string local)
        {
            try
            {
                var user = servicePerson.GetAllNewVersion(p => p._id == iduser).Result.FirstOrDefault();
                var log = new ViewLog()
                {
                    Description = "Access Event ",
                    Local = local,
                    _idPerson = user._id
                };
                serviceLog.NewLog(log);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string ImportTraning(Stream stream)
        {
            try
            {
                var serviceExcel = new ServiceExcel();
                var list = serviceExcel.ImportTraning(stream);

                foreach (var item in list)
                {
                    try
                    {
                        var course = serviceCourse.GetNewVersion(p => p.Name == item.NameCourse).Result;
                        if (course == null)
                        {
                            course = new Course()
                            {
                                Name = item.NameCourse,
                                Content = item.Content,
                                Wordkload = item.Workload
                            };
                            course = serviceCourse.InsertNewVersion(course).Result;
                        }

                        var entity = serviceEntity.GetNewVersion(p => p.Name == item.NameEntity).Result;
                        if (entity == null)
                            entity = serviceEntity.InsertNewVersion(new Entity { Name = item.NameEntity }).Result;

                        var cpf = item.Cpf.Replace(".", "").Replace("-", "").Trim().PadLeft(11, '0');
                        var person = servicePerson.GetNewVersion(p => p.User.Document == cpf).Result;
                        if (person != null)
                        {
                            var eventHistoric = new EventHistoric()
                            {
                                Course = course.GetViewList(),
                                Workload = item.Workload,
                                Begin = item.DateBegin.Value,
                                End = item.DateEnd.Value,
                                Name = item.NameEvent,
                                Entity = entity.GetCrudEntity(),
                                Event = new ViewListEvent()
                                {
                                    Name = item.NameEvent,
                                    NameCourse = item.NameCourse,
                                    _idCourse = course._id,
                                    _id = ObjectId.GenerateNewId().ToString()
                                },
                                Person = person.GetViewListBase()
                            };

                            var hst = serviceEventHistoric.InsertNewVersion(eventHistoric).Result;
                        }
                    }
                    catch (Exception e)
                    {
                        var x = e.Message;
                    }

                }
                //throw new Exception("not_person");



                return "import_ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        #endregion

    }
#pragma warning restore 1998
}
