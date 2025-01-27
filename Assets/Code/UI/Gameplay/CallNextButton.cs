using Cysharp.Threading.Tasks;
using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class CallNextButton : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick => m_OnClick;
        
        [Header("Components")]
        [SerializeField] private Image           m_Body;
        [SerializeField] private Image           m_Icon;
        [SerializeField] private TextMeshProUGUI m_Text;

        [Header("Colors")] 
        [SerializeField] private Color m_DefaultTextColor;
        [SerializeField] private Color m_ClickedTextColor;
        
        [Header("Localization")]
        [SerializeField] private LocalizedString m_UnclickedText;
        [SerializeField] private LocalizedString m_ClickedText;
        
        [Space]
        [SerializeField] private UnityEvent m_OnClick;

        private bool m_IsClicked;
        private MotionHandle m_MotionHandle;


        private void Awake()
        {
            LocalizationSettings.SelectedLocaleChanged += _ => RefreshLocalizable().Forget();
            RefreshLocalizable().Forget();
        }
        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= _ => RefreshLocalizable();
        }

        private async UniTask RefreshLocalizable()
        {
            await LocalizationSettings.InitializationOperation;
            
            m_Text.text = await (m_IsClicked
                               ? m_ClickedText.GetLocalizedStringAsync()
                               : m_UnclickedText.GetLocalizedStringAsync());
        }
        
        public void Show()
        {
            m_Text.text = m_UnclickedText.GetLocalizedString();
            gameObject.SetActive(true);

            #region Body

            Color bodyColor      = m_Body.color;
            Color startBodyColor = bodyColor;
            startBodyColor.a = 0.0f;

            MotionHandle bodyHandle = LMotion.Create(0.0f, 1.0f, 0.2f)
                                             .WithEase(Ease.InOutSine)
                                             .Bind(time =>
                                             {
                                                 m_Body.color = Color.Lerp(startBodyColor, bodyColor, time);
                                             });

            #endregion

            #region Icon
            
            Vector2 iconPosition      = m_Icon.rectTransform.anchoredPosition;
            Vector2 startIconPosition = iconPosition + new Vector2(0.0f, 100.0f);

            MotionHandle iconHandle = LMotion.Create(0.0f, 1.0f, 0.5f)
                                             .WithEase(Ease.OutElastic)
                                             .Bind(time =>
                                             {
                                                 m_Icon.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
                                                 m_Icon.rectTransform.anchoredPosition = Vector2.Lerp(startIconPosition, iconPosition, time);
                                                 m_Text.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(90.0f, 0.0f, time));
                                             });

            #endregion

            #region Text

            Color startTextColor = m_DefaultTextColor;
            startTextColor.a = 0.0f;
            
            Vector2 textPosition      = m_Text.rectTransform.anchoredPosition;
            Vector2 startTextPosition = textPosition + new Vector2(0.0f, 100.0f);
            
            MotionHandle textHandle = LMotion.Create(0.0f, 1.0f, 0.3f)
                                             .Bind(time =>
                                             {
                                                 float expoTime    = EaseUtility.InExpo(time);
                                                 float elasticTime = EaseUtility.OutBounce(time);
                                                 
                                                 m_Text.color = Color.Lerp(startTextColor, m_DefaultTextColor, expoTime);
                                                 m_Text.rectTransform.anchoredPosition = Vector2.Lerp(startTextPosition, textPosition, elasticTime);
                                                 m_Text.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(90.0f, 0.0f, elasticTime));
                                             });

            #endregion

            m_MotionHandle = LSequence.Create()
                                   .Append(bodyHandle)
                                   .Insert(0.1f, iconHandle)
                                   .Insert(0.2f, textHandle)
                                   .Run();
        }
        public void Hide()
        {
            m_MotionHandle.TryComplete();
            
            Color bodyColor    = m_Body.color;
            Color endBodyColor = bodyColor;
            endBodyColor.a = 0.0f;
            
            Color startTextColor = m_DefaultTextColor;
            Color endTextColor   = m_ClickedTextColor;

            Color iconColor = m_Icon.color;
            Color endIconColor = iconColor;
            endIconColor.a = 0.0f;

            Vector2 startTextPosition = m_Text.rectTransform.anchoredPosition;
            Vector2 endTextPosition = startTextPosition + new Vector2(0.0f, 100.0f);

            m_MotionHandle = LMotion.Create(0.0f, 1.0f, 0.5f)
                                    .WithOnComplete(() =>
                                    {
                                        // Reset colors
                                        m_Body.color = bodyColor;
                                        m_Icon.color = iconColor;

                                        // Reset text
                                        m_Text.color = m_DefaultTextColor;
                                        m_Text.rectTransform.anchoredPosition = startTextPosition;
                                        
                                        // Disable gameobject
                                        gameObject.SetActive(false);
                                        m_IsClicked = false;
                                    })
                                    .Bind(time =>
                                    {
                                        float outExpo = EaseUtility.OutExpo(time);
                                        m_Body.color = Color.Lerp(bodyColor, endBodyColor, outExpo);
                                        m_Icon.color = Color.Lerp(iconColor, endIconColor, outExpo);

                                        float textTime = EaseUtility.InExpo(Mathf.Abs(time - 0.2f) / 0.8f);
                                        startTextColor.a = endTextColor.a = 1.0f - textTime;
                                        m_Text.color = Color.Lerp(startTextColor, endTextColor, Mathf.Sin(time * Mathf.PI * 4.0f) * 0.5f + 0.5f);
                                        m_Text.rectTransform.anchoredPosition = Vector2.Lerp(startTextPosition, endTextPosition, textTime);
                                    });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Prevent multiple clicks
            if (m_IsClicked) return;
            
            m_IsClicked = true;
            
            m_Text.text = m_ClickedText.GetLocalizedString();
            m_OnClick.Invoke();
            
            Hide();
        }
    }
}