using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Proyecto
    {
        public Proyecto(int proyectoId, string proyectoNombre)
        {
            ProyectoId = proyectoId;
            ProyectoNombre = proyectoNombre;
        }

        public int ProyectoId { get; set; }
        public string ProyectoNombre { get; set; }
        public Proyecto()
        {
        }
    }
}
