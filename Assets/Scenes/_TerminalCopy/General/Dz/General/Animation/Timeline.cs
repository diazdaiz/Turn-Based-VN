using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dz.Animation {
    public partial class Timeline : ITimeline {
        public bool IsPlaying { get; private set; }
        public List<Keyframe> Keyframes { get; set; }
        float timer;

        public Timeline(List<Keyframe> keyframes) {
            Keyframes = keyframes;
        }

        public void AddKeyframe(float atSecond, List<float> keyframe, Keyframe.KeyframeTransition keyframeTransition = Keyframe.KeyframeTransition.Linear) {
            if (IsPlaying) {
                Debug.LogWarning("cancelling keyframe add, don't add keyframe when playing the animation");
                return;
            }
            Keyframes.Add(new Keyframe(keyframe, keyframeTransition));
            Keyframes = Keyframes.OrderBy(kv => kv.Position).ToList();
        }

        public void Play() {
            timer = 0f;
            IsPlaying = true;
        }

        public void Finish() {
            timer = Keyframes[^1].Position;
            IsPlaying = false;
        }

        public void FixedUpdate(float dt) {
            if (!IsPlaying) { return; }
            timer += dt;
            if (timer >= Keyframes[^1].Position) {
                Finish();
            }
        }

        public List<float> GetAnimatedValues() {
            if (Keyframes == null || Keyframes.Count == 0) return null;
            if (Keyframes.Count == 1) return Keyframes[0].Floats;

            for (int i = 0; i < Keyframes.Count - 1; i++) {
                if (!(Keyframes[i].Position > timer && timer < Keyframes[i + 1].Position)) continue;
                return InterpolateKeyframe(Keyframes[i], Keyframes[i + 1], Keyframes[i].Transition);
            }
            return Keyframes[^1].Floats;
        }

        List<float> InterpolateKeyframe(Keyframe fromKeyframe, Keyframe toKeyframe, Keyframe.KeyframeTransition transition) {
            float t = (timer - fromKeyframe.Position) / (toKeyframe.Position - fromKeyframe.Position);
            List<float> result = new(fromKeyframe.Floats); //biar banyaknya sama

            for (int i = 0; i < result.Count; i++) {
                float from = fromKeyframe.Floats[i];
                float to = toKeyframe.Floats[i];
                if (transition == Keyframe.KeyframeTransition.Linear) result[i] = (to - from) * t;
            }

            return result;
        }
    }
}