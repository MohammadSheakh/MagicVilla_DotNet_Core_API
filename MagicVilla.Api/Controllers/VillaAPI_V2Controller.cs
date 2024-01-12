using MagicVilla.Api.Database;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Modules.Villas.Dtos;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Api.Controllers
{
    [Route("api/VillaAPI_V2")]
    [ApiController]
    public class VillaAPI_V2Controller : ControllerBase
    {
        // as we are registerd a service in our container .. 
        // we can extract that using dependency injection 

        // create private readonly variable 
        private readonly ApplicationDbContext _db;
        public VillaAPI_V2Controller(
            //in constructor we will get that with dependency injection
            ApplicationDbContext db
            ) 
        {
            // assign that to the local variable 
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            
            return Ok(await _db.Villas.ToListAsync()); // 
        }

        [HttpGet("id", Name = "GetVilla")] // "{id : int}" // explicitly bole deowa jabe ..
        // [ProducesResponseType(200, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status200OK)]// ok 
        [ProducesResponseType(404)]// not found
        [ProducesResponseType(400)]// bad request
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest(); // 400
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
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
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDto)
        {
            if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                // exist 
                ModelState.AddModelError("CustomError", "Villa Already exist");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            

            // we need to convert villaDto to villaModel

            //var model = new Villa
            //{

            //};

            Villa model = new()
            {
                Amenity = villaDto.Amenity,
                Details = villaDto.Details,
                //Id = villaDto.Id,
                ImageUrl = villaDto.ImageUrl,
                Name = villaDto.Name,
                Occupancy = villaDto.Occupancy,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft,
            };

            await _db.Villas.AddAsync(model);

            await _db.SaveChangesAsync();

            // create houar pore automatically id populate hoy 
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }


        [HttpDelete("id", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);
            var villa =await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent(); // 204
        }


        [HttpPut("id", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            //var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDTO.Name;

            Villa model = new()
            {
                Amenity = villaDto.Amenity,
                Details = villaDto.Details,
                Id = villaDto.Id,
                ImageUrl = villaDto.ImageUrl,
                Name = villaDto.Name,
                Occupancy = villaDto.Occupancy,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft,
            };

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("id", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            // validation
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            // dont track this object ... 
            // as entity framework can not track two object at a time

            //villa.Name = "new name ";
            //_db.SaveChanges(); // this is another way to update

            if (villa == null)
            {
                return BadRequest();
            }

            // we need to convert villa to villaDto

            VillaUpdateDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };

            patchDto.ApplyTo(villaDTO, ModelState); //⚫ jsonpatch.com to see more details .. 

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
            };

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

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
