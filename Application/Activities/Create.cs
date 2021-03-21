using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            //uma propriedade aqui nessa classe funciona como um parâmetro
            //aqui significa que vamos passa um objeto do tipo Activity pro Handler quando for criado
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            //Unit é um objeto de mediator, não faz nada, mas diz para a API 
            //que o request terminou para que possa prosseguir
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                    x.UserName == _userAccessor.GetUserName());

                var attendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);

                //não está usando AddAsync pq não está acessando o banco de dados direto aqui
                //só está salvando em memória, o EF está tracking a mudança em memória
                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create activity.");

                //equivalente a retornar nada, é só um jeito do controller saber que tá ok
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
