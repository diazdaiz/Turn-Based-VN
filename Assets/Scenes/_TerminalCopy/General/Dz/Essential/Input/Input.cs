using UnityEngine;

namespace Dz.Input {
    public partial class Input : MonoBehaviour {
        public static Input Instance {
            get {
                return instance;
            }
        }
        static Input instance;

        public void Start() {
            if (instance == null) {
                instance = this;
            }
        }
    }
}
