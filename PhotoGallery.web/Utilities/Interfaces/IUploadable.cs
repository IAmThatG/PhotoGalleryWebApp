using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.web.Utilities.Interfaces
{
    internal interface IUploadable : IFileable
    {
        /// <summary>
        /// Must be a valid extension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        bool ValidateExtension();
    }
}
