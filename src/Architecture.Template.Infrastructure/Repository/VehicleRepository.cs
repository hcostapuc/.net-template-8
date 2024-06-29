using Domain.Entities;
using Domain.Interfaces.Repository;
using Infrastructure.Common;
using Infrastructure.Context;

namespace Infrastructure.Repository;
public sealed class VehicleRepository(ApplicationDbContext context) : 
                    BaseRepository<VehicleEntity>(context), IVehicleRepository
{
}
