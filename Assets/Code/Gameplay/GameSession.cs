using System;
using Content.GameStates;
using Gameplay.GameCycle;
using VContainer;

namespace Gameplay
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
                OnScoreChanged.Invoke(value);
            }
        }
        private uint m_Score;

        public uint Corrects
        {
            get => m_Corrects;
            set
            {
                m_Corrects = value;
                OnCorrectsChanged.Invoke(value);
            }
        }
        private uint m_Corrects;

        public uint Mistakes
        {
            get => m_Mistakes;
            set
            {
                m_Mistakes = value;
                OnMistakesChanged.Invoke(value);
                
                // If the player makes 3 mistakes, the game is over
                if (m_Mistakes >= 3)
                    m_GameStateMachine.SetState(new GameOverState()).Forget();
            }
        }
        private uint m_Mistakes;
        
        
        public event Action<uint> OnScoreChanged    = delegate {  };
        public event Action<uint> OnCorrectsChanged = delegate { };
        public event Action<uint> OnMistakesChanged = delegate { };
    }
}