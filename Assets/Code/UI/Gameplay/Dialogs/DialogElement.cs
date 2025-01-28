using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace UI.Gameplay.Dialogs
{
    public class DialogElement : MonoBehaviour
    {
        public RectTransform RectTransform { get; private set; }
        public DialogSpeaker Speaker       { get; private set; }
        public DialogMessage Message
        {
            set
            {
                m_Text.text = value.Text;
                Speaker     = value.Speaker;
                Apply();
            }
        }
        
        
        [SerializeField] private Vector2 m_Padding = new(5.0f, 5.0f);
        [SerializeField] private TextMeshProUGUI m_Text;

        
        private void Awake() => RectTransform = (RectTransform)transform;
        private void Apply()
        {
            DialogManager manager       = GetComponentInParent<DialogManager>();
            Vector2         preferredValues = m_Text.GetPreferredValues(manager.MaxElementWidth - m_Padding.x * 2.0f, float.MaxValue);
            float width                     = Mathf.Min(manager.MaxElementWidth, preferredValues.x);

            RectTransform.sizeDelta        = new Vector2(width, preferredValues.y) + m_Padding * 2.0f;
            m_Text.rectTransform.sizeDelta = -m_Padding * 2.0f;
            
            m_Text.horizontalAlignment = Speaker == DialogSpeaker.Inspector
                                             ? HorizontalAlignmentOptions.Left
                                             : HorizontalAlignmentOptions.Right;
        }
        
        public async UniTask Show()
        {
            await LMotion.Create(0.0f, 1.0f, 0.3f)
                          .WithEase(Ease.OutCubic)
                          .Bind(time => RectTransform.localScale = Vector3.one * time);
        }
        public async UniTask Hide()
        {
            await LMotion.Create(0.0f, 1.0f, 0.2f)
                          .WithEase(Ease.OutCubic)
                          .Bind(time => RectTransform.localScale = Vector3.one * (1.0f - time));
        }
    }
}