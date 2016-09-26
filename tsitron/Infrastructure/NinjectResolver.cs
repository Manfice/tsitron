using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Web.Common;
using tsitron.Domain.Abstract;
using tsitron.Domain.Context;

namespace tsitron.Infrastructure
{
    public class NinjectResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectResolver() : this(new StandardKernel()) { }

        public NinjectResolver(IKernel ninjectKernel)
        {
            _kernel = ninjectKernel;
            AddBindings(_kernel);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        private void AddBindings(IKernel kernel)
        {
            kernel.Bind<ISellerRepository>().To<EfSellerRypository>().InRequestScope();
            kernel.Bind<IWebSellerRepository>().To<EfWebSellerRepository>().InRequestScope();
            kernel.Bind<IAdminRepository>().To<EfAdminRepository>().InRequestScope();
            kernel.Bind<IHomeRepository>().To<EfHomeRepository>().InRequestScope();
        }

        public void Dispose()
        {
            
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