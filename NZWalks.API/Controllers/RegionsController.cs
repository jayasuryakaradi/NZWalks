using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //GET ALL REGIONS
        [HttpGet]
        [Authorize(Roles ="Reader, Writer")]

        public async Task<IActionResult> GetAll()
        {
            // get data from database - domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map Domain Models to DTO's
            //Return DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //GET REGION BY ID
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Reader, Writer")]

        public async Task<IActionResult> GetById(Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //var region=dbContext.Regions.FirstOrDefault(r => r.Id==id);
            //Drawbacks of using Find, Find() menthod takes only Id, it dosent take other attributes like Name, City. In order to find the result with other attributes we need to use FirstOrDefault

            //get region domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //map/convert region domain model to region dto
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl,
            //};
            //map/convert region domain model to region dto
            //return dto back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //POST NEW REGION
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionDto)
        {
           
                //map or convert dto to domain model
                //var regionDomainModel = new Region
                //{
                //    Name = addRegionDto.Name,
                //    Code = addRegionDto.Code,
                //    RegionImageUrl = addRegionDto.RegionImageUrl,
                //};
                var regionDomainModel = mapper.Map<Region>(addRegionDto);

                //use domain model to create region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //map domain model back to dto
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
           
        }

        //UPDATE EXISTING REGION
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionDto)
        {


            //MAP Dto to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionDto.Code,
            //    RegionImageUrl = updateRegionDto.RegionImageUrl,
            //    Name = updateRegionDto.Name,
            //};
            var regionDomainModel = mapper.Map<Region>(updateRegionDto);
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
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

        //DELETE EXISTING REGION
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize (Roles = "Writer")]

        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }


            //return deleted region
            //map domain to dto
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomregionDomainModelain.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);

        }
    }
}