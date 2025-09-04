using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CapaModelo;

namespace SistemaTramiteDoc.Services
{
    public class AuthenticationService
    {
        private readonly DBSTD _context;

        public AuthenticationService(DBSTD context)
        {
            _context = context;
        }

        public bool IsUserAuthorized(string username)
        {
            return _context.TUsuarios.Any(u => u.username == username);
        }
        public List<int> GetUserRoleId(int? id)
        {
            return _context.TUsuarioxRol
                .Where(u => u.IdUsuario == id)
                .Select(u => (int)(short)u.IdRol) // ⚠️ Convierte de SHORT (Int16) a INT
                .ToList();
        }
        public int? GetUserId(string username)
        {
            return _context.TUsuarios
            .Where(u => u.username == username)
            .Select(u => (int?)(short)u.IdUsuario) // ⚠️ Convierte de SHORT (Int16) a INT
            .FirstOrDefault();
        }
        public string GeNombreUser(string username)
        {
            return _context.TUsuarios
            .Where(u => u.username == username)
            .Select(u => (string)u.Nombres) // ⚠️ Convierte de SHORT (Int16) a INT
            .FirstOrDefault();
        }
        public bool VerificarNumeroDocumento(string numeroDoc)
        {
            bool existe = _context.TSolicitudes.Any(s => s.NumeroDoc == numeroDoc);
            return existe;
        }
        public string GetEmail(int? idSubSolicitud)
        {
            int idUsuario= _context.TSubSolicitud
            .Where(u => u.IdSubSolicitud == idSubSolicitud)
            .Select(u => (int)(short )u.IdResponsable) // ⚠️ Convierte de SHORT (Int16) a INT
            .FirstOrDefault();

            return _context.TUsuarios
            .Where(u => u.IdUsuario == idUsuario)
            .Select(u => (string)u.correo) // ⚠️ Convierte de SHORT (Int16) a INT
            .FirstOrDefault();
        }
    }
}