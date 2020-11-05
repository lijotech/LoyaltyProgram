using System;
using MemberAPI.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace MemberAPI.Data.Tests.Infrastructure
{
    public class DatabaseTestBase : IDisposable
    {
        protected readonly MemberContext Context;

        public DatabaseTestBase()
        {
            var options = new DbContextOptionsBuilder<MemberContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            Context = new MemberContext(options);

            Context.Database.EnsureCreated();

            DatabaseInitializer.Initialize(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();

            Context.Dispose();
        }
    }
}