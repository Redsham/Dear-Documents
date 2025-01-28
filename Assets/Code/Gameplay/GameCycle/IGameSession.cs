using System;

namespace Gameplay.GameCycle
{
    public interface IGameSession
    {
        #region Fields

        public uint Score    { get; set; }
        /// <summary>
        /// Count of correct decisions
        /// </summary>
        public uint Corrects { get; set; }
        /// <summary>
        /// Count of wrong decisions
        /// </summary>
        public uint Mistakes { get; set; }

        #endregion

        #region Events

        public event Action<uint> OnScoreChanged;
        public event Action<uint> OnCorrectsChanged;
        public event Action<uint> OnMistakesChanged;

        #endregion
    }
}