using MagicVilla.Api.Database;
using MagicVilla.Api.Models;
using MagicVilla.Api.Services.Villa.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers
{
    [Route("api/VillaAPI")]
    // [Route("api/[Controller]")]
    [ApiController] // this is for active validation
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas () {

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

    }
}
