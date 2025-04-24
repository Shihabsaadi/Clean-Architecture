using Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Behaviours
{
	public class ValidationBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;
		public ValidationBehaviours(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (_validators.Any()) 
			{
			  var validationContext =	new ValidationContext<TRequest>(request);
			  var result = await Task.WhenAll(_validators.Select(x=>x.ValidateAsync(validationContext,cancellationToken)));
			  var failers = result.SelectMany(x => x.Errors).Where(_=>_!=null).ToList();
				if(failers.Any())
				{
					throw new ValidationErrorException(failers);
				}
			}
			var response = await next();
			return response;
		}
	}
}
