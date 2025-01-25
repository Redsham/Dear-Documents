using UnityEngine;

namespace Character.Animations.Poses
{
    public class IdlePose : CharacterPose
    {
        public override void ComputePose(float time)
        {
            Body = new CharacterLimb(
                new Vector3(0.0f, Mathf.Sin(time) * 0.1f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, Mathf.Sin(time * 1.5f) * 2.0f)
            );
            
            Head = new CharacterLimb(
                new Vector3(0.0f, Mathf.Sin(time * 2.0f) * 0.02f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, Mathf.Sin(time * 2.0f) * 5.0f)
            );
        }
    }
}