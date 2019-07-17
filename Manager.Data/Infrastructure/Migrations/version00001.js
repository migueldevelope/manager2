db.AutoManager.drop();
db.Certification.drop();
db.CertificationPerson.drop();
db.CompanyMandatory.drop();
db.ConfigurationNotification.drop();
db.Course.drop();
db.CourseESocial.drop();
db.DictionarySphere.drop();
db.DictionarySystem.drop();
db.Entity.drop();
db.Event.drop();
db.EventESocial.drop();
db.EventHistoric.drop();
db.Goals.drop();
db.GoalsCompany.drop();
db.GoalsManager.drop();
db.GoalsPeriod.drop();
db.GoalsPerson.drop();
db.GoalsPersonControl.drop();
db.HistoricPerson.drop();
db.MandatoryTraining.drop();
db.Maturity.drop();
db.MaturityRegister.drop();
db.MonitoringOld.drop();
db.PlanOld.drop();
db.Meritocracy.drop();
db.MeritocracyActivities.drop();
db.MeritocracyScore.drop();
db.OccupationMandatory.drop();
db.PersonMandatory.drop();
db.TrainingPlan.drop();
db.Workflow.drop();
db.StructPlan.drop();





db.Area.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        Order: item.Order,
        Template: null
    };
    if (item.Template != null)
        model.Template = item.Template._id;

    db.AreaNew.insert(model);
});

db.Area.renameCollection('old_Area');
db.AreaNew.renameCollection('Area');





db.Axis.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        TypeAxis: item.TypeAxis,
        Template: null
    };
    if (item.Template != null)
        model.Template = item.Template._id;

    db.AxisNew.insert(model);
});


db.Axis.renameCollection('old_Axis');
db.AxisNew.renameCollection('Axis');





db.Log.find({}).forEach(function (item) {
    var log = {
        _id: item._id,
        Status: item.Status,
        Person: null,
        Description: item.Description,
        DataLog: item.DataLog,
        Local: item.Local,
    };
    if (item.Person != null) {
        log.Person = {
            _id: item.Person._id,
            Name: item.Person.User.Name
        }
    }

    db.LogNew.insert(log);
});

db.Log.renameCollection('old_Log');
db.LogNew.renameCollection('Log');





db.Company.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        _idAccount: item._idAccount,
        Status: item.Status,
        Logo: item.Logo,
        Template: null,
        Skills: []
    };
    if (item.Skills != null) {
        var i = 0;
        item.Skills.forEach(function (skill) {
            var view = {
                _id: skill._id,
                Name: skill.Name,
                Concept: skill.Concept,
                TypeSkill: skill.TypeSkill
            };
            model.Skills[i] = view;
            i += 1;
        });
    }

    db.CompanyNew.insert(model);
});


db.Company.renameCollection('old_Company');
db.CompanyNew.renameCollection('Company');





db.Establishment.find({}).forEach(function (item) {
    db.EstablishmentNew.insert({
        _id: item._id,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status
    });
});

db.Establishment.renameCollection('old_Establishment');
db.EstablishmentNew.renameCollection('Establishment');





db.LogMessages.find({}).forEach(function (item) {
    db.LogMessagesNew.insert({
        _id: item._id,
        Subject: item.Subject,
        Message: item.Message,
        StatusMessage: item.StatusMessage,
        Person: {
            _id: item.Person._id,
            Name: item.Person.User.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status
    });
});

db.LogMessages.renameCollection('old_LogMessages');
db.LogMessagesNew.renameCollection('LogMessages');





db.ProcessLevelOne.find({}).forEach(function (item) {
    db.ProcessLevelOneNew.insert({
        _id: item._id,
        Name: item.Name,
        Area: {
            _id: item.Area._id,
            Name: item.Area.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        Order: item.Order
    });
});

db.ProcessLevelOne.renameCollection('old_ProcessLevelOne');
db.ProcessLevelOneNew.renameCollection('ProcessLevelOne');





db.ProcessLevelTwo.find({}).forEach(function (item) {
    db.ProcessLevelTwoNew.insert({
        _id: item._id,
        Name: item.Name,
        ProcessLevelOne: {
            _id: item.ProcessLevelOne._id,
            Name: item.ProcessLevelOne.Name,
            Area: {
                _id: item.ProcessLevelOne.Area._id,
                Name: item.ProcessLevelOne.Area.Name
            },
            Order: item.ProcessLevelOne.Order
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        Order: item.Order
    });
});

db.ProcessLevelTwo.renameCollection('old_ProcessLevelTwo');
db.ProcessLevelTwoNew.renameCollection('ProcessLevelTwo');





db.Sphere.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        TypeSphere: item.TypeSphere,
        Template: null
    };
    if (item.Template != null)
        model.Template = item.Template._id;

    db.SphereNew.insert(model);
});

db.Sphere.renameCollection('old_Sphere');
db.SphereNew.renameCollection('Sphere');





db.TextDefault.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        Content: item.Content,
        TypeText: item.TypeText,
        Template: null
    };

    if (item.Template != null)
        model.Template = item.Template._id;

    db.TextDefaultNew.insert(model);
});

db.TextDefault.renameCollection('old_TextDefault');
db.TextDefaultNew.renameCollection('TextDefault');





db.Skill.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        _idAccount: item._idAccount,
        Status: item.Status,
        Concept: item.Concept,
        TypeSkill: item.TypeSkill,
        Template: null
    };

    if (item.Template != null)
        model.Template = item.Template._id;

    db.SkillNew.insert(model);
});

db.Skill.renameCollection('old_Skill');
db.SkillNew.renameCollection('Skill');





db.Schooling.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        Order: item.Order,
        _idAccount: item._idAccount,
        Status: item.Status,
        Complement: item.Complement,
        Type: item.Type,
        Template: null
    };

    if (item.Template != null)
        model.Template = item.Template._id;

    db.SchoolingNew.insert(model);
});

db.Schooling.renameCollection('old_Schooling');
db.SchoolingNew.renameCollection('Schooling');






db.User.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Name: item.Name,
        Document: item.Document,
        Mail: item.Mail,
        Phone: item.Phone,
        Password: item.Password,
        DateBirth: item.DateBirth,
        DateAdm: item.DateAdm,
        Schooling: null,
        PhotoUrl: item.PhotoUrl,
        Coins: item.Coins,
        ChangePassword: item.ChangePassword,
        ForeignForgotPassword: item.ForeignForgotPassword,
        PhoneFixed: item.PhoneFixed,
        DocumentID: item.DocumentID,
        DocumentCTPF: item.DocumentCTPF,
        Sex: item.Sex,
        Nickname: item.Nickname,
        UserAdmin: item.UserAdmin,
        UserTermOfServices: item.UserTermOfServices
    };

    if (item.Schooling != null) {
        model.Schooling = {
            _id: item.Schooling._id,
            Name: item.Schooling.Name,
            Order: item.Schooling.Order,
        };
    }

    db.UserNew.insert(model);
});

db.User.renameCollection('old_User');
db.UserNew.renameCollection('User');





db.Questions.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        _idAccount: item._idAccount,
        Status: item.Status,
        Content: item.Content,
        TypeQuestion: item.TypeQuestion,
        Order: item.Order,
        TypeRotine: item.TypeRotine,
        Template: null
    };

    if (item.Template != null)
        model.Template = item.Template._id;

    db.QuestionsNew.insert(model);
});

db.Questions.renameCollection('old_Questions');
db.QuestionsNew.renameCollection('Questions');






db.Group.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Name: item.Name,
        Company: {
            _id: item.Company._id,
            Name: item.Company.Name
        },
        Axis: {
            _id: item.Axis._id,
            Name: item.Axis.Name,
            TypeAxis: item.Axis.TypeAxis
        },
        Sphere: {
            _id: item.Sphere._id,
            Name: item.Sphere.Name,
            TypeSphere: item.Sphere.TypeSphere
        },
        Line: item.Line,
        Skills: [],
        Schooling: [],
        Scope: [],
        Template: null
    };

    if (item.Template != null)
        model.Template = item.Template._id;

    if (item.Skills != null) {
        var i = 0;
        item.Skills.forEach(function (skill) {
            var view = {
                _id: skill._id,
                Name: skill.Name,
                Concept: skill.Concept,
                TypeSkill: skill.TypeSkill
            };
            model.Skills[i] = view;
            i += 1;
        });
    }

    if (item.Scope != null) {
        var i = 0;
        item.Scope.forEach(function (scope) {
            var view = {
                _id: scope._id,
                Name: scope.Name,
                Order: scope.Order,
            };
            model.Scope[i] = view;
            i += 1;
        });
    }

    if (item.Schooling != null) {
        var i = 0;
        item.Schooling.forEach(function (schooling) {
            var view = {
                _id: schooling._id,
                Name: schooling.Name,
                Complement: schooling.Complement,
                Order: schooling.Order,
                Type: schooling.Type
            };
            model.Schooling[i] = view;
            i += 1;
        });
    }

    db.GroupNew.insert(model);
});

db.Group.renameCollection('old_Group');
db.GroupNew.renameCollection('Group');





db.Person.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        StatusUser: item.StatusUser,
        Company: null,
        Occupation: null,
        Manager: item.Manager,
        DocumentManager: item.DocumentManager,
        DateLastOccupation: item.DateLastOccupation,
        Salary: item.Salary,
        DateLastReadjust: item.DateLastReadjust,
        DateResignation: item.DateResignation,
        SalaryScales: item.SalaryScales,
        TypeJourney: item.TypeJourney,
        Establishment: null,
        HolidayReturn: item.HolidayReturn,
        MotiveAside: item.MotiveAside,
        TypeUser: item.TypeUser,
        Registration: item.Registration,
        User: null
    };

    if (item.User != null) {
        model.User = {
            _id: item.User._id,
            Name: item.User.Name,
            Document: item.User.Document,
            Mail: item.User.Mail,
            Phone: item.User.Phone,
            Password: item.User.Password,
            DateBirth: item.User.DateBirth,
            DateAdm: item.User.DateAdm,
            Schooling: null,
            PhotoUrl: item.User.PhotoUrl,
            PhoneFixed: item.User.PhoneFixed,
            DocumentID: item.User.DocumentID,
            DocumentCTPF: item.User.DocumentCTPF,
            Sex: item.User.Sex,
            Nickname: item.User.Nickname
        };
        if (item.User.Schooling != null) {
            model.User.Schooling = {
                _id: item.User.Schooling._id,
                Name: item.User.Schooling.Name,
                Order: item.User.Schooling.Order
            };
        }
    }
    if (item.Company != null) {
        model.Company = {
            _id: item.Company._id,
            Name: item.Company.Name
        };
    }

    if (item.Establishment != null) {
        model.Establishment = {
            _id: item.Establishment._id,
            Name: item.Establishment.Name
        };
    }
    if (item.Occupation != null) {
        model.Occupation = {
            _id: item.Occupation._id,
            Name: item.Occupation.Name,
            _idGroup: null,
            NameGroup: null,
            Cbo: null,
            _idArea: null
        };
        if (item.Occupation.Group != null) {
            model.Occupation._idGroup = item.Occupation.Group._id;
            model.Occupation.NameGroup = item.Occupation.Group.Name;
        }
        if (item.Occupation.Cbo != null) {
            model.Occupation.Cbo = {
                _id: item.Occupation.Cbo._id,
                Name: item.Occupation.Cbo.Name,
                Code: item.Occupation.Cbo.Code
            };
        }
        if (item.Occupation.Process != null) {
            if (item.Occupation.Process[0].ProcessLevelOne != null) {
                if (item.Occupation.Process[0].ProcessLevelOne.Area != null)
                    model.Occupation._idArea = item.Occupation.Process[0].ProcessLevelOne.Area._id;
            }
        }
    }

    db.PersonNew.insert(model);
});

db.Person.renameCollection('old_Person');
db.PersonNew.renameCollection('Person');





db.Occupation.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Name: item.Name,
        Group: null,
        Line: item.Line,
        Skills: [],
        Schooling: [],
        Activities: [],
        Template: null,
        Cbo: null,
        SpecificRequirements: item.SpecificRequirements,
        Process: [],
        SalaryScales: item.SalaryScales,
    };

    if (item.Group != null) {
        model.Group = {
            _id: item.Group._id,
            Name: item.Group.Name,
            Line: item.Group.Line,
            Company: null,
            Sphere: null,
            Axis: null
        };
        if (item.Group.Company != null) {
            model.Group.Company = {
                _id: item.Group.Company._id,
                Name: item.Group.Company.Name
            };
        }
        if (item.Group.Axis != null) {
            model.Group.Axis = {
                _id: item.Group.Axis._id,
                Name: item.Group.Axis.Name,
                TypeAxis: item.Group.Axis.TypeAxis
            };
        }
        if (item.Group.Sphere != null) {
            model.Group.Sphere = {
                _id: item.Group.Sphere._id,
                Name: item.Group.Sphere.Name,
                TypeSphere: item.Group.Sphere.TypeSphere
            };
        }
    }


    if (item.Cbo != null) {
        model.Cbo = {
            _id: item.Cbo._id,
            Name: item.Cbo.Name,
            Code: item.Cbo.Code
        };
    }

    if (item.Template != null)
        model.Template = item.Template._id;

    if (item.Skills != null) {
        var i = 0;
        item.Skills.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    Name: skill.Name,
                    Concept: skill.Concept,
                    TypeSkill: skill.TypeSkill
                };
                model.Skills[i] = view;
                i += 1;
            }
        });
    }

    if (item.Schooling != null) {
        var i = 0;
        item.Schooling.forEach(function (schooling) {
            var view = {
                _id: schooling._id,
                Name: schooling.Name,
                Complement: schooling.Complement,
                Order: schooling.Order,
                Type: schooling.Type
            };
            model.Schooling[i] = view;
            i += 1;
        });
    }

    if (item.Activities != null) {
        var i = 0;
        item.Activities.forEach(function (activities) {
            var view = {
                _id: activities._id,
                Name: activities.Name,
                Order: activities.Order
            };
            model.Activities[i] = view;
            i += 1;
        });
    }

    if (item.Process != null) {
        var i = 0;
        item.Process.forEach(function (process) {
            var view = {
                _id: process._id,
                Name: process.Name,
                Order: process.Order,
                ProcessLevelOne: []
            };

            if (process.ProcessLevelOne != null) {
                view.ProcessLevelOne = {
                    _id: process.ProcessLevelOne._id,
                    Name: process.ProcessLevelOne.Name,
                    Order: process.ProcessLevelOne.Order,
                    Area: {
                        _id: process.ProcessLevelOne.Area._id,
                        Name: process.ProcessLevelOne.Area.Name
                    }
                }
            }
            model.Process[i] = view;
            i += 1;
        });
    }

    db.OccupationNew.insert(model);
});

db.Occupation.renameCollection('old_Occupation');
db.OccupationNew.renameCollection('Occupation');





db.SalaryScale.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Grades: item.Grades,
        Company: null
    };

    if (item.Company != null) {
        model.Company = {
            _id: item.Company._id,
            Name: item.Company.Name,
        }
    }

    db.SalaryScaleNew.insert(model);
});

db.SalaryScale.renameCollection('old_SalaryScale');
db.SalaryScaleNew.renameCollection('SalaryScale');





db.Plan.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Name: item.Name,
        Description: item.Description,
        Deadline: item.Deadline,
        Skills: [],
        DateInclude: item.DateInclude,
        TypePlan: item.TypePlan,
        SourcePlan: item.SourcePlan,
        TypeAction: item.TypeAction,
        StatusPlan: item.StatusPlan,
        TextEnd: item.TextEnd,
        TextEndManager: item.TextEndManager,
        DateEnd: item.DateEnd,
        Evaluation: item.Evaluation,
        Result: item.Result,
        StatusPlanApproved: item.StatusPlanApproved,
        Attachments: item.Attachments,
        NewAction: item.NewAction,
        StructPlans: [],
        _idMonitoring: item._idMonitoring,
        _idItem: item._idItem,
        Person: null
    };

    if (item.Skills != null) {
        var i = 0;
        item.Skills.forEach(function (skill) {
            var view = {
                _id: skill._id,
                Name: skill.Name,
                Concept: skill.Concept,
                TypeSkill: skill.TypeSkill
            };
            model.Skills[i] = view;
            i += 1;
        });
    }

    if (item.Person != null) {
        item.Person = {
            _id: item.Person._id,
            Name: item.Person.User.Name,
            _idManager: null,
            Manager: null
        };
        if (item.Person.Manager != null) {
            model.Person._idManager = item.Person.Manager._id;
            model.Person.Manager = item.Person.Manager.Name;
        }
    }

    if (item.StructPlans != null) {
        var i = 0;
        item.StructPlans.forEach(function (struct) {
            var view = {
                _id: struct._id,
                Course: null,
                TypeAction: struct.TypeAction,
                TypeResponsible: struct.TypeResponsible,
                PlanActivity: null
            };
            if (struct.Course != null) {
                struct.Course = {
                    _id: struct.Course._id,
                    Name: struct.Course.Name
                };
            }
            if (struct.PlanActivity != null) {
                struct.PlanActivity = {
                    _id: struct.PlanActivity._id,
                    Name: struct.PlanActivity.Name
                };
            }

            model.StructPlans[i] = view;
            i += 1;
        });
    }

    db.PlanNew.insert(model);
});

db.Plan.update({ SourcePlan: null }, { $set: { Evaluation: 0, SourcePlan: 0, TypeAction: 0, TypePlan: 0, StatusPlan: 0, StatusPlanApproved: 0, NewAction: 0 } }, { multi: true });
db.Plan.update({ NewAction: null }, { $set: { NewAction: 0 } }, { multi: true });

db.Plan.renameCollection('old_Plan');
db.PlanNew.renameCollection('Plan');






db.Checkpoint.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        DateBegin: item.DateBegin,
        DateEnd: item.DateEnd,
        TextDefault: item.TextDefault,
        Comments: item.Comments,
        StatusCheckpoint: item.StatusCheckpoint,
        DataAccess: item.DataAccess,
        TypeCheckpoint: item.TypeCheckpoint,
        Person: null,
        Occupation: null,
        Questions: []
    };

    if (item.Person != null) {
        model.Person = {
            _id: item.Person._id,
            Name: item.Person.User.Name,
            TypeJourney: item.Person.TypeJourney,
            Occupation: null,
            _idManager: null,
            Manager: null
        };
        if (item.Person.Occupation != null)
            model.Person.Occupation = item.Person.Occupation.Name;

        if (item.Person.Manager != null) {
            model.Person._idManager = item.Person.Manager._id;
            model.Person.Manager = item.Person.Manager.Name;
        }
    }

    if (item.Occupation != null) {
        model.Occupation = {
            _id: item.Occupation._id,
            Name: item.Occupation.Name,
            _idGroup: item.Occupation.Group._id,
            NameGroup: item.Occupation.Group.Name,
            Cbo: null,
            _idArea: null,
        };
        if (item.Occupation.Cbo != null) {
            model.Cbo = {
                _id: item.Occupation.Cbo._id,
                Name: item.Occupation.Cbo.Name,
                Code: item.Occupation.Cbo.Code
            };
        }
        if (item.Occupation.Process != null) {
            if (item.Occupation.Process[0].ProcessLevelOne != null) {
                if (item.Occupation.Process[0].ProcessLevelOne.Area != null)
                    model.Occupation._idArea = item.Occupation.Process[0].ProcessLevelOne.Area._id;
            }
        }
    }

    if (item.Questions != null) {
        var i = 0;
        item.Questions.forEach(function (questions) {
            if (questions != null) {
                var view = {
                    _id: questions._id,
                    Mark: questions.Mark,
                    Question: null,
                    Itens: []
                };
                if (questions.Question != null) {
                    view.Question = {
                        _id: questions.Question._id,
                        Name: questions.Question.Name,
                        Content: questions.Question.Content,
                        TypeQuestion: questions.Question.TypeQuestion,
                        Order: questions.Question.Order,
                        TypeRotine: questions.Question.TypeRotine
                    };
                }

                if (questions.Itens != null) {
                    var x = 0;
                    questions.Itens.forEach(function (itq) {
                        var viewItq = {
                            _id: itq._id,
                            Mark: itq.Mark,
                            Question: null
                        };
                        if (itq.Question != null) {
                            viewItq.Question = {
                                _id: itq.Question._id,
                                Name: itq.Question.Name,
                                Content: itq.Question.Content,
                                TypeQuestion: itq.Question.TypeQuestion,
                                Order: itq.Question.Order,
                                TypeRotine: itq.Question.TypeRotine
                            };
                        }
                        view.Itens[x] = viewItq;
                    });
                }

                model.Questions[i] = view;
                i += 1;
            }

        });

    }

    db.CheckpointNew.insert(model);
});


db.Checkpoint.renameCollection('old_Checkpoint');
db.CheckpointNew.renameCollection('Checkpoint');





db.OnBoarding.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Person: null,
        DateBeginPerson: item.DateBeginPerson,
        DateBeginManager: item.DateBeginManager,
        DateBeginEnd: item.DateBeginEnd,
        DateEndPerson: item.DateEndPerson,
        DateEndManager: item.DateEndManager,
        DateEndEnd: item.DateEndEnd,
        CommentsPerson: item.CommentsPerson,
        CommentsManager: item.CommentsManager,
        CommentsEnd: item.CommentsEnd,
        SkillsCompany: [],
        SkillsGroup: [],
        SkillsOccupation: [],
        Scopes: [],
        Schoolings: [],
        Activities: [],
        StatusOnBoarding: item.StatusOnBoarding
    };

    if (item.Person != null) {
        model.Person = {
            _id: item.Person._id,
            Name: null,
            TypeJourney: item.Person.TypeJourney,
            Occupation: null,
            _idManager: null,
            Manager: null
        };
        if (item.Person.User != null)
            model.Person.Name = item.Person.User.Name;

        if (item.Person.Occupation != null)
            model.Person.Occupation = item.Person.Occupation.Name;

        if (item.Person.Manager != null) {
            model.Person._idManager = item.Person.Manager._id;
            model.Person.Manager = item.Person.Manager.Name;
        }
    }

    if (item.SkillsCompany != null) {
        var i = 0;
        item.SkillsCompany.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Skill: {
                        _id: skill.Skill._id,
                        Name: skill.Skill.Name,
                        Concept: skill.Skill.Concept,
                        TypeSkill: skill.Skill.TypeSkill
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment,
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                model.SkillsCompany[i] = view;
                i += 1;
            }
        });
    }

    if (item.SkillsGroup != null) {
        var i = 0;
        item.SkillsGroup.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Skill: {
                        _id: skill.Skill._id,
                        Name: skill.Skill.Name,
                        Concept: skill.Skill.Concept,
                        TypeSkill: skill.Skill.TypeSkill
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment,
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                model.SkillsGroup[i] = view;
                i += 1;
            }
        });
    }

    if (item.SkillsOccupation != null) {
        var i = 0;
        item.SkillsOccupation.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Skill: {
                        _id: skill.Skill._id,
                        Name: skill.Skill.Name,
                        Concept: skill.Skill.Concept,
                        TypeSkill: skill.Skill.TypeSkill
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment,
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                model.SkillsOccupation[i] = view;
                i += 1;
            }
        });
    }

    if (item.Scopes != null) {
        var i = 0;
        item.Scopes.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Scope: {
                        _id: skill.Scope._id,
                        Name: skill.Scope.Name,
                        Order: skill.Scope.Order
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment,
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                model.Scopes[i] = view;
                i += 1;
            }
        });
    }

    if (item.Schoolings != null) {
        var i = 0;
        item.Schoolings.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Schooling: {
                        _id: skill.Schooling._id,
                        Name: skill.Schooling.Name,
                        Order: skill.Schooling.Order,
                        Complement: skill.Schooling.Complement,
                        Type: skill.Schooling.Type
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment,
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                model.Schoolings[i] = view;
                i += 1;
            }
        });
    }

    if (item.Activities != null) {
        var i = 0;
        item.Activities.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Activitie: {
                        _id: skill.Activitie._id,
                        Name: skill.Activitie.Name,
                        Order: skill.Activitie.Order
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment,
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                model.Activities[i] = view;
                i += 1;
            }
        });
    }

    db.OnBoardingNew.insert(model);
});

db.OnBoarding.renameCollection('old_OnBoarding');
db.OnBoardingNew.renameCollection('OnBoarding');





db.Monitoring.find({}).forEach(function (item) {
    var model = {
        _id: item._id,
        _idAccount: item._idAccount,
        Status: item.Status,
        Person: null,
        DateBeginPerson: item.DateBeginPerson,
        DateBeginManager: item.DateBeginManager,
        DateBeginEnd: item.DateBeginEnd,
        DateEndPerson: item.DateEndPerson,
        DateEndManager: item.DateEndManager,
        DateEndEnd: item.DateEndEnd,
        CommentsPerson: item.CommentsPerson,
        CommentsManager: item.CommentsManager,
        CommentsEnd: item.CommentsEnd,
        SkillsCompany: [],
        Schoolings: [],
        Activities: [],
        StatusMonitoring: item.StatusMonitoring
    };

    if (item.Person != null) {
        model.Person = {
            _id: item.Person._id,
            Name: item.Person.User.Name,
            TypeJourney: item.Person.TypeJourney,
            Occupation: null,
            _idManager: null,
            Manager: null
        };
        if (item.Person.Occupation != null)
            model.Person.Occupation = item.Person.Occupation.Name;

        if (item.Person.Manager != null) {
            model.Person._idManager = item.Person.Manager._id;
            model.Person.Manager = item.Person.Manager.Name;
        }
    }

    if (item.SkillsCompany != null) {
        var i = 0;
        item.SkillsCompany.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    Plans: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Praise: skill.Praise,
                    Skill: {
                        _id: skill.Skill._id,
                        Name: skill.Skill.Name,
                        Concept: skill.Skill.Concept,
                        TypeSkill: skill.Skill.TypeSkill
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                if (skill.Plans != null) {
                    var x = 0;
                    skill.Plans.forEach(function (plan) {

                        var viewPlan = {
                            _id: plan._id,
                            Name: plan.Name,
                            Description: plan.Description,
                            Deadline: plan.Deadline,
                            Skills: [],
                            TypePlan: plan.TypePlan,
                            SourcePlan: plan.SourcePlan,
                            StatusPlan: plan.StatusPlan,
                            StatusPlanApproved: plan.StatusPlanApproved,
                            TypeAction: plan.TypeAction,
                            Attachments: plan.Attachments,
                            NewAction: plan.NewAction,
                            TextEnd: plan.TextEnd,
                            TextEndManager: plan.TextEndManager,
                            Evaluation: plan.Evaluation
                        };
                        if (plan.Skills != null) {
                            var z = 0;
                            plan.Skills.forEach(function (planSkill) {
                                var viewPlanSkill = {
                                    _id: planSkill._id,
                                    Name: planSkill.Name,
                                    Concept: planSkill.Concept,
                                    TypeSkill: planSkill.TypeSkill
                                };
                                viewPlan.Skills[z] = viewPlanSkill;
                            });
                        }
                        view.Plans[x] = viewPlan;
                    });
                }


                model.SkillsCompany[i] = view;
                i += 1;
            }
        });
    }

    if (item.Schoolings != null) {
        var i = 0;
        item.Schoolings.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    Plans: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Praise: skill.Praise,
                    Schooling: {
                        _id: skill.Schooling._id,
                        Name: skill.Schooling.Name,
                        Order: skill.Schooling.Order,
                        Complement: skill.Schooling.Complement,
                        Type: skill.Schooling.Type
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                if (skill.Plans != null) {
                    var x = 0;
                    skill.Plans.forEach(function (plan) {

                        var viewPlan = {
                            _id: plan._id,
                            Name: plan.Name,
                            Description: plan.Description,
                            Deadline: plan.Deadline,
                            Skills: [],
                            TypePlan: plan.TypePlan,
                            SourcePlan: plan.SourcePlan,
                            StatusPlan: plan.StatusPlan,
                            StatusPlanApproved: plan.StatusPlanApproved,
                            TypeAction: plan.TypeAction,
                            Attachments: plan.Attachments,
                            NewAction: plan.NewAction,
                            TextEnd: plan.TextEnd,
                            TextEndManager: plan.TextEndManager,
                            Evaluation: plan.Evaluation
                        };
                        if (plan.Skills != null) {
                            var z = 0;
                            plan.Skills.forEach(function (planSkill) {
                                var viewPlanSkill = {
                                    _id: planSkill._id,
                                    Name: planSkill.Name,
                                    Concept: planSkill.Concept,
                                    TypeSkill: planSkill.TypeSkill
                                };
                                viewPlan.Skills[z] = viewPlanSkill;
                            });
                        }
                        view.Plans[x] = viewPlan;
                    });
                }


                model.Schoolings[i] = view;
                i += 1;
            }
        });
    }

    if (item.Activities != null) {
        var i = 0;
        item.Activities.forEach(function (skill) {
            if (skill != null) {
                var view = {
                    _id: skill._id,
                    CommentsManager: skill.CommentsManager,
                    CommentsPerson: skill.CommentsPerson,
                    Comments: [],
                    Plans: [],
                    StatusViewManager: skill.StatusViewManager,
                    StatusViewPerson: skill.StatusViewPerson,
                    Praise: skill.Praise,
                    Activities: {
                        _id: skill.Activities._id,
                        Name: skill.Activities.Name,
                        Order: skill.Activities.Order
                    }
                };
                if (skill.Comments != null) {
                    var x = 0;
                    skill.Comments.forEach(function (comment) {

                        var viewIt = {
                            Comments: comment.Comments,
                            Date: comment.Date,
                            StatusView: comment.StatusView,
                            UserComment: comment.UserComment
                        };
                        view.Comments[x] = viewIt;
                    });
                }
                if (skill.Plans != null) {
                    var x = 0;
                    skill.Plans.forEach(function (plan) {

                        var viewPlan = {
                            _id: plan._id,
                            Name: plan.Name,
                            Description: plan.Description,
                            Deadline: plan.Deadline,
                            Skills: [],
                            TypePlan: plan.TypePlan,
                            SourcePlan: plan.SourcePlan,
                            StatusPlan: plan.StatusPlan,
                            StatusPlanApproved: plan.StatusPlanApproved,
                            TypeAction: plan.TypeAction,
                            Attachments: plan.Attachments,
                            NewAction: plan.NewAction,
                            TextEnd: plan.TextEnd,
                            TextEndManager: plan.TextEndManager,
                            Evaluation: plan.Evaluation
                        };
                        if (plan.Skills != null) {
                            var z = 0;
                            plan.Skills.forEach(function (planSkill) {
                                var viewPlanSkill = {
                                    _id: planSkill._id,
                                    Name: planSkill.Name,
                                    Concept: planSkill.Concept,
                                    TypeSkill: planSkill.TypeSkill
                                };
                                viewPlan.Skills[z] = viewPlanSkill;
                            });
                        }
                        view.Plans[x] = viewPlan;
                    });
                }


                model.Activities[i] = view;
                i += 1;
            }
        });
    }

    db.MonitoringNew.insert(model);
});

db.Monitoring.renameCollection('old_Monitoring');
db.MonitoringNew.renameCollection('Monitoring');







db.Monitoring.find({}).forEach((moni) => {
    var person = {
        _id: moni.Person._id,
        Name: moni.Person.Name,
        _idManager: moni.Person._idManager,
        NameManager: moni.Person.Manager
    };

    moni.Activities.forEach((item) => {
        item.Plans.forEach((it) => {
            db.Plan.update({ _id: it._id },
                { $set: { _idMonitoring: moni._id, _idItem: item._id, Person: person } },
                { multi: true }
            )
        });
    });
});