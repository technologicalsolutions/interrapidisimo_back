using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Inter.DAL.Context
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {        
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            var connectionString = "data source=DESARROLLO;initial catalog=inter_university;persist security info=True;user id=sa;password=Abcde123456;MultipleActiveResultSets=True;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
