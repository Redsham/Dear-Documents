using Gameplay.Items.Renderers;
using Gameplay.Persons.Data;

namespace Gameplay.Items.Documents
{
    public abstract class DocumentItemRendererBase : TableItemRenderer
    {
        public Document Document { get; private set; }
        public Person Person => Document.Person;

        public virtual void AssignDocument(Document document)
        {
            if (Document != null)
                throw new System.Exception("Re-assigning document is not allowed");
            
            Document = document;
            OnDocumentAssigned();
        }
        public abstract void OnDocumentAssigned();
    }
    
    public abstract class DocumentItemRenderer<T> : DocumentItemRendererBase where T : Document
    {
        public new T Document => (T)base.Document;
        
        public override void AssignDocument(Document document)
        {
            if (document is not T typedDocument)
                throw new System.Exception($"Document is not of type {typeof(T)}");
            
            base.AssignDocument(document);
        }
    }
}