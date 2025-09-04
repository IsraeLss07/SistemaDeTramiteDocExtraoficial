using CapaDatos;
using CapaModelo;
using ClassLibrary1;
using SistemaTramiteDoc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SistemaTramiteDoc.Controllers
{
    public class DetalleSolicitudController : Controller
    {
        private readonly AuthenticationService _authService;

        public DetalleSolicitudController()
        {
            _authService = new AuthenticationService(new DBSTD());
        }

        public ActionResult VerDetalles(string numeroDoc)
        {
            return RedirectToAction("Inicio2", new { numeroDoc = numeroDoc });
        }
        public ActionResult Inicio2(string numeroDoc)
        {
            var solicitud = CD_Solicitudes.Instancia.ObtenerSolicitudPorNumeroDoc(numeroDoc);
            ViewBag.Solicitud = solicitud;
            return View();
        }
        public JsonResult Modificar(string NumeroDoc, int parametro)
        {
            bool respuesta = false;
            var userName = User.Identity.Name;
            var id = _authService.GetUserId(userName);
            respuesta = CD_Solicitudes.Instancia.ModificarSolicitud(NumeroDoc, parametro,id);
            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult VerDetalles2(string numeroDoc)
        {
            return RedirectToAction("Detalle", new { numeroDoc = numeroDoc });
        }
        public ActionResult Detalle(string numeroDoc)
        {
            var userName = User.Identity.Name;
            var id = _authService.GetUserId(userName);
            var areaExterna = CD_Solicitudes.Instancia.GetAreasExternas();
            var personasPorAreaExterna = new Dictionary<int, List<Usuario>>();
            foreach (var area_externa in areaExterna)
            {
                personasPorAreaExterna[area_externa.IdAreaExterna] = CD_Solicitudes.Instancia.GetResponsablesPorAreaExterna(area_externa.IdAreaExterna);
            }
            ViewBag.AreaExterna = areaExterna;
            ViewBag.PersonasPorAreaExterna = personasPorAreaExterna;
            var aprobadores = CD_Solicitudes.Instancia.GetAprobadores();
            ViewBag.Aprobadores = aprobadores;
            var solicitud = CD_Solicitudes.Instancia.ObtenerSolicitudPorNumeroDoc(numeroDoc);
            int idSolicitud = (int)solicitud.IdSolicitud;
            solicitud.SubSolicitudes = CD_SubSolicitudes.Instancia.ObtenerSubSolicitudesColaboradores(idSolicitud, id);
            ViewBag.Solicitud = solicitud;
            return View();
        }
        public JsonResult ModificarDetalle(string NumeroDoc, DateTime? FechaExterna,DateTime? FechaInterna, int? Plazo)
        {
            bool respuesta = false;
            respuesta = CD_Solicitudes.Instancia.ModificarSolicitudDetalle(NumeroDoc, FechaExterna, FechaInterna, Plazo);
            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DescargarArchivo(int idSolicitud, string nombreArchivo)
        {
            var archivo = CD_Solicitudes.Instancia.ObtenerArchivoPorNombre(idSolicitud, nombreArchivo);
            if (archivo != null)
            {
                return File(archivo.fileBytes, archivo.ContentType, archivo.fileName);
            }
            return HttpNotFound();
        }
        
        public ActionResult MostrarArchivo(int idSolicitud, string nombreArchivo)
        {
            var archivo = CD_Solicitudes.Instancia.ObtenerArchivoPorNombre(idSolicitud, nombreArchivo);
            if (archivo != null)
            {
                return File(archivo.fileBytes, archivo.ContentType);
            }
            return HttpNotFound();
        }


        [HttpGet]
        public JsonResult VerificarArchivo(int idSolicitud, string nombreArchivo, int idSubSolicitud)
        {
            bool archivoExiste = CD_Solicitudes.Instancia.Verificar(idSolicitud, nombreArchivo, idSubSolicitud);
            return Json(archivoExiste, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VerificarArchivo2(int idSolicitud, string nombreArchivo)
        {
            bool archivoExiste = CD_Solicitudes.Instancia.Verificar2(idSolicitud, nombreArchivo);
            return Json(archivoExiste, JsonRequestBehavior.AllowGet);
        }
		public JsonResult EliminarArchivo(int idSolicitud, string nombreArchivo)
		{
			bool respuesta = false;
            respuesta = CD_Solicitudes.Instancia.EliminarArchivo(idSolicitud, nombreArchivo);
			return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
		}
	}
}