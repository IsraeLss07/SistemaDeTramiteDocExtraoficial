using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Institucion
    {
        public Institucion(int institucionId, string institucionNombre)
        {
            InstitucionId = institucionId;
            InstitucionNombre = institucionNombre;
        }

        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; }

        public Institucion()
        {
        }
    }


}
