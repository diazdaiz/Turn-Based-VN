using System.Collections.Generic;

namespace Dz.Animation {
    public class AnimationManager {
        readonly List<Timeline> timelines = new();

        public void Play(Timeline timeline) {
            if (timeline == null) return;
            timeline.Play();
            timelines.Add(timeline);
        }

        public void FixedUpdate(float dt) {
            for (int i = 0; i < timelines.Count; i++) {
                if (!timelines[i].IsPlaying) return;
                timelines[i].FixedUpdate(dt);
            }
        }
    }
}
