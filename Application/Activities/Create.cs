using Domain;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            //uma propriedade aqui nessa classe funciona como um parâmetro
            //aqui significa que vamos passa um objeto do tipo Activity pro Handler quando for criado
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            //Unit é um objeto de mediator, não faz nada, mas diz para a API 
            //que o request terminou para que possa prosseguir
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //não está usando AddAsync pq não está acessando o banco de dados direto aqui
                //só está salvando em memória, o EF está tracking a mudança em memória
                _context.Activities.Add(request.Activity);

                await _context.SaveChangesAsync();

                //equivalente a retornar nada, é só um jeito do controller saber que tá ok
                return Unit.Value;
            }
        }
    }
}
