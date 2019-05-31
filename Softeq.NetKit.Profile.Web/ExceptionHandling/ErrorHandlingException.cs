// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;

namespace Softeq.NetKit.Profile.Web.ExceptionHandling
{
    public static class ErrorHandlingException
    {
        public static List<ErrorDto> ToErrorModel(this ModelStateDictionary modelSate)
        {
            var modelError = new List<ErrorDto>();
            foreach (var error in modelSate)
            {
                modelError.Add(new ErrorDto()
                {
                    Code = error.Key,
                    Description = error.Value.Errors.FirstOrDefault()?.ErrorMessage

                });
            }
            return modelError;
        }
    }
}