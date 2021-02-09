using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;

namespace AppExample.Application.Feature.ServiceType.Commands.Delete
{
    public class DeleteServiceTypeCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteServiceTypeCommandHandler : IRequestHandler<DeleteServiceTypeCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteServiceTypeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ServiceTypes.FindAsync(request.Id);

            if (entity == null) throw new Exception("Тип сервиса не найден");

            _context.ServiceTypes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}