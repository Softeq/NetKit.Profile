// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Profile.Domain.Infrastructure;

namespace Softeq.NetKit.Profile.Domain.Models
{
    public class Entity<T> : IBaseEntity<T>
    {
        public virtual T Id { get; set; }
    }
}