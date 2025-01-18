using Cysharp.Threading.Tasks;
using Gameplay.Items;
using Gameplay.Persons.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class TestBehaviour : MonoBehaviour
{
    [Inject]
    public void Construct(ItemsManager manager, IPersonBuilder personBuilder, ItemsDropper dropper)
    {
        dropper.DropAll(personBuilder.Build()).Forget();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}