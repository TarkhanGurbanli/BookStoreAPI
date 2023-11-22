using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.RoleDtos;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RolesController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpPost("createRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleCreateDto)
        {
            var result = await _roleService.CreateRole(_mapper.Map<AppRole>(roleCreateDto));

            if (result.Success) return StatusCode(StatusCodes.Status201Created, result);

            return BadRequest(result);
        }

        [HttpDelete("deleteRole/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var result = await _roleService.DeleteRole(roleId);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("getAllRoles")]
        [ProducesResponseType(typeof(List<AppRole>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();

            if (result.Success) return Ok(result.Data);

            return NotFound(result.Message);
        }

        [HttpPut("updateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDto roleUpdateDto)
        {
            var result = await _roleService.UpdateRole(_mapper.Map<AppRole>(roleUpdateDto));

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("addRoleWithUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRoleWithUser([FromBody] UserRoleCreateDto userRoleCreateDto)
        {
            var result = await _roleService.AddRoleWithUser(userRoleCreateDto.UserId, userRoleCreateDto.RoleId);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("deleteRoleWithUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRoleWithUser([FromBody] UserRoleDeleteDto userRoleDeleteDto)
        {
            var result = await _roleService.DeleteRoleWithUser(userRoleDeleteDto.UserId, userRoleDeleteDto.RoleId);

            if (result.Success) return Ok(result);

            return BadRequest(result);
        }
    }
}
