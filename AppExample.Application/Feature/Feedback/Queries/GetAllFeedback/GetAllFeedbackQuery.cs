using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Application.Common.Interfaces;
using MediatR;

namespace AppExample.Application.Feature.Feedback.Queries.GetAllFeedback
{
    public class GetAllFeedbackQuery : IRequest<IQueryable<FeedbackDto>>
    {
    }

    public class GetAllFeedbackQueryHandler : IRequestHandler<GetAllFeedbackQuery, IQueryable<FeedbackDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetAllFeedbackQueryHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IQueryable<FeedbackDto>> Handle(GetAllFeedbackQuery request,
            CancellationToken cancellationToken)
        {
            return await Task.FromResult(_context.Feedbacks.AsQueryable()
                .Select(feedback => new FeedbackDto
                {
                    Id = feedback.Id,
                    Date = feedback.Date,
                    Message = feedback.Message,
                    UserId = feedback.UserId,
                    Email = feedback.Email
                }));
        }
    }
}