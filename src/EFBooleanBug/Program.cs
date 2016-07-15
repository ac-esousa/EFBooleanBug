using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EFBooleanBug
{
    public class EntityA
    {
        [Key]
        public int Id { get; set; }

        public EntityB EntityB { get; set; }
    }

    public class EntityB
    {
        [Key]
        public int Id { get; set; }

        public bool IsActive { get; set; }
    }

    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        { }

        public DbSet<EntityA> EntityA { get; set; }
        public DbSet<EntityB> EntityB { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(c => c.UseInMemoryDatabase());

            var services = collection.BuildServiceProvider();
            var context = services.GetService<MyContext>();

            var data = context.EntityA
                .Where(e => e.EntityB.IsActive)
                .ToList();


        }
    }
}
