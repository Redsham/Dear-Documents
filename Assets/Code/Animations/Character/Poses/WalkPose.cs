using UnityEngine;

namespace Animations.Character.Poses
{
    public class WalkPose : CharacterPose
    {
        private readonly float m_WalkSpeed;
        
        public WalkPose(float walkSpeed) => m_WalkSpeed = walkSpeed;
        
        public override void ComputePose(float time)
        {
            float value = Mathf.Sin(time * m_WalkSpeed * 2.0f);
            Body = new CharacterLimb(
                new Vector3(0.0f, value * value * 0.3f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, value * value * Mathf.Sign(value) * 5.0f)
            );
            
            value                = Mathf.Sin(time * m_WalkSpeed * 1.0f);
            Head = new CharacterLimb(
                new Vector3(0.0f, value * value * 0.03f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, value * value * Mathf.Sign(value) * 5.0f)
            );
        }
    }
}