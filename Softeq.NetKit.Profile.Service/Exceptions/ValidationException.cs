// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;

namespace Softeq.NetKit.ProfileService.Exceptions
{
    public class ValidationException : ServiceException
    {
        public ValidationException(ErrorDto error) : base(error)
        {
        }
        public ValidationException(IEnumerable<ErrorDto> errors) : base(errors)
        {
        }

        public ValidationException(string message) : base(message, new ErrorDto(ErrorCode.ValidationError, message))
        {
        }

        public ValidationException(Exception innerException) : base("See inner exception.", innerException, new ErrorDto(ErrorCode.ValidationError, innerException.Message))
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException, new ErrorDto(ErrorCode.ValidationError, message))
        {
        }

        public ValidationException(string message, IEnumerable<string> errors) : base(message, errors.Select(x => new ErrorDto(ErrorCode.ValidationError, x)).ToArray())
        {
        }
    }
}