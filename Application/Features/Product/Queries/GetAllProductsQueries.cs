using MediatR;
using System;

namespace Application.Features.Product.Queries
{
	public class GetAllProductsQuery:IRequest<IEnumerable<Domain.Entities.Product>>
	{
		internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Domain.Entities.Product>>
		{
			public async Task<IEnumerable<Domain.Entities.Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
			{
				var list = new List<Domain.Entities.Product>();
				return list;
			}
		}
	}
}
