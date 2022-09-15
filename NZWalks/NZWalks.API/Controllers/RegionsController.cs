using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using System.Runtime;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;


        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var region = await regionRepository.GetAllAsync();

            //Return DTO Regions
            /*
                        var regionsDTO = new List<Models.DTO.Region>();
                        regions.ToList().ForEach(region =>
                        {
                            var regionDTO = new Models.DTO.Region()
                            {
                                Id=region.Id,
                                Code=region.Code,
                                Name=region.Name,
                                Area = region.Area,
                                Lat=region.Lat,
                                Long=region.Long,
                                Population=region.Population,
                            };
                            regionsDTO.Add(regionDTO);
                        });*/
            mapper.Map<List<Models.DTO.Region>>(region);

            return Ok(region);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid Id)
        {
            var region = await regionRepository.GetAsync(Id);

            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validate request
           if(!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest();
            }

            //Request(DTO) to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            //Pass Details to Repository
            var Region = await regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { Id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("(id)")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {

            //Get Region from DB

            var region = await regionRepository.DeleteAsync(id);

            //If Null NotFound

            if (region == null)
            {
                return NotFound();
            }

            //Convert response back to DTO

            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //Return ok response

            return Ok(regionDTO);

        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Validate incoming request

            if(ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest();
            }

            //Convert DTO to Domain Model

            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };

            //Update Region using repository

            region = await regionRepository.UpdateAsync(id, region);


            //If Null then Not found
            if (region == null)
            {
                return NotFound();
            }

            //Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //Return Ok response
            return Ok(regionDTO);

        }

        #region Private Methods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest==null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), 
                    $"Add Region Data is required.");
                return false;
            }

                if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Name),
                            $"{nameof(addRegionRequest.Name)} Can't be null or empty or whitspace");
                    
                }
                if (addRegionRequest.Area<=0)
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Area),
                            $"{nameof(addRegionRequest.Area)} Can't be less than or equal to zero");
                    
                }
            
                if (addRegionRequest.Population  <0)
                {
                    ModelState.AddModelError(nameof(addRegionRequest.Population),
                            $"{nameof(addRegionRequest.Population)} Can't be less than zero");
                     
                }
                  if(ModelState.ErrorCount>0)
                  {
                      return false;
                  }

                return true;
            
            
        }
        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"Add Region Data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                        $"{nameof(updateRegionRequest.Name)} Can't be null or empty or whitspace");

            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                        $"{nameof(updateRegionRequest.Area)} Can't be less than or equal to zero");

            }
       
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                        $"{nameof(updateRegionRequest.Population)} Can't be less than zero");

            }
            if(ModelState.ErrorCount>0)
              {
                  return false;
              }
            return true;
        }
        #endregion
    }
}
