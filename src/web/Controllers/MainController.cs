using Application.Dto;
using Application.IdeaUpdates.Commands.AddIdeaUpdate;
using Application.IdeaUpdates.Commands.SetIdeaUpdateAsRead;
using Application.IdeaUpdates.Queries.GetIdeaUpdates;
using Backend.Challenge.Models.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Backend.Challenge.Controllers
{
    public class MainController : Controller
    {
        private readonly IMediator _mediator;

        public MainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // TODO: An action to return a paged list of status updates
        [HttpGet("user/{id}/updates")]
        [ProducesResponseType(typeof(IdeaUpdatesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIdeaUpdates(
            [FromRoute, Required] int id,
            [FromQuery] bool newUpdatesOnly = false,
            [FromQuery, Range(1, int.MaxValue)] int pageNumber = 1,
            [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = new GetIdeaUpdatesQuery(
                id,
                newUpdatesOnly,
                pageNumber,
                pageSize);

            var ideaUpdates = await _mediator.Send(query);

            return Ok(ideaUpdates);
        }

        // TODO: An action to add a status update
        [HttpPost("idea/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddIdeaUpdate(
            [FromRoute, Required] int id,
            [FromBody] AddIdeaUpdateCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            command.IdeaId = id;

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPatch("idea/{id}/read")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetIdeaUpdateAsRead(
        [FromRoute, Required] string id,
        [FromQuery, Required] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new SetIdeaUpdateAsReadCommand
            {
                RavenId = id,
                UserId = userId
            };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}