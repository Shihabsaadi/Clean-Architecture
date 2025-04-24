using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Features.Product.Queries
{
	public class GetAllProductsQuery:IRequest<ApiResponse<IEnumerable<Domain.Entities.Product>>>
	{
		internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ApiResponse<IEnumerable<Domain.Entities.Product>>>
		{
			private readonly IApplicationDbContext _context;
			public GetAllProductsQueryHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<ApiResponse<IEnumerable<Domain.Entities.Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
			{
				var products= await _context.Products.ToListAsync(cancellationToken);
				if (products == null)
				{
					throw new ApiException("Products not found");
				}
				return new ApiResponse<IEnumerable<Domain.Entities.Product>>(products, "Data Fetched successfully");
			}
		}
	}
}
