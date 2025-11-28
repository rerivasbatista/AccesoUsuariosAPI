![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Programming-239120?style=for-the-badge&logo=csharp)
![JWT](https://img.shields.io/badge/JWT-Authentication-000000?style=for-the-badge&logo=jsonwebtokens)
![Swagger](https://img.shields.io/badge/Swagger-API%20Docs-85EA2D?style=for-the-badge&logo=swagger)
![EF
Core](https://img.shields.io/badge/EntityFramework-Core-512BD4?style=for-the-badge&logo=dotnet)
![BCrypt](https://img.shields.io/badge/Security-BCrypt-red?style=for-the-badge)
![REST
API](https://img.shields.io/badge/REST-API-005571?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Production%20Ready-success?style=for-the-badge)

# 🚀 AccesoUsuariosAPI

API REST desarrollada en **.NET 8** para la **gestión de usuarios con
autenticación JWT**, consumo de **API externa**, validaciones con
**FluentValidation** y seguridad con **BCrypt**.\
Incluye endpoints protegidos que solo pueden ser consumidos con un
**token válido**.

------------------------------------------------------------------------

## 🏗️ Arquitectura del Proyecto

El proyecto está construido utilizando una:

> ✅ **Arquitectura en Capas (Layered Architecture)**\
> alineada con principios de **Clean Architecture** y **Separación de
> Responsabilidades**.

### 📐 Capas del Proyecto

    API (Program.cs)
    │
    ├── Dtos         → Transporte de datos (Request / Response)
    ├── Models       → Entidades del dominio
    ├── Data         → DbContext (Entity Framework Core)
    ├── Repositories → Acceso a datos
    ├── Services     → Lógica de negocio
    ├── Helpers      → Seguridad (JWT)

------------------------------------------------------------------------

## 🔐 Seguridad

-   Autenticación con **JWT**
-   Hash de contraseñas con **BCrypt**
-   Expiración y firma criptográfica con HS256
-   Protección de endpoints con `.RequireAuthorization()`

------------------------------------------------------------------------

## 🧪 Pruebas Unitarias

-   Framework: **xUnit**
-   Base de datos: **EF InMemory**
-   Pruebas cubren:
    -   Registro exitoso
    -   Login correcto
    -   Rechazo de credenciales incorrectas
    -   Validación de cifrado con BCrypt
    -   Generación de JWT

Ejecución:

``` bash
dotnet test
```

------------------------------------------------------------------------
## 🔑 Endpoints

### ✅ Registro

POST /api/accesousuarios/register

### ✅ Login

POST /api/accesousuarios/login

### ✅ Lectura de API externa (JWT)

GET /api/external/posts

### ✅ Inserción en API externa (JWT)

POST /api/external/posts

------------------------------------------------------------------------

## ⚙️ Configuración y Despliegue

### Desarrollo

-   EF InMemory
-   Swagger activo
-   `dotnet run`

### Producción

-   EF + SQL Server
-   Migraciones activas
-   HTTPS
-   Despliegue en IIS, Docker, Azure, AWS

Migraciones:

``` bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

------------------------------------------------------------------------

## 🔁 CI/CD

Preparado para: - GitHub Actions - Azure DevOps - GitLab CI

Flujo: 1. Push 2. Restore 3. Test 4. Build 5. Deploy

------------------------------------------------------------------------

## 🚀 Escalabilidad

-   Diseño desacoplado
-   Preparado para microservicios
-   Soporte para Redis, balanceo de carga, roles

------------------------------------------------------------------------

## ✅ Buenas Prácticas

-   Repository + Service
-   DTOs
-   JWT
-   BCrypt
-   Pruebas reales
-   Principios SOLID
-   Inyección de dependencias

------------------------------------------------------------------------

## ❗ Manejo de Errores

-   Sin token: `401 Unauthorized`
-   Credenciales inválidas: `400 Bad Request`
-   Validaciones: `400 Bad Request`
------------------------------------------------------------------------

## 🏆 Uso del Proyecto

Ideal para:

-   ✅ Pruebas técnicas
-   ✅ Portafolio profesional
-   ✅ Evaluaciones en banca, seguros y fintech
-   ✅ Proyectos empresariales

------------------------------------------------------------------------

## 👨‍💻 Autor

**Ramon Emilio Rivas Batista**\
Desarrollador Backend Senior
Especialista en Seguridad, JWT y Arquitectura Limpia
