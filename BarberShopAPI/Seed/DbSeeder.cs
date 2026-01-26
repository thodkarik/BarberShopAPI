using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.Security;

namespace BarberShopAPI.Data.Seed
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    Email = "admin@barbershop.gr",
                    Password = PasswordHashUtil.Hash("admin123"),
                    FirstName = "Admin",
                    LastName = "User",
                    UserRole = UserRole.Admin
                });
            }

            if (!context.Users.Any(u => u.Username == "barber1"))
            {
                context.Users.Add(new User
                {
                    Username = "barber1",
                    Email = "barber@barbershop.gr",
                    Password = PasswordHashUtil.Hash("barber123"),
                    FirstName = "John",
                    LastName = "Barber",
                    UserRole = UserRole.Barber
                });
            }

            if (!context.Users.Any(u => u.Username == "customer1"))
            {
                context.Users.Add(new User
                {
                    Username = "customer1",
                    Email = "customer@barbershop.gr",
                    Password = PasswordHashUtil.Hash("customer123"),
                    FirstName = "Mike",
                    LastName = "Customer",
                    UserRole = UserRole.Customer
                });
            }

            if (!context.Users.Any(u => u.Username == "reception1"))
            {
                context.Users.Add(new User
                {
                    Username = "reception1",
                    Email = "reception1@test.com",
                    Password = PasswordHashUtil.Hash("reception123"),
                    FirstName = "Reception",
                    LastName = "User",
                    UserRole = UserRole.Receptionist
                });
            }

            context.SaveChanges();

            var barberUser = context.Users.First(u => u.Username == "barber1");
            var customerUser = context.Users.First(u => u.Username == "customer1");

            if (!context.Barbers.Any(b => b.UserId == barberUser.Id))
            {
                context.Barbers.Add(new Barber
                {
                    FirstName = "John",
                    LastName = "Barber",
                    PhoneNumber = "6900000000",
                    UserId = barberUser.Id
                });
            }

            if (!context.Customers.Any(c => c.UserId == customerUser.Id))
            {
                context.Customers.Add(new Customer
                {
                    FirstName = "Mike",
                    LastName = "Customer",
                    PhoneNumber = "6911111111",
                    UserId = customerUser.Id
                });
            }

            context.SaveChanges();

            if (!context.Services.Any(s => s.Name == "Simple Haircut"))
            {
                context.Services.Add(new Service
                {
                    Name = "Simple Haircut",
                    Description = "Basic haircut service",
                    DurationMinutes = 30,
                    Price = 10
                });
            }

            if (!context.Services.Any(s => s.Name == "Full Care"))
            {
                context.Services.Add(new Service
                {
                    Name = "Full Care",
                    Description = "Haircut and beard care",
                    DurationMinutes = 60,
                    Price = 18
                });
            }

            context.SaveChanges();
        }
    }
}

