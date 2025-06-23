
# 🚗 CarCare System - Web API Project

The **CarCare System** is a complete web-based application built with ASP.NET Core using Onion Architecture. It facilitates the interaction between Users and technicians, allowing users to request vehicle services, track request statuses, manage vehicles, and more. It supports role-based access (Admin, technician, User), with advanced features like background jobs, image handling, validation, and secure authentication.

---

## 🧩 Features

### Phase 01: Foundations
- Implemented **RESTful API** principles.
- Structured with **Onion Architecture** for separation of concerns.
- Used **AutoMapper**, **FluentValidation**, and **EF Core**.
- Built the following modules:
  - `DbContext` configuration.
  - `DbInitializer` for seeding initial data.
  - Generic **Repository** and **Unit of Work** patterns.

### Phase 02: Core Functionalities
- Developed modules: `Auth`, `Service Requests`, `Vehicles`, `Mechanics`, `Customers`.
- Applied Specification Pattern for dynamic filtering , Search , Ordering and sorting.
- Added image upload functionality for profiles.
- Integrated **Hangfire** to:
  - Auto resend requests.
- Implemented **Fluent Validation** on all DTOs.
- Caching enabled via in-memory or distributed cache.
- Integrate With Remote Database To Implement Caching With Redis **Up Stash**
- Email/SMS services abstraction via `CarCare.Application.Abstraction`.

### Phase 03: Service Request Module
- User Approach
| - Choose Service and choose the Request Type (automatic, manual)
|     - in automatic Request the app Auto-send to next technician using Hangfire if mechanic declines or timeout.
|     - in manual Request the app go to choose another technician by User
| - Go to the payment page
- technician Approach
 - technician receive service requests and respond (Accept, Reject).
 - in automatic Request the app Auto-send to next technician using Hangfire if mechanic declines or timeout.
 - in manual Request the app back to choose another technician by User
- CRUD support with status tracking: Pending, InProgress, Completed, Cancelled.
- Includes advanced filtering (by location, status).

### Phase 04: Vehicle Module
- Users can register vehicles with VIN_Number, Model, Color, Year and plate number.
- Users can Update his vehicle
- Users can Delete his vehicle
- technician sees only related vehicle data.

### Phase 05: Identity & Authentication
- Role-based system: **Admin**, **Technician**, **User**.
- Implemented **JWT Authentication** + **Refresh Tokens**.
- Extended **Swagger UI** for secure testing.
- Password hashing and identity management via `ASP.NET Identity`.

### Phase 06: Contact
- Admin can broadcast messages to all users or for Specific User (Technician or User).
- Admin can Update messages
- Admin can Delete messages

### Phase 07: Feedback
- Users and technician can send Feedback to the app with a message
- they can get thier feedback or update it

### Phase 08: Dashboard & Management
- Admin dashboard for overview of:
  - (Create, Update or Delete) Service Type
  - Total users, technician, requests.
  - Feedback and contact inquiries.
- Export reports.

### Phase 09: Payment Integration
- Integrated Stripe for payment processing.
- Developed and tested payment endpoints with webhooks for event notifications.

## 🛠️ Technologies Used
- **Framework:** ASP.NET Core 8
- **Database:** SQL Server + EF Core
- **Background Jobs:** Hangfire
- **Caching:** Redis
- **Authentication:** ASP.NET Identity, JWT, Refresh Tokens
- **Validation:** FluentValidation
- **Payment:** Stripe abstraction supported
- **Notifications:** Email + SMS via abstraction layer
- **Documentation:** Swagger, Postman

---

### Testing
- **xUnit** - Unit testing framework
- **FakeItEasy** - Mocking library
- **FluentAssertions** - Readable assertions
- **MockQueryable.FakeItEasy** - EF Core query mocking

## 🚀 Deployment
- Integrate With Monster Api To Deploy Project At Remote Service
- **Front End Deploy** [[Car Care](http://carcare-git-main-mahmoudshaaban00s-projects.vercel.app)] 


---

## 📘 Key Learnings
- Real-world Onion Architecture and dependency inversion.
- Background job management with fallback logic.
- Secure JWT authentication and scalable API practices.
- Dynamic filtering and specifications using expressions.
- Integration of email/SMS/push services.

---

## 📂 Project Structure (Simplified)
```plaintext
CarCare
├── CarCare.API                           # Presentation Layer
│   ├── Middleware                        # Exception handling, Logging
│   ├── Extensions                        # Identity Extension, Initializer Extension
│   └── Program.cs                        # Entry point
│
├── CarCare.Apis.Controllers              # Separate Controllers Project
│   ├── Controllers
│      ├── AccountController.cs
│      ├── ContactController.cs
│      ├── FeedbackController.cs
│      └── RequestController.cs
│   ├── Filters
│      └── Cached Attribute
│
├── CarCare.Domain                        # Domain Layer
│   ├── Entities                          # Core business models (Technician, Request, etc.)
│   ├── Domain.Interfaces                 # IRepository, IUnitOfWork
│   ├── Specifications                    
│
├── CarCare.Application                   # Application Layer (Union Architecture)
│   ├── Services                          # AuthService, RequestService, etc.
│   ├── Mapping                           # AutoMapper Profiles
│   └── ServiceManager                    
│
├── CarCare.Application.Abstraction       # Abstractions for external services (Email, SMS, Stripe)
│   ├── DTOs                              # Input/Output models
│   ├── Services                          # IAuthService, IRequestService, IEmailService, ISmsService, etc.
│   └── Validators                        # LoginValidator, RegisterValidator, CreateVehicleValidator, etc.
│
├── CarCare.Infrastructure                # Infrastructure Layer
│   ├── AttachmentService
│   ├── CacheService                      # Redis.
│   └── PaymentService                    # Stripe.
│
├── CarCare.Persistence                   # Persistence Layer
│   ├── Data                              # DbContext, Configurations, Migrations, Interceptors, DbInitializer, etc.
│   ├── Repositories                      # GenericRepository, ServiceRequestRepository, etc.
│   ├── UnitOfWork                      
│   └── ExternalServices                  # Stripe, Twilio, Email.
│
└── CarCare.Shared                        # Shared Utilities
│   ├── AppSettings                       # EmailSettings, SMSSettings
│   └── ErrorModoule                      # ApiExeptionResponse, ApiResponse, ApiValidationErrorResponse, BadRequestExeption, etc.
```

---

## 🤝 Contributions & Contact
This project is open for improvements and extensions!

> Developed by Mahmoud Ahmed AbdelTawab and Mohamed Hamdy Mahmoud

> 📧 mahmoud.ahmed.topa@gmail.com  
> 🔗 [LinkedIn](https://linkedin.com/in/mahmoud-ahmed-abdeltwab)  
> 💻 [GitHub](https://github.com/Mahmoud-Ahmed-23)

> 📧 mohammedhamdi726@gmail.com  
> 🔗 [LinkedIn](https://www.linkedin.com/in/mohamedhamdy23/)  
> 💻 [GitHub](https://github.com/01124833532mo)
