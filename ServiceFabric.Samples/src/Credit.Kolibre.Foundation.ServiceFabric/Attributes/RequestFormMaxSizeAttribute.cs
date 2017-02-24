// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.AspNet
// File             : RequestFormMaxSizeAttribute.cs
// Created          : 2017-02-15  15:01
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Credit.Kolibre.Foundation.ServiceFabric.Attributes
{
    /// <summary>
    ///     Filter to set size limits for request form data
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestFormMaxSizeAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private readonly FormOptions _formOptions;

        public RequestFormMaxSizeAttribute()
        {
            _formOptions = new FormOptions
            {
                BufferBodyLengthLimit = long.MaxValue,
                MultipartBodyLengthLimit = long.MaxValue,
                MultipartBoundaryLengthLimit = int.MaxValue,
                MultipartHeadersCountLimit = int.MaxValue,
                ValueLengthLimit = int.MaxValue
            };
        }

        #region IAuthorizationFilter Members

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IFeatureCollection features = context.HttpContext.Features;
            IFormFeature formFeature = features.Get<IFormFeature>();

            if (formFeature?.Form == null)
            {
                // Request form has not been read yet, so set the limits
                features.Set(new FormFeature(context.HttpContext.Request, _formOptions));
            }
        }

        #endregion

        #region IOrderedFilter Members

        public int Order { get; set; }

        #endregion
    }
}