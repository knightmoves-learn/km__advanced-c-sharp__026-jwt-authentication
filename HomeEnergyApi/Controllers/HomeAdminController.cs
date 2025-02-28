using Microsoft.AspNetCore.Mvc;
using HomeEnergyApi.Models;
using HomeEnergyApi.Services;
using HomeEnergyApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace HomeEnergyApi.Controllers
{
    [ApiController]
    [Route("admin/Homes")]
    public class HomeAdminController : ControllerBase
    {
        private IWriteRepository<int, Home> repository;
        private ZipCodeLocationService zipCodeLocationService;
        private IMapper mapper;
        private HomeUtilityProviderService homeUtilityProviderService;

        public HomeAdminController(IWriteRepository<int, Home> repository, ZipCodeLocationService zipCodeLocationService, HomeUtilityProviderService homeUtilityProviderService, IMapper mapper)
        {
            this.repository = repository;
            this.zipCodeLocationService = zipCodeLocationService;
            this.homeUtilityProviderService = homeUtilityProviderService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateHome([FromBody] HomeDto homeDto)
        {
            Home home = mapper.Map<Home>(homeDto);
            repository.Save(home);
            homeUtilityProviderService.Associate(home, homeDto.UtilityProviderIds);
            return Created($"/Homes/{repository.Count()}", home);
        }

        public IActionResult UpdateHome([FromBody] HomeDto homeDto, [FromRoute] int id)
        {
            Home home = mapper.Map<Home>(homeDto); ;
            if (id > (repository.Count() - 1))
            {
                return NotFound();
            }
            repository.Update(id, home);
            return Ok(home);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHome(int id)
        {
            if (id > repository.Count())
            {
                return NotFound();
            }
            var home = repository.RemoveById(id);
            return Ok(home);
        }

        [HttpPost("Location/{zipCode}")]
        public async Task<IActionResult> ZipLocation([FromRoute] int zipCode)
        {
            Place place = await zipCodeLocationService.Report(zipCode);
            return Ok(place);
        }
    }
}