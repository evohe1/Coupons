using AmazonGrpcService;
using AmazonGrpcService.Parsers;
using Grpc.Core;
using GrpcService.Protos;

namespace AmazonGrpcService.Services
{
    public class ProductService : Crawl.CrawlBase
    {
        public override async Task<ProductInfo> GetProductInfo(ProductUrl request, ServerCallContext context)
        {
            var amazonParser = new AmazonParser();

            var Description = await amazonParser.GetProductDetail(request.Url);

            ProductInfo product = new ProductInfo();
            product.Price = Description.Price;
            if (!double.IsNaN(Description.PreviousPrice))
            {
                product.PreviousPrice = Description.PreviousPrice;
            }



            if (Description.Jpgs?.Count > 0)
            {
                foreach (string jpg in Description.Jpgs)
                {
                    product.Images.Add(new Images
                    {
                        Urljpg = jpg
                    });

                }
            }

            if (Description.Infos?.Count > 0)
            {
                foreach (var item in Description.Infos)
                {
                    product.Description.Add(new Description
                    {
                        Info = item
                    });
                }

            }


            if (Description.Categories?.Count > 0)
            {
                foreach (var item in Description.Categories)
                {

                    product.Category.Add(new Category
                    {
                        Category_ = item
                    });
                }
            }
            return product;



        }

    }

}