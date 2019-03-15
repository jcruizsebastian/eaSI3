using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eaSI3Web.GlobalErrorHandling
{
    public class HttpStatusContentResult : ActionResult
    {       
        private HttpStatusCode _statusCode;
        private string _statusDescription;

        public HttpStatusContentResult( HttpStatusCode statusCode = HttpStatusCode.OK,
                                       string statusDescription = null)
        {      
            _statusCode = statusCode;
            _statusDescription = statusDescription;

            //base.context.HttpContext.Response
        }

        
    }
}
