using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using World.API.DTOs.Country;
using World.API.DTOs.States;
using World.API.Models;
using World.API.Repository.IRepository;

namespace World.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public StateController(IStateRepository stateRepository, IMapper mapper, ILogger<StateController> logger)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateStateDTO>> Create([FromBody] CreateStateDTO stateDto)
        {
            var result = _stateRepository.IsRecordExists(x => x.Name == stateDto.Name);
            if (result)
            {
                return Conflict("State Already Exists in Database");
            }

            var state = _mapper.Map<State>(stateDto);

            await _stateRepository.Create(state);
            return CreatedAtAction("GetById", new { id = state.Id }, state);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<State>> Update(int id, [FromBody]UpdateStateDTO stateDto)
        {
            if (stateDto == null || id != stateDto.Id)
            {
                return BadRequest();
            }

            var state = _mapper.Map<State>(stateDto);


            await _stateRepository.Update(state);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<State>> DeletetById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var state = await _stateRepository.Get(id);
            if (state == null)
            {
                return NotFound();
            }
            await _stateRepository.Delete(state);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetAll()
        {
            var states = await _stateRepository.GetAll();
            if (states == null)
            {
                return NoContent();
            }
            var statesDto = _mapper.Map<List<StateDTO>>(states);
            return Ok(statesDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateDTO>> GetById(int id)
        {
            var state = await _stateRepository.Get(id);

            if (state == null)
            {
                _logger.LogError($"Error while try to get record id : {id}");
                return NoContent();
            }
            var stateDto = _mapper.Map<StateDTO>(state);
            return Ok(stateDto);
        }
    }
}
