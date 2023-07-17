using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N5.Core.DTOs;
using N5.Core.Shared;
using N5.Shared.Pagination;
using N5.WebAPI.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace N5.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly ILogger<PermissionController> _logger;
        private readonly IPermissionService _permissionService;

        public PermissionController(
            ILogger<PermissionController> logger,
            IPermissionService permissionService)
        {
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestPermission(RequestPermissionViewModel permission)
        {
            var permissionDto = new PermissionDto
            {
                ApellidoEmpleado = permission.ApellidoEmpleado,
                NombreEmpleado = permission.NombreEmpleado,
                TipoPermiso = permission.Tipo
            };
            var result = await _permissionService.Request(permissionDto);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ModifyPermission(ModifyPermissionViewModel permission)
        {
            var permissionDto = new PermissionDto
            {
                Id = permission.Id,
                ApellidoEmpleado = permission.ApellidoEmpleado,
                NombreEmpleado = permission.NombreEmpleado,
                TipoPermiso = permission.Tipo
            };
            var result = await _permissionService.Modify(permissionDto);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions([FromQuery] PaginationParams paginationParams)
        {
            var pagedPermissions = await _permissionService.Get(paginationParams);
            var permissionsViewModel = new PagedResult<GetPermissionViewModel>
            {
                Items = pagedPermissions.Items.Select(x => new GetPermissionViewModel
                {
                    Id = x.Id,
                    NombreEmpleado = x.NombreEmpleado,
                    ApellidoEmpleado = x.ApellidoEmpleado,
                    FechaPermiso = x.FechaPermiso,
                    Tipo = new PermissionTypeViewModel
                    {
                        Id = x.Tipo.Id,
                        Description = x.Tipo.Description
                    }
                }),
                PageNumber = pagedPermissions.PageNumber,
                PageSize = pagedPermissions.PageSize,
                TotalCount = pagedPermissions.TotalCount
            };
            return Ok(permissionsViewModel);
        }

        [Route("types")]
        [HttpGet]
        public async Task<IActionResult> GetTypes()
        {
            var types = await _permissionService.GetTypes();
            var typesViewModel = types.Select(x =>
                new PermissionTypeViewModel
                {
                    Id = x.Id,
                    Description = x.Description
                });
            return Ok(typesViewModel);
        }
    }
}