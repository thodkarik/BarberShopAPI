# BarberShopAPI

Backend Web API για σύστημα διαχείρισης barbershop με ραντεβού, ρόλους χρηστών και authentication.

Η εφαρμογή αναπτύχθηκε στο πλαίσιο της τελικής εργασίας του Coding Factory (ΟΠΑ).

---

## Περιγραφή
Το σύστημα υποστηρίζει:
- Δημιουργία και διαχείριση ραντεβού
- Authentication με JWT
- Authorization με ρόλους:
  - Customer
  - Barber
  - Receptionist
  - Admin
- Business rules για ωράριο και διάρκεια ραντεβού
- REST API με Swagger documentation

---

## Τεχνολογίες
- ASP.NET Core 8 Web API
- Entity Framework Core (Model-First)
- Microsoft SQL Server
- JWT Authentication
- AutoMapper
- Serilog
- Swagger

---

## Αρχιτεκτονική
Ακολουθείται στρωματοποιημένη αρχιτεκτονική:
- Controllers (API endpoints)
- Services (business logic)
- Repositories (data access)
- DTOs
- Domain Models
- Middleware για global exception handling

---

## Database
- SQL Server
- Δημιουργία βάσης μέσω EF Core Migrations
- Seed δεδομένων για development (users & services)

---

## Πώς γίνεται build & run (Backend)

### Προαπαιτούμενα
- .NET SDK 8
- SQL Server (π.χ. SQL Express)
- Visual Studio 2022

### 1) Clone του repository
git clone https://github.com/thodkarik/BarberShopAPI.git

### 2) Ρύθμιση connection string
Στο appsettings.Development.json ορίζεται το connection string προς SQL Server.

Τα ευαίσθητα δεδομένα (JWT key, credentials) δεν γίνονται commit και ορίζονται μέσω User Secrets.

### 3) Δημιουργία βάσης δεδομένων
Από Visual Studio:
Tools → NuGet Package Manager → Package Manager Console

Εκτέλεση:
Update-Database

Η εντολή:
- Δημιουργεί τη βάση BarberShopDB
- Εφαρμόζει migrations
- Κάνει seed τα αρχικά δεδομένα

### 4) Εκκίνηση της εφαρμογής
Τρέχουμε το project από Visual Studio (F5) ή μέσω command line:
dotnet run --project BarberShopAPI/BarberShopAPI.csproj

### 5) Swagger
Η τεκμηρίωση του API είναι διαθέσιμη στο:
http://localhost:7236/swagger

---

## Seeded users (development)

- Admin: admin / admin123
- Barber: barber1 / barber123
- Customer: customer1 / customer123
- Receptionist: reception1 / reception123

---

## Σημειώσεις
- Τα GET endpoints είναι public.
- CREATE / UPDATE / DELETE Services επιτρέπονται μόνο σε Admin.
- Τα ραντεβού ακολουθούν ωράριο 10:00–20:00, Δευτέρα–Σάββατο.
- Η διάρκεια των ραντεβού είναι 30 ή 60 λεπτά.

---

## Author
Θοδωρής Καρίκης
