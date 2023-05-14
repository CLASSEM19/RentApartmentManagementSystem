using System;
namespace ApartmentRentManagementSystem.Contracts
{
    public interface ISoftDelete
    {
         DateTime? DeletedOn{get; set;}
         int? DeletedBy{get; set;}
         bool IsDeleted{get; set;}
    }
}