using System;
using System.Collections.Generic;

namespace Gameplay.Dialogs
{
    public class DialogManager
    {
        public event Action<DialogMessage> OnMessageAdded = delegate { };
        public event Action<DialogMessage> OnMessageRemoved = delegate { };
        
        private readonly Queue<DialogMessage> m_Messages = new();


        public void AddMessage(DialogMessage message)
        {
            m_Messages.Enqueue(message);
            OnMessageAdded.Invoke(message);
        }
        public DialogMessage PopMessage()
        {
            DialogMessage message = m_Messages.Dequeue();
            OnMessageRemoved.Invoke(message);
            return message;
        }
        public bool HasMessages() => m_Messages.Count > 0;
    }
}