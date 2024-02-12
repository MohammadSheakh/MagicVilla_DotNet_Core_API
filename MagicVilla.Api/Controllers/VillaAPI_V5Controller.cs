using AutoMapper;
using Azure;
using MagicVilla.Api.Database;
using MagicVilla.Api.Entities;
using MagicVilla.Api.Modules.Villas.Dtos;
using MagicVilla.Api.Modules.Villas.Interface;
using MagicVilla.Api.Modules.Villas.Services.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPI_V5Controller : ControllerBase
    {
     
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        protected APIResponse _response;
        public VillaAPI_V5Controller(
            //in constructor we will get that with dependency injection
            ApplicationDbContext db, // we are not using it here 
            IMapper mapper,
            IVillaRepository dbVilla
            )
        {
            // assign that to the local variable 
            _db = db; // we are not using it here 
            _mapper = mapper;
            _dbVilla = dbVilla;
            this._response = new();  // all the return type in _response now 
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try { 
                //IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();

                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;

                //return Ok(_mapper.Map<List<VillaDTO>>(villaList));
                return Ok(_response);
                
            }
            catch(Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() }; 
            }

            return _response;
        }

        [HttpGet("id", Name = "GetVilla")] // "{id : int}" // explicitly bole deowa jabe ..
                                            // [ProducesResponseType(200, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status200OK)]// ok 
        [ProducesResponseType(404)]// not found
        [ProducesResponseType(400)]// bad request
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response); // 400
                    // return BadRequest(); 
                }
                //var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
                var villa = await _dbVilla.GetAsync(u => u.Id == id); // we want to get by condition ..
                                                                        //so, filter is the same here
                if (villa == null)
                {
                    return NotFound(); // 404
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);

                // return Ok(_mapper.Map<VillaDTO>(villa));
            }
            catch(Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;


        }

        [HttpPost] // http verb
        [ProducesResponseType(StatusCodes.Status200OK)]// ok 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(500)]// internal server error
        [ProducesResponseType(400)]// bad request
        //public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDto)
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaDto)
        {
            try
            {
                //if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null) // we pass filter
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


            //await _db.Villas.AddAsync(model);
            //await _db.SaveChangesAsync();
            await _dbVilla.CreateVillaAsync(model);


            _response.Result = _mapper.Map<VillaDTO>(model);
            _response.StatusCode = HttpStatusCode.Created;
            //return Ok(_response);


            // create houar pore automatically id populate hoy 
            //return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
            return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);


            }
            catch(Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString()
            };
        }

        return _response;

        }

        [HttpDelete("id", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> DeleteVilla(int id)
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                //var villa = DataStore.villaList.FirstOrDefault(u => u.Id == id);
                //var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

                var villa = await _dbVilla.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                //_db.Villas.Remove(villa);
                //await _db.SaveChangesAsync();

                await _dbVilla.RemoveAsync(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccessfull = true;

                // return NoContent(); // 204
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }


        [HttpPut("id", Name = "UpdateVilla")]
        //public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDto)
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDto)
        {
            try
            {
                if (villaDto == null || id != villaDto.Id)
                {
                    return BadRequest();
                }

                Villa model = _mapper.Map<Villa>(villaDto);

                // _db.Villas.Update(model);
                //await _db.SaveChangesAsync();

                await _dbVilla.UpdateVillaAsync(model);


                // _response.Result = _mapper.Map<VillaDTO>(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccessfull = true;

                return Ok(_response);


                //return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
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

            //var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

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


            //_db.Villas.Update(model);
            //await _db.SaveChangesAsync();
            _dbVilla.UpdateVillaAsync(model);

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
