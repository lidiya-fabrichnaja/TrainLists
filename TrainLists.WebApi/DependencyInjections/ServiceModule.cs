using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using TrainLists.Application.Contracts;

namespace TrainLists.WebApi.DependencyInjections
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AssignableTo<IService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}