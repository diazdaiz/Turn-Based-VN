
using UnityEngine;

public partial class RedLouse : Enemy {
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite enemyTexture2D;
    [SerializeField] Sprite curlUpTexture2D;

    public override void GenerateIntentions() {
        base.GenerateIntentions();
        possibleIntentions = new() {
            new Intention.Attack(5, 7),
            new Intention.Buff(new Status.Strength(this, 3))
        };
        int rng = Dz.Random.Randomizer.Range(0, possibleIntentions.Count);
        intention = possibleIntentions[rng];
    }

    public override void Start() {
        initialStatuses = new() {
            { typeof(Status.CurlUp) ,new Status.CurlUp(this, 3, 7) }
        };
        CombatAction.OnTriggersFirst[typeof(CombatAction.EnemiesTurn)] += OnStartTurn;
    }

    public override void OnDestroy() {
        CombatAction.OnTriggersFirst[typeof(CombatAction.EnemiesTurn)] -= OnStartTurn;
    }

    public override void OnDamaged(CombatAction combatAction) {
        if (combatAction is CombatAction.Damage damage && damage.Source is DamageSources.BasicAttack basicAttack && damage.Receiver == this) {
            //Curl
            if (Statuses.ContainsKey(typeof(Status.CurlUp))) {
                spriteRenderer.sprite = curlUpTexture2D;
            }
        }
        base.OnDamaged(combatAction);
    }

    void OnStartTurn(CombatAction combatAction) {
        //if curl, uncurl
        if (spriteRenderer.sprite == curlUpTexture2D) {
            spriteRenderer.sprite = enemyTexture2D;
        }
    }
}
