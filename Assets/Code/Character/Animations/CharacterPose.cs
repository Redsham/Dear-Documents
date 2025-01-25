namespace Character.Animations
{
    public abstract class CharacterPose
    {
        public CharacterLimb Body { get; protected set; }
        public CharacterLimb Head { get; protected set; }
        
        public abstract void ComputePose(float time);
    }
}