using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CapaModelo
{
    [Table("TSubSolicitud")]
    public class SubSolicitud
    {

        [Key]
        [Column("IdSubSolicitud")]
        public int? IdSubSolicitud { get; set; }

        [NotMapped]
        public string Titulo { get; set; }
        [NotMapped]
        public string NombreAreaExterna { get; set; }
        [NotMapped]
        public string NombreResponsable { get; set; }
        [NotMapped]
        public int IdSolicitud { get; set; }
        [NotMapped]
        public string Comentario { get; set; }
        [NotMapped]
        public string TituloSolicitud { get; set; }
        [NotMapped]
        public string Observacion { get; set; }
        [NotMapped]
        public string ComentarioAreaExterna { get; set; }
        [NotMapped]
        public DateTime? FechaSolicitud { get; set; }
        [NotMapped]
        public DateTime? FechaRespuesta { get; set; }
        [NotMapped]
        public string NombreEstado { get; set; }

        [Column("IdResponsable")]
        public int IdResponsable { get; set; }

        [NotMapped]
        public List<HttpPostedFileBase> Archivos { get; set; }
        [NotMapped]
        public List<string> NombresArchivos { get; set; }
        [NotMapped]
        public List<string> ContentTypes { get; set; }

        [Column("IdEstadoSubSolicitud")]
        public int IdEstadoSubSolicitud { get; set; }


        public SubSolicitud()
        {
        }

        public SubSolicitud(
            string nombreAreaExterna,
            string comentario,
            string observacion,
            string comentarioAreaExterna,
            string nombreResponsable,
            DateTime fechaSolicitud,
            DateTime fechaRespuesta,
            string nombreEstado)
                {
                    NombreAreaExterna = nombreAreaExterna;
                    Comentario = comentario;
                    Observacion = observacion;
                    ComentarioAreaExterna = comentarioAreaExterna;
                    NombreResponsable = nombreResponsable;
                    FechaSolicitud = fechaSolicitud;
                    FechaRespuesta = fechaRespuesta;
                    NombreEstado = nombreEstado;
        }


        // Constructor que inicializa todas las propiedades de la clase
        public SubSolicitud(
        int? idSubSolicitud,
        string titulo,
        string nombreAreaExterna,
        string nombreResponsable,
        int idSolicitud,
        string comentario,
        string tituloSolicitud,
        string observacion,
        string comentarioAreaExterna,
        DateTime fechaSolicitud,
        DateTime fechaRespuesta,
        string nombreEstado)
         {
            IdSubSolicitud = idSubSolicitud;
            Titulo = titulo;
            NombreAreaExterna = nombreAreaExterna;
            NombreResponsable = nombreResponsable;
            IdSolicitud = idSolicitud;
            Comentario = comentario;
            TituloSolicitud = tituloSolicitud;
            Observacion = observacion;
            ComentarioAreaExterna = comentarioAreaExterna;
            FechaSolicitud = fechaSolicitud;
            FechaRespuesta = fechaRespuesta;
            NombreEstado = nombreEstado;
         }
    }
}
