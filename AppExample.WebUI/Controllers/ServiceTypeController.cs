using System.Threading.Tasks;
using AppExample.Application.Feature.ServiceType.Commands.Create;
using AppExample.Application.Feature.ServiceType.Commands.Delete;
using AppExample.Application.Feature.ServiceType.Commands.Update;
using AppExample.Application.Feature.ServiceType.Queries.GetServiceTypes;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppExample.WebUI.Controllers
{
    public class ServiceTypeController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetServiceTypesAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.RequireTotalCount = true;
            return Ok(DataSourceLoader.Load(await Mediator.Send(new GetServiceTypesQuery()), loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceTypeAsync(CreateServiceTypeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceTypeAsync(int id, UpdateServiceTypeCommand command)
        {
            if (id != command.Id) return BadRequest();
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceTypeAsync(int id)
        {
            await Mediator.Send(new DeleteServiceTypeCommand {Id = id});
            return NoContent();
        }
    }
}