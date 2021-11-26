using AutoMapper;
using Contracts.CityDTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ActionFilters;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly WeatherDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<CitiesController> logger;
        private readonly IMemoryCache cache;

        public CitiesController(IRepositoryManager repository, WeatherDbContext context, IMapper mapper, 
            ILogger<CitiesController> logger, IMemoryCache cache)
        {
            this.repository = repository;
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities([FromQuery] CityParameters requestParameters, bool includeTemperature = false)
        {
            var cities = await repository.CityRepository.GetAllCitiesAsync(includeTemperature, requestParameters, trackChanges: false);

            if (cities == null)
            {
                logger.LogInformation($"Can't find any city information");
                return NotFound();
            }

            var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);

            foreach(var city in citiesDto)
            {
                city.CurrentTemerature = await repository.UnitOfWork.FindCurrentCityTemp(city.Id);
            }

            return Ok(citiesDto);
        }

        [HttpGet("{id}", Name = "CityById")]
        public async Task<IActionResult> GetCityById(Guid id, bool includeTemperature = false)
        {
            var city = await repository.CityRepository.GetCityByIdAsync(id, includeTemperature, trackChanges: false);

            if (city == null)
            {
                logger.LogInformation($"City with id: {id} does not found");
                return NotFound();
            }

            CityDto cityDto = null;

            if(!cache.TryGetValue(id, out cityDto))
            {
                cityDto = mapper.Map<CityDto>(city);

                cityDto.CurrentTemerature = await repository.UnitOfWork.FindCurrentCityTemp(cityDto.Id);

                cache.Set(cityDto.Id, cityDto, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            return Ok(cityDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddCity([FromBody]CityForCreationDto cityDto)
        {
            var cityEntity = mapper.Map<City>(cityDto);

            repository.CityRepository.Add(cityEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            var cityToReturn = mapper.Map<CityDto>(cityEntity);

            return CreatedAtRoute("CityById", new { id = cityToReturn.Id }, cityToReturn);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCityExistAttribute))]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody]CityForUpdateDto cityDto)
        {
            var cityEntity = HttpContext.Items["city"] as City;

            mapper.Map(cityDto, cityEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCityExistAttribute))]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = HttpContext.Items["city"] as City;

            repository.CityRepository.Remove(city);
            await repository.UnitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
