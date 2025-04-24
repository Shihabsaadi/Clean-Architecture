using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using MediatR;


namespace Application.Features.Product.Commands
{
	public class DeleteProductCommand : IRequest<ApiResponse<Domain.Entities.Product>>
	{
		public int Id { get; set; }

		internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse<Domain.Entities.Product>>
		{
			private readonly IApplicationDbContext _context;
			public DeleteProductCommandHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<ApiResponse<Domain.Entities.Product>> Handle(DeleteProductCommand request,CancellationToken cancellationToken)
			{
				var product = _context.Products.Find(request.Id);
				if (product == null)
				{
					throw new ApiException("Product Not Found");
				}

				_context.Products.Remove(product);
				await _context.SaveChangesAsysnc();
				return new ApiResponse<Domain.Entities.Product>(product, "Data Deleted successfully");

			}
		}
	}
}
