using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Cmd.Infrastructure.Dispatchers;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeletePostController(ILogger<DeletePostController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly ILogger<DeletePostController> _logger = logger;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpDelete]
        public async Task<IActionResult> RemoveCommentsAsync(Guid id, DeletePostCommand command)
        {
            command.Id = id;
            try
            {
                await _commandDispatcher.SendAsync(command);
                return Ok(new DeletePostResponse
                {
                    Id = id,
                    Message = "Delete post request completed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");

                return BadRequest(new DeletePostResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrive the aggregate, client passed an incorrect post id!");

                return BadRequest(new DeletePostResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to remove delete a post";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new DeletePostResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
