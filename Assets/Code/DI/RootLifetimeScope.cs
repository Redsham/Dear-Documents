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
            builder.RegisterInstance(m_InputActions);
        }
    }
}