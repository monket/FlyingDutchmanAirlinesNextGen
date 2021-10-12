using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.Stubs
{
    public class FlyingDutchmanAirlinesContextStub : FlyingDutchmanAirlinesContext
    {
        public FlyingDutchmanAirlinesContextStub(DbContextOptions<FlyingDutchmanAirlinesContext> options) : base(options)
        {
            base.Database.EnsureDeleted();
            OnSaveChanges = ct => base.SaveChangesAsync(ct);
        }

        public Func<CancellationToken, Task<int>> OnSaveChanges { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return OnSaveChanges(cancellationToken);
        }
    }
}