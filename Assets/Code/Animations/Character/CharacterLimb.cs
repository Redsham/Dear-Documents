using UnityEngine;

namespace Animations.Character
{
    public struct CharacterLimb
    {
        public CharacterLimb(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
        
        
        public Vector3 Position;
        public Quaternion Rotation;
        
        
        public static CharacterLimb Blend(CharacterLimb a, CharacterLimb b, float t)
        {
            return new CharacterLimb(
                Vector3.Lerp(a.Position, b.Position, t),
                Quaternion.Slerp(a.Rotation, b.Rotation, t)
            );
        }
    }
}