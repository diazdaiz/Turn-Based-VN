using UnityEngine;

public partial class PotionsRandomizer : GeneralObjectsRandomizer {
    public static PotionsRandomizer Instance;

    public override object Get() {
        return Instantiate(base.Get() as Potion);
    }

    public override void Start() {
        Instance = this;
        RefreshPool();

        Debug.Log(Get().GetType().Name);
    }
}
