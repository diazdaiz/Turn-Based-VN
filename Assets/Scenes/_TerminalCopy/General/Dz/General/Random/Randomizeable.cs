namespace Dz.Random {
    public class Randomizable {
        public object Obj { get; set; }
        public float Weight { get; set; }

        public Randomizable(object obj, float weight) {
            Obj = obj;
            Weight = weight;
        }
    }
}