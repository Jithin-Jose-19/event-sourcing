using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RemoveCommentController(ILogger<RemoveCommentController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly ILogger<RemoveCommentController> _logger = logger;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpDelete]
        public async Task<IActionResult> RemoveCommentsAsync(Guid id, RemoveCommentCommand command)
        {
            command.Id = id;
            try
            {
                await _commandDispatcher.SendAsync(command);
                return Ok(new RemoveCommentResponse
                {
                    Id = id,
                    Message = "Remove comment request completed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");

                return BadRequest(new RemoveCommentResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrive the aggregate, client passed an incorrect post id!");

                return BadRequest(new RemoveCommentResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to remove a comment on a post";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new RemoveCommentResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
