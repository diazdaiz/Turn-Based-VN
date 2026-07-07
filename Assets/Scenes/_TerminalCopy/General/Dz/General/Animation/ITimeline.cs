namespace Dz.Animation {
    public interface ITimeline {
        void Play();
        void FixedUpdate(float dt);
        bool IsPlaying { get; }
    }
}