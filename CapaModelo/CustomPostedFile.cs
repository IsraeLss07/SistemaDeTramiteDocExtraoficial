using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CapaModelo
{
    [NotMapped]
    public class CustomPostedFile : HttpPostedFileBase
    {
        public byte[] fileBytes;
        public string fileName;
        public string contentType;
        private readonly Stream fileStream;


        public CustomPostedFile(byte[] fileBytes, string fileName, string contentType)
        {
            this.fileName = fileName;
            this.fileBytes = fileBytes;
            this.contentType = contentType;
            this.fileStream = new MemoryStream(fileBytes); // Crear el stream una vez

        }
        public override int ContentLength => fileBytes.Length;

        public override string FileName => fileName;

        public override string ContentType => contentType;

        public override Stream InputStream => fileStream; // Reutilizarlo, no crear nuevo cada vez
    }
}
