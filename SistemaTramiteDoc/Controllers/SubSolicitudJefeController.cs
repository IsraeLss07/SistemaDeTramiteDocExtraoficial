using CapaModelo;
using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaTramiteDoc.Controllers
{
    public class SubSolicitudJefeController : Controller
    {
        [HttpPost]
        public JsonResult Guardar(SubSolicitudJefe objeto)
        {
            bool respuesta = false;

            if (objeto.IdSubSolicitudJefe == null)
            {

                respuesta = CD_SubSolcitudJefe.Instancia.RegistrarSubSolicitudJefe(objeto);
            }
            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Obtener(int IdSolicitud)
        {
            List<SubSolicitudJefe> oListaSubSolicitudesJefes = CD_SubSolcitudJefe.Instancia.ObtenerSubSolicitudesJefes(IdSolicitud);
            return Json(new { data = oListaSubSolicitudesJefes }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult Eliminar(int IdSubSolicitudJefe)
        {
            bool respuesta = false;

            respuesta = CD_SubSolcitudJefe.Instancia.EliminarSubSolicitudJefe(IdSubSolicitudJefe);

            return Json(new { OperacionExitosa = respuesta }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult EnviarSolicitudDeAprobacion(int idSolicitud)
        {
            bool respuesta1 = false;
            bool respuesta2 = false;
            int tipo = 3;
            List<string> listaDestinatarios;
            //listaDestinatarios = CD_SubSolcitudJefe.buscarDestinatarios(idSolicitud);
            //respuesta1 = CD_SubSolcitudJefe.Instancia.EnviarCorreoSolicitudAprobacion(listaDestinatarios,idSolicitud);
            respuesta2= CD_SubSolcitudJefe.Instancia.CambiarEstadoSubSolicitudJefe(idSolicitud,tipo);
            return Json(new { OperacionExitosa = respuesta2 }, JsonRequestBehavior.AllowGet);

        }
        

    }
}