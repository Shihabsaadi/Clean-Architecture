using Application.Interface;
using Application.Wrappers;
using AutoMapper;
using MediatR;


namespace Application.Features.Product.Commands
{
	public class CreateProductCommand:IRequest<ApiResponse<Domain.Entities.Product>>
	{
        public  string Name { get; set; }
        public  string Description { get; set; }
		internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<Domain.Entities.Product>>
		{
			private readonly IApplicationDbContext _context;
			private readonly IMapper _mapper;
			private readonly IAuthenticatedUser _authenticatedUser;
			public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthenticatedUser authenticatedUser)
			{
				_context = context;
				_mapper = mapper;
				_authenticatedUser = authenticatedUser;
			}
			public async Task<ApiResponse<Domain.Entities.Product>> Handle(CreateProductCommand request,CancellationToken cancellationToken)
			{
			    var product = _mapper.Map<Domain.Entities.Product>(request);
				product.CreatedBy = _authenticatedUser.UserId;
				product.CreatedOn = DateTime.Now;
				await _context.Products.AddAsync(product);
				await _context.SaveChangesAsysnc();
				return new ApiResponse<Domain.Entities.Product>(product, "Data Created successfully");

			}
		}
	}
}
