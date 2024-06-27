using SiteDeals.Core.Model;
using SiteDeals.Core.Repositories;
using SiteDeals.Core.UnitOfWorks;
using sun.print.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Service.Service
{
    public class TagService : Service<Tag>
    {
        IGenericRepository<Tag> _repository;
        IUnitOfWork _unitOfWork;

        public TagService(IGenericRepository<Tag> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
