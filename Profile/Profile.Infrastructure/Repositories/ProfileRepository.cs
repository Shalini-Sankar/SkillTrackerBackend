using Profile.Application.Contracts;
using Profile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SkillTrackerContext _context;

        public ProfileRepository(SkillTrackerContext dbcontext)
        {
            this._context = dbcontext;
        }

        public async Task<ProfileEntity> AddAsync(ProfileEntity entity)
        {
            await this._context.Profile.AddAsync(entity);
            await this._context.SaveChangesAsync();
            return entity;
        }

        public async Task<ProfileEntity> UpdateAsync(ProfileEntity entity)
        {
            ProfileEntity existingProfile = _context.Profile.FirstOrDefault(s => s.EmpId == entity.EmpId || s.UserId == entity.UserId);
            if (existingProfile != null)
            {
                foreach (var skill in entity.Skills)
                {
                    var exSkill = existingProfile.Skills.FirstOrDefault(s => s.Name.Equals(skill.Name, StringComparison.OrdinalIgnoreCase));
                    if (exSkill != null)
                        exSkill.Proficiency = skill.Proficiency;
                    else
                    {
                        Skill newSkill = new Skill();
                        newSkill.Name = skill.Name;
                        newSkill.IsTechnical = skill.IsTechnical;
                        newSkill.Proficiency = skill.Proficiency;
                        existingProfile.Skills.Add(newSkill);
                    }
                }

                existingProfile.LastModifiedDate = DateTime.Now;

                this._context.Update(existingProfile);
                await this._context.SaveChangesAsync();
                return entity;
            }
            else 
                return null;
        }

        public Task<IList<ProfileEntity>> AddRangeAsync(IList<ProfileEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object hashKey, object rangeKey)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileEntity> GetItem(string hadhKey)
        {
            throw new NotImplementedException();
        }

        public async Task<ProfileEntity> GetTime(string hadhKey)
        {
            throw new NotImplementedException();
        }

        public async Task PutItem(ProfileEntity entity)
        {
            throw new NotImplementedException();
        }

    }
}
