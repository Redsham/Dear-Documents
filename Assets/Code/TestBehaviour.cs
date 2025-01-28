using Content.GameStates;
using Gameplay.GameCycle;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class TestBehaviour : MonoBehaviour
{
    [Inject] private GameStateMachine m_GameStateMachine;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
        
        if (Input.GetKeyDown(KeyCode.A))
            m_GameStateMachine.SetState(new GameOverState()).Forget();
    }
}