using EShop.Application.Repositories;
using MediatR;

namespace EShop.Application.Features.Queries.Products.GetAllProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQueryRequest, GetProductsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetProductsQueryResponse> Handle(GetProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var products = _unitOfWork.ProductReadRepository.GetAll(tracking: false);
            var totalCount = products.Count();

            var selecetedProducts = products
                        .OrderBy(p => p.CreatedTime)
                        .Skip(request.Size * request.Page)
                        .Take(request.Size)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                            p.Description,
                            p.Stock
                        });

            return new() { Products = selecetedProducts, TotalCount = totalCount };


        }
    }
}
