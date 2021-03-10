namespace Game1.GameManagment.Assets
{
    public enum AnimationEffects
    {
        None = 0,
        Looping = 1,
        BackAndForth = 2,
        BackWard = 3
    }


    public interface IAnimation : IDrawableSheet
    {
        bool AnimationEnded { get; }

        float FrameTime { get; set; }

        AnimationEffects AnimationEffect { get; set; }

        bool Paused { get; set; }
    }
}
