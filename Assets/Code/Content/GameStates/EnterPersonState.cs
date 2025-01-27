using System.Threading;
using Character;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Persons.Interfaces;
using UnityEngine;
using VContainer;

namespace Content.GameStates
{
    public class EnterPersonState : IGameState
    {
        [Inject] private CharacterBehaviour m_Character;
        [Inject] private IPersonBuilder     m_PersonBuilder;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            // Generate a new person and enable the character
            m_Character.Person = m_PersonBuilder.Build();
            m_Character.gameObject.SetActive(true);
            
            // Move the character to the center of the screen
            await m_Character.Animator.GoTo(Vector2.zero, cancellation);
            
            // Transition to the next state
            return new InspectPersonState();
        }
    }
}