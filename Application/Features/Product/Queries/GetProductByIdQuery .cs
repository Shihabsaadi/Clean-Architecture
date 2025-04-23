using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Features.Product.Queries
{
	public class GetProductByIdQuery : IRequest<Domain.Entities.Product>
	{
        public int Id { get; set; }
        internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Domain.Entities.Product>
		{
			private readonly IApplicationDbContext _context;
			public GetProductByIdQueryHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<Domain.Entities.Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
			{
				var product= await _context.Products.FindAsync(request.Id);
				return product?? new Domain.Entities.Product();
			}
		}
	}
}
