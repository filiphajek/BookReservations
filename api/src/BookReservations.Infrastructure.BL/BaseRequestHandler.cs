using MapsterMapper;

namespace BookReservations.Infrastructure.BL;

public abstract class BaseRequestHandler
{
    protected IMapper Mapper { get; }

    protected BaseRequestHandler(IMapper mapper)
    {
        Mapper = mapper;
    }
}
