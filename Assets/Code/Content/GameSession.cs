using System;
using Content.GameStates;
using Gameplay.GameCycle;
using VContainer;

namespace Content
{
    public class GameSession : IGameSession
    {
        [Inject] private readonly GameStateMachine m_GameStateMachine;
        
        
        public uint Score
        {
            get => m_Score;
            set
            {
                m_Score = value;
                OnScoreChanged.Invoke((int)value);
            }
        }
        public uint Mistakes
        {
            get => m_Mistakes;
            set
            {
                m_Mistakes = value;
                OnMistakesChanged.Invoke((int)value);
                
                // If the player makes 3 mistakes, the game is over
                if (m_Mistakes >= 3)
                    m_GameStateMachine.SetState(new GameOverState()).Forget();
            }
        }
        
        public event Action<int> OnScoreChanged    = delegate {  };
        public event Action<int> OnMistakesChanged = delegate { };
        
        private uint m_Score;
        private uint m_Mistakes;
    }
}