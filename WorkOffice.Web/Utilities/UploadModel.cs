using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkOffice.Web.Utilities
{
    public class UploadModel
    {
        public IFormFile file { get; set; }
    }
}
