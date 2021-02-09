using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;

namespace AppExample.Application.Feature.Units.Commands.Delete
{
    public class DeleteUnitCommand : IRequest
    {
        public int Id { get; set; }
    }
    
    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteUnitCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Units.FindAsync(request.Id);

            if (entity == null) throw new Exception("Единицы измерения не найдены");

            _context.Units.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}