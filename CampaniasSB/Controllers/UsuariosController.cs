using CampaniasSB.Classes;
using CampaniasSB.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CampaniasSB.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsuariosController : Controller
    {
        private readonly CampaniasContext db = new CampaniasContext();

        // GET: Usuarios
        public ActionResult Index()
        {
            Session["iconoTitulo"] = "fas fa-users";
            Session["homeB"] = string.Empty;
            Session["rolesB"] = string.Empty;
            Session["compañiasB"] = string.Empty;
            Session["usuariosB"] = "active";
            Session["regionesB"] = string.Empty;
            Session["ciudadesB"] = string.Empty;
            Session["restaurantesB"] = string.Empty;
            Session["familiasB"] = string.Empty;
            Session["materialesB"] = string.Empty;
            Session["campañasB"] = string.Empty;

            return View();
        }
        public ActionResult GetData()
        {
            var usuarioList = db.Usuarios.ToList();

            return Json(new { data = usuarioList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.CompañiaId = new SelectList(CombosHelper.GetCompañias(true), "CompañiaId", "Nombre");
                ViewBag.RolId = new SelectList(CombosHelper.GetRoles(true), "RolId", "Nombre");

                return PartialView(new Usuario());
            }
            else
            {
                var nombreUsuario = db.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefault().NombreUsuario;
                var rolId = db.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefault().RolId;
                var compañiaId = db.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefault().CompañiaId;

                Session["UsuarioEditado"] = nombreUsuario.ToLower();
                Session["RolEditado"] = rolId;

                ViewBag.CompañiaId = new SelectList(CombosHelper.GetCompañias(true), "CompañiaId", "Nombre", compañiaId);
                ViewBag.RolId = new SelectList(CombosHelper.GetRoles(true), "RolId", "Nombre", rolId);

                return PartialView(db.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEdit(Usuario usuario)
        {
            if (usuario.UsuarioId == 0)
            {
                db.Usuarios.Add(usuario);
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var rol = db.Roles.Where(r => r.RolId == usuario.RolId).FirstOrDefault();
                    var random = new Random();
                    var password = string.Format("{0}{1}{2:04}*",
                        usuario.Nombres.Trim().ToUpper().Substring(0, 1),
                        usuario.Apellidos.Trim().ToLower().Substring(0, 1) + "Lt",
                        random.Next(10000));

                    UsuariosHelper.CreateUserASP(usuario.NombreUsuario, rol.Nombre, password);

                    var subject = "Bienvenido a la Plataforma de Campañas";
                    var body = string.Format(@"
                    <h1>Bienvenido a la Plataforma de Campañas</h1>
                    <p>Tu Usuario es: <strong>{1}</strong></p>
                    <p>Tu password es: <strong>{0}</strong></p>
                    <p>Link de la Plataforma: <a href='portal.litoprocess.com/Campanias'>portal.litoprocess.com/Campanias</a>",
                    password, usuario.NombreUsuario);

                    await MailHelper.SendMail(usuario.NombreUsuario, "jesuscabrerag@yahoo.com.mx", subject, body);

                    UsuariosHelper.AddRole(usuario.NombreUsuario, rol.Nombre, password);

                    return Json(new { success = true, message = "USUARIO AGREGADO" }, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                db.Entry(usuario).State = EntityState.Modified;
                var response = DBHelper.SaveChanges(db);
                if (response.Succeeded)
                {
                    var currentUser = Session["UsuarioEditado"].ToString();
                    var currentRol = (int)Session["RolEditado"];
                    var currentRolNombre = db.Roles.Where(r => r.RolId == currentRol).FirstOrDefault();
                    var newRol = db.Roles.Where(r => r.RolId == usuario.RolId).FirstOrDefault();

                    if (currentUser == usuario.NombreUsuario || currentRol == newRol.RolId)
                    {
                        UsuariosHelper.UpdateUserName(currentUser, usuario.NombreUsuario.ToLower(), currentRolNombre.Nombre, newRol.Nombre);
                    }

                    return Json(new { success = true, message = "USUARIO ACTUALIZADO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuarios.Where(x => x.UsuarioId == id).FirstOrDefault();
            db.Usuarios.Remove(usuario);
            var response = DBHelper.SaveChanges(db);
            if (response.Succeeded)
            {
                UsuariosHelper.DeleteUser(usuario.NombreUsuario);
                return Json(new { success = true, message = "USUARIO ELIMINADO" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}