using AutoMapper;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using SiteDeals.Core;
using SiteDeals.Core.Model;
using SiteDeals.Core.Repositories;
using SiteDeals.Core.UnitOfWorks;
using SiteDeals.MVCWebUI.Areas.Identity.Data;
using SiteDeals.Repository;
using SiteDeals.Repository.Repositories;
using System.Security.Claims;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;

namespace SiteDeals.Service.Service
{
    public class ProductService : Service<Product>
    {
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        private AppDbContext _appDbContext;
        private readonly UserManager<SiteDealsMVCWebUIUser> _userManager;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, ProductRepository productRepository, AppDbContext appDbContext, UserManager<SiteDealsMVCWebUIUser> userManager ) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public Task<int> IncreaseLikes(int pId, int uId, HttpRequest _httpRequest)
        {
            var localIp = GlobalHelper.getLocalIp(_httpRequest);
            try
            {
                Product? p = _appDbContext.Products.FirstOrDefault(p => p.Id == pId);
                try
                {
                    ProductLikes pl = _appDbContext.ProductLikes.First(pl => pl.UserId == uId && pl.ProductId == pId);
                }
                catch
                {
                    ProductLikes pl = new()
                    {
                        ProductId = pId,
                        UserId = uId,
                        Liked = true,
                        IpAddress = localIp,
                        CreatedAt = DateTime.Now
                    };
                    p.Likes++;
                    _appDbContext.Update(p);
                    _appDbContext.Add(pl);
                }
                _appDbContext.SaveChanges();
                return Task.FromResult(p.Likes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<int> DecreaseLikes(int pId, int uId, HttpRequest _httpRequest)
        {
            var localIp = GlobalHelper.getLocalIp(_httpRequest);
            try
            {
                Product? p = _appDbContext.Products.FirstOrDefault(p => p.Id == pId);
                try
                {
                    ProductLikes pl = _appDbContext.ProductLikes.First(pl => pl.UserId == uId && pl.ProductId == pId);
                }
                catch
                {
                    ProductLikes pl = new()
                    {
                        ProductId = pId,
                        UserId = uId,
                        Liked = false,
                        IpAddress = localIp,
                        CreatedAt = DateTime.Now
                    };
                    p.Likes--;
                    _appDbContext.Update(p);
                    _appDbContext.Add(pl);
                }
                _appDbContext.SaveChanges();
                return Task.FromResult(p.Likes);
            }
            catch (Exception)
            {
                throw;
            }
        }

      

     
        //public async Task<string> GetLinkAmazon(int id)
        //{
        //    var productLink = await _productRepository.GetByIdAsync(id);
        //    HttpClient client = new HttpClient();
        //    var doc = new HtmlDocument();
        //    HttpResponseMessage response = await client.GetAsync(productLink.Link).ConfigureAwait(false);
        //    response.EnsureSuccessStatusCode();
        //    var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //    doc.LoadHtml(res);
        //    doc.DocumentNode.SelectNodes("//")
        //    return "selam";
        //}  

      
    

        public async Task<List<ProductReviews>> GetProductReviews(int productId, int page, int count)
        {
            var reviews = await _productRepository.GetAllReviews(productId, page, count);
            return reviews;
        }



        public async Task<IAsyncEnumerable<Product>> GetProductsForHome(int count)
        {
            return await _productRepository.GetProductsForHome(count);
        }

        public async override Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

     
        public async Task<Product> Create([FromBody] Product product, ClaimsPrincipal userClaimsPrincipal)
        {
            try
            {
                var user = await _userManager.GetUserAsync(userClaimsPrincipal);

                product.CreatedAt = DateTime.Now;
                product.CreatedById = user.Id;
                product.CreatedByUserName = user.UserName;
                product.Status = 0;


                //  _appDbContext.Entry(product.Images).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                // _appDbContext.Entry(product).Collection(x => x.Images).Query();

                product.Vendor = new Uri(product.Link).GetLeftPart(UriPartial.Authority);



                foreach (var image in product.Images)
                {
              //      _appDbContext.Entry(image).Reload();
                    if (image.CreatedBy != user.Id)
                    {
                        throw new Exception("Bir sen mi akıllısın?");
                    }
                }

                product.CloudinaryPublicId = product.Images.FirstOrDefault()?.CloudinaryPublicId;


                foreach (var tag in product.Tags)
                {
                    tag.ProductId = product.Id;
                    tag.CreatedBy = user.Id;
                    tag.CreatedAt = DateTime.Now;
                }


               await AddAsync(product);
               


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return product;
          

        }

    }
}
