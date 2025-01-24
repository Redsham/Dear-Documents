using Animations.Character;
using Content.Person.Documents;
using Cysharp.Threading.Tasks;
using Gameplay.Items;
using Gameplay.Persons.Data;
using Gameplay.Persons.Interfaces;
using Gameplay.Stamp;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class TestBehaviour : MonoBehaviour
{
    [SerializeField] private CharacterAnimator m_Character;
    
    private Person m_Person;
    private bool   m_IsInspected = true;
    
    private ItemsDropper   m_Dropper;
    private IPersonBuilder m_PersonBuilder;
    
    
    [Inject]
    public void Construct(ItemsMover mover, ItemsDropper dropper, IPersonBuilder personBuilder)
    {
        mover.CanReturnItem = _ =>
        {
            Passport passport = dropper.GetDocument<Passport>();
            return passport is not { DecisionOnEntry: DecisionOnEntry.None };
        };
        mover.OnReturnItem += dropper.ReturnItem;
        
        m_Dropper       = dropper;
        m_PersonBuilder = personBuilder;
    }

    private async UniTask NextPerson()
    {
        m_IsInspected                  = false;
        m_Person                       = m_PersonBuilder.Build();
        m_Character.transform.position = new Vector3(-6.0f, 0.0f, 0.0f);
        
        await m_Character.Walk(new Vector2(0.0f, 0.0f));
        await m_Dropper.DropAll(m_Person);
        await UniTask.WaitUntil(() => m_Dropper.AllReturned);
        await UniTask.WaitForSeconds(0.1f);

        float direction = m_Person.GetDocument<Passport>().DecisionOnEntry == DecisionOnEntry.Approved ? 1.0f : -1.0f;
        await m_Character.Walk(new Vector2(6.0f * direction, 0.0f));
        m_IsInspected = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
        
        if(Input.GetKeyDown(KeyCode.A) && m_IsInspected)
            NextPerson().Forget();
    }
}