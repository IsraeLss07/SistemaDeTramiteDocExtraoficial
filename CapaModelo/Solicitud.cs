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
    [Table("TSolicitudes")]
    public class Solicitud
    {
        [Key]
        [Column("IDSolicitud")]
        public int? IdSolicitud { get; set; }
        [NotMapped]
        public string Titulo { get; set; }
        [Column("NUMERODOCUMENTO")]
        public string NumeroDoc { get; set; }
        [NotMapped]
        public string Referencia { get; set; }
        [NotMapped]
        public string Comentarios { get; set; }
        [NotMapped]
        public string AreaExternaNombre { get; set; }
        [NotMapped]
        public string AnalistaNombre { get; set; }
        [NotMapped]
        public string AnalistaCorreo { get; set; }
        [NotMapped]
        public string Asunto { get; set; }
        [NotMapped]
        public DateTime FechaEventoExterno { get; set; }
        [NotMapped]
        public string NombreFase { get; set; }
        [NotMapped]
        public string NombreEstado { get; set; }
        [NotMapped]
        public string NombreEmpresa { get; set; }
        [NotMapped]
        public string NombreFormato { get; set; }
        [NotMapped]
        public string NombreProyecto { get; set; }
        [NotMapped]
        public DateTime FechaRecepcion { get; set; }
        [NotMapped]
        public DateTime FechaVencInterno { get; set; }
        [NotMapped]
        public string TipoDoc { get; set; }
        [NotMapped]
        public string NombreInstitucion { get; set; }
        [NotMapped]
        public DateTime FechaEmision { get; set; }
        [NotMapped]
        public int PlazoAtc { get; set; }
        [NotMapped]
        public string NombreResponsable { get; set; }
        [NotMapped]
        public string FueRegistradoPor { get; set; }
        [NotMapped]
        public DateTime FechaRegistro { get; set; }
        [NotMapped]
        public string Piso { get; set; }
        [NotMapped]
        public string Estante { get; set; }
        [NotMapped]
        public string File { get; set; }
        [NotMapped]
        public int Penalidad { get; set; }
        [NotMapped]
        public int? IdPersonaRegistra { get; set; }
        [NotMapped]
        public List<HttpPostedFileBase> Archivos { get; set; }
        [NotMapped]
        public List<string> NombresArchivos { get; set; }
        [NotMapped]
        public List<int> TipoArchivo { get; set; }
        [NotMapped]
        public List<string> ContentTypes { get; set; }
        [NotMapped]
        public List<SubSolicitud> SubSolicitudes { get; set; }


        public Solicitud()
        {
        }


        public Solicitud(string nombreEmpresa, string nombreFormato, string nombreProyecto, DateTime fechaRecepcion, DateTime fechaVencInterno, string tipoDoc, string nombreInstitucion, DateTime fechaEmision, int plazoAtc, string nombreResponsable, string fueRegistradoPor, DateTime fechaRegistro, string piso, string estante, string file, int penalidad)
        {
            NombreEmpresa = nombreEmpresa;
            NombreFormato = nombreFormato;
            NombreProyecto = nombreProyecto;
            FechaRecepcion = fechaRecepcion;
            FechaVencInterno = fechaVencInterno;
            TipoDoc = tipoDoc;
            NombreInstitucion = nombreInstitucion;
            FechaEmision = fechaEmision;
            PlazoAtc = plazoAtc;
            NombreResponsable = nombreResponsable;
            FueRegistradoPor = fueRegistradoPor;
            FechaRegistro = fechaRegistro;
            Piso = piso;
            Estante = estante;
            File = file;
            Penalidad = penalidad;
        }
    }
}
