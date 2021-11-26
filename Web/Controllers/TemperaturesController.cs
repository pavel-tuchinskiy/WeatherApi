using AutoMapper;
using Contracts.TemperatureDTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.RequestFeatures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web.ActionFilters;

namespace Web.Controllers
{
    [Route("api/Cities/{cityId}/[controller]/[action]")]
    [ApiController]
    public class TemperaturesController : ControllerBase
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ILogger<CitiesController> logger;
        private readonly IWebHostEnvironment env;

        public TemperaturesController(IRepositoryManager repository, IMapper mapper, ILogger<CitiesController> logger, IWebHostEnvironment env)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.env = env;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> GetAllTemperature(Guid cityId, [FromQuery]TemperatureParameters requestParameters)
        {
            if (!requestParameters.ValidDateRange)
                return BadRequest("Invalid date range.");

            var temperatures = await repository.TemperatureRepository.GetAllTempByCityAsync(cityId, requestParameters, false);

            if (temperatures == null)
            {
                logger.LogInformation($"Can't find any temperature information for city with id {cityId}");
                return NotFound();
            }

            var temperaturesDto = mapper.Map<IEnumerable<TemperatureDto>>(temperatures);

            return Ok(temperaturesDto);
        }

        [HttpGet("{id}", Name = "TemperatureById")]
        public async Task<IActionResult> GetTemperatureById(Guid cityId, Guid id)
        {
            var city = await repository.CityRepository.GetCityByIdAsync(cityId, false, trackChanges: false);

            if (city == null)
            {
                logger.LogInformation($"City with id: {cityId} does not found");
                return NotFound();
            }

            var temperatureEntity = await repository.TemperatureRepository.GetTempByIdAsync(cityId, id, trackChanges: false);

            if (temperatureEntity == null)
            {
                logger.LogInformation($"Temperature data with id: {id} does not found");
                return NotFound();
            }

            var temperatureDto = mapper.Map<TemperatureDto>(temperatureEntity);

            return Ok(temperatureDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> AddTemperature(Guid cityId, [FromBody] TemperatureForCreationDto temperatureDto)
        {
            var city = await repository.CityRepository.GetCityByIdAsync(cityId, false, trackChanges: false);

            if (city == null)
            {
                logger.LogInformation($"City with id: {cityId} does not found");
                return NotFound();
            }

            var temperatureEntity = mapper.Map<Temperature>(temperatureDto);

            repository.TemperatureRepository.Add(cityId, temperatureEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            var temperatureToReturn = mapper.Map<TemperatureDto>(temperatureEntity);

            return CreatedAtRoute("TemperatureById", new { cityId, id = temperatureToReturn.Id }, temperatureToReturn);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTemperatureExistAttrubute))]
        public async Task<IActionResult> UpdateTemperature(Guid cityId, Guid id, [FromBody] TemperatureForUpdateDto temperatureDto)
        {
            var temperatureEntity = HttpContext.Items["temperature"] as Temperature;

            mapper.Map(temperatureDto, temperatureEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTemperatureExistAttrubute))]
        public async Task<IActionResult> PartiallyUpdateTemperature(Guid cityId, Guid id,
            [FromBody] JsonPatchDocument<TemperatureForUpdateDto> patchDoc)
        {
            if(patchDoc == null)
            {
                logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var temperatureEntity = HttpContext.Items["temperature"] as Temperature;

            var temperatureToPatch = mapper.Map<TemperatureForUpdateDto>(temperatureEntity);

            patchDoc.ApplyTo(temperatureToPatch, ModelState);

            TryValidateModel(temperatureToPatch);

            if (!ModelState.IsValid)
            {
                logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            mapper.Map(temperatureToPatch, temperatureEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateTemperatureExistAttrubute))]
        public async Task<IActionResult> DeleteTemperature(Guid cityId, Guid id)
        {
            var temperatureEntity = HttpContext.Items["temperature"] as Temperature;

            repository.TemperatureRepository.Remove(temperatureEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete(Name = "SaveAndDeleteTemperature")]
        public async Task<IActionResult> SaveAndDeleteTemperature(Guid cityId, string date)
        {
            var city = await repository.CityRepository.GetCityByIdAsync(cityId, false, trackChanges: false);

            if (city == null)
            {
                logger.LogInformation($"City with id: {cityId} does not found");
                return NotFound();
            }

            var dateParse = DateTime.Parse(date);
            var temperatureEntity = await repository.TemperatureRepository.GetTempByDateAsync(cityId, dateParse, trackChanges: false);

            if (temperatureEntity == null)
            {
                logger.LogInformation($"Temperature data for date: {date} does not found");
                return NotFound();
            }

            var temperatureDto = mapper.Map<TemperatureDto>(temperatureEntity);

            var fileName = string.Concat(temperatureDto.DateTime.ToString().Replace(':', '-').Replace('.', '-').Trim(), ".json");
            var directoryPath = Path.Combine(env.WebRootPath, city.Name);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var uploadDir = Path.Combine(directoryPath, fileName);

            var json = JsonSerializer.Serialize<Temperature>(temperatureEntity);

            using (var writer = new StreamWriter(uploadDir))
            {
                await writer.WriteAsync(json);
            }

            repository.TemperatureRepository.Remove(temperatureEntity);
            await repository.UnitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost(Name = "RetrieveTemperature")]
        public async Task<IActionResult> RetrieveTemperature(Guid cityId, string date)
        {
            var city = await repository.CityRepository.GetCityByIdAsync(cityId, false, trackChanges: false);

            if (city == null)
            {
                logger.LogInformation($"City with id: {cityId} does not found");
                return NotFound();
            }

            var dateParse = DateTime.Parse(date);

            var fileName = string.Concat(dateParse.ToString().Replace(':', '-').Replace('.', '-').Trim(), ".json");
            var directoryPath = Path.Combine(env.WebRootPath, city.Name);
            var filePath = Path.Combine(directoryPath, fileName);

            var json = System.IO.File.ReadAllText(filePath);

            var temperature = JsonSerializer.Deserialize<Temperature>(json);

            repository.TemperatureRepository.Add(cityId, temperature);
            await repository.UnitOfWork.SaveChangesAsync();

            var temperatureToReturn = mapper.Map<TemperatureDto>(temperature);

            return CreatedAtRoute("TemperatureById", new { cityId, id = temperatureToReturn.Id }, temperatureToReturn);
        }
    }
}
