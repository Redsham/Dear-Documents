namespace Gameplay.Items.Citations
{
    public class CitationBehaviour : ItemBehaviour
    {
        public CitationType CitationType
        {
            get => m_CitationType;
            set
            {
                m_CitationType = value;
                ((CitationTableRenderer)TableRenderer).SetType(value).Forget();
            }
        }
        private CitationType m_CitationType;
    }
}