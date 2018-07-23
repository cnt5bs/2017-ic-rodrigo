using App.Models;
using App.Service;
using App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ViewModels;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace UI.Controllers
{
    public class CMSController : Controller
    {
        UsersService userService = new UsersService();
        PostService postService = new PostService();
        QuestionarioService questionarioService = new QuestionarioService();
        PagesService pageService = new PagesService();

        public ActionResult setCorrectAnswer(string userID, string questionaryID, string questionID, string answerID, string postID)
        {
            Post post = postService.findPostByID(postID);
            Questionario questionario = post.Orientacao[0].questionario[0];
            post.Orientacao[0].questionario[0].perguntas.Where(q => q.name == questionID).SingleOrDefault().correctAnswerID = answerID;
            questionarioService.UpdateQuestionarioByID(questionaryID, new Dictionary<string, object> { { "perguntas", questionario.perguntas } });

            post.Orientacao[0].questionario[0] = questionario;

            postService.UpdatePostByID(postID, new Dictionary<string, object> { { "Orientacao", post.Orientacao } });

            return View("CreateOption", new CreateOptionsVM() { questionName = questionario.perguntas.Where(q => q.name == questionID).SingleOrDefault().name, options = questionario.perguntas.Where(q => q.name == questionID).SingleOrDefault().options, post = post, autorization = authorize(userID) });
        }

        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult doLogin(string login, string password)
        {
            User user = userService.findUserByLoginAndPassword(login, password);
            CMSDashboard cmsVM = prepareCMSDashboard(user.ID);
            if (cmsVM.autorization.user != null && cmsVM.autorization.user.adm == "S")
            {
                ViewBag.RecordLogin = "S";
                return View("Dashboard", cmsVM);
            }
            return View("Login");
        }
        public ActionResult Dashboard(string userID)
        {
            User user = userService.findUserByID(userID);
            CMSDashboard cmsVM = prepareCMSDashboard(user.ID);
            if (cmsVM.autorization.user != null && cmsVM.autorization.user.adm == "S")
            {
                return View("Dashboard", cmsVM);
            }

            return View("Login");
        }

        public ActionResult AddNotAdmUser(User user, string userID)
        {
            user.adm = "N";
            userService.InsertUser(user);
            UserNotAdmVM userNAdmVM = prepareUserNotAdm(userID);
            return View("UsersNotAdm", userNAdmVM);
        }

        public ActionResult AddUsers(string csv, string userID)
        {
            userService.AddCsvUsers(csv);
            UserNotAdmVM userNAdmVM = prepareUserNotAdm(userID);
            return View("UsersNotAdm", userNAdmVM);
        }

        public ActionResult createAdmUser(User user, string userID)
        {
            if (userService.findUserByLoginAndPassword(user.login, user.password) == null)
            {
                user.adm = "S";
                user = userService.InsertUser(user);
                return RedirectToAction("Dashboard", "CMS", new { userID = user.ID });
            }
            return View("CriarConta");
        }

        public ActionResult NotAdmUserLoadPage(string userID)
        {
            UserNotAdmVM userNAdmVM = prepareUserNotAdm(userID);
            return View("UsersNotAdm", userNAdmVM);
        }
        public ActionResult DeleteNotAdmUser(string userID, string userDeletedID)
        {
            userService.DeactivateUsersByID(new List<string>() { userDeletedID });
            UserNotAdmVM userNAdmVM = prepareUserNotAdm(userID);
            return View("UsersNotAdm", userNAdmVM);
        }
        public ActionResult DeleteUser(string userID, string userDeletedID)
        {
            userService.DeactivateUsersByID(new List<string>() { userDeletedID });
            CreateAdmVM cAdmUserVM = prepareCreateAdmUser(userID);
            return View("CreateAdmUser", cAdmUserVM);
        }
        public ActionResult CreateAdmUserLoadPage(string userID)
        {
            CreateAdmVM cAdmUserVM = prepareCreateAdmUser(userID);
            return View("CreateAdmUser", cAdmUserVM);
        }


        public ActionResult LoadCreatePostPage(string userID)
        {
            PostEditVM postEditVM = preparePostEditVM(userID, "");
            return View("CreatePost", postEditVM);
        }
        public ActionResult CreatePost(string userID, Post post)
        {
            post = postService.InsertPost(post);
            PostEditVM postEditVM = preparePostEditVM(userID, post.ID);
            return View("CreateOrientacao", postEditVM);
        }
        public ActionResult DeletePost(string userID, string postID)
        {
            postService.DeactivatePostByID(new List<string>() { postID });
            CMSDashboard cmsVM = prepareCMSDashboard(userID);
            return View("Dashboard", cmsVM);
        }
        public ActionResult ManagePagesLoadPage(string postID, string userID)
        {
            PagesEditVM pagesEditVM = preparePagesEdit(postID, userID);
            return View("ManagePages", pagesEditVM);
        }


        public ActionResult AddTextPage(string postID, string text, string name, bool hasOrientacao, string OrientacaoPath, string userID)
        {
            Pages newPage = new Pages()
            {
                OrientacaoPath = (OrientacaoPath == null) ? "" : OrientacaoPath,
                hasOrientacao = hasOrientacao,
                ordem = 0,
                text = text,
                type = "text",
                name = name,
                ID = name
            };

            //Verifica páginas existentes
            Pages pageExistent = pageService.findPagesByID(name);

            //Se não existie, insere no banco de páginas
            if (name != "Orientacao" && name != "questionario-Orientacao" && pageExistent == null)
            {
                newPage = pageService.InsertPages(newPage);
            }

            //Atualiza páginas da postagem
            Post p = postService.findPostByID(postID);
            p.pages.Add(newPage);
            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "pages", p.pages } });

            //Volta para página
            PagesEditVM peVM = preparePagesEdit(userID, postID);
            return View("ManagePages", peVM);
        }

        public ActionResult UpdatePageOrder(string postID, string pageName, int ordem, string userID)
        {
            Post p = postService.findPostByID(postID);
            p.pages.Where(page => page.name == pageName && page.active == true).FirstOrDefault().ordem = Convert.ToInt32(ordem);
            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "pages", p.pages } });
            PagesEditVM peVM = preparePagesEdit(userID, postID);
            return View("ManagePages", peVM);
        }

        public ActionResult DeletePage(string postID, string pageName, string userID)
        {
            Post p = postService.findPostByID(postID);
            p.pages.Where(page => page.name == pageName && page.active == true).FirstOrDefault().active = false;
            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "pages", p.pages } });
            PagesEditVM peVM = preparePagesEdit(userID, postID);
            return View("ManagePages", peVM);
        }

        public ActionResult LoadEditPostPage(string userID, string postID)
        {
            QuestionarioVM questionarioVM = prepareQuestionarioVM(userID, postID);
            return View("EditPost", questionarioVM);
        }



        public ActionResult EditPost(string userID, Post post, string OrientacaoPath)
        {

            Post p = postService.findPostByID(post.ID);


            if (p.Orientacao[0] != null)
                p.Orientacao[0].path = OrientacaoPath;
            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() {
                    { "ordem", post.ordem } ,

                });
            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() {
            { "descricao", post.descricao},

                });
            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() {
                    { "observacao", post.observacao},

                });
            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() {
                    { "subtitulo", post.subtitulo},

                });
            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() {
                    { "nome", post.nome},

                });
            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() {
                    { "Orientacao", p.Orientacao}

                });





            CMSDashboard cmsVM = prepareCMSDashboard(userID);
            return View("Dashboard", cmsVM);
        }

        public ActionResult CreateOrientacao(string postID, Orientacao Orientacao, string userID)
        {
            Orientacao.incorporacao = true;
            Orientacao.temQuestionario = true;
            Post post = postService.findPostByID(postID);
            if (post.Orientacao == null) post.Orientacao = new List<Orientacao>();
            if (post.Orientacao.Count > 0) post.Orientacao[0] = Orientacao;
            else post.Orientacao.Add(Orientacao);

            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "Orientacao", post.Orientacao } });
            if (post.Orientacao[0].temQuestionario)
                return View("CreateExam", preparePostEditVM(userID, postID));
            else
                return RedirectToAction("LoadCreatePostPage");
        }

        public ActionResult CreateExam(string postID, Questionario questionario, string userID)
        {
            Post post = postService.findPostByID(postID);
            if (post.Orientacao[0].questionario == null) post.Orientacao[0].questionario = new List<Questionario>();
            if (post.Orientacao[0].questionario.Count > 0) post.Orientacao[0].questionario[0] = questionario;
            else post.Orientacao[0].questionario.Add(questionario);

            Prototipo prototipo = new Prototipo();
            prototipo.questionario = questionario;

            prototipo = questionarioService.InsertProtipo(prototipo);

            //questionario = questionarioService.InsertQuestionario(questionario);

            post.Orientacao[0].questionario[0].ID = "";//questionario.ID;
            post.Orientacao[0].questionario[0].prototypeID = prototipo.ID;

            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "Orientacao", post.Orientacao } });

            return View("CreateQuestion", preparePostEditVM(userID, postID));
        }

        public ActionResult CreateQuestionLoadPage(string postID, string userID)
        {
            Post post = postService.findPostByID(postID);
            return View("CreateQuestion", preparePostEditVM(userID, postID));
        }

        public ActionResult CreateQuestion(string postID, string questionarioID, Pergunta pergunta, string userID)
        {
            Post post = postService.findPostByID(postID);
            if (post.Orientacao[0].questionario[0].perguntas == null)
                post.Orientacao[0].questionario[0].perguntas = new List<Pergunta>();
            if (post.Orientacao[0].questionario[0].perguntas.Where(p => p.name == pergunta.name).SingleOrDefault() == null)
            {
                post.Orientacao[0].questionario[0].perguntas.Add(pergunta);
                postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "Orientacao", post.Orientacao } });
            }
            return View("CreateQuestion", preparePostEditVM(userID, postID));
        }

        public ActionResult DeactivateQuestion(string questionaryID, string questionName, string postID, string userID)
        {
            Post post = postService.findPostByID(postID);
            post.Orientacao[0].questionario[0].perguntas.Where(p => p.name == questionName).SingleOrDefault().active = false;
            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "Orientacao", post.Orientacao } });



            return View("CreateQuestion", preparePostEditVM(userID, postID));
        }

        public ActionResult DeactivateOption(string questionaryID, string questionName, string postID, string optionName, string userID)
        {
            Post post = postService.findPostByID(postID);
            post.Orientacao[0].questionario[0].perguntas.Where(p => p.name == questionName).SingleOrDefault().options.Where(o => o.nome == optionName).SingleOrDefault().active = false;

            postService.UpdatePostByID(post.ID, new Dictionary<string, object>() { { "Orientacao", post.Orientacao } });

            CreateOptionsVM optionVM = prepareOptionsVM(postID, questionName, userID);
            return View("CreateOption", optionVM);
        }

        public ActionResult OptionsLoadPage(string postID, string questionarioID, string questionName, string userID)
        {
            Post post = postService.findPostByID(postID);
            CreateOptionsVM optionVM = prepareOptionsVM(postID, questionName, userID);
            return View("CreateOption", optionVM);
        }
        public ActionResult CreateOption(string postID, string questionarioID, string questionName, Option option, string userID)
        {
            Post post = postService.findPostByID(postID);

            if (post.Orientacao[0].questionario[0].perguntas.Where(u => u.name == questionName).SingleOrDefault().options == null)
                post.Orientacao[0].questionario[0].perguntas.Where(u => u.name == questionName).SingleOrDefault().options = new List<Option>();

            if (post.Orientacao[0].questionario[0].perguntas.Where(u => u.name == questionName).SingleOrDefault().options.Where(opt => opt.nome == option.nome).SingleOrDefault() == null)
            {
                post.Orientacao[0].questionario[0].perguntas.Where(u => u.name == questionName).SingleOrDefault().options.Add(option);
                postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "Orientacao", post.Orientacao } });
            }
            CreateOptionsVM optionVM = prepareOptionsVM(postID, questionName, userID);
            return View("CreateOption", optionVM);

        }




        public ActionResult SendEmail(string subject, string email, HttpPostedFileBase img, string test, string user, string password, string userName)
        {


            if (System.IO.File.Exists(Server.MapPath("~/Email/" + img.FileName)))
                System.IO.File.Delete(Server.MapPath("~/Email/" + img.FileName));
            img.SaveAs(Server.MapPath("~/Email/" + img.FileName));



            MailAddress fromAddress = new MailAddress(user, userName);
            List<User> users = new List<App.Models.User>();
            if (test == "1")
            {
                users = userService.ListActiveUsers().Where(u => u.adm == "S" && u.email.Contains("@") && u.email.Contains(".com")).ToList();
            }
            else if (test == "2")
            {
                users = userService.ListActiveUsers().Where(u => u.adm == "N" && u.email.Contains("@") && u.email.Contains(".com")).ToList();
            }
            else
            {
                users = userService.ListActiveUsers().Where(u => u.email.Contains("@") && u.email.Contains(".com")).ToList();
            }

            foreach (var u in users)
            {
                MailAddress toAddress = new MailAddress(u.email, u.email);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(user, password),
                    Timeout = 20000
                };

                string body = "<html><body>" + email.Replace("{{Pesquisador.nome}}", u.name).Replace("{{Pesquisador.email}}", u.email).Replace("{{Pesquisador.login}}", u.login).Replace("{{Pesquisador.password}}", u.password) + "<img src=\"http://od.mobie.com.br/Email/" + img.FileName + "\"></body></html>";
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                LinkedResource inline = new LinkedResource(Server.MapPath("~/Email/" + img.FileName), MediaTypeNames.Image.Jpeg);
                inline.ContentId = Guid.NewGuid().ToString();
                avHtml.LinkedResources.Add(inline);


                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = String.Format(email.Replace("{{Pesquisador.Nome}}", u.name).Replace("{{Pesquisador.email}}", u.email).Replace("{{Pesquisador.login}}", u.login).Replace("{{Pesquisador.password}}", u.password) +
                                                @"<img src=""http://od.mobie.com.br/Email/{0}"" width=""500"" height=""500"" />", img.FileName),
                    IsBodyHtml = true
                };
                message.AlternateViews.Add(avHtml);


                smtp.Send(message);

            }

            return View();
        }

        public ActionResult BindPostLoadPage(string postID, string userID)
        {
            return View("BindingPosts", prepareBindingPostsVM(userID, postID));
        }

        public ActionResult BindPostToPost(string postID, string postBindID, string userID)
        {
            Post post = postService.findPostByID(postID);
            if (post.correlatedPosts == null)
                post.correlatedPosts = new List<Post>();
            Post postBind = postService.findPostByID(postBindID);
            post.correlatedPosts.Add(postBind);
            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "correlatedPosts", post.correlatedPosts } });
            return View("BindingPosts", prepareBindingPostsVM(userID, postID));
        }

        public ActionResult UnbindPostFromPost(string postID, string postUnbindID, string userID)
        {
            Post post = postService.findPostByID(postID);
            post.correlatedPosts.Remove(post.correlatedPosts.Where(p => p.ID == postUnbindID).SingleOrDefault());
            postService.UpdatePostByID(postID, new Dictionary<string, object>() { { "correlatedPosts", post.correlatedPosts } });
            return View("BindingPosts", prepareBindingPostsVM(userID, postID));
        }

        public ActionResult EditUserLoadPage(string userID, string userEditID)=>View("EditUser", prepareEditUser(userID, userEditID));
        public ActionResult SaveEditUser(User user, string userID)
        {
            Dictionary<string, object> propertiesToUpdate = new Dictionary<string, object>() { { "login", user.login },
                                                                                   { "name", user.name },
                                                                                   { "email", user.email } };
            if (user.password != null)
                propertiesToUpdate.Add("password", user.password);
            userService.UpdateUserByID(user.ID, propertiesToUpdate);
            return View("EditUser", prepareEditUser(userID, user.ID));
        }


        private EditUserVM prepareEditUser(string userID, string userEditID) => new EditUserVM()
        {
            userEdit = userService.findUserByID(userEditID),
            autorization = authorize(userID)
        };



        private BindingPostsVM prepareBindingPostsVM(string userID, string postID)
        {
            return new BindingPostsVM()
            {
                autorization = authorize(userID),
                post = postService.findPostByID(postID),
                allPostsBind = postService.ListActivePostsBind(postID),
                allPostsNotBind = postService.ListActivePostsNotBind(postID)
            };
        }

        private PostEditVM preparePostEditVM(string userID, string postID)
        {
            return new PostEditVM()
            {
                autorization = authorize(userID),
                post = postService.findPostByID(postID)

            };
        }

        private QuestionarioVM prepareQuestionarioVM(string userID, string postID)
        {
            return new QuestionarioVM()
            {
                autorization = authorize(userID),
                post = postService.findPostByID(postID),
                Orientacao = postService.findPostByID(postID).Orientacao[0],

            };
        }

        private CreateOptionsVM prepareOptionsVM(string postID, string questionName, string userID)
        {
            Post post = postService.findPostByID(postID);
            return new CreateOptionsVM()
            {
                autorization = authorize(userID),
                post = post,
                options = post.Orientacao[0].questionario[0].perguntas.Where(u => u.name == questionName).SingleOrDefault().options,
                questionName = questionName
            };
        }

        private CreateAdmVM prepareCreateAdmUser(string userID)
        {
            return new CreateAdmVM()
            {
                autorization = authorize(userID),
                users = userService.ListActiveUsers().Where(u => u.adm == "S").OrderByDescending(u => u.creationDate).ToList()
            };
        }
        private UserNotAdmVM prepareUserNotAdm(string userID)
        {
            return new UserNotAdmVM()
            {
                autorization = authorize(userID),
                users = userService.ListActiveUsers().Where(u => u.adm == "N").OrderByDescending(u => u.creationDate).ToList()
            };
        }

        private PagesEditVM preparePagesEdit(string postID, string userID)
        {
            return new PagesEditVM()
            {
                autorization = authorize(userID),
                pages = pageService.ListActivePages().OrderByDescending(u => u.creationDate).ToList(),
                post = postService.findPostByID(postID)
            };
        }

        private CMSDashboard prepareCMSDashboard(string userID)
        {
            return new CMSDashboard()
            {
                autorization = authorize(userID),
                posts = postService.ListActivePosts().OrderByDescending(u => u.creationDate).ToList()
            };
        }

        private Autentication authorize(string userID)
        {
            return new Autentication()
            {
                user = userService.findUserByID(userID)
            };
        }
    }
}