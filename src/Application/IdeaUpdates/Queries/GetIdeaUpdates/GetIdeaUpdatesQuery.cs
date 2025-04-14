using Application.Dto;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;

namespace Application.IdeaUpdates.Queries.GetIdeaUpdates
{
    public class GetIdeaUpdatesQuery : IRequest<PagedResult<IdeaUpdatesDto>>
    {
        public int UserId { get; set; }
        public bool NewUpdatesOnly { get; set; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetIdeaUpdatesQuery(
            int userId,
            bool newUpdatesOnly,
            int pageNumber,
            int pageSize)
        {
            UserId = userId;
            NewUpdatesOnly = newUpdatesOnly;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public class GetIdeaUpdatesQueryHandler : IRequestHandler<GetIdeaUpdatesQuery, PagedResult<IdeaUpdatesDto>>
        {
            private readonly IIdeaUpdateRepository _ideaUpdateRepository;
            private readonly IMapper _mapper;

            public GetIdeaUpdatesQueryHandler(
                IIdeaUpdateRepository repository,
                IMapper mapper)
            {
                _ideaUpdateRepository = repository;
                _mapper = mapper;
            }

            public async Task<PagedResult<IdeaUpdatesDto>> Handle(
                GetIdeaUpdatesQuery request,
                CancellationToken cancellationToken)
            {
                var entities = await _ideaUpdateRepository.GetAllWithRelationsAsync(
                    request.UserId, 
                    request.NewUpdatesOnly,
                    request.PageNumber,
                    request.PageSize);

                if (entities == null || entities.Items.Count == 0)
                {
                    throw new NotFoundException("UserId", nameof(Domain.Entities.IdeaUpdates));
                }

                Guard.Against.NotFound(request.UserId, entities);

                return _mapper.Map<PagedResult<IdeaUpdatesDto>>(entities);
            }
        }
    }
}
