using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using HomeEnergyApi.Models;
using HomeEnergyApi.Dtos;

namespace HomeEnergyApi.Controllers
{
    [ApiController]
    [Route("admin/UtilityProviders")]
    public class UtilityProviderAdminController : ControllerBase
    {
        private IWriteRepository<int, UtilityProvider> repository;
        private IMapper mapper;

        public UtilityProviderAdminController(IWriteRepository<int, UtilityProvider> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UtilityProviderDto utilityProviderDto)
        {
            UtilityProvider utilityProvider = mapper.Map<UtilityProvider>(utilityProviderDto);
            repository.Save(utilityProvider);
            return Created($"/UtilityProviders/{repository.Count()}", utilityProvider);
        }
    }
}