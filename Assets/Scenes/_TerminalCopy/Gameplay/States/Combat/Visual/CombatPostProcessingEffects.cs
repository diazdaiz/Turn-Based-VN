using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CombatPostProcessingEffects : MonoBehaviour {
    [SerializeField] Volume volume;
    CharacterCombat Character;
    float HP => (float)Character.HP / (float)Character.MaxHealth * 100f; //nanti jadi current HP/MaxHP

    Bloom bloom;
    Vignette vignette;
    ColorAdjustments colorAdjustments;
    bool inCombat = false;

    private void Awake() {
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
    }

    void OnPlayerTeamAttack(CombatAction combatAction) {
        if (combatAction is CombatAction.Attack attack && CombatManager.Instance.PlayerTeam.Contains(attack.Attacker)) {
            StartCoroutine(PlayerAttackEffect());
        }
    }

    void OnPlayerTeamDamaged(CombatAction combatAction) {
        if (combatAction is CombatAction.Damage damage && CombatManager.Instance.PlayerTeam.Contains(damage.Receiver)) {
            StartCoroutine(PlayerDamagedEffect());
        }
    }

    // Update is called once per frame
    void Update() {
        if (!inCombat && CombatManager.Instance.IsCombating) {
            for (int i = 0; i < CombatManager.Instance.PlayerTeam.Count; i++) {
                CombatManager.Instance.PlayerTeam[i].OnAttack += OnPlayerTeamAttack;
                CombatManager.Instance.PlayerTeam[i].OnTakeDamage += OnPlayerTeamDamaged;
            }
            Character = CombatManager.Instance.PlayerTeam[0];
            inCombat = true;
        }
        if (!inCombat) {
            return;
        }
        bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, 0, 10 * bloom.intensity.value * Time.deltaTime);
        float targetVignette = 0.467f * (100f - HP) / 100f;
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, targetVignette, Mathf.Abs(targetVignette - vignette.intensity.value) * Time.deltaTime * 5);
        float targetSaturation = -70 * (100f - HP) / 100f;
        colorAdjustments.saturation.value = Mathf.MoveTowards(colorAdjustments.saturation.value, targetSaturation, Mathf.Abs(targetSaturation - colorAdjustments.saturation.value) * Time.deltaTime * 5);
    }

    IEnumerator PlayerAttackEffect() {
        float delayTimer = 0;
        while (delayTimer < 0.8f) {
            delayTimer += Time.deltaTime;
            yield return null;
        }

        bloom.tint.value = new Color(48f / 255f, 182f / 255f, 255f / 255f);
        float t = 0;
        while (t < 0.12f) {
            t += Time.deltaTime;
            bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, 40, 500 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator PlayerDamagedEffect() {
        float delayTimer = 0;
        while (delayTimer < 0.8f) {
            delayTimer += Time.deltaTime;
            yield return null;
        }

        bloom.tint.value = new Color(111f / 255f, 0f, 5f / 255f);
        float t = 0;
        while (t < 0.12f) {
            t += Time.deltaTime;
            bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, 40, 500 * Time.deltaTime);
            yield return null;
        }
    }
}
