using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapaModelo
{
    [Table("TUsuario")]
    public class Usuario {

        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; } // Clave primaria
        public string Nombres { get; set; } // Nombre del usuario

        [Column("IDROL")]
        public int IdRol { get; set; } // Identificador del rol

        [Column("username")]
        public string username { get; set; }
        [Column("CORREO")]
        public string correo { get; set; }


        public Usuario(int idUsuario, string nombres, string apellidoPaterno, string apellidoMaterno, int idRol)
        {
            IdUsuario = idUsuario;
            Nombres = nombres;
            IdRol = idRol;
        }

        public Usuario()
        {
        }
    }
}
