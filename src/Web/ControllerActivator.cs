using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Domain.Repositories;
using Presentation.Controllers;
using Persistance.Repositories;
using Services;

namespace Web;

public sealed class ControllerActivator : IControllerActivator, IDisposable
{
    private readonly IDbConfiguration _dBConfiguration;

    public ControllerActivator(IDbConfiguration dBconfiguration) => this._dBConfiguration = dBconfiguration;
    public object Create(ControllerContext context)
    {
        return this.Create(context, context.ActionDescriptor.ControllerTypeInfo.AsType());
    }

    public ControllerBase Create(ControllerContext context, Type controllerType)
    {
        // Scoped services
        IUnitOfWork unitOfWork = new UnitOfWork(this._dBConfiguration);


        switch (controllerType.Name)
        {
            case nameof(OwnersController):
                return new OwnersController(
                    new OwnerService(
                        new OwnerRepository(unitOfWork)
                    )
                );

            case nameof(AccountsController):
                return new AccountsController(
                    new AccountService(
                        new AccountRepository(unitOfWork)
                    )
                );

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