using App.Models;
using App.Service;
using System.Web.Hosting;
using System.Web.Mvc;
namespace UI.Controllers
{
    public class LoginController : Controller
    {
        UsersService userService = new UsersService();
        
        
        // GET: Login
        public ActionResult Index()
        {
            Machine.Configuration.InsertAppPath(HostingEnvironment.ApplicationPhysicalPath);
            return View();
        }
        public ActionResult Gateway()
        {
            ViewBag.EraseUser = "S";
            return View("Index");
        }


        public ActionResult loadCreateAccountPage()
        {
            return View("CriarConta");
        }
        public ActionResult loadRecuperaSenhaPage()
        {
            return View("RecuperarSenha");
        }
        public ActionResult createAccount(User user)
        {
            if (userService.findUserByLoginAndPassword(user.login, user.password) == null)
            {
                //user.adm = "N";
                Machine.Memory.InsertSingleMemory(user, "user");
                user = userService.InsertUser(user);
                return RedirectToAction("ChooseInterestsLoadPage", "Home", new { userID = user.ID });
            }
            return View("CriarConta");
        }


    }
}