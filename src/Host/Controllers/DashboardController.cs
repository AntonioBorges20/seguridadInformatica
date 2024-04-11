using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ApplicationCore.Wrappers;
using ApplicationCore.Commands;
using ApplicationCore.DTOs;
using Domain.Entities;

namespace Host.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        private readonly IMediator _mediator;
        public DashboardController(IDashboardService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        /// <summary>
        /// Get de todos los usuarios
        /// </summary>
        /// <returns></returns>
        [Route("getData")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var result = await _service.GetData();
            return Ok(result);
        }

        /// <summary>
        /// /// Create de tabla
        /// </summary>
        /// <returns></returns>

        [HttpPost("create")]
        public async Task<ActionResult<Response<int>>> Create([FromBody] UserDto request)
        {
            var result = await _service.CreateUser(request);
            //var result = await _mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// /// Optener IP
        /// </summary>
        /// <returns></returns>

        [Route("getIP")]
        [HttpGet]
        public async Task<IActionResult> GetIpAddress()
        {
            var result = await GetIpAddress();
            return Ok(result);
        }



        /// <summary>
        /// /// Logs
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost("logs")]

        public async Task<ActionResult<Response<int>>> Create([FromBody] LogsDto request)
        {
            var result = await _service.CreateLogs(request);
            return Ok(result);
        }

        /// <summary>
        /// /// Delete
        /// </summary>
        /// <returns></returns>

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _service.DeleteUser(id);
            return Ok(result);
        }

        /// <summary>
        /// /// Update
        /// </summary>
        /// <returns></returns>

        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, UserDto request)
        {
            var result = await _service.UpdateUser(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Paginated
        /// </summary>
        /// <returns></returns>
        [Route("paginated")]
        [HttpGet]
        public async Task<IActionResult> GetPaginated()
        {
            var result = await _service.GetPaginated();
            return Ok(result);
        }

    }
}
