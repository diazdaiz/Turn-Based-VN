public partial class RelicsRandomizer : GeneralObjectsRandomizer {
    public static RelicsRandomizer Instance;

    public override object Get() {
        return Instantiate(base.Get() as Relic);
    }

    public override void Start() {
        Instance = this;
        RefreshPool();
    }
}