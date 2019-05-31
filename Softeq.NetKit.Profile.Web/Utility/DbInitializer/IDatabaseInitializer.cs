// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.NetKit.Profile.Web.Utility.DbInitializer
{
    public interface IDatabaseInitializer
    {
        Task Seed();
    }
}