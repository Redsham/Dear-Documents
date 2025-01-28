using System.Threading;
using Character;
using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Items.Citations;
using Gameplay.Stamp;
using UnityEngine;
using VContainer;

namespace Content.GameStates
{
    public class ExitPersonState : IGameState
    {
        [Inject] private CharacterBehaviour m_Character;
        [Inject] private CitationPrinter    m_CitationPrinter;
        [Inject] private IGameSession       m_GameSession;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            DecisionOnEntry decision = m_Character.Person.GetDocument<Passport>().DecisionOnEntry;
            
            // Move the character to the exit
            float direction = decision == DecisionOnEntry.Approved ? 1.0f : -1.0f;
            await m_Character.Animator.GoTo(new Vector2(direction * 6.0f, 0.0f), cancellation);
            
            // Check if the character has a correct stamp
            bool isCorrect = decision == (m_Character.Person.Inconsistency != null ? DecisionOnEntry.Denied : DecisionOnEntry.Approved);
            if (!isCorrect)
            {
                m_GameSession.Mistakes++;
                m_CitationPrinter.Print((CitationType)(m_GameSession.Mistakes - 1));
            }
            else
            {
                m_GameSession.Score += 10;
            }
                             
            // Transition to the next state
            return new WaitPersonState();
        }
    }
}