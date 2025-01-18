using Gameplay.Items.Renderers;
using Gameplay.Persons.Data;

namespace Gameplay.Items.Documents
{
    public abstract class DocumentItemRenderer : TableItemRenderer
    {
        public Document Document { get; private set; }

        public void AssignDocument(Document document)
        {
            Document = document;
            OnDocumentAssigned();
        }
        public abstract void OnDocumentAssigned();
    }
}