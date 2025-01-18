using Gameplay.Items;
using Gameplay.Persons;
using Gameplay.Persons.Interfaces;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Registering items services
            builder.RegisterComponentInHierarchy<ItemsManager>();
            builder.RegisterComponentInHierarchy<ItemsMover>();
            builder.RegisterComponentInHierarchy<TableContainer>();
            builder.RegisterComponentInHierarchy<ItemsDropper>();
            
            // Registering person services
            builder.Register<IDocumentBuilder, DocumentBuilder>(Lifetime.Singleton);
            builder.Register<IInconsistencyBuilder, InconsistencyBuilder>(Lifetime.Singleton);
            builder.Register<IReasonOfEntryBuilder, ReasonOfEntryBuilder>(Lifetime.Singleton);
            builder.Register<IPersonNameService, PersonNameService>(Lifetime.Singleton);
            builder.Register<IPersonBuilder, PersonBuilder>(Lifetime.Singleton);
        }
    }
}