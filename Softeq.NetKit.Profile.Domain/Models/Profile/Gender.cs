// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Softeq.NetKit.Profile.Domain.Models.Profile
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        Unspecified = 0,
        [Display(Name = "Male")]
        Male = 1,
        [Display(Name = "Female")]
        Female = 2,
        [Display(Name = "Unknown")]
        Unknown = 3
    }
}