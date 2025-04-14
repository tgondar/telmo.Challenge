using Application.Interfaces;
using MediatR;

namespace Application.IdeaUpdates.Commands.AddIdeaUpdate
{
    public class AddIdeaUpdateCommand : IRequest
    {
        public int IdeaId { get; set; }
        public int StatusId { get; set; }
    }
    public class AddIdeaUpdateCommandHandler : IRequestHandler<AddIdeaUpdateCommand>
    {
        private readonly IIdeaUpdateRepository _repository;

        public AddIdeaUpdateCommandHandler(IIdeaUpdateRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(AddIdeaUpdateCommand request, CancellationToken cancellationToken)
        {
            var emulateAFunctionThatWillFireAnUpdateToAllUsers = new int[] { 1 };

            foreach (var item in emulateAFunctionThatWillFireAnUpdateToAllUsers)
            {
                var ideaUpdate = new Domain.Entities.IdeaUpdates
                {
                    IdeaId = request.IdeaId,
                    StatusId = request.StatusId,

                    UserId = item,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.SaveAsync(ideaUpdate);
            }
        }
    }
}
