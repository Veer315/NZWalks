using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllWalksAsync()

        {
            //Fetch Data from database- Domain Walks
            var walkDomain = await walkRepository.GetAllAsync();
            //Convert Domain walk to DTO
            var walkDTO = mapper.Map<List<Models.DTO.Walk>>(walkDomain);
            //Return Response
            return Ok(walkDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);
            //Convert Domain to DTO
            var walksDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            //Return response
            return Ok(walksDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain Object 
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            //Pass domain object to repository to persist this 
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert the domain object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //Send DTO Response back to the client
            return CreatedAtAction(nameof(GetWalkAsync), new { Id = walkDTO.Id }, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO To Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            //Pass Details to Repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //Handle Null
            if (walkDomain == null)
            {
                return NotFound();
            }

            //Convert back Domain to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId=walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //Return Response
            return Ok(walkDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeleteWalkAsync(Guid id)
        {
            //Call repository to delete walk
            var walkDomain=await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO=mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }

        
    }
}
