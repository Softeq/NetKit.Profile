// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;

namespace Softeq.NetKit.ProfileService.Exceptions
{
    public interface IServiceException
    {
        List<ErrorDto> Errors { get; set; }
    }
}