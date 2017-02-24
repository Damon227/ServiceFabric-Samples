// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.AspNet
// File             : RequestModelValidationAttribute.cs
// Created          : 2017-02-15  15:01
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Linq;
using Credit.Kolibre.Foundation.ServiceFabric.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Credit.Kolibre.Foundation.ServiceFabric.Filters
{
    public class RequestModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        Name = e.Key,
                        Message = e.Value.Errors.First().ErrorMessage
                    }).ToArray();

                var error = errors.FirstOrDefault();
                if (error != null)
                {
                    BadRequestResponse response = new BadRequestResponse
                    {
                        Code = 400,
                        Message = error.Message
                    };
                    actionContext.Result = new BadRequestObjectResult(response);
                }
            }
        }
    }
}