using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CombatCardSelection : MonoBehaviour {
    public List<Card> Cards => Combat.SelectionPile;
    public int AskedAmount { get; set; }
    public bool MustBeExactAmountForConfirmed { get; set; }
    public Action OnConfirmed { get; set; }
    CombatManager Combat => CombatManager.Instance;

    public void Confirm() {
        if (MustBeExactAmountForConfirmed && Cards.Count != AskedAmount) {
            return;
        }
        OnConfirmed?.Invoke();
        Game.Scene.OpenSubscene(GameplaySubscenes.Combat);
        Game.Scene.MinimizeSubscene(GameplaySubscenes.CombatCardSelector);
    }
}
