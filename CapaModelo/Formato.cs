using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Formato
    {
        public Formato(int formatoId, string formatoNombre)
        {
            FormatoId = formatoId;
            FormatoNombre = formatoNombre;
        }

        public int FormatoId { get; set; }
        public string FormatoNombre { get; set; }

        public Formato()
        {
        }

    }
}
