using System.Collections.Generic;

namespace Dz.Animation {
    public class Keyframe {
        /// <summary>
        /// in second
        /// </summary>
        public float Position => position;
        public List<float> Floats => floats;
        public KeyframeTransition Transition => transition;

        float position;
        List<float> floats;
        public enum KeyframeTransition { Linear }
        KeyframeTransition transition;

        public Keyframe(List<float> floats, KeyframeTransition transition = KeyframeTransition.Linear) {
            this.floats = floats;
            this.transition = transition;
        }

    }
}