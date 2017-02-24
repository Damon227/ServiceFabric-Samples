// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.AspNet
// File             : KolibreController.cs
// Created          : 2017-02-15  15:01
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Linq;
using Credit.Kolibre.Foundation.ServiceFabric.Model;
using Microsoft.AspNetCore.Mvc;

namespace Credit.Kolibre.Foundation.ServiceFabric.Controller
{
    public class KolibreController : Microsoft.AspNetCore.Mvc.Controller
    {
        [NonAction]
        public BadRequestObjectResult BadRequest(int code, bool includeStackTrace, string message, params string[] data)
        {
            includeStackTrace = includeStackTrace || Request.Query.ContainsKey("stacktrace") || Request.Query.ContainsKey("stackTrace") || Request.Query.ContainsKey("StackTrace");

            BadRequestResponse response = new BadRequestResponse
            {
                Code = code,
                Message = message,
                Data = data.ToList(),
                StackTrace = includeStackTrace ? Environment.StackTrace : string.Empty
            };

            return BadRequest(response);
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(bool includeStackTrace, string message, params string[] data)
        {
            return BadRequest(0, includeStackTrace, message, data);
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(string message, params string[] data)
        {
            return BadRequest(0, false, message, data);
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(string message)
        {
            return BadRequest(0, false, message, Enumerable.Empty<string>().ToArray());
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(string message, int code, params string[] data)
        {
            return BadRequest(code, false, message, data);
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(int code, string message, params string[] data)
        {
            return BadRequest(code, false, message, data);
        }

        [NonAction]
        public OkObjectResult OK()
        {
            return Ok(new object());
        }
    }
}