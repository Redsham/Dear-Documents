using Character;
using Content;
using Gameplay;
using Gameplay.GameCycle;
using Gameplay.Items;
using Gameplay.Items.Citations;
using Gameplay.Persons;
using Gameplay.Persons.Interfaces;
using UI.Gameplay;
using UI.Gameplay.Dialogs;
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
            builder.RegisterComponentInHierarchy<ItemsDropper>();
            builder.RegisterComponentInHierarchy<CitationPrinter>();
            
            // Registering person services
            builder.Register<IDocumentBuilder, DocumentBuilder>(Lifetime.Singleton);
            builder.Register<IInconsistencyBuilder, InconsistencyBuilder>(Lifetime.Singleton);
            builder.Register<IReasonOfEntryBuilder, ReasonOfEntryBuilder>(Lifetime.Singleton);
            builder.Register<IPersonNameService, PersonNameService>(Lifetime.Singleton);
            builder.Register<IPersonBuilder, PersonBuilder>(Lifetime.Singleton);
            
            // Registering game cycle services
            builder.Register<GameStateMachine>(Lifetime.Singleton);
            builder.Register<IGameSession, GameSession>(Lifetime.Singleton);
            
            // Registering character
            builder.RegisterComponentInHierarchy<CharacterBehaviour>();
            
            // Registering UI components
            builder.RegisterComponentInHierarchy<DialogManager>();
            builder.RegisterComponentInHierarchy<CallNextButton>();
            
            // Registering game entry point
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}