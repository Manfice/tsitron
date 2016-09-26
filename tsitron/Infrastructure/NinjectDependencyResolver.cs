using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using tsitron.Domain.Abstract;
using tsitron.Domain.Context;

namespace tsitron.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<IUserRepository>().To<EfUsersRepository>().InRequestScope();
            _kernel.Bind<ISellerRepository>().To<EfSellerRypository>();
            _kernel.Bind<IGoodsRepository>().To<EfGoodsRepository>();
            _kernel.Bind<IAdminRepository>().To<EfAdminRepository>();
            _kernel.Bind<IHomeRepository>().To<EfHomeRepository>();
            _kernel.Bind<ICartRepository>().To<EfCartRepository>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}