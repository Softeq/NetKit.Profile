// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.EntityFrameworkCore;

namespace Softeq.NetKit.Profile.SQLRepository.Mappings.Abstract
{
    internal interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder builder);
    }
}