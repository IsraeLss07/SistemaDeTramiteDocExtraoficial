using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaTramiteDoc.Controllers
{
    public class VistaAprobadorController : Controller
    {
        // GET: VistaAprobador
        public ActionResult DetalleAprobador(string numeroDoc)
        {
            var solicitud = CD_Solicitudes.Instancia.ObtenerSolicitudPorNumeroDoc(numeroDoc);
            ViewBag.Solicitud = solicitud;
            return View();
        }
    }
}