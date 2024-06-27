using SiteDeals.Core.Model;
using SiteDeals.Core.Repositories;
using SiteDeals.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Service.Service
{
    public class ProductReviewService : Service<ProductReviews>
    {
        private readonly IGenericRepository<ProductReviews> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ProductReviewService(IGenericRepository<ProductReviews> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
