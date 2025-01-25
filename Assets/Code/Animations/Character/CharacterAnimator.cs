using Animations.Character.Poses;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

namespace Animations.Character
{
    [ExecuteAlways]
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private float m_PoseTransitionDuration = 0.1f;
        [SerializeField] private float m_WalkSpeed = 1.0f;
        [SerializeField] private Vector2 m_TestPosition;
        
        [Header("Parts")]
        [SerializeField] private Transform m_Body;
        [SerializeField] private Transform m_Head;
        
        private Vector3 m_BodyPosition;
        private Vector3 m_HeadPosition;
        
        private CharacterPose m_IdlePose;
        private CharacterPose m_WalkPose;
        
        private CharacterPose Pose
        {
            get => m_Pose;
            set
            {
                if (m_Pose == value)
                    return;
                
                if(m_PoseTransition > 0.1f)
                    m_PrevPose = m_Pose;
                
                m_Pose     = value;
                m_PoseTransition = 0.0f;
            }
        }
        private CharacterPose m_Pose;
        private CharacterPose m_PrevPose;
        private float         m_PoseTransition;
        
        private void Awake()
        {
            m_BodyPosition = m_Body.localPosition;
            m_HeadPosition = m_Head.localPosition;
        }
        private void Start()
        {
            CreatePoses();
            m_Pose = m_IdlePose;
        }
        private void Update()
        {
            m_PoseTransition += Time.deltaTime / m_PoseTransitionDuration;
            m_PoseTransition = Mathf.Clamp01(m_PoseTransition);

            UpdatePose();
        }

        private void CreatePoses()
        {
            m_IdlePose = new IdlePose();
            m_WalkPose = new WalkPose(m_WalkSpeed);
        }
        private void UpdatePose()
        {
            if (m_Pose == null)
                return;

            float time = Time.time;
            bool blend = m_PoseTransition < 1.0f && m_PrevPose != null;

            CharacterLimb body;
            CharacterLimb head;
            
            // Compute current pose
            m_Pose.ComputePose(time);
            
            // Blend between previous and current pose
            if (blend)
            {
                m_PrevPose.ComputePose(time);
                body = CharacterLimb.Blend(m_PrevPose.Body, m_Pose.Body, m_PoseTransition);
                head = CharacterLimb.Blend(m_PrevPose.Head, m_Pose.Head, m_PoseTransition);
            }
            else
            {
                body = m_Pose.Body;
                head = m_Pose.Head;
            }
            
            
            // Apply pose to parts
            m_Body.localPosition = m_BodyPosition + body.Position;
            m_Body.localRotation = body.Rotation;
            
            m_Head.localPosition = m_HeadPosition + head.Position;
            m_Head.localRotation = head.Rotation;
        }
        
        public async UniTask Walk(Vector2 endPosition)
        {
            Vector2 startPosition = transform.position;
            
            float distance = Vector2.Distance(startPosition, endPosition);
            float duration = distance / m_WalkSpeed;
            
            Pose = m_WalkPose;
            
            await LMotion.Create(0.0f, 1.0f, duration)
                         .WithEase(Ease.InOutSine)
                         .WithOnCancel(() => Pose           = m_IdlePose)
                         .WithOnComplete(() => Pose         = m_IdlePose)
                         .Bind((time) => transform.position = Vector2.Lerp(startPosition, endPosition, time)).ToUniTask();
        }
    }
}