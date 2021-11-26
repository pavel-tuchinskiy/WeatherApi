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
    public class ValidateCityExistAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly ILogger<ValidateCityExistAttribute> logger;

        public ValidateCityExistAttribute(IRepositoryManager repositoryManager, ILogger<ValidateCityExistAttribute> logger)
        {
            this.repositoryManager = repositoryManager;
            this.logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            var city = await repositoryManager.CityRepository.GetCityByIdAsync(id, false, trackChanges);

            if (city == null)
            {
                logger.LogInformation($"Company with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("city", city);
                await next();
            }
        }
    }
}
