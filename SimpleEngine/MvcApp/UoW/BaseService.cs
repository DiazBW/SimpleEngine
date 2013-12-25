using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.UoW
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            _unitOfWork = unitOfWork;
        }

        protected IGameRepository GameRepository
        {
            get
            {
                return _unitOfWork.GameRepository;
            }
        }
    }
}