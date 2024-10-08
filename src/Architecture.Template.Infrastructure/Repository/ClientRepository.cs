﻿using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Infrastructure.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;
public sealed class ClientRepository(ApplicationDbContext context) : 
                    BaseRepository<ClientEntity>(context), IClientRepository
{
    public async Task<ClientEntity> SelectDetailAsync(Expression<Func<ClientEntity, bool>> expression, CancellationToken cancellationToken = default) =>
        await _dataset.AsNoTracking()
                 .Include(x => x.VehicleCollection)
                 .Include(x => x.WashOrderCollection)
                 .SingleOrDefaultAsync(expression, cancellationToken);
}
