using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class AreaExterna
    {
        public int IdAreaExterna { get; set; } // Clave primaria
        public string Nombre { get; set; } // Nombre del Area Externa
        public AreaExterna()
        {
        }

        public AreaExterna(int idAreaExterna, string nombre)
        {
            IdAreaExterna = idAreaExterna;
            Nombre = nombre;
        }


    }
}
