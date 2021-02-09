using System.Threading.Tasks;
using AppExample.Application.Feature.Feedback.Commands.Create;
using AppExample.Application.Feature.Feedback.Queries.GetAllFeedback;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppExample.WebUI.Controllers
{
    public class FeedbackController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddFeedbackAsync(CreateFeedbackCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> AddFeedbackAsync(DataSourceLoadOptions loadOptions)
        {
            return Ok(await DataSourceLoader.LoadAsync(await Mediator.Send(new GetAllFeedbackQuery()), loadOptions));
        }
    }
}