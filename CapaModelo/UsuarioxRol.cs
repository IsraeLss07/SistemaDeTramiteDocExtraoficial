using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    [Table("TUsuarioxRol")]
    public class UsuarioxRol
    {
        [Key, Column(Order = 0)]
        public int? IdUsuario { get; set; } // Clave primaria
        [Key, Column(Order = 1)]
        public int? IdRol { get; set; } // Clave primaria}

        public UsuarioxRol(int idUsuario, int idRol )
        {
            IdUsuario = idUsuario;
            IdRol = idRol;
        }

        public UsuarioxRol()
        {
        }
    }
}
