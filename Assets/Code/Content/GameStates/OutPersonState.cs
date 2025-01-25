using System.Threading;
using Character;
using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Stamp;
using VContainer;

namespace Content.GameStates
{
    public class OutPersonState : IGameState
    {
        [Inject] private CharacterBehaviour m_Character;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            float direction = m_Character.Person.GetDocument<Passport>().DecisionOnEntry == DecisionOnEntry.Approved ? 1.0f : -1.0f;
            await m_Character.Animator.GoTo(new UnityEngine.Vector2(direction * 6.0f, 0.0f));
            return new WaitPersonState();
        }
    }
}