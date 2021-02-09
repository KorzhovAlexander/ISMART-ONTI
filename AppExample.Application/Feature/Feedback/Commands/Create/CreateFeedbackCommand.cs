using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using AppExample.Application.Common.Interfaces;
using MediatR;

namespace AppExample.Application.Feature.Feedback.Commands.Create
{
    public class CreateFeedbackCommand : IRequest<int>
    {
        public string Message { get; set; }

        public string Email { get; set; }
    }

    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateFeedbackCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
        {
            var entity = new Core.Entities.Feedback
            {
                Message = request.Message,
                Date = DateTime.Now,
                UserId = _currentUserService.UserId == 0 ? (int?) null : _currentUserService.UserId,
                Email = request.Email
            };

            await _context.Feedbacks.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}