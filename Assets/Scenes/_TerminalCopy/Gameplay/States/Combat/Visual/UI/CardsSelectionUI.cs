using UnityEngine;

public partial class CardsSelectionUI : MonoBehaviour {
    CombatManager combat => CombatManager.Instance;
    [SerializeField] GameObject background;
    [SerializeField] GameObject button;

    void Update() {
        if (combat.CurrentAction == null) {
            return;
        }

        CombatAction.CardsSelection cardSelection = combat.CurrentAction[^1] as CombatAction.CardsSelection;
        // if (cardSelection == null) {
        //     background.SetActive(false);
        //     button.SetActive(false);
        // }
        // else {
        //     background.SetActive(true);
        //     button.SetActive(true);
        // }
    }
}
