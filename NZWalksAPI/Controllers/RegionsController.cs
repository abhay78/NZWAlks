using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Text.Json;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles="Reader")]
        public  async Task<IActionResult> GetAll()
        {
            //var regions = new List<Region>{
            //    new Region
            //    {
            //        Id=Guid.NewGuid (),
            //        Name ="Auckland",
            //        Code="Akl", 
            //        RegionImageUrl="Https://google.com"
            //    },
            //    new Region
            //    {
            //        Id=Guid.NewGuid (),
            //        Name ="Alaska",
            //        Code="Aks",
            //        RegionImageUrl="Https://pixabay.com"
            //    },
            //};
            //return Ok(regions);
            //
            //var regionDomainModel = await dbContext.Regions.ToListAsync();


            logger.LogInformation("Get Region Action Method was Invoked");
            var regionDomainModel =  await regionRepository.GetAllAsync();
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            //var regionDto = new List<RegionDto>();
            logger.LogInformation($"Finished GetAllRegions request wit data:{JsonSerializer.Serialize(regionDomainModel)}");
            var regionDto = mapper.Map<List<RegionDto>>(regionDomainModel);
            //foreach (var region in regionDomainModel)
            //{
            //    regionDto.Add(new RegionDto{
            //        Id= region.Id,
            //        Code= region.Code,
            //        Name= region.Name,
            //        RegionImageUrl= region.RegionImageUrl,
            //    });

            //}
            
            return Ok(regionDto);
            
        }
        [HttpGet]
        [Route("{Id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task <IActionResult> GetById([FromRoute] Guid Id)
        {
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x=> x.Id == Id);
            var regionDomainModel= await regionRepository.GetByIdAsync(Id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            //var regionDto = new RegionDto
            //{
            //    Id=regionDomainModel.Id,
            //    Code= regionDomainModel.Code,  
            //    Name = regionDomainModel.Name,  
            //    RegionImageUrl= regionDomainModel.RegionImageUrl,
            //};

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
               //Convert Dto to doain Model
                //var regiondomainModel = new Region
                //{
                //    Code = addRegionRequestDto.Code,
                //    Name = addRegionRequestDto.Name,
                //    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                //};
                var regiondomainModel = mapper.Map<Region>(addRegionRequestDto);
                // await dbContext.Regions.AddAsync(regiondomainModel);
                //dbContext.SaveChanges();

                regiondomainModel = await regionRepository.CreateAsync(regiondomainModel);
                ///Map domain Model to Region Dto

                //var regionDto = new RegionDto
                //{
                //    Id = regiondomainModel.Id,
                //    Code = regiondomainModel.Code,
                //    Name = regiondomainModel.Name,
                //    RegionImageUrl = regiondomainModel.RegionImageUrl,
                //};
                var regionDto = mapper.Map<RegionDto>(regiondomainModel);
                return CreatedAtAction(nameof(GetById), new { regiondomainModel.Id }, regionDto);

           
           
        }
        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           
                //var regiondomainModel= await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionRequestDto.Code,
                //    Name = updateRegionRequestDto.Name,
                //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
                //};
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                //regiondomainModel.Code = updateRegionRequestDto.Code;
                // regiondomainModel.Name= updateRegionRequestDto.Name;
                // regiondomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
                //// dbContext.Regions.Add(regiondomainModel);
                // await dbContext.SaveChangesAsync();
                //Convert DomainModel to Dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                return Ok(regionDto);
            
            

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           //
           //var regionDomainModel= dbContext.Regions.FirstOrDefault(x=>x.Id == id);
           var regionDomainModel= await regionRepository .DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();

            }
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();
            //Convert Region Domain Model to Dto

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
