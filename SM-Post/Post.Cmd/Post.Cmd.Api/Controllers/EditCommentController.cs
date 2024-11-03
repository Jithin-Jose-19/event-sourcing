using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly ILogger<EditCommentController> _logger = logger;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpPut("id")]
        public async Task<IActionResult> EditCommentAsync(Guid id, EditCommentCommand command)
        {
            command.Id = id;
            try
            {
                await _commandDispatcher.SendAsync(command);
                return Ok(new EditCommentResponse
                {
                    Id = id,
                    Message = "Edit comment request completed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");

                return BadRequest(new EditCommentResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrive the aggregate, client passed an incorrect post id!");

                return BadRequest(new EditCommentResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to edit a comment";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new EditCommentResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
