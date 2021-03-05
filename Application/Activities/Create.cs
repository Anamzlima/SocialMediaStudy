using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
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

            public Handler(DataContext context)
            {
                _context = context;
            }

            //Unit é um objeto de mediator, não faz nada, mas diz para a API 
            //que o request terminou para que possa prosseguir
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
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
