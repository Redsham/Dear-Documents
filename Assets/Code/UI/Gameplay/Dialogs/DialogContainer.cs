using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Dialogs;
using UnityEngine;
using VContainer;

namespace UI.Gameplay.Dialogs
{
    public class DialogContainer : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }

        public float MaxElementWidth => (RectTransform.rect.width - ColumnsSpacing) / 2.0f;
        public float ColumnsSpacing  => m_ColumnsSpacing;
        public float RowsSpacing     => m_RowsSpacing;

        [SerializeField] private float m_ColumnsSpacing = 10.0f;
        [SerializeField] private float m_RowsSpacing    = 5.0f;

        [Header("Pool")] [SerializeField] private DialogElement m_ElementPrefab;
        [SerializeField]                  private int           m_PoolSize = 5;

        private readonly List<DialogElement>  m_ActiveElements = new();
        private readonly Queue<DialogElement> m_ElementsPool   = new();

        private DialogManager m_Manager;
        private bool          m_DialogInProgress;


        [Inject]
        public void Construct(DialogManager manager)
        {
            m_Manager = manager;

            manager.OnMessageAdded += _ =>
            {
                if (m_DialogInProgress)
                    return;
                
                Dialog().Forget();
            };
        }

        #region Unity Methods

        private void Awake() => RectTransform = (RectTransform)transform;
        private void Start() => Prewarm();
        private void Update()
        {
            if (m_ActiveElements.Count == 0)
                return;

            UpdateTransforms();
        }

        #endregion
        
        #region Pool

        private void Prewarm()
        {
            #if UNITY_EDITOR

            if (!Application.isPlaying)
                return;

            #endif

            for (int i = 0; i < m_PoolSize; i++)
            {
                DialogElement element = Instantiate(m_ElementPrefab, transform);
                element.gameObject.SetActive(false);

                m_ElementsPool.Enqueue(element);
            }
        }
        private DialogElement GetElement()
        {
            DialogElement element = m_ElementsPool.Count > 0
                                        ? m_ElementsPool.Dequeue()
                                        : Instantiate(m_ElementPrefab, transform);
            
            element.RectTransform.SetAsLastSibling();
            element.gameObject.SetActive(true);

            m_ActiveElements.Add(element);
            return element;
        }
        private void ReturnElement(DialogElement element)
        {
            element.gameObject.SetActive(false);

            m_ActiveElements.Remove(element);
            m_ElementsPool.Enqueue(element);
        }

        #endregion

        private void UpdateTransforms()
        {
            float positionY = 0.0f;

            for (int i = m_ActiveElements.Count - 1; i >= 0; i--)
            {
                DialogElement element     = m_ActiveElements[i];
                RectTransform elementRect = element.RectTransform;

                float side = element.Speaker == DialogSpeaker.Inspector ? 0.0f : 1.0f;

                elementRect.anchorMin = elementRect.anchorMax = new Vector2(side, 0.0f);
                elementRect.pivot     = new Vector2(side, 0.0f);

                elementRect.anchoredPosition =  new Vector2(0.0f, positionY);
                positionY                    += elementRect.rect.height * elementRect.localScale.y + RowsSpacing;
            }
        }
        
        private async UniTask Dialog()
        {
            m_DialogInProgress = true;

            while (m_Manager.HasMessages() || m_ActiveElements.Count > 0)
            {
                if (m_Manager.HasMessages())
                {
                    DialogMessage message = m_Manager.PopMessage();
                    ShowMessage(message).Forget();
                    await UniTask.WaitForSeconds(0.2f);
                }
                else
                    await UniTask.Yield();
            }
            
            m_DialogInProgress = false;
        }
        private async UniTask ShowMessage(DialogMessage message)
            {
                DialogElement element = GetElement();
                element.Message = message;

                await element.Show();
                await UniTask.WaitForSeconds(2.0f);
                await element.Hide();
                
                ReturnElement(element);
            }
    }
}