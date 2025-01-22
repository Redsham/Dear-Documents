using Gameplay.Persons.Data;

namespace Gameplay.Items.Documents
{
    public class DocumentBehaviourBase : ItemBehaviour
    {
        public Document Document { get; private set; }

        public void AssignDocument(Document document)
        {
            Document = document;

            if (this is IOnDocumentAssigned onDocumentAssigned)
                onDocumentAssigned.OnDocumentAssigned(document);

            if (TableRenderer is DocumentItemRendererBase renderer)
                renderer.AssignDocument(document);
        }
    }
    
    
    public interface IOnDocumentAssigned
    {
        void OnDocumentAssigned(Document document);
    }
}