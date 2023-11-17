using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await walkRepository.Createasync(walkDomainModel);
                var walkDto = mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);
            
            


        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
                                         [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize=100)
        {
            var walkDomain = await walkRepository.GetWAlkAsync(filterOn,filterQuery,sortBy ,isAscending ?? true,pageNumber ,pageSize);
            var walkDto=mapper.Map<List<WalkDto>>(walkDomain);
            return Ok(walkDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if(walkDomain == null)
            {
                return NotFound();

            }
            var walkDto= mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid id,UpdateWalkRequestDto updateWalkRequestDto)
        {
            
                var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
                if (walkDomain == null)
                {
                    return NotFound();
                }
                var walkDto = mapper.Map<WalkDto>(walkDomain);
                return Ok(walkDto);
            
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
    }
}
