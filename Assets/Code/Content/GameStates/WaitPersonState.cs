using System;
using System.Threading;
using Character;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using UI.Gameplay;
using UnityEngine;
using Utility;
using VContainer;

namespace Content.GameStates
{
    public class WaitPersonState : IGameState
    {
        [Inject] private CharacterBehaviour m_Character;
        [Inject] private CallNextButton     m_CallNextButton;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            // Reset the character position and disable it
            m_Character.transform.position = new Vector3(-6.0f, 0.0f, 0.0f);
            m_Character.gameObject.SetActive(false);
            
            // Show the call next button
            m_CallNextButton.Show();
            
            // Wait for the player to press the space key
            try { await EventWaiter.WaitUnityEvent(m_CallNextButton.OnClick, cancellationToken: cancellation); }
            catch (OperationCanceledException) { /* Ignore */ }
            
            // Transition to the next state
            return new EnterPersonState();
        }
    }
}