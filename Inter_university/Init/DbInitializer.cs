using Inter.DAL.Context;
using Inter.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inter_university.Init
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                bool CreateObjects = false;  
                logger.LogInformation("Verificando base de datos...");

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Aplicando migraciones pendientes...");
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migraciones aplicadas exitosamente.");
                    CreateObjects = true;
                }
                else
                {
                    var canConnect = await context.Database.CanConnectAsync();
                    if (!canConnect)
                    {
                        logger.LogInformation("Creando base de datos...");
                        await context.Database.MigrateAsync();
                        logger.LogInformation("Base de datos creada exitosamente.");
                        CreateObjects = true;
                    }
                    else
                    {
                        logger.LogInformation("Base de datos ya existe y está actualizada.");
                    }
                }

                if (CreateObjects)
                {
                    logger.LogInformation("Iniciando creación de datos iniciales...");
                    await CreateRolesAsync(roleManager, logger);
                    await CreateSuperAdminAsync(userManager, logger);
                    await CreateTeachersAsync(userManager, logger);
                    await CreateCoursesAsync(context, userManager, logger);
                }
                else
                {
                    logger.LogInformation("Omisión de creación de datos iniciales.");                 
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al inicializar la base de datos");
                throw;
            }
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            string[] roleNames = { "Administrador", "Profesor", "Estudiante" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    logger.LogInformation($"Creando rol: {roleName}");
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            logger.LogInformation("Roles verificados correctamente.");
        }

        private static async Task CreateSuperAdminAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            const string superAdminEmail = "superadmin@interrapidisimo.com";
            const string superAdminPassword = "Abcde123456+.+";

            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin == null)
            {
                logger.LogInformation("Creando usuario Super Administrador...");

                superAdmin = new ApplicationUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FullName = "Super Administrador",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(superAdmin, superAdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "Administrador");
                    logger.LogInformation($"Usuario Super Administrador creado exitosamente: {superAdminEmail}");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError($"Error al crear Super Administrador: {errors}");
                }
            }
            else
            {
                logger.LogInformation("Usuario Super Administrador ya existe.");
            }
        }

        private static async Task CreateTeachersAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            var teachers = new[]
            {
                new { Email = "profesor1@universidad.com", Name = "Dr. Carlos Martínez" },
                new { Email = "profesor2@universidad.com", Name = "Dra. Ana García" },
                new { Email = "profesor3@universidad.com", Name = "Dr. Roberto López" },
                new { Email = "profesor4@universidad.com", Name = "Dra. María Rodríguez" },
                new { Email = "profesor5@universidad.com", Name = "Dr. Juan Pérez" }
            };

            foreach (var teacherData in teachers)
            {
                var teacher = await userManager.FindByEmailAsync(teacherData.Email);
                if (teacher == null)
                {
                    teacher = new ApplicationUser
                    {
                        UserName = teacherData.Email,
                        Email = teacherData.Email,
                        FullName = teacherData.Name,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(teacher, "Profesor123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(teacher, "Profesor");
                        logger.LogInformation($"Profesor creado: {teacherData.Name}");
                    }
                }
            }

            logger.LogInformation("Profesores verificados correctamente.");
        }

        private static async Task CreateCoursesAsync(AppDbContext context, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            if (await context.Courses.AnyAsync())
            {
                logger.LogInformation("Las materias ya existen.");
                return;
            }

            var teachers = await userManager.GetUsersInRoleAsync("Profesor");
            var teacherList = teachers.ToList();

            if (teacherList.Count < 5)
            {
                logger.LogWarning("No hay suficientes profesores para crear las materias.");
                return;
            }

            var courses = new[]
            {
                new { Name = "Programación", Code = "PROG-101", TeacherIndex = 0 },
                new { Name = "Algebra", Code = "ALG-101", TeacherIndex = 0 },
                new { Name = "Programación avanzada", Code = "PROG_A-101", TeacherIndex = 1 },
                new { Name = "Bases de Datos", Code = "BD-101", TeacherIndex = 1 },
                new { Name = "Estructuras de Datos", Code = "ED-101", TeacherIndex = 2 },
                new { Name = "Algoritmos", Code = "ALG-101", TeacherIndex = 2 },
                new { Name = "Electricidad", Code = "EC-101", TeacherIndex = 3 },
                new { Name = "Sistemas Operativos", Code = "SO-101", TeacherIndex = 3 },
                new { Name = "Ingeniería de Software", Code = "IS-101", TeacherIndex = 4 },
                new { Name = "Arquitectura de Computadoras", Code = "ARQ-101", TeacherIndex = 4 }
            };

            foreach (var courseData in courses)
            {
                var course = new Course
                {
                    Name = courseData.Name,
                    Code = courseData.Code,
                    Description = $"Curso de {courseData.Name}",
                    Credits = 3,
                    TeacherId = teacherList[courseData.TeacherIndex].Id
                };

                context.Courses.Add(course);
            }

            await context.SaveChangesAsync();
            logger.LogInformation("10 materias creadas exitosamente (2 por profesor, 3 créditos cada una).");
        }
    }
}