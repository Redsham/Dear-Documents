using Content.GameStates;
using Gameplay.GameCycle;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class TestBehaviour : MonoBehaviour
{
    [Inject] private GameStateManager m_GameStateManager;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
        
        if (Input.GetKeyDown(KeyCode.A))
            m_GameStateManager.SetState(new GameOverState()).Forget();
    }
}