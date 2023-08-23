using EShop.Application.Repositories.ProductRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Features.Commands.UpdateProduct
{
    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductWriteRepository writeRepository;
        private readonly IProductReadRepository readRepository;

        public UpdateProductCommandHandler(IProductWriteRepository writeRepository, IProductReadRepository readRepository)
        {
            this.readRepository = readRepository;
            this.writeRepository = writeRepository;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await readRepository.GetAsync(request.Id);
            if(product is not null) 
            { 
                product.Description = request.Description;
                product.Price = request.Price;
                product.Stock = request.Stock;
                product.Name = request.Name;

                writeRepository.Update(product);
                await writeRepository.SaveChangesAsync();
                return new() { Result = true };
            }
            return new() { Result = false};
        }
    }
}
