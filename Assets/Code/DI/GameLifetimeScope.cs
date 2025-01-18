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
            builder.RegisterComponentInHierarchy<ItemsManager>();
            builder.RegisterComponentInHierarchy<ItemsMover>();
            builder.RegisterComponentInHierarchy<ItemsDropper>();
            
            builder.Register<IDocumentBuilder,      DocumentBuilder>(Lifetime.Singleton);
            builder.Register<IInconsistencyBuilder, InconsistencyBuilder>(Lifetime.Singleton);
            builder.Register<IReasonOfEntryBuilder, ReasonOfEntryBuilder>(Lifetime.Singleton);
            builder.Register<IPersonBuilder,        PersonBuilder>(Lifetime.Singleton);
        }
    }
}