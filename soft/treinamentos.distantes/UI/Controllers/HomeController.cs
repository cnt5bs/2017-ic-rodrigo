using App.Models;
using App.Service;
using App.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using UI.ViewModels;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        UsersService userService = new UsersService();
        PostService postsService = new PostService();
        PostService postService = new PostService();
        QuestionarioService questionarioService = new QuestionarioService();
        Machine.Response conclusionFabric = new Machine.Response();

        public ActionResult ChooseInterestsLoadPage(string userID)
        {
            return View("ChooseInterests", prepareChooseInterestsVM(userID));
        }

        public ActionResult ChooseHelpInterestsLoadPage(string userID, string postID)
        {
            return View("ChooseSpecificInterests", prepareChooseHelpInterestsVM(userID, postID));
        }

        public ActionResult SaveSpecificInterests(string postID, string userID, string postBindID)
        {
            User user = userService.findUserByID(userID);
            Post post = postService.findPostByID(postID);

            if (!user.posts.Select(p => p.ID).Contains(postID))
            {
                user.posts.Add(post);
                Machine.Memory.InsertRelatedMemories(user, post, "save-interests");
            }
            userService.UpdateUserByID(userID, new Dictionary<string, object>() { { "posts", user.posts } });

            return View("ChooseSpecificInterests", prepareChooseHelpInterestsVM(userID, postBindID));
        }

        public ActionResult SpecificDeleteInterests(string postID, string userID, string postBindID)
        {
            User user = userService.findUserByID(userID);
            Post post = postService.findPostByID(postID);

            if (user.posts.Select(p => p.ID).Contains(postID))
            {
                user.posts.Remove(user.posts.Where(p => p.ID == postID).SingleOrDefault());
                Machine.Memory.InsertRelatedMemories(user, post, "delete-interest");
            }

            userService.UpdateUserByID(userID, new Dictionary<string, object>() { { "posts", user.posts } });
            return View("ChooseSpecificInterests", prepareChooseHelpInterestsVM(userID, postBindID));
        }

        public ActionResult ResultsLoadPage(string userID)
        {
            return View("Results", prepareAnalyticsVM(userID));
        }
        public ActionResult SingleExamResultLoadPage(string userID, string postID)
        {
            return View("SingleExamResult", prepareAnalyticsVM(userID));
        }

        public ActionResult Dashboard(string userID)
        {
            DashboardVM d = prepareDashboard(userID);
            if (d.autorization.user != null)
                Machine.Memory.InsertSingleMemory(d.autorization.user, "login-dashboard");
            else
                return RedirectToAction("Index", "Login", "");
            return View("Dashboard", d);
        }
        
        [HttpPost]
        public ActionResult doLogin(string login, string password)
        {
            User u = userService.findUserByLoginAndPassword(login, password);
            DashboardVM d = prepareDashboard(u.ID);

            if (u != null)
            {
                ViewBag.RecordLogin = "S";
                return View("Dashboard", d);
            }
            return View("Index");

        }

        public ActionResult LoadPostPage(string userID, string postID)
        {
            PostVM postVM = preparePostPage(userID, postID);
            return View("VerPostagem", postVM);
        }

        public ActionResult SaveInterests(string postID, string userID)
        {
            User user = userService.findUserByID(userID);
            Post post = postService.findPostByID(postID);

            if (!user.posts.Select(p => p.ID).Contains(postID))
            {
                user.posts.Add(post);
                Machine.Memory.InsertRelatedMemories(user, post, "save-interests");
            }
            userService.UpdateUserByID(userID, new Dictionary<string, object>() { { "posts", user.posts } });

            return View("ChooseInterests", prepareChooseInterestsVM(userID));
        }

        public ActionResult DeleteInterests(string postID, string userID)
        {
            User user = userService.findUserByID(userID);
            Post post = postService.findPostByID(postID);

            if (user.posts.Select(p => p.ID).Contains(postID))
            {
                user.posts.Remove(user.posts.Where(p => p.ID == postID).SingleOrDefault());
                Machine.Memory.InsertRelatedMemories(user, post, "delete-interest");
            }

            userService.UpdateUserByID(userID, new Dictionary<string, object>() { { "posts", user.posts } });
            return View("ChooseInterests", prepareChooseInterestsVM(userID));
        }
        
        public ActionResult PostNextStep(string postID, int currentIndex, string userID)
        {
            Post p = postService.findPostByID(postID, false);

            Machine.Memory.InsertRelatedMemories(userService.findUserByID(userID), currentIndex, postService.findPostByID(postID).nome + "-post-flow");

            if (p.pages.Count > currentIndex + 1)
                if (p.pages.Where(u => u.active == true).OrderByDescending(u => u.ordem).ToList()[currentIndex + 1].name == "questionario-Orientacao")
                {
                    if (p.Orientacao[0].questionario != null)
                    {
                        currentIndex++;
                        QuestionarioVM questionarioVM = prepareQuestionarioVM(userID, postID, currentIndex);
                        Questionario questionario = questionarioService.findQuestionarioByuserIDAndPrototype(questionarioVM.questionario.prototypeID, userID);

                        questionarioVM.questionIndex = 0;
                        if (questionario != null)
                            if (questionario.perguntas[0].resposta != null)
                                if (questionario.perguntas[0].resposta.Count > 0)
                                {
                                    return RedirectToAction("MakeTestNextStep", new { userID = userID, postID = postID, questionID = p.Orientacao[0].questionario[0].ID, questionIndex = 0, resposta = "NaN", pageCurrentIndex = currentIndex });
                                }

                        return View("FazerExame", questionarioVM);
                    }
                    else
                    {
                        DashboardVM d = prepareDashboard(userID);
                        return View("Dashboard", d);
                    }
                }
                else
                    return View("VerPostagem", preparePostPage(userID, postID, currentIndex + 1));
            else
            {
                DashboardVM d = prepareDashboard(userID);
                return View("Dashboard", d);
            }
        }

        public ActionResult MakeTest(string userID, string postID, string questionID, int pageCurrentIndex)
        {
            QuestionarioVM questionarioVM = prepareQuestionarioVM(userID, postID, pageCurrentIndex);
            Questionario questionario = questionarioService.findQuestionarioByuserIDAndPrototype(questionarioVM.questionario.prototypeID, userID);
            questionarioVM.questionIndex = 0;

            Machine.Memory.InsertRelatedMemories(userService.findUserByID(userID), postService.findPostByID(postID), "make-test");

            if (questionario != null)
            {

                if (questionario.perguntas[0].resposta != null)
                    if (questionario.perguntas[0].resposta.Count > 0)
                    {
                        return RedirectToAction("MakeTestNextStep", new { userID = userID, postID = postID, questionID = questionID, questionIndex = 0, pageCurrentIndex = pageCurrentIndex, resposta = "NaN" });
                    }
            }
            return View("FazerExame", questionarioVM);
        }

        public ActionResult MakeTestNextStep(string userID, string postID, string questionID, int questionIndex, string resposta, int pageCurrentIndex, string aparece)
        {
            Post post = postsService.findPostByID(postID, false);
            Questionario questionario = new Questionario();
            User user = userService.findUserByID(userID);
            questionario = questionarioService.findQuestionarioByuserIDAndPrototype(post.Orientacao[0].questionario[0].prototypeID, userID);
            if (resposta != "NaN")
            {
                if (questionario == null)
                {
                    questionario = post.Orientacao[0].questionario[0];
                    questionario.prototypeID = post.Orientacao[0].questionario[0].prototypeID;
                    questionario.ID = null;
                    questionario.owner = userService.findUserByID(userID);
                    questionario.perguntas[questionIndex].resposta = new List<Resposta>();
                    questionario.perguntas[questionIndex].resposta.Add(new Resposta()
                    {
                        respostaAtual = new Option()
                        {
                            nome = resposta,
                            aparece = aparece
                        }
                    });
                    questionarioService.InsertQuestionario(questionario);
                }
                else
                {
                    if (questionario.perguntas.Count != post.Orientacao[0].questionario[0].perguntas.Count)
                    {
                        List<Pergunta> perguntasNaoExistentes = post.Orientacao[0].questionario[0].perguntas.Where(p => !questionario.perguntas
                                                                                                        .Select(s => s.name)
                                                                                                        .Contains(p.name))
                                                                                                        .ToList();
                        questionario.perguntas.AddRange(perguntasNaoExistentes);
                    }

                    if (questionario.perguntas[questionIndex].resposta == null) questionario.perguntas[questionIndex].resposta = new List<Resposta>();
                    questionario.perguntas[questionIndex].resposta.Add(new Resposta()
                    {
                        respostaAtual = new Option()
                        {
                            nome = resposta,
                            aparece = aparece
                        }
                    });
                    questionarioService.UpdateQuestionarioByID(questionario.ID, new Dictionary<string, object>() { { "perguntas", questionario.perguntas } });

                }

                Machine.Memory.InsertRelatedMemories(user, questionario.perguntas[questionIndex].resposta.Last().respostaAtual.nome, questionario.prototypeID + "-question-answer" + questionario.perguntas[questionIndex].name);
            }
            else if (questionario.perguntas.Count != post.Orientacao[0].questionario[0].perguntas.Count)
            {
                List<Pergunta> perguntasNaoExistentes = post.Orientacao[0].questionario[0].perguntas.Where(p => !questionario.perguntas.Select(s => s.name).ToList().Contains(p.name)).ToList();
                questionario.perguntas.AddRange(perguntasNaoExistentes);
                questionarioService.UpdateQuestionarioByID(questionario.ID, new Dictionary<string, object>() { { "perguntas", questionario.perguntas } });
            }
            if (questionario.perguntas.Count <= (questionIndex + 1))
            {
                return RedirectToAction("PostNextStep", "Home", new { postID = post.ID, currentIndex = pageCurrentIndex, userID = userID });
            }

            if (questionario.perguntas[questionIndex + 1].resposta != null)
                if (questionario.perguntas[questionIndex + 1].resposta.Count > 0)
                {
                    int nextIndex = questionIndex + 1;
                    return RedirectToAction("MakeTestNextStep", new { userID = userID, postID = postID, questionID = questionID, questionIndex = nextIndex, resposta = resposta, pageCurrentIndex = pageCurrentIndex });
                }

            QuestionarioVM questionarioVM = prepareQuestionarioVM(userID, postID, pageCurrentIndex);
            questionarioVM.questionIndex = questionIndex + 1;
            return View("FazerExame", questionarioVM);
        }

        private ChooseInterestsVM prepareChooseInterestsVM(string userID)
        {
            User user = userService.findUserByID(userID);
            Machine.Memory.InsertSingleMemory(user, "subjects-choice");
            return new ChooseInterestsVM()
            {
                autorization = authorize(userID),
                posts = postService.ListActivePosts(),
                userPosts = user.posts
            };

        }
        private ChooseInterestsVM prepareChooseHelpInterestsVM(string userID, string postID)
        {
            User user = userService.findUserByID(userID);
            Machine.Memory.InsertSingleMemory(user, "subjects-choice");
            return new ChooseInterestsVM()
            {
                autorization = authorize(userID),
                posts = postService.findPostByID(postID).correlatedPosts,
                userPosts = user.posts,
                postBind = postService.findPostByID(postID),
            };

        }

        private AnalyticsVM prepareAnalyticsVM(string userID)
        {
            User user = userService.findUserByID(userID);
            Machine.Memory.InsertSingleMemory(user, "analytics-dashboard");
            JObject responses = new JObject();
            string probableAnswer = "NaN";
            Dictionary<Post, int> post_nota = new Dictionary<Post, int>();
            Dictionary<Post, int> post_nota_expected = new Dictionary<Post, int>();

            List<Post> badResultsPosts = new List<Post>();

            foreach (var interest in user.posts)
            {
                int examExpectedResult = 0;
                int userResult = 0;

                Post post = postService.findPostByID(interest.ID);

                Questionario questionary = questionarioService.findQuestionarioByuserIDAndPrototype(interest.Orientacao[0].questionario[0].prototypeID, userID);
                if (questionary != null)
                {
                    foreach (var question in questionary.perguntas)
                    {
                        responses = conclusionFabric.GetConclusion(user, interest.Orientacao[0].questionario[0].prototypeID + "-question-answer" + question.name, question.options.Select(o => o.nome).ToList());
                        probableAnswer = responses["group"].Value<string>();
                        
                        if (post.Orientacao[0].questionario[0].perguntas.Where(p => p.name == question.name && p.correctAnswerID == probableAnswer).Count() > 0)
                            examExpectedResult++;
                        
                        if (post.Orientacao[0].questionario[0].perguntas.Where(p => p.name == question.name && p.correctAnswerID == question.resposta.Last().respostaAtual.nome).Count() > 0)
                            userResult++;
                    }
                    post_nota.Add(interest, userResult);
                    post_nota_expected.Add(interest, examExpectedResult);

                    if (examExpectedResult > userResult)
                        badResultsPosts.Add(interest);
                }
                else
                {
                    post_nota.Add(interest, 0);
                    post_nota_expected.Add(interest, 0);

                }
                
            }

            return new AnalyticsVM()
            {
                autorization = authorize(userID),
                posts = postService.ListActivePosts(),
                userPosts = user.posts,
                post_nota = post_nota,
                post_esperada = post_nota_expected,
                badResultsPosts = badResultsPosts
            };

        }

        private SingleExamAnalyticsVM prepareSingleResultAnalyticsVM(string userID)
        {
            User user = userService.findUserByID(userID);
            Machine.Memory.InsertSingleMemory(user, "single-analytics-dashboard");

            return new SingleExamAnalyticsVM()
            {
                autorization = authorize(userID),
                posts = postService.ListActivePosts(),
                userPosts = user.posts,
                resposta_usuario = new Dictionary<Pergunta, Resposta>(),
                resposta_corretas = new Dictionary<Pergunta, Resposta>(),
                resposta_esperadas = new Dictionary<Pergunta, Resposta>()
            };

        }

        private DashboardVM prepareDashboard(string userID)
        {
            return new DashboardVM()
            {
                autorization = authorize(userID),
                posts = postsService.ListActivePosts()
            };
        }

        private PostVM preparePostPage(string userID, string postID)
        {
            return preparePostPage(userID, postID, 0);
        }

        private PostVM preparePostPage(string userID, string postID, int ordem)
        {
            return new PostVM()
            {
                autorization = authorize(userID),
                posts = postsService.ListActivePosts(),
                post = postsService.findPostByID(postID, false),
                pageIndex = ordem
            };
        }

        private QuestionarioVM prepareQuestionarioVM(string userID, string postID, int pageCurrentIndex)
        {
            return new QuestionarioVM()
            {
                autorization = authorize(userID),
                post = postsService.findPostByID(postID, false),
                Orientacao = postsService.findPostByID(postID, false).Orientacao[0],
                questionario = postsService.findPostByID(postID, false).Orientacao[0].questionario[0],
                pageCurrentIndex = pageCurrentIndex,
                posts = postService.ListActivePosts()
            };
        }

        private Autentication authorize(string userID)
        {
            Autentication auth = new Autentication()
            {
                user = userService.findUserByID(userID)
            };
            if (auth.user.posts != null)
            {
                List<string> postsIds = auth.user.posts.Select(u => u.ID).ToList();
                auth.user.posts = new List<Post>();

                foreach (var postId in postsIds)
                {
                    auth.user.posts.Add(postService.findPostByID(postId));
                }
            }
            else auth.user.posts = new List<Post>();

            return auth;
        }
    }
}