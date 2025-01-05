using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using World.API.Data;
using World.API.DTOs.Country;
using World.API.Models;
using World.API.Repository.IRepository;

namespace World.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CountryController(ICountryRepository countryRepository, IMapper mapper, ILogger<CountryController> logger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateCountryDTO>> Create([FromBody]CreateCountryDTO countryDto) 
        {
            var result = _countryRepository.IsRecordExists(x => x.Name == countryDto.Name);
            if (result) 
            {
                return Conflict("Country Already Exists in Database");
            }

            var country = _mapper.Map<Country>(countryDto);

            await _countryRepository.Create(country);
            return CreatedAtAction("GetById", new {id = country.Id}, country);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Country>> Update(int id, [FromBody] UpdateCountryDTO countryDto)
        {
            if (countryDto == null || id != countryDto.Id)
            {
                return BadRequest();
            }

            var country = _mapper.Map<Country>(countryDto);
          

            await _countryRepository.Update(country);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Country>> DeletetById(int id)
        {
            if(id == 0)
            {
                return BadRequest(); 
            }

            var country = await _countryRepository.Get(id);

            if (country == null) 
            { 
                return NotFound();
            }
            await _countryRepository.Delete(country);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetAll() 
        {
            var countries = await _countryRepository.GetAll();
      
            if (countries == null) 
            {
                return NoContent();
            }
            var countriesDto = _mapper.Map<List<CountryDTO>>(countries);
            return Ok(countriesDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CountryDTO>> GetById(int id) 
        {
            var country = await _countryRepository.Get(id);

            if(country == null)
            {
                _logger.LogError($"Error while try to get record id : {id}");
                return NoContent();
            }
            var countryDto = _mapper.Map<CountryDTO>(country);
            return Ok(countryDto);
        }
    }
}
