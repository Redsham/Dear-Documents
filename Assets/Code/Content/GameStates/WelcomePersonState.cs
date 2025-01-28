using System;
using System.Threading;
using Character;
using Content.Person.Documents;
using Content.Person.ReasonsOfEntry;
using Cysharp.Threading.Tasks;
using Gameplay.GameCycle;
using Gameplay.Items;
using UI.Gameplay.Dialogs;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using Utility;
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
            StringTable dialogsTable = await LocalizationSettings.StringDatabase.GetTableAsync("Dialogs");
            StringTable reasonsOfEntryTable = await LocalizationSettings.StringDatabase.GetTableAsync("ReasonsOfEntry");
            
            // Welcome the person
            await m_DialogManager.ShowDialog(new[]
            {
                new DialogMessage(DialogSpeaker.Inspector, dialogsTable.GetEntry("greeting_request_documents").GetLocalizedString()),
                new DialogMessage(DialogSpeaker.Person, dialogsTable.GetEntry("greeting_provide_documents").GetLocalizedString()),
            });
            
            // Drop all items from the person
            await m_Dropper.DropAll(m_Character.Person);
            
            // Ask the person about the purpose of the visit
            Type reasonType = m_Character.Person.ReasonOfEntry.GetType();
            string reasonKey = TextUtils.CamelToSnake(reasonType.Name.Replace("Reason", ""));
            await m_DialogManager.ShowDialog(new[]
            {
                new DialogMessage(DialogSpeaker.Inspector, dialogsTable.GetEntry("ask_reason_of_entry").GetLocalizedString()),
                new DialogMessage(DialogSpeaker.Person, reasonsOfEntryTable.GetEntry(reasonKey).GetLocalizedString()),
            });

            // Ask the person about the duration of the visit
            if (reasonType != typeof(ResidenceReason))
            {
                int days = m_Character.Person.GetDocument<EntryPermit>().Duration;
                await m_DialogManager.ShowDialog(new[]
                {
                    new DialogMessage(DialogSpeaker.Inspector, dialogsTable.GetEntry("ask_duration_of_stay").GetLocalizedString()),
                    new DialogMessage(DialogSpeaker.Person, dialogsTable.GetEntry("person_duration_response").GetLocalizedString(days)),
                });
            }
            
            await m_DialogManager.ShowDialog(new[]
            {
                new DialogMessage(DialogSpeaker.Inspector, dialogsTable.GetEntry("wait_inspecting").GetLocalizedString()),
            });
            
            return new InspectPersonState();
        }
    }
}