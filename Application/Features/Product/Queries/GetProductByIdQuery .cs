using Application.Exceptions;
using Application.Interface;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Features.Product.Queries
{
	public class GetProductByIdQuery : IRequest<ApiResponse<Domain.Entities.Product>>
	{
        public int Id { get; set; }
        internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<Domain.Entities.Product>>
		{
			private readonly IApplicationDbContext _context;
			public GetProductByIdQueryHandler(IApplicationDbContext context)
			{
				_context = context;
			}
			public async Task<ApiResponse<Domain.Entities.Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
			{
				var product= await _context.Products.FindAsync(request.Id);
				if (product == null)
				{
					throw new ApiException("Product Not Found");
				}
				return new ApiResponse<Domain.Entities.Product>(product, "Data Fetched successfully");
			}
		}
	}
}
