using ApartmentRentManagementSystem.Entities;
using ApartmentRentManagementSystem.Identity;
using Microsoft.EntityFrameworkCore;
namespace ApartmentRentManagementSystem.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
         : base(options)
        {

        }

    
        public DbSet<Admin> Admins{get; set;}
        public DbSet<Address> Address{get; set;}
        public DbSet<Apartment> Apartments{get; set;}
        public DbSet<Category> Categories{get; set;}
        public DbSet<Customer> Customers{get; set;}
        public DbSet<CustomerApartment> CustomerApartments{get; set;}
        public DbSet<Landlord> Landlords{get; set;}
        public DbSet<HouseEquipment> HouseEquipments{get; set;}
        public DbSet<Image> Images{get; set;}
        public DbSet<Utility> Utilities{get; set;}
        public DbSet<User> User{get; set;}
        public DbSet<Role> Role{get; set;}
        public DbSet<UserRole> UserRole{get; set;}
        public DbSet<Message> Messages{get; set;}
        public DbSet<UserMessage> UserMessages{get; set;}
        public DbSet<Complaint> Complaints{get; set;}

    }
    
}