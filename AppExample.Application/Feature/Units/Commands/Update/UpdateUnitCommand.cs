using System;
using System.Threading;
using System.Threading.Tasks;
using AppExample.Application.Common.Access;
using MediatR;

namespace AppExample.Application.Feature.Units.Commands.Update
{
    public class UpdateUnitCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
    }

    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateUnitCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Units.FindAsync(request.Id);
            
            if (entity == null) throw new Exception("Единицы измерения не найдены");

            entity.FullName = request.FullName;
            entity.ShortName = request.ShortName;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}