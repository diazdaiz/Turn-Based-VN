using UnityEngine;

public partial class Hero : CharacterCombat {
    public Deck StartingDeck => startingDeck;
    public Relic StartingRelic => startingRelic;
    public Relic StartingRelic2 => startingRelic;

    [SerializeField] Deck startingDeck;
    [SerializeField] Relic startingRelic;
    [SerializeField] Relic startingRelic2;
}
