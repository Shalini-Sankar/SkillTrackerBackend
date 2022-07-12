using System;

namespace SkillTracker.Entities
{
    public abstract class EntityBase
    {
        
        public string CreatedBy { get; set; }

        
        public DateTime CreatedDate { get; set; }

        
        public string LastModifiedBy { get; set; }

       
        public DateTime? LastModifiedDate { get; set; }
    }
}
