using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using MediatR;


namespace Application.Features.Product.Commands
{
	public class UpdateProductCommand : IRequest<ApiResponse<Domain.Entities.Product>>
	{
		public int Id { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }
		internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<Domain.Entities.Product>>
		{
			private readonly IApplicationDbContext _context;
			public UpdateProductCommandHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<ApiResponse<Domain.Entities.Product>> Handle(UpdateProductCommand request,CancellationToken cancellationToken)
			{
				var product =await _context.Products.FindAsync(request.Id);
				if (product != null) {
					product.Name = request.Name;
					product.Description = request.Description;
					product.ModifiedOn=DateTime.Now;
					await _context.SaveChangesAsysnc();
					return new ApiResponse<Domain.Entities.Product>(product, "Data Updated successfully");
				}
				throw new ApiException("Product Not Found"); ;
			}
		}
	}
}
