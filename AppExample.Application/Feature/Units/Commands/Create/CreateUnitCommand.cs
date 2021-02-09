using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;

namespace AppExample.Application.Feature.Units.Commands.Create
{
    public class CreateUnitCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
    }

    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateUnitCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = new Core.Entities.Unit
            {
                FullName = request.FullName,
                ShortName = request.ShortName,
                Description = request.Description
            };

            await _context.Units.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}