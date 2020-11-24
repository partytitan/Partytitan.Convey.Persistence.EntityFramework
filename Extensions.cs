using System;
using System.Reflection;
using Convey;
using Convey.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Partytitan.Convey.Persistence.EntityFramework.Configuration;
using Partytitan.Convey.Persistence.EntityFramework.Configuration.Enums;
using Partytitan.Convey.Persistence.EntityFramework.Repositories;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;

namespace Partytitan.Convey.Persistence.EntityFramework
{
    public static class Extensions
    {
        private const string SectionName = "entityframework";


        public static IConveyBuilder AddEntityFramework<T>(this IConveyBuilder builder, string sectionName = SectionName) where T : DbContext
        {
            var migrationsAssembly = Assembly.GetCallingAssembly().FullName;

            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            var entityFrameworkOptions = builder.GetOptions<EntityFrameworkOptions>(sectionName);
            return builder.AddEntityFramework<T>(entityFrameworkOptions, migrationsAssembly);
        }

        public static IConveyBuilder AddEntityFramework<T>(this IConveyBuilder builder,
            EntityFrameworkOptions entityFrameworkOptions, string migrationsAssembly) where T : DbContext
        {
            builder.Services.AddSingleton(entityFrameworkOptions);
            switch (entityFrameworkOptions.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    builder.Services.RegisterSqlServerDbContext<T>(entityFrameworkOptions.ConnectionString, migrationsAssembly);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(entityFrameworkOptions.DatabaseType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseType)))}.");
            }

            return builder;
        }

        public static IConveyBuilder AddEfRepository<TDbContext, TEntity, TIdentifiable>(this IConveyBuilder builder)
            where TDbContext : DbContext
            where TEntity : class, IIdentifiable<TIdentifiable>
        {
            builder.Services.AddTransient<IEfRepository<TDbContext, TEntity, TIdentifiable>, EfRepository<TDbContext, TEntity, TIdentifiable>>();

            return builder;
        }

        private static void RegisterSqlServerDbContext<T>(this IServiceCollection services, string connectionString, string migrationsAssembly)
            where T : DbContext
        {
            services.AddDbContext<T>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}
