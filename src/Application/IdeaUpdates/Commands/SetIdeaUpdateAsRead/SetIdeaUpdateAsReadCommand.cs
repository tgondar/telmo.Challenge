using Application.Interfaces;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.IdeaUpdates.Commands.SetIdeaUpdateAsRead
{
    public class SetIdeaUpdateAsReadCommand : IRequest
    {
        public string RavenId { get; set; }
        public int UserId { get; set; }
    }

    public class MarkIdeaUpdateAsReadCommandHandler : IRequestHandler<SetIdeaUpdateAsReadCommand>
    {
        private readonly IIdeaUpdateRepository _repository;

        public MarkIdeaUpdateAsReadCommandHandler(IIdeaUpdateRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(SetIdeaUpdateAsReadCommand request, CancellationToken cancellationToken)
        {
            var ideaUpdate = await _repository.GetIdeaByIdAndUserIdAsync(request.RavenId, request.UserId);

            if (ideaUpdate == null || ideaUpdate.UserId != request.UserId)
            {
                throw new NotFoundException("IdeaUpdate", request.RavenId);
            }

            ideaUpdate.IsRead = true;

            await _repository.SaveAsync(ideaUpdate);
        }
    }
}
