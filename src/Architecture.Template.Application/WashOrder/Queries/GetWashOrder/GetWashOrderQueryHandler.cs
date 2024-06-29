using Domain.Interfaces.Repository;

namespace Application.WashOrder.Queries.GetWashOrder;
public sealed class GetWashOrderQueryHandler(IWashOrderRepository washOrderRepository,
                                IMapper mapper) : IRequestHandler<GetWashOrderQuery, GetWashOrderRootDto>
{
    private readonly IWashOrderRepository _washOrderRepository = washOrderRepository ?? 
                                                                 Guard.Against.Null(washOrderRepository, nameof(washOrderRepository));
    private readonly IMapper _mapper = mapper ?? 
                                       Guard.Against.Null(mapper, nameof(mapper));

    public async Task<GetWashOrderRootDto> Handle(GetWashOrderQuery request, CancellationToken cancellationToken)
    {
        var washOrderEntity = await _washOrderRepository.SelectAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, washOrderEntity, nameof(washOrderEntity.Id));
        return _mapper.Map<WashOrderEntity, GetWashOrderRootDto>(washOrderEntity);
    }
}