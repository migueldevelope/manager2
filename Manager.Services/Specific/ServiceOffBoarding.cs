using Manager.Core.Base;
using Manager.Core.Business;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Commons;
using Manager.Views.BusinessCrud;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Services.Specific
{
    public class ServiceOffBoarding : Repository<OffBoarding>, IServiceOffBoarding
    {
        private readonly ServiceGeneric<OffBoarding> serviceOffBoarding;
        private readonly ServiceGeneric<Person> servicePerson;
        private readonly ServiceGeneric<Questions> serviceQuestions;

        #region Constructor
        public ServiceOffBoarding(DataContext context) : base(context)
        {
            try
            {
                serviceOffBoarding = new ServiceGeneric<OffBoarding>(context);
                servicePerson = new ServiceGeneric<Person>(context);
                serviceQuestions = new ServiceGeneric<Questions>(context);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void SetUser(IHttpContextAccessor contextAccessor)
        {
            User(contextAccessor);
            serviceOffBoarding._user = _user;
            servicePerson._user = _user;
            serviceQuestions._user = _user;
        }
        public void SetUser(BaseUser user)
        {
            _user = user;
            serviceOffBoarding._user = user;
            servicePerson._user = user;
            serviceQuestions._user = _user;
        }
        #endregion

        #region OffBoarding
        public string Delete(string id)
        {
            try
            {
                OffBoarding item = serviceOffBoarding.GetNewVersion(p => p._id == id).Result;
                item.Status = EnumStatus.Disabled;
                serviceOffBoarding.Update(item, null).Wait();
                return "OffBoarding deleted!";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string New(string idperson, EnumStepOffBoarding step)
        {
            try
            {
                OffBoarding offboarding = null;
                try
                {
                    offboarding = serviceOffBoarding.GetNewVersion(p => p.Person._id == idperson).Result;
                }
                catch (Exception)
                {
                    offboarding = new OffBoarding();
                }


                if (offboarding == null)
                {
                    var person = servicePerson.GetNewVersion(p => p._id == idperson).Result.GetViewListPersonInfo();

                    var view = new OffBoarding()
                    {
                        Person = person
                    };
                    view.Step1 = LoadMap(view.Step1, EnumStepOffBoarding.Step1);
                    view.Step2 = LoadMap(view.Step1, EnumStepOffBoarding.Step2);
                    if (step == EnumStepOffBoarding.Step1)
                    {
                        view.DateBeginStep1 = DateTime.Now;
                    }
                    else
                    {
                        view.DateBeginStep2 = DateTime.Now;
                    }
                    offboarding = serviceOffBoarding.InsertNewVersion(view).Result;
                }
                else
                {
                    if (step == EnumStepOffBoarding.Step1)
                    {
                        if (offboarding.DateBeginStep1 == null)
                        {
                            offboarding.DateBeginStep1 = DateTime.Now;
                        }
                    }
                    else
                    {
                        if (offboarding.DateBeginStep2 == null)
                        {
                            offboarding.DateBeginStep2 = DateTime.Now;
                        }
                    }
                    var i = serviceOffBoarding.Update(offboarding, null);
                }



                return offboarding._id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string Update(ViewCrudOffBoarding view, EnumStepOffBoarding step)
        {
            try
            {
                OffBoarding offboarding = serviceOffBoarding.GetNewVersion(p => p._id == view._id).Result;
                if (step == EnumStepOffBoarding.Step1)
                {

                }
                serviceOffBoarding.Update(offboarding, null).Wait();

                return "OffBoarding altered!";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UpdateQuestionsMark(string id, EnumStepOffBoarding step, string idquestion, byte mark)
        {
            try
            {
                OffBoarding offboarding = serviceOffBoarding.GetNewVersion(p => p._id == id).Result;

                if (step == EnumStepOffBoarding.Step1)
                {
                    foreach (var item in offboarding.Step1.Questions)
                    {
                        if (item.Question._id == idquestion)
                        {
                            item.Mark = mark;
                            var i = serviceOffBoarding.Update(offboarding, null);
                            return "ok";
                        }

                    }
                }
                else
                {
                    foreach (var item in offboarding.Step2.Questions)
                    {
                        if (item.Question._id == idquestion)
                        {
                            item.Mark = mark;
                            var i = serviceOffBoarding.Update(offboarding, null);
                            return "ok";
                        }

                    }
                }

                return "ok";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string UpdateQuestionsText(string id, EnumStepOffBoarding step, string idquestion, ViewResponse response)
        {
            try
            {
                OffBoarding offboarding = serviceOffBoarding.GetNewVersion(p => p._id == id).Result;

                if (step == EnumStepOffBoarding.Step1)
                {
                    foreach (var item in offboarding.Step1.Questions)
                    {
                        if (item.Question._id == idquestion)
                        {
                            item.Response = response.Response;
                            var i = serviceOffBoarding.Update(offboarding, null);
                            return "ok";
                        }

                    }
                }
                else
                {
                    foreach (var item in offboarding.Step2.Questions)
                    {
                        if (item.Question._id == idquestion)
                        {
                            item.Response = response.Response;
                            var i = serviceOffBoarding.Update(offboarding, null);
                            return "ok";
                        }

                    }
                }

                return "ok";
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ViewCrudOffBoarding Get(string id)
        {
            try
            {
                return serviceOffBoarding.GetNewVersion(p => p._id == id).Result.GetViewCrud();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ViewListOffBoarding> List(ref long total, int count = 10, int page = 1, string filter = "")
        {
            try
            {
                List<ViewListOffBoarding> detail = serviceOffBoarding.GetAllNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper()), count, count * (page - 1), "Person.Name").Result
                  .Select(x => new ViewListOffBoarding()
                  {
                      _id = x._id,
                      Name = x.Person.Name
                  }).ToList();
                total = serviceOffBoarding.CountNewVersion(p => p.Person.Name.ToUpper().Contains(filter.ToUpper())).Result;
                return detail;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region private 

        private ViewCrudFormOffBoarding LoadMap(ViewCrudFormOffBoarding offboarding, EnumStepOffBoarding step)
        {
            try
            {
                offboarding.Questions = new List<ViewCrudOffBoardingQuestions>();
                var itens = new List<ViewCrudOffBoardingQuestions>();

                foreach (var item in serviceQuestions.GetAllNewVersion(p => p.TypeQuestion == EnumTypeQuestion.Rating & p.TypeRotine == EnumTypeRotine.OffBoarding).Result)
                {
                    offboarding.Questions.Add(new ViewCrudOffBoardingQuestions()
                    {
                        Question =
                      new ViewCrudQuestions()
                      {
                          _id = item._id,
                          Content = item.Content,
                          Name = item.Name,
                          Order = item.Order,
                          TypeQuestion = item.TypeQuestion,
                          TypeRotine = item.TypeRotine
                      },
                        _id = ObjectId.GenerateNewId().ToString()
                    });
                }
                if (step == EnumStepOffBoarding.Step2)
                {
                    foreach (var item in serviceQuestions.GetAllNewVersion(p => p.TypeQuestion == EnumTypeQuestion.Text & p.TypeRotine == EnumTypeRotine.OffBoarding).Result)
                    {
                        offboarding.Questions.Add(new ViewCrudOffBoardingQuestions()
                        {
                            Question =
                           new ViewCrudQuestions()
                           {
                               _id = item._id,
                               Content = item.Content,
                               Name = item.Name,
                               Order = item.Order,
                               TypeQuestion = item.TypeQuestion,
                               TypeRotine = item.TypeRotine
                           },
                            _id = ObjectId.GenerateNewId().ToString(),
                        });
                    }
                }


                return offboarding;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

    }
}
