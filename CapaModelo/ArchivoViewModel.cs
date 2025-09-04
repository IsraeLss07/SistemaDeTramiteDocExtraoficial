using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CapaModelo
{
    [NotMapped]
    public class ArchivoViewModel
    {
        [NotMapped]
        public HttpPostedFileBase Archivos { get; set; }

        public string NombresArchivos { get; set; }
        public int TipoArchivo { get; set; }
        public string ContentTypes { get; set; }
    }
}
