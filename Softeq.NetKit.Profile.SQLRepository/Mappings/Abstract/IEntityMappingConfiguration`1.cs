// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Profile.SQLRepository.Mappings.Abstract
{
    internal interface IEntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}