﻿using AutoMapper;
using MagicVilla.Api.Database;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Modules.Villas.Dtos;
//using MagicVilla.Api.Services.Villa.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPI_V3Controller : ControllerBase
    {
        // as we are registerd a service in our container .. 
        // we can extract that using dependency injection 

        // we want to use automapper inside controller

        // create private readonly variable 
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaAPI_V3Controller(
            //in constructor we will get that with dependency injection
            ApplicationDbContext db,
            IMapper mapper
            )
        {
            // assign that to the local variable 
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<List<VillaDTO>>(villaList)); 
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
            return Ok(_mapper.Map<VillaDTO>(villa));
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

            Villa model = _mapper.Map<Villa>(villaDto);
            

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
            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

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

            Villa model = _mapper.Map<Villa>(villaDto);

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

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);


            patchDto.ApplyTo(villaDTO, ModelState); //⚫ jsonpatch.com to see more details .. 

            // convert villaUpdateDto to villa model

            Villa model = _mapper.Map<Villa>(villaDTO);


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
