using Cysharp.Threading.Tasks;
using Gameplay.Items;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class TestBehaviour : MonoBehaviour
{
    private Person m_Person;
    private ItemsDropper m_Dropper;
    private bool m_IsDropped;
    
    [Inject]
    public void Construct(ItemsManager manager, IPersonBuilder personBuilder, ItemsDropper dropper)
    {
        m_Dropper = dropper;
        m_Person = personBuilder.Build();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
        
        if (Input.GetKeyDown(KeyCode.A) && !m_IsDropped)
        {
            m_Dropper.DropAll(m_Person).Forget();
            m_IsDropped = true;
        }
    }
}