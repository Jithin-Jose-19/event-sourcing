﻿using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Cmd.Infrastructure.Dispatchers;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LikePostController(ILogger<LikePostController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        private readonly ILogger<LikePostController> _logger = logger;
        private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

        [HttpPut("id")]
        public async Task<IActionResult> LikePostMessageAsync(Guid id)
        {
            try
            {
                await _commandDispatcher.SendAsync(new LikePostCommand() { Id = id});
                return Ok(new LikePostResponse
                {
                    Id = id,
                    Message = "Like post request completed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");

                return BadRequest(new LikePostResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Could not retrive the aggregate, client passed an incorrect post id!");

                return BadRequest(new LikePostResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to like a post";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new LikePostResponse
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }
    }
}
