# Inter University - Sistema de Gestión Universitaria

API REST desarrollada en .NET 8.0 para la gestión de notas y cursos universitarios, con autenticación JWT y roles de usuario (Administrador, Profesor, Estudiante).

## 📋 Tabla de Contenidos

- [Características](#características)
- [Tecnologías](#tecnologías)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Configuración de Base de Datos](#configuración-de-base-de-datos)
- [Migraciones](#migraciones)
- [Ejecución](#ejecución)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Endpoints de la API](#endpoints-de-la-api)
- [Datos de Prueba](#datos-de-prueba)
- [Autenticación](#autenticación)

## ✨ Características

- **Autenticación y Autorización**: Sistema completo con JWT y ASP.NET Identity
- **Gestión de Roles**: Tres tipos de usuarios (Administrador, Profesor, Estudiante)
- **Gestión de Cursos**: Cada curso tiene 3 créditos y está asignado a un profesor
- **Inscripciones**: Sistema de inscripción de estudiantes a cursos
- **Swagger UI**: Documentación interactiva de la API
- **CORS**: Configurado para permitir solicitudes desde cualquier origen
- **Inicialización Automática**: Creación automática de roles, usuarios y cursos de prueba

## 🚀 Tecnologías

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0.23**
- **SQL Server**
- **ASP.NET Identity**
- **JWT Bearer Authentication**
- **Swagger/OpenAPI**

## 📦 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express o superior)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 🔧 Instalación

1. **Clonar el repositorio**

```bash
git clone <url-del-repositorio>
cd interrapidisimo_back
```

2. **Restaurar dependencias**

```bash
dotnet restore
```

3. **Configurar la cadena de conexión**

Edita el archivo `Inter_university/appsettings.json` y actualiza la cadena de conexión con tus datos de SQL Server:

```json
{
  "ConnectionStrings": {
    "interDb": "data source=TU_SERVIDOR;initial catalog=inter_university;persist security info=True;user id=TU_USUARIO;password=TU_CONTRASEÑA;MultipleActiveResultSets=True;TrustServerCertificate=True"
  },
  "llavejwt": "SIsheRALsTomItHy"
}
```

## 🗄️ Configuración de Base de Datos

La aplicación utiliza Entity Framework Core para gestionar la base de datos. La base de datos se creará automáticamente al ejecutar las migraciones.

### Estructura de la Base de Datos

- **ApplicationUser**: Usuarios del sistema (heredando de IdentityUser)
- **Course**: Materias/Cursos universitarios
- **StudentEnrollment**: Inscripciones de estudiantes a cursos
- **Roles**: Administrador, Profesor, Estudiante

## 🔄 Migraciones

Para configurar correctamente la base de datos, sigue estos pasos en orden:

### Paso 1: Navegar al proyecto DAL

```bash
cd Inter.DAL
```

### Paso 2: Eliminar la migración anterior (si existe)

```bash
dotnet ef migrations remove
```

### Paso 3: Crear nueva migración

```bash
dotnet ef migrations add InitialCreate
```

### Paso 4: Aplicar la migración a la base de datos

```bash
dotnet ef database update
```

### Paso 5: Volver al proyecto principal y ejecutar

```bash
cd ..\Inter_university
dotnet run
```

> **Nota**: Las migraciones deben ejecutarse desde el proyecto `Inter.DAL` ya que es donde se encuentra el `DbContext`.

## ▶️ Ejecución

Una vez completadas las migraciones, puedes ejecutar la aplicación:

```bash
cd Inter_university
dotnet run
```

La aplicación estará disponible en:
- **HTTPS**: `https://localhost:7xxx`
- **HTTP**: `http://localhost:5xxx`
- **Swagger UI**: `https://localhost:7xxx/swagger`

> Los números de puerto pueden variar. Verifica la salida de la consola al ejecutar la aplicación.

## 📁 Estructura del Proyecto

```
interrapidisimo_back/
│
├── Inter.DAL/                      # Capa de Acceso a Datos
│   ├── Context/
│   │   ├── AppDbContext.cs        # Contexto de Entity Framework
│   │   └── ApplicationDbContextFactory.cs
│   ├── Models/
│   │   ├── ApplicationUser.cs     # Modelo de usuario extendido
│   │   ├── Course.cs              # Modelo de curso
│   │   └── StudentEnrollment.cs   # Modelo de inscripción
│   ├── Dto/
│   │   └── User/
│   │       ├── RegisterDto.cs     # DTO para registro
│   │       └── LoginDto.cs        # DTO para login
│   ├── Migrations/                # Migraciones de EF Core
│   └── Inter.DAL.csproj
│
├── Inter_university/              # Proyecto Web API
│   ├── Controllers/
│   │   └── UsuarioController.cs   # Controlador de autenticación
│   ├── Init/
│   │   └── DbInitializer.cs       # Inicialización de datos
│   ├── Properties/
│   ├── Program.cs                 # Punto de entrada
│   ├── Startup.cs                 # Configuración de servicios
│   ├── appsettings.json           # Configuración
│   └── Inter_university.csproj
│
└── Inter_university.slnx          # Archivo de solución
```

## 🔌 Endpoints de la API

### Autenticación

#### Registro de Usuario
```http
POST /api/Usuario/register
Content-Type: application/json

{
  "email": "usuario@example.com",
  "password": "Password123",
  "fullName": "Nombre Completo"
}
```

#### Login
```http
POST /api/Usuario/login
Content-Type: application/json

{
  "email": "usuario@example.com",
  "password": "Password123"
}
```

> **Nota**: La implementación completa de estos endpoints está pendiente. Actualmente solo validan el modelo.

## 👥 Datos de Prueba

Al iniciar la aplicación por primera vez, se crean automáticamente los siguientes datos:

### Roles
- Administrador
- Profesor
- Estudiante

### Usuario Administrador
- **Email**: `superadmin@interrapidisimo.com`
- **Password**: `Abcde123456+.+`
- **Rol**: Administrador

### Profesores (5 usuarios)
- **Contraseña para todos**: `Profesor123`

| Email | Nombre |
|-------|--------|
| profesor1@universidad.com | Dr. Carlos Martínez |
| profesor2@universidad.com | Dra. Ana García |
| profesor3@universidad.com | Dr. Roberto López |
| profesor4@universidad.com | Dra. María Rodríguez |
| profesor5@universidad.com | Dr. Juan Pérez |

### Cursos (10 materias)
Cada profesor tiene asignadas 2 materias, cada una con 3 créditos:

| Materia | Código | Profesor |
|---------|--------|----------|
| Programación | PROG-101 | Dr. Carlos Martínez |
| Algebra | ALG-101 | Dr. Carlos Martínez |
| Programación avanzada | PROG_A-101 | Dra. Ana García |
| Bases de Datos | BD-101 | Dra. Ana García |
| Estructuras de Datos | ED-101 | Dr. Roberto López |
| Algoritmos | ALG-101 | Dr. Roberto López |
| Electricidad | EC-101 | Dra. María Rodríguez |
| Sistemas Operativos | SO-101 | Dra. María Rodríguez |
| Ingeniería de Software | IS-101 | Dr. Juan Pérez |
| Arquitectura de Computadoras | ARQ-101 | Dr. Juan Pérez |

## 🔐 Autenticación

La API utiliza JWT (JSON Web Tokens) para la autenticación. Para acceder a endpoints protegidos:

1. Realiza login y obtén el token
2. Incluye el token en el header de tus peticiones:

```http
Authorization: Bearer {tu-token-jwt}
```

### Configuración JWT

- **Clave secreta**: Configurada en `appsettings.json` como `llavejwt`
- **Validación de emisor**: Deshabilitada
- **Validación de audiencia**: Deshabilitada
- **Validación de tiempo de vida**: Habilitada
- **ClockSkew**: 0 (sin tolerancia de tiempo)

## 📝 Swagger UI

La documentación interactiva de la API está disponible en Swagger UI. Para usar autenticación en Swagger:

1. Accede a `/swagger`
2. Haz clic en el botón "Authorize"
3. Ingresa el token en el formato: `Bearer {token}`
4. Haz clic en "Authorize" y cierra el modal
5. Ahora puedes probar los endpoints protegidos

## 🔍 Características Técnicas

### Entity Framework Core
- Code-First approach
- Migraciones automáticas
- Lazy Loading deshabilitado
- Relaciones configuradas con Navigation Properties

### ASP.NET Identity
- Gestión de usuarios y roles
- Contraseñas hasheadas
- Requisitos de contraseña personalizables (actualmente relajados para desarrollo)

### CORS
- Configurado para permitir cualquier origen, método y header
- **Importante**: Revisar y ajustar en producción por seguridad

### JSON Serialization
- MaxDepth: 64 niveles de anidamiento
- ReferenceHandler: IgnoreCycles (evita referencias circulares)

## 🛠️ Comandos Útiles

### Ver migraciones aplicadas
```bash
cd Inter.DAL
dotnet ef migrations list
```

### Revertir migración
```bash
dotnet ef database update {nombre-migración-anterior}
```

### Eliminar base de datos
```bash
dotnet ef database drop
```

### Generar script SQL de migración
```bash
dotnet ef migrations script
```

## ⚠️ Consideraciones de Seguridad

Para un entorno de producción, se recomienda:

1. **Cambiar la clave JWT**: Usar una clave más robusta y almacenarla de forma segura
2. **Configurar CORS**: Restringir los orígenes permitidos
3. **HTTPS**: Forzar el uso de HTTPS en producción
4. **Requisitos de contraseña**: Habilitar todos los requisitos de seguridad
5. **Connection String**: Usar variables de entorno o Azure Key Vault
6. **Deshabilitar Swagger en producción**: O protegerlo con autenticación

## 📄 Licencia

Este proyecto es parte de una prueba técnica para Interrapidísimo.

## 👨‍💻 Autor

Desarrollado como parte de una prueba técnica.

---

**Nota**: Este README proporciona información para configurar y ejecutar el proyecto en un entorno de desarrollo. Para despliegue en producción, se deben considerar aspectos adicionales de seguridad y configuración.
