using System;
using System.Linq.Expressions;
using System.Reflection;
using DB.Models;
using DB.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace DB.Data
{
    public partial class DemoDbContext : DbContext
    {
        private const string DeletedAtProperty = "DeletedAt";

        private static readonly MethodInfo PropertyMethod = typeof(EF)
            .GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)
            .MakeGenericMethod(typeof(DateTime?));

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(PropertyMethod, parm, Expression.Constant(DeletedAtProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(null));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    modelBuilder
                        .Entity(entity.ClrType)
                        .HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Email).IsUnique();

                entity.HasOne(x => x.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.DeletedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            DefineData(modelBuilder);
        }
    }
}