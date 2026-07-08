using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CombatPostProcessingEffects : MonoBehaviour {
    [SerializeField] Volume volume;
    [SerializeField] float HP = 1f; //nanti jadi current HP/MaxHP

    Bloom bloom;
    Vignette vignette;
    ColorAdjustments colorAdjustments;

    private void Awake() {
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            StartCoroutine(PlayerAttackedEffect());
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(PlayerAttackEffect());
        }
        bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, 0, 10 * bloom.intensity.value * Time.deltaTime);
        float targetVignette = 0.467f * (100 - HP) / 100f;
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, targetVignette, Mathf.Abs(targetVignette - vignette.intensity.value) * Time.deltaTime * 5);
        float targetSaturation = -70 * (100 - HP) / 100f;
        colorAdjustments.saturation.value = Mathf.MoveTowards(colorAdjustments.saturation.value, targetSaturation, Mathf.Abs(targetSaturation - colorAdjustments.saturation.value) * Time.deltaTime * 5);
    }

    IEnumerator PlayerAttackEffect() {
        bloom.tint.value = new Color(48f / 255f, 182f / 255f, 255f / 255f);
        float t = 0;
        while (t < 0.12f) {
            t += Time.deltaTime;
            bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, 40, 500 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator PlayerAttackedEffect() {
        bloom.tint.value = new Color(111f / 255f, 0f, 5f / 255f);
        float t = 0;
        while (t < 0.12f) {
            t += Time.deltaTime;
            bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, 40, 500 * Time.deltaTime);
            yield return null;
        }
    }
}
