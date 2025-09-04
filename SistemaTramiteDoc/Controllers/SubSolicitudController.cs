using CapaModelo;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using CapaDatos;
using System.Web;
using System.Web.Mvc;
using SistemaTramiteDoc.Services;

namespace SistemaTramiteDoc.Controllers
{
    public class SubSolicitudController : Controller
    {

        [HttpPost]
        public JsonResult Guardar(List<SubSolicitud> objetos)
        {
            bool respuesta = false;

            if (objetos.Count > 0)
            {
                respuesta = CD_SubSolicitudes.Instancia.RegistrarSubSolicitudes(objetos);
            }

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Obtener(int IdSolicitud)
        {
            List<SubSolicitud> oListaSubSolicitudes = CD_SubSolicitudes.Instancia.ObtenerSubSolicitudes(IdSolicitud);
            return Json(new { data = oListaSubSolicitudes }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult Eliminar(int IdSubSolicitud)
        {
            bool respuesta = false;

            respuesta = CD_SubSolicitudes.Instancia.EliminarSubSolicitud(IdSubSolicitud);

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EnviarAreas(int? idSolicitud)
        {
            bool respuesta = false;

            respuesta = CD_SubSolicitudes.Instancia.EnviarAAreasSubSolicitudes(idSolicitud);

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ModificarEstadoSubSolicitudValidado(int? IdSubSolicitud)
        {
            bool respuesta = false;

            respuesta = CD_SubSolicitudes.Instancia.CambiarEstadoSubSolicitudValidad(IdSubSolicitud);

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ModificarEstadoSubSolicitudObservado(int? IdSubSolicitud,string Observacion,int? IdSolicitud)
        {
            bool respuesta = false;

            respuesta = CD_SubSolicitudes.Instancia.ModificarEstadoSubSolicitudObservado(IdSubSolicitud,Observacion, IdSolicitud);

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }

    }

}