using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace CapaModelo
{
    public class DBSTD : DbContext
    {
        public DbSet<Usuario> TUsuarios { get; set; }
        public DbSet<UsuarioxRol> TUsuarioxRol { get; set; }
        public DbSet<SubSolicitud> TSubSolicitud { get; set; }
        public DbSet<Solicitud> TSolicitudes { get; set; }

    }
}
