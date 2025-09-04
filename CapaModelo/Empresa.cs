using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Empresa
    {
        public Empresa(int empresaId, string empresaNombre)
        {
            EmpresaId = empresaId;
            EmpresaNombre = empresaNombre;
        }

        public int EmpresaId { get; set; }
        public string EmpresaNombre { get; set; }
        public Empresa()
        {
        }
    }
}
