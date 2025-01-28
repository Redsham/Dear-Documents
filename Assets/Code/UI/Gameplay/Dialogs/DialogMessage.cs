namespace UI.Gameplay.Dialogs
{
    public readonly struct DialogMessage
    {
        public DialogMessage(DialogSpeaker speaker, string text)
        {
            Speaker = speaker;
            Text    = text;
        }
        
        
        public readonly DialogSpeaker Speaker;
        public readonly string        Text;
    }
}