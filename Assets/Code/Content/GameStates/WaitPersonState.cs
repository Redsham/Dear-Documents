using System.Threading;
using Character;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Persons.Interfaces;
using UnityEngine;
using VContainer;

namespace Content.GameStates
{
    public class WaitPersonState : IGameState
    {
        [Inject] private CharacterBehaviour m_Character;
        [Inject] private IPersonBuilder     m_PersonBuilder;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            m_Character.transform.position = new Vector3(-6.0f, 0.0f, 0.0f);
            m_Character.gameObject.SetActive(false);
            
            // Wait for the player to press the space key
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || cancellation.IsCancellationRequested);

            m_Character.Person = m_PersonBuilder.Build();
            m_Character.gameObject.SetActive(true);
            await m_Character.Animator.GoTo(Vector2.zero);
            
            return new InspectPersonState();
        }
    }
}