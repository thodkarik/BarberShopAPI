using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.Security;

namespace BarberShopAPI.Data.Seed
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Αν υπάρχουν ήδη δεδομένα, δεν ξανακάνουμε seed
            if (context.Services.Any() || context.Barbers.Any() || context.Customers.Any())
                return;

            // Users
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@barbershop.gr",
                Password = PasswordHashUtil.Hash("admin123"), // demo μόνο
                FirstName = "Admin",
                LastName = "User",
                UserRole = UserRole.Admin
            };

            var barberUser = new User
            {
                Username = "barber1",
                Email = "barber@barbershop.gr",
                Password = PasswordHashUtil.Hash("barber123"),
                FirstName = "John",
                LastName = "Barber",
                UserRole = UserRole.Barber
            };

            var customerUser = new User
            {
                Username = "customer1",
                Email = "customer@barbershop.gr",
                Password = PasswordHashUtil.Hash("customer123"),
                FirstName = "Mike",
                LastName = "Customer",
                UserRole = UserRole.Customer
            };

            context.Users.AddRange(adminUser, barberUser, customerUser);
            context.SaveChanges();

            // Barber
            var barber = new Barber
            {
                FirstName = "John",
                LastName = "Barber",
                PhoneNumber = "6900000000",
                UserId = barberUser.Id
            };

            // Customer
            var customer = new Customer
            {
                FirstName = "Mike",
                LastName = "Customer",
                PhoneNumber = "6911111111",
                UserId = customerUser.Id
            };

            context.Barbers.Add(barber);
            context.Customers.Add(customer);
            context.SaveChanges();

            // Services
            var haircut = new Service
            {
                Name = "Simple Haircut",
                Description = "Basic haircut service",
                DurationMinutes = 30,
                Price = 10
            };

            var fullCare = new Service
            {
                Name = "Full Care",
                Description = "Haircut and beard care",
                DurationMinutes = 60,
                Price = 18
            };

            context.Services.AddRange(haircut, fullCare);
            context.SaveChanges();
        }
    }
}
