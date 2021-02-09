using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;

namespace AppExample.Application.Feature.ServiceType.Commands.Update
{
    public class UpdateServiceTypeCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateServiceTypeCommandHandler : IRequestHandler<UpdateServiceTypeCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateServiceTypeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ServiceTypes.FindAsync(request.Id);

            if (entity == null) throw new Exception($"Тип сервис \"{request.Name}\" не найден");

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}