using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficuiltyController : Controller
    {
        private readonly IWalkDifficuiltyRepository walkDifficuiltyRepository;
        private readonly IMapper mapper;

        public WalkDifficuiltyController(IWalkDifficuiltyRepository walkDifficuiltyRepository, IMapper mapper)
        {
            this.walkDifficuiltyRepository = walkDifficuiltyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult>  GetAllWalkDifficuilties()
        {
            var walkDifficuiltiesDomain = await walkDifficuiltyRepository.GetAllAsync();
            var walkDifficuiltiesDTO=mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficuiltiesDomain);

            return Ok(walkDifficuiltiesDTO);
           
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficuiltyById")]
        public async Task<IActionResult> GetWalkDifficuiltyById(Guid id)
        {
            var walkDifficuilty = await walkDifficuiltyRepository.GetAsync(id);
            if (walkDifficuilty == null)
            {
                return NotFound();
            }
            //convert domain to DTO
           var walkDifficuiltyDTO= mapper.Map<Models.DTO.WalkDifficulty>(walkDifficuilty);
            
                return Ok(walkDifficuiltyDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficuiltyAsync(Models.DTO.AddWalkDifficuiltyRequest addWalkDifficuiltyRequest)
        {
            //convert DTO to Domain
            var walkDifficuiltyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficuiltyRequest.Code
            };
            //Call repository
            walkDifficuiltyDomain = await walkDifficuiltyRepository.AddAsync(walkDifficuiltyDomain);
            //cponvert domain to dto
            var walkDifficuiltyDTO= mapper.Map<Models.DTO.WalkDifficulty> (walkDifficuiltyDomain);
            //Return
            return CreatedAtAction(nameof(GetWalkDifficuiltyById), new { id = walkDifficuiltyDTO.Id },walkDifficuiltyDTO);


        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>UpdateWalkDifficuiltyAsync(Guid id, Models.DTO.UpdateWalkDifficuiltyRequest updateWalkDifficuiltyRequest)
        {
            //convert DTO -Domain
            var walkDifficuiltyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficuiltyRequest.Code
            };
            //Call repo to update
           walkDifficuiltyDomain= await walkDifficuiltyRepository.UpdateAsync(id, walkDifficuiltyDomain);
            if(walkDifficuiltyDomain==null)
            {
                return NotFound();
            }
            //cponvert domain to dto
            var walkDifficuiltyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficuiltyDomain);
            //return response
            return Ok(walkDifficuiltyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficuilty(Guid id)
        {
            var walkDifficuiltyDomain=await walkDifficuiltyRepository.DeleteAsync(id);

            if (walkDifficuiltyDomain==null)
            { 
                return NotFound(); 
            }
            var walkDifficuiltyDTO=mapper.Map<WalkDifficulty>(walkDifficuiltyDomain);
            return Ok(walkDifficuiltyDTO);
        }
    }

}
