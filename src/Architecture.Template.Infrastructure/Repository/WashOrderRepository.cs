using Domain.Entities;
using Domain.Interfaces.Repository;
using Infrastructure.Common;
using Infrastructure.Context;

namespace Infrastructure.Repository;
public sealed class WashOrderRepository(ApplicationDbContext context) : 
                    BaseRepository<WashOrderEntity>(context), IWashOrderRepository
{
}