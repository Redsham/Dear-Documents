using System.Threading;
using Character;
using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Items;
using Gameplay.Stamp;
using VContainer;

namespace Content.GameStates
{
    public class InspectPersonState : IGameState, IOnStateEnter, IOnStateExit
    {
        [Inject] private CharacterBehaviour m_Character;
        [Inject] private ItemsDropper m_Dropper;
        [Inject] private ItemsMover m_Mover;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            await m_Dropper.DropAll(m_Character.Person);
            await UniTask.WaitUntil(() => m_Dropper.AllReturned, cancellationToken: cancellation);

            return new OutPersonState();
        }

        public void OnStateEnter()
        {
            m_Mover.CanReturnItem = _ =>
            {
                Passport passport = m_Dropper.GetDocument<Passport>();
                return passport is not { DecisionOnEntry: DecisionOnEntry.None };
            };
            m_Mover.OnReturnItem += m_Dropper.ReturnItem;
        }
        public void OnStateExit()
        {
            m_Mover.CanReturnItem = _ => false;
            m_Mover.OnReturnItem -= m_Dropper.ReturnItem;
        }
    }
}