using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class SubSolicitudJefe
    {
        public int? IdSubSolicitudJefe { get; set; }
        public string NombreEstado { get; set; }
        public int IdSolicitud { get; set; }
        public int IdSubSolicitud { get; set; }
        public string ComentarioSubSolicitudJefe { get; set; }
        public string NombreAprobador { get; set; }

        public SubSolicitudJefe()
        {
        }

        public SubSolicitudJefe(int? idSubSolicitudJefe, string nombreEstado, int idSolicitud, int idSubSolicitud, string comentarioSubSolicitudJefe, string nombreAprobador)
        {
            IdSubSolicitudJefe = idSubSolicitudJefe;
            NombreEstado = nombreEstado;
            IdSolicitud = idSolicitud;
            IdSubSolicitud = idSubSolicitud;
            ComentarioSubSolicitudJefe = comentarioSubSolicitudJefe;
            NombreAprobador = nombreAprobador;
        }
    }
}
