using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private InputActionAsset m_InputActions;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register the InputActionAsset instance
            builder.RegisterInstance(m_InputActions);
        }
    }
}