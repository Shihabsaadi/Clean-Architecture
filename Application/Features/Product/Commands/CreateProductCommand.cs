using Application.Interface;
using AutoMapper;
using MediatR;


namespace Application.Features.Product.Commands
{
	public class CreateProductCommand:IRequest<int>
	{
        public  string Name { get; set; }
        public  string Description { get; set; }
		internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand,int>
		{
			private readonly IApplicationDbContext _context;
			private readonly IMapper _mapper;
			public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
			{
				_context = context;
				_mapper = mapper;
			}
			public async Task<int> Handle(CreateProductCommand request,CancellationToken cancellationToken)
			{
			    var product = _mapper.Map<Domain.Entities.Product>(request);
				product.CreatedOn = DateTime.Now;
				await _context.Products.AddAsync(product);
				await _context.SaveChangesAsysnc();
				return product.Id;

			}
		}
	}
}
