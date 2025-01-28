using System;

namespace Gameplay.GameCycle
{
    public interface IGameSession
    {
        #region Fields

        public uint Score    { get; set; }
        public uint Mistakes { get; set; }

        #endregion

        #region Events

        public event Action<int> OnScoreChanged;
        public event Action<int> OnMistakesChanged;

        #endregion
    }
}