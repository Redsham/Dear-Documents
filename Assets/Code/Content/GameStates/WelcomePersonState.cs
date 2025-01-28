using System.Threading;
using Character;
using Content.Person.Documents;
using Content.Person.ReasonsOfEntry;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Items;
using UI.Gameplay.Dialogs;
using VContainer;

namespace Content.GameStates
{
    public class WelcomePersonState : IGameState
    {
        [Inject] private ItemsDropper m_Dropper;
        [Inject] private DialogManager      m_DialogManager;
        [Inject] private CharacterBehaviour m_Character;
        
        public async UniTask<IGameState> Handle(CancellationToken cancellation)
        {
            // Welcome the person
            await m_DialogManager.ShowDialog(new[]
            {
                new DialogMessage(DialogSpeaker.Inspector, "Dear, documents!"),
                new DialogMessage(DialogSpeaker.Person, "Here you go."),
            });
            
            // Drop all items from the person
            await m_Dropper.DropAll(m_Character.Person);
            
            // Ask the person about the purpose of the visit
            await m_DialogManager.ShowDialog(new[]
            {
                new DialogMessage(DialogSpeaker.Inspector, "Purpose of visit?"),
                new DialogMessage(DialogSpeaker.Person, m_Character.Person.ReasonOfEntry.ToString()),
            });

            // Ask the person about the duration of the visit
            if (m_Character.Person.ReasonOfEntry.GetType() != typeof(ResidenceReason))
            {
                int days = m_Character.Person.GetDocument<EntryPermit>().Duration.Days;
                
                await m_DialogManager.ShowDialog(new[]
                {
                    new DialogMessage(DialogSpeaker.Inspector, "How long do you plan to stay?"),
                    new DialogMessage(DialogSpeaker.Person, days + " days."),
                });
            }
            
            await m_DialogManager.ShowDialog(new[]
            {
                new DialogMessage(DialogSpeaker.Inspector, "Wait."),
            });
            
            return new InspectPersonState();
        }
    }
}