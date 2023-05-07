using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliary
{
    public class Files
    {
        /// <summary>
        /// Creates a file using a resource.
        /// </summary>
        /// <param name="stream">The stream of the file (resource).</param>
        /// <param name="filePath">The full name of the file (including file).</param>
        public static void CopyStream(Stream stream, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}
