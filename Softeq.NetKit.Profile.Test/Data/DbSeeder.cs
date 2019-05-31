// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Models.Profile;

namespace Softeq.NetKit.Profile.Test.Data
{
    public class DbSeeder
    {
        private readonly IUnitOfWork _unitOfWork;

        public DbSeeder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Seed()
        {
            using (StreamReader reader = File.OpenText("SeedValues.json"))
            {
                var values = JToken.Parse(reader.ReadToEnd());
                await SeedProfiles(values["Profiles"]);
            }
        }

        public async Task SeedProfiles(JToken values)
        {
            foreach (var data in values)
            {
                var profile = (await _unitOfWork.UserProfileRepository.Query(x => x.Id == new Guid(data["Id"].Value<string>()))
                    .ToListAsync())
                    .FirstOrDefault();
                if (profile == null)
                {
                    _unitOfWork.UserProfileRepository.Add(new UserProfile
                    {
                        Id = new Guid(data["Id"].Value<string>()),
                        FirstName = data["FirstName"].Value<string>(),
                        LastName = data["LastName"].Value<string>(),
                        Location = data["Location"].Value<string>(),
                        Email = data["Email"].Value<string>(),
                        Gender = data["Gender"].ToObject<Gender>(),
                        Bio = data["Bio"].Value<string>()
                    });
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}