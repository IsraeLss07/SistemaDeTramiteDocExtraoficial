using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class TipoDocumento
    {
        public TipoDocumento(int tipoDocumentoId, string tipoDocumentoNombre)
        {
            TipoDocumentoId = tipoDocumentoId;
            TipoDocumentoNombre = tipoDocumentoNombre;
        }

        public int TipoDocumentoId { get; set; }
        public string TipoDocumentoNombre { get; set; }

        public TipoDocumento()
        {
        }
    }
}
