🧼 BaseCleanArchitecture (.NET 8 + Clean Architecture)

Este proyecto implementa una arquitectura limpia usando .NET 8, Entity Framework Core con MySQL, y las mejores prácticas de desarrollo para crear aplicaciones escalables, mantenibles y fáciles de probar.
📐 Arquitectura

Aplicamos Clean Architecture, dividiendo la solución en las siguientes capas:

    Domain: Entidades del negocio y contratos (interfaces).
    Application: Lógica de negocio, CQRS (comandos y consultas), validaciones.
    Infrastructure: Acceso a datos (EF Core), implementaciones técnicas.
    API (Presentation): Controladores, configuración DI, middlewares, Swagger.

🧰 Tecnologías utilizadas

    .NET 8
    ASP.NET Core Web API
    Entity Framework Core 8.0.16
    Pomelo.EntityFrameworkCore.MySql 8.0.4
    AutoMapper
    MediatR (CQRS y Mediator Pattern)
    FluentValidation
    Serilog (logging estructurado)
    Swagger (Swashbuckle) (documentación automática)
    xUnit + Moq (pruebas unitarias)
    Docker (opcional)

📁 Estructura del proyecto

BaseCleanArchitecture
│
├── src
│   ├── BaseCleanArchitecture.API
│   │   ├── Controllers
│   │   ├── Middlewares
│   │   └── Extensions
│   ├── BaseCleanArchitecture.Application
│   │   ├── Commands
│   │   ├── Queries
│   │   ├── DTOs
│   │   ├── Validators
│   │   └── Mappings
│   ├── BaseCleanArchitecture.Domain
│   │   ├── Entities
│   │   └── Interfaces
│   └── BaseCleanArchitecture.Infrastructure
│       ├── Persistence
│       └── Repositories
│       └── Services
└── tests
    └── BaseCleanArchitecture.UnitTests
        ├── CommandsTests
        ├── QueriesTests
        └── ValidatorsTests



Hash:
https://bcrypt-generator.com/