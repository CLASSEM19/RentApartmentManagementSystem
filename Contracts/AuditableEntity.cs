using System;
namespace ApartmentRentManagementSystem.Contracts
{
    public abstract class AuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
    {
        public int CreatedBy{get; set;}
        public DateTime CreatedOn{get; set;} = DateTime.UtcNow;
        public int LastModifiedBy{get; set;}
        public DateTime? LastModifiedOn{get; set;} = DateTime.UtcNow;
        public DateTime? DeletedOn{get; set;}
        public int? DeletedBy{get; set;}
        public bool IsDeleted{get; set;}
    }
}