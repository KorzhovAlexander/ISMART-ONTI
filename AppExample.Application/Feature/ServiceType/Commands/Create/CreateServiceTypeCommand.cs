using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;

namespace AppExample.Application.Feature.ServiceType.Commands.Create
{
    public class CreateServiceTypeCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateServiceTypeCommandHandler : IRequestHandler<CreateServiceTypeCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateServiceTypeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Core.Entities.ServiceType
            {
                Name = request.Name,
                Description = request.Description
            };

            await _context.ServiceTypes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}