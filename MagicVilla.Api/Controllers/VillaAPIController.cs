using MagicVilla.Api.Database;
using MagicVilla.Api.Helper.Logger;
using MagicVilla.Api.Entities;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MagicVilla.Api.Modules.Villas.Dtos;

namespace MagicVilla.Api.Controllers
{
   

    [Route("api/VillaAPI")]
    // [Route("api/[Controller]")]
    [ApiController] // this is for active validation
    public class VillaAPIController : ControllerBase
    {
        // create and assign field logger .. 
        //private readonly ILogger<VillaAPIController> _logger; // default logger
        // ⚫ replace with new logger that we implement 


        // dependency injection

        // for controller -> ctor + tab 
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ILogging _logger;

        public VillaAPIController(ILogging logger) // dependency injection
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas () {
            //_logger.LogInformation("Getting all villas");
            _logger.Log("Getting all villas", "");

            //return new List<VillaDTO>
            //{
            //    new VillaDTO {Id = 1, Name = "Pool view"},
            //    new VillaDTO {Id = 2, Name = "Beach view"},
            //};
            return Ok(DataStore.villaList);
        }

        [HttpGet("id", Name = "GetVilla")] // "{id : int}" // explicitly bole deowa jabe ..
        // [ProducesResponseType(200, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status200OK)]// ok 
        [ProducesResponseType(404)]// not found
        [ProducesResponseType(400)]// bad request
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                //_logger.LogError("Get Villa Error with Id : ", id);
                return BadRequest(); // 400
            }
            var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa  == null)
            {
                return NotFound(); // 404
            }
            return Ok(villa);
        }

        [HttpPost] // http verb
        [ProducesResponseType(StatusCodes.Status200OK)]// ok 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(500)]// internal server error
        [ProducesResponseType(400)]// bad request
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    
            //    return BadRequest(ModelState);
            //}
            //// for custom validation 
            if(DataStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                // exist 
                ModelState.AddModelError("CustomError", "Villa Already exist");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                // this is not a create request
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = DataStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1; // we need just id and increament by 1
            DataStore.villaList.Add(villaDto);

            // return Ok(villaDto);
            return CreatedAtRoute("GetVilla", new {id = villaDto.Id}, villaDto);
        }


        [HttpDelete("id", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            DataStore.villaList.Remove(villa);
            return NoContent(); // 204
        }


        [HttpPut("id", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);

            villa.Name = villaDTO.Name;

            return NoContent();
        }

        [HttpPatch("id", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDto)
        {
            // validation
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState); //⚫ jsonpatch.com to see more details .. 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
              //path: "/name", op : "replace", value: "new name "
             // array er object hishebe pass korte hobe postman e 

            return NoContent();
        }
    }
}
