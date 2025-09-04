using CapaModelo;
using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaTramiteDoc.Services;
using System.Data.SqlClient;
using System.Data;

namespace SistemaTramiteDoc.Controllers
{
    [Authorize]
    public class SolicitudesController : Controller
    {
        private readonly AuthenticationService _authService;

        public SolicitudesController()
        {
            _authService = new AuthenticationService(new DBSTD());
        }

        // GET: Solicitudes
        public ActionResult Inicio()
        {
            var userName = User.Identity.Name; // Obtiene el nombre del usuario autenticado

            if (!_authService.IsUserAuthorized(userName))
            {
                return RedirectToAction("AccessDenied", "Solicitudes");
            }
            ViewBag.UserName = userName;
            var id = _authService.GetUserId(userName);
            var nombres = _authService.GeNombreUser(userName);
            var roleId = _authService.GetUserRoleId(id); // Obtiene el ID del rol del usuario
            var empresas = CD_Solicitudes.Instancia.GetEmpresas();
            var tipoDocumentoS = CD_Solicitudes.Instancia.GetTipoDeDocumentoS();
            var formatos = CD_Solicitudes.Instancia.GetFormatos();
            var instituciones = CD_Solicitudes.Instancia.GetInstituciones();
            var proyectos = CD_Solicitudes.Instancia.GetProyectos();
            var analistas = CD_Solicitudes.Instancia.GetAnalistas();
            ViewBag.Id = id;
            ViewBag.Nombres = nombres;
            ViewBag.RoleId = roleId;
            ViewBag.Empresas = empresas;
            ViewBag.TipoDocumentos = tipoDocumentoS;
            ViewBag.Formatos = formatos;
            ViewBag.Instituciones = instituciones;
            ViewBag.Proyectos = proyectos;
            ViewBag.Analistas = analistas;
            // Lógica para cargar la vista de inicio
            return View();
        }
        public JsonResult Obtener()
        {
            List<Solicitud> oListaSolicitudes = CD_Solicitudes.Instancia.ObtenerSolicitudes();
            return Json(new { data = oListaSolicitudes }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerSolicitudesColaborador(int IdUsuario)
        {
            List<Solicitud> oListaSolicitudes = CD_Solicitudes.Instancia.ObtenerSolicitudesColaborador(IdUsuario);
            return Json(new { data = oListaSolicitudes }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerNombresAreasExternas(int idSolicitud)
        {
            string cadena = CD_Solicitudes.Instancia.ObtenerNombresAreasExternas(idSolicitud);
            return Json(new { data = cadena }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Guardar(Solicitud objeto)
        {
            var userName = User.Identity.Name;
            int? id = _authService.GetUserId(userName);
            objeto.IdPersonaRegistra = id;
            bool respuesta = false;
            Dictionary<string, byte[]> archivos = null;

            if (objeto.Archivos != null && objeto.Archivos.Count > 0)
            {
                archivos = new Dictionary<string, byte[]>();
                foreach (var archivo in objeto.Archivos)
                {
                    if (archivo != null && archivo.ContentLength > 0)
                    {
                        using (var binaryReader = new System.IO.BinaryReader(archivo.InputStream))
                        {
                            archivos.Add(archivo.FileName, binaryReader.ReadBytes(archivo.ContentLength));
                        }
                    }
                }
            }
            respuesta = CD_Solicitudes.Instancia.RegistrarSolicitud(objeto, archivos);

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPorExpDoc(string parametroEntrada)
        {
            List<Solicitud> oListaSolicitudes = CD_Solicitudes.Instancia.ObtenerSolicitudesPorExpDoc(parametroEntrada);
            return Json(new { data = oListaSolicitudes }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult RegistrarSolicitud()
        {
            var empresas = CD_Solicitudes.Instancia.GetEmpresas();
            var tipoDocumentoS = CD_Solicitudes.Instancia.GetTipoDeDocumentoS();
            var formatos = CD_Solicitudes.Instancia.GetFormatos();
            var instituciones = CD_Solicitudes.Instancia.GetInstituciones();
            var proyectos = CD_Solicitudes.Instancia.GetProyectos();
            var analistas = CD_Solicitudes.Instancia.GetAnalistas();
            ViewBag.Empresas = empresas;
            ViewBag.TipoDocumentos = tipoDocumentoS;
            ViewBag.Formatos = formatos;
            ViewBag.Instituciones = instituciones;
            ViewBag.Proyectos = proyectos;
            ViewBag.Analistas = analistas;
            return View();
        }
        [HttpPost]
        public JsonResult CargarDocSolicitudesJefes(Solicitud objeto)
        {
            bool respuesta = false;
            Dictionary<string, byte[]> archivos = new Dictionary<string, byte[]>();

            if (objeto.Archivos != null && objeto.Archivos.Count > 0)
            {
                foreach (var archivo in objeto.Archivos)
                {
                    if (archivo != null && archivo.ContentLength > 0)
                    {
                        using (var binaryReader = new System.IO.BinaryReader(archivo.InputStream))
                        {
                            archivos.Add(archivo.FileName, binaryReader.ReadBytes(archivo.ContentLength));
                        }
                    }
                }
            }
            respuesta=CD_Solicitudes.Instancia.AgregarArchivosJefes(objeto, archivos);
            return Json(new { OperacionExitosa = respuesta });
        }

        public JsonResult ObtenerSolicitudesJefes(int IdUsuario)
        {
            List<Solicitud> oListaSolicitudes = CD_Solicitudes.Instancia.ObtenerSolicitudesJefes(IdUsuario);
            return Json(new { data = oListaSolicitudes }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerSolicitudPorNroDoc1(string numeroDoc)
        {
            Solicitud solicitud = CD_Solicitudes.Instancia.ObtenerSolicitudPorNumeroDoc(numeroDoc);
            return Json(new { data = solicitud }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CambiarEstadoSubSolicitudJefe(int idUsuario, int idSolicitud, string comentarios, int estado)
        {
            bool respuesta = true;
            respuesta = CD_Solicitudes.Instancia.CambiarEstadoSubSolicitudJefe(idUsuario, idSolicitud, comentarios, estado);
            return Json(new { data = respuesta }, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public JsonResult CambiarEstadoFaseSolicitud( int idSolicitud, int estado,int fase)
		{
			bool respuesta = true;
			respuesta = CD_Solicitudes.Instancia.CambiarEstadoFaseSolicitud(idSolicitud,  estado, fase);
			return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
		}
        [HttpGet]
        public JsonResult VerificarNumeroDocumento(string numeroDoc)
        {
            bool existe = _authService.VerificarNumeroDocumento(numeroDoc);
            return Json(new { existe }, JsonRequestBehavior.AllowGet);
        }

    }
}
