using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ActionFilters
{
    public class ValidateTemperatureExistAttrubute : IAsyncActionFilter
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILogger<ValidateTemperatureExistAttrubute> logger;

        public ValidateTemperatureExistAttrubute(IRepositoryManager repositoryManager, ILogger<ValidateTemperatureExistAttrubute> logger)
        {
            this.repositoryManager = repositoryManager;
            this.logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") ? true : false;

            var cityId = (Guid)context.ActionArguments["cityId"];
            var city = await repositoryManager.CityRepository.GetCityByIdAsync(cityId, false, trackChanges);

            if(city == null)
            {
                logger.LogInformation($"City with id: {cityId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["id"];
            var temperature = await repositoryManager.TemperatureRepository.GetTempByIdAsync(cityId, id, trackChanges);

            if(temperature == null)
            {
                logger.LogInformation($"Temperature with id: {id} doesn't exist for city with id : {cityId}");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("temperature", temperature);
                await next();
            }
        }
    }
}
