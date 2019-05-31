// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.ProfileService.Utility.ErrorHandling
{
    public class ValidationErrorDto : ErrorDto
    {
        public ValidationErrorDto(string code, string description, string propertyName, string validationErrorCode)
        {
            Code = code;
            Description = description;
            PropertyName = propertyName;
            ValidationErrorCode = validationErrorCode;
        }
        public string PropertyName { get; set; }

        public string ValidationErrorCode { get; set; }
    }
}