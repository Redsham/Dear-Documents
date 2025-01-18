using Gameplay.Persons.Data;

namespace Gameplay.Items.Documents
{
    public class DocumentBehaviour : ItemBehaviour
    {
        public Document Document { get; private set; }

        public void AssignDocument(Document document)
        {
            Document = document;

            if (this is IOnDocumentAssigned onDocumentAssigned)
                onDocumentAssigned.OnDocumentAssigned(document);

            if (TableRenderer is DocumentItemRenderer<Document> renderer)
                renderer.AssignDocument(document);
        }
    }
    
    public interface IOnDocumentAssigned
    {
        void OnDocumentAssigned(Document document);
    }
}