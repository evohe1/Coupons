using java.io;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SiteDeals.Core.Model;
using SiteDeals.Core.Repositories;
using SiteDeals.Core.UnitOfWorks;
using SiteDeals.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;
using SiteDeals.MVCWebUI.Areas.Identity.Data;
using System.Security.Claims;
using OpenQA.Selenium.DevTools.V125.FedCm;
using CloudinaryDotNet;

namespace SiteDeals.Service.Service
{
    public class ProductImageService : Service<ProductImages>
    {
        private IGenericRepository<ProductImages> _repository;
        private IUnitOfWork _unitOfWork;
       
        public ProductImageService(IGenericRepository<ProductImages> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductImages> UploadPhoto([FromBody] ImageModel image, int userId ,string publicId ,string secureUrl)
        {

            var productImage = new ProductImages()
            {
                CloudinaryPublicId = publicId,
                ProductId = null,
                Url = secureUrl,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            _repository.AddAsync(productImage);
            _unitOfWork.CommitAsync();
           


            return productImage;
        }


    }
}
