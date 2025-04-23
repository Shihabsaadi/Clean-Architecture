using Application.Interface;
using MediatR;


namespace Application.Features.Product.Commands
{
	public class DeleteProductCommand : IRequest<int>
	{
		public int Id { get; set; }

		internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
		{
			private readonly IApplicationDbContext _context;
			public DeleteProductCommandHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<int> Handle(DeleteProductCommand request,CancellationToken cancellationToken)
			{
				var product = _context.Products.Find(request.Id);
				if(product == null)
					return default;

				_context.Products.Remove(product);
				await _context.SaveChangesAsysnc();
				return request.Id;

			}
		}
	}
}
