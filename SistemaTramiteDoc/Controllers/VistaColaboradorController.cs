using CapaDatos;
using CapaModelo;
using ClassLibrary1;
using SistemaTramiteDoc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using AuthenticationService = SistemaTramiteDoc.Services.AuthenticationService;

namespace SistemaTramiteDoc.Controllers
{
    public class VistaColaboradorController : Controller
    {

        private readonly AuthenticationService _authService;

        public VistaColaboradorController()
        {
            _authService = new AuthenticationService(new DBSTD());
        }

        [HttpPost]
        public JsonResult CambiarEstadoSubSolicitud(SubSolicitud objeto)
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
            string correo = _authService.GetEmail(objeto.IdSubSolicitud);
            var resultado = CD_SubSolicitudes.Instancia.CambiarEstadoSubSolicitud(objeto.IdSubSolicitud);
            CD_SubSolicitudes.Instancia.EnviarCorreo(correo, objeto.IdSubSolicitud);
            CD_SubSolicitudes.Instancia.AgregarComentario(objeto.IdSubSolicitud, objeto.Comentario);
            CD_SubSolicitudes.Instancia.AgregarArchivos(objeto, archivos);
            return Json(new { OperacionExitosa = resultado });
        }

        public ActionResult DetalleRespuesta(string numeroDoc)
        {
            var userName = User.Identity.Name;
            var id = _authService.GetUserId(userName);  
            var solicitud = CD_Solicitudes.Instancia.ObtenerSolicitudPorNumeroDoc(numeroDoc);
            int idSolicitud = (int)solicitud.IdSolicitud;
            solicitud.SubSolicitudes = CD_SubSolicitudes.Instancia.ObtenerSubSolicitudesColaboradores(idSolicitud,id);
            ViewBag.Solicitud = solicitud;
            return View();
        }

        public JsonResult Obtener(int IdSolicitud)
        {
            var userName = User.Identity.Name;
            var id = _authService.GetUserId(userName);
            List<SubSolicitud> oListaSubSolicitudes = CD_SubSolicitudes.Instancia.ObtenerSubSolicitudesColaboradores(IdSolicitud, id);
            return Json(new { data = oListaSubSolicitudes }, JsonRequestBehavior.AllowGet);
        }

        
    }
}