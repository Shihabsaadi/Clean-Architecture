using Application.Interface;
using MediatR;


namespace Application.Features.Product.Commands
{
	public class UpdateProductCommand : IRequest<int>
	{
		public int Id { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }
		internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
		{
			private readonly IApplicationDbContext _context;
			public UpdateProductCommandHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<int> Handle(UpdateProductCommand request,CancellationToken cancellationToken)
			{
				var product =await _context.Products.FindAsync(request.Id);
				if (product != null) {
					product.Name = request.Name;
					product.Description = request.Description;
					product.ModifiedOn=DateTime.Now;
					await _context.SaveChangesAsysnc();
					return product.Id;
				}
				return default;
			}
		}
	}
}
