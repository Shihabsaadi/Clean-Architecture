using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Features.Product.Queries
{
	public class GetAllProductsQuery:IRequest<IEnumerable<Domain.Entities.Product>>
	{
		internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Domain.Entities.Product>>
		{
			private readonly IApplicationDbContext _context;
			public GetAllProductsQueryHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<IEnumerable<Domain.Entities.Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
			{
				return await _context.Products.ToListAsync(cancellationToken);
			}
		}
	}
}
