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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficuiltyRepository walkDifficuiltyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper,IRegionRepository regionRepository,IWalkDifficuiltyRepository walkDifficuiltyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficuiltyRepository = walkDifficuiltyRepository;
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
            //validate incoming request

            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
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
            //Validate incoming request
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);  
            }
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

        #region private methods
        private async Task <bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                
                    ModelState.AddModelError(nameof(addWalkRequest),
                            $"{nameof(addWalkRequest)} Can't be less than or equal to zero");
                return false;
            
            }
            if(!string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                            $"{nameof(addWalkRequest.Name)} Name is required");
            }
            if (addWalkRequest.Length<=0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                            $"{nameof(addWalkRequest.Length)} Length is required");
            }
            var region= await regionRepository.GetAsync(addWalkRequest.RegionId);
            {
                if(region==null)
                {
                    ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                            $"{nameof(addWalkRequest.RegionId)} Invalid region id. ");
                }
                var walkDifficuilty = await walkDifficuiltyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
                {
                    if(walkDifficuilty==null)
                    {
                        ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                          $"{nameof(addWalkRequest.WalkDifficultyId)} Invalid  WalkDifficultyId. ");
                    }
                    if(ModelState.ErrorCount>0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {

                ModelState.AddModelError(nameof(updateWalkRequest),
                        $"{nameof(updateWalkRequest)} Can't be less than or equal to zero");
                return false;

            }
            if (!string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                            $"{nameof(updateWalkRequest.Name)} Name is required");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                            $"{nameof(updateWalkRequest.Length)} Length is required");
            }
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            {
                if (region == null)
                {
                    ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                            $"{nameof(updateWalkRequest.RegionId)} Invalid region id. ");
                }
                var walkDifficuilty = await walkDifficuiltyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
                {
                    if (walkDifficuilty == null)
                    {
                        ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                          $"{nameof(updateWalkRequest.WalkDifficultyId)} Invalid  WalkDifficultyId. ");
                    }
                    if (ModelState.ErrorCount > 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        
    }
        #endregion
    }
}
