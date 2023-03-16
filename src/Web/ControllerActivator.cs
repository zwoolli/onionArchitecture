using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Domain.Repositories;
using Presentation.Controllers;
using Persistance.Repositories;
using Services.Abstractions;
using Services;

namespace Web;

public sealed class ControllerActivator : IControllerActivator, IDisposable
{
    private readonly IDbConfiguration _dbConfiguration;

    public ControllerActivator(IDbConfiguration dBconfiguration) => this._dbConfiguration = dBconfiguration;
    public object Create(ControllerContext context)
    {
        return this.Create(context, context.ActionDescriptor.ControllerTypeInfo.AsType());
    }

    public ControllerBase Create(ControllerContext context, Type controllerType)
    {
        // Scoped services
        IDbConnector dbConnector = new DbConnector(this._dbConfiguration);

        IRepositoryManager repositoryManager = new RepositoryManager(
            new OwnerRepository(dbConnector),
            new AccountRepository(dbConnector),
            new UnitOfWork(dbConnector)
        );

        IServiceManager serviceManager = new ServiceManager(
            new OwnerService(repositoryManager),
            new AccountService(repositoryManager)
        );

        switch (controllerType.Name)
        {
            case nameof(OwnersController):
                return new OwnersController(serviceManager);

            case nameof(AccountsController):
                return new AccountsController(serviceManager);

            default:
            throw new InvalidOperationException($"Unknown controller {controllerType}.");
        }
    }

    public void Dispose()
    {
        Console.WriteLine("Disposing the controller activator!");
    }

    public void Release(ControllerContext context, object controller)
    {
        (controller as IDisposable)?.Dispose();
    }
}