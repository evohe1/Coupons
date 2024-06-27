using AmazonGrpcService.Protos;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteDeals.Core;
using SiteDeals.Core.Model;
using SiteDeals.Core.Services;
using SiteDeals.MVCWebUI.Areas.Identity.Data;
using SiteDeals.MVCWebUI.Models;
using SiteDeals.Repository;
using SiteDeals.Service.Service;

namespace SiteDeals.MVCWebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly UserManager<SiteDealsMVCWebUIUser> _userManager;
        private readonly TagService _tagService;
        private readonly ProductImageService _productImageService;
        private SignInManager<SiteDealsMVCWebUIUser> _signInManager;
        private readonly ProductLikesService _productLikesService;
        private readonly ProductReviewService _productReviewService;
        [ViewData]
        public int ProductId { get; set; }

        public ProductController(ProductService productService, UserManager<SiteDealsMVCWebUIUser> userManager, SignInManager<SiteDealsMVCWebUIUser> signInManager,TagService tagService, ProductImageService productImageService, ProductLikesService productLikesService,ProductReviewService productReviewService)
        {
            _productService = productService;
            _userManager = userManager;
            _tagService = tagService;
            _signInManager = signInManager;
            _productImageService = productImageService;
            _productLikesService = productLikesService;
            _productReviewService = productReviewService;
        }
        public async Task<IActionResult> Grid()
        {
            var deneme = await _productService.GetAllAsync();
            var channel = GrpcChannel.ForAddress("http://localhost:5032");
            var greetclient = new Amazon.AmazonClient(channel);
            return View(deneme);
        }

        [Authorize]
        public async Task<int> DownVote(int id)
        {
            var userId = Convert.ToInt32(_userManager.GetUserId(User));
            var test = _productService.DecreaseLikes(id, userId, Request);
            return test.Result;
        }

        [Authorize]
        public async Task<int> UpVote(int id)
        {
            var userId = Convert.ToInt32(_userManager.GetUserId(User));
            var test = _productService.IncreaseLikes(id, userId, Request);
            return test.Result;
        }

        [HttpPost]
        [Route("/senin-kampanyan/doldur")]
        [Authorize]
        public async Task<GrpcService.Protos.ProductInfo> CrawlProductInfo([FromBody] Product product)
        {
            if (String.IsNullOrEmpty(product.Link))
            {
                return null;
            }
            var host = ExtractDomainNameFromUrl(product.Link).ToLower();
            string grpcHost = "";
            if (host.Contains("amazon"))
            {
                grpcHost = "https://amazon.grpc.kampanyan.com";
            }
          
            else
            {
                return null;
            }

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(grpcHost, new GrpcChannelOptions { HttpHandler = httpHandler });

            var crawlClient = new GrpcService.Protos.Crawl.CrawlClient(channel);
            GrpcService.Protos.ProductInfo productInfo = await crawlClient.GetProductInfoAsync(new GrpcService.Protos.ProductUrl
            {
                Url = product.Link
            });

            return productInfo;
        }

        public static string ExtractDomainNameFromUrl(string url)
        {
            if (!url.Contains("://"))
                url = "http://" + url;

            return new Uri(url).Host;
        }

        // GET: UserController/Create
        [Route("/senin-kampanyan")]
        public async Task<ActionResult> Create()
        {
            var model = new ProductModel();
            model.Tags =await _tagService.GetAllAsync();
            return View(model);
        }

        [HttpPost]
        [Route("/senin-kampanyan")]
        [Authorize]
        public async Task<JsonResult> Create([FromBody] Product product)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                product.CreatedAt = DateTime.Now;
                product.CreatedById = user.Id;
                product.CreatedByUserName = user.UserName;
                product.Status = 0;


                //  _appDbContext.Entry(product.Images).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                // _appDbContext.Entry(product).Collection(x => x.Images).Query();

                product.Vendor = new Uri(product.Link).GetLeftPart(UriPartial.Authority);



                foreach (var image in product.Images)
                {
                    //_appDbContext.Entry(image).Reload();
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


                _productService.AddAsync(product);
                


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return Json(new { url = Url.RouteUrl("ProductRoute", new { id = product.Id, slug = GlobalHelper.GenerateSlug(product.Name) }) });

        }

        [HttpPost]
        [Route("/senin-kampanyan/fotograf-yukle")]
        [Authorize]
        public async Task<ProductImages> UploadPhoto([FromBody] ImageModel image)
        {

            var userId = Convert.ToInt32(_userManager.GetUserId(User));


            Account account = new Account("kampanyanv2-com", "123456789", "123456789");
            Cloudinary cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.Url)
            };
            var uploadResult = cloudinary.Upload(uploadParams);

           var productImage = await _productImageService.UploadPhoto(image, userId, uploadResult.PublicId, uploadResult.SecureUri.ToString());


            return productImage;
        }



        [HttpDelete]
        [Route("/senin-kampanyan/fotograf-sil")]
        public async Task<bool> DeletePhoto([FromBody] ImageModel imageModel)
        {
            var userId = Convert.ToInt32(_userManager.GetUserId(User));
            var image = _productImageService.Where(x => x.Id == imageModel.Id).FirstOrDefault();
            if (image != null && userId == image.CreatedBy)
            {
                await _productImageService.RemoveAsync(image);
               

                Account account = new Account("kampanyanv2-com", "123456789", "123456789");
                Cloudinary cloudinary = new Cloudinary(account);
                var deleteResult = cloudinary.DeleteResources(image.CloudinaryPublicId);
            }
            else
            {
                throw new Exception();
            }




            return true;
        }

        [Route("urun-firsatlari/{id:int}/{*slug}", Name = "ProductRoute")]
        public async Task<IActionResult> Index(string? slug, int id)
        {
            var user = await _userManager.GetUserAsync(User);
            //var userId = _userManager.GetUserId(User);

            var userId = Convert.ToInt32(_userManager.GetUserId(User));
            var allProducts =await _productService.GetAllAsync();
            var product2 = _productService.Where(x => x.Id == 2).FirstOrDefault();

            ProductId = id;

            var model = new ProductModel();
            model.Product = await _productService.GetByIdAsync(id).ConfigureAwait(false);
            model.ProductReviews = await _productService.GetProductReviews(model.Product.Id, 1, 10).ConfigureAwait(false);
            model.ProductImages = model.Product.Images?.Select(x => x.Url);
            model.DateRelative = GetPrettyDate(model.Product.CreatedAt);

            if (model.ProductReviews is null || model.ProductReviews.Count() == 0)
            {
                model.ProductReviews = new List<ProductReviews>() { new ProductReviews() {
                    ProductId = id
                }};
            }
            else
            {
                foreach (var review in model.ProductReviews)
                {
                    review.DateRelative = GetPrettyDate(review.CreatedAt);
                }

            }

            var isLiked = _productLikesService.Where(x => x.UserId == userId).FirstOrDefault(x => x.ProductId == model.Product.Id);
            {
                if (isLiked != null)
                {
                    if (isLiked.Liked)
                    {
                        model.IsLiked = Models.isLiked.liked;
                    }
                    else if (!isLiked.Liked)
                    {
                        model.IsLiked = Models.isLiked.disliked;
                    }
                    else
                    {
                        model.IsLiked = Models.isLiked.unknown;
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> List()
        {
            var products = await _productService.GetProductsForHome(10).ConfigureAwait(false);
            var model = new List<ProductModel>();
            var userId = Convert.ToInt32(_userManager.GetUserId(User));

            await foreach (var product in products)
            {
                var p = new ProductModel()
                {
                    Product = product,
                    DateRelative = GetPrettyDate(product.CreatedAt),
                };

                var isLiked = _productLikesService.Where(x => x.UserId == userId).FirstOrDefault(x => x.ProductId == product.Id);
                {
                    if (isLiked != null)
                    {
                        if (isLiked.Liked)
                        {
                            p.IsLiked = Models.isLiked.liked;
                        }
                        else if (!isLiked.Liked)
                        {
                            p.IsLiked = Models.isLiked.disliked;
                        }
                        else
                        {
                            p.IsLiked = Models.isLiked.unknown;
                        }
                    }
                }

                var img = _productImageService.Where(x => x.ProductId == product.Id).FirstOrDefault();
                if (img != null)
                {
                    p.Product.Images.Add(img);
                }

                model.Add(p);

            }
            return View(model);
        }

        //// https://support.google.com/webmasters/thread/64043955/review-snippets-for-product-pages-with-pagination?hl=en
        //// https://developers.google.com/search/docs/advanced/ecommerce/pagination-and-incremental-page-loading#sequentially
        //public async Task<IActionResult> GetProductReviews(int productId, int page, int count)
        //{
        //    var model = await _productService.GetProductReviewsAsync(productId, page, count).ConfigureAwait(false);
        //    //await foreach (var review in model)
        //    //{
        //    //    review.DateRelative = GetPrettyDate(review.CreatedAt);
        //    //}
        //    return PartialView(model);
        //}

        public async Task<IActionResult> PostComment(int productId, string comment)
        {
            var user = await _userManager.GetUserAsync(User);
            ProductReviews review = new ProductReviews();
            review.ProductId = productId;
            review.Review = comment;
            review.Approved = false;
            review.Username = user.UserName;
            review.UserId = user.Id;
            review.Likes = 0;
            review.IpAddress = GlobalHelper.getLocalIp(Request);
            review.CreatedAt = DateTime.Now;

            var productReview = await _productReviewService.AddAsync(review);
           

            var reviews = await _productService.GetProductReviews(productId, 1, 10).ConfigureAwait(false);

            reviews.Insert(0, review);

            return PartialView("_GetProductReviews", reviews);
        }


        public string GetPrettyDate(DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return d.ToString("dd.MM.yyyy");
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "şimdi";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 dakika önce";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} dakika önce",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 saat önce";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} saat önce",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "Dün";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} gün önce",
                    dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} hafta önce",
                    Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

    }
}
// hebele hubele