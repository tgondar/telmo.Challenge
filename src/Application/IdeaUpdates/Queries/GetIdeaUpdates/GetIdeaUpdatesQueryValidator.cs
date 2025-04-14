using FluentValidation;

namespace Application.IdeaUpdates.Queries.GetIdeaUpdates
{
    public class GetIdeaUpdatesQueryValidator : AbstractValidator<GetIdeaUpdatesQuery>
    {
        public GetIdeaUpdatesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("User Id must be greater than 0.");
        }
    }
}
