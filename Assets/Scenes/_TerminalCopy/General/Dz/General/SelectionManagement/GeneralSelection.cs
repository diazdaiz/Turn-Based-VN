using UnityEngine;
using UnityEngine.UI;

public partial class GeneralSelection : Selectable {
    [SerializeField] bool enable = true;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Image image;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite disabledSprite;

    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip selectClip;
    [SerializeField] AudioClip confirmClip;

    // if true, play hover sound every time Hover() is called even when already hovered
    [SerializeField] bool playHoverEveryTime = false;

    Sprite lastAppliedSprite;

    public void OnEnable() {
        IsEnabled = enable;
        UpdateVisual(true);
    }

    public void OnDisable() {
        UpdateVisual();
    }

    public override void Hover() {
        bool wasHovered = IsHovered;
        base.Hover();

        // play hover sound only on state change unless configured otherwise
        if (!wasHovered || playHoverEveryTime) {
            PlayClip(hoverClip);
        }

        UpdateVisual();
    }

    public override void Unhover() {
        base.Unhover();
        UpdateVisual();
    }

    public override void Select() {
        bool wasSelected = IsSelected;
        base.Select();

        if (!wasSelected) {
            PlayClip(selectClip);
        }

        UpdateVisual();
        Debug.Log("select");
    }

    public override void Unselect() {
        base.Unselect();
        UpdateVisual();
    }

    public override void Confirm() {
        base.Confirm();
        PlayClip(confirmClip);
    }

    void UpdateVisual(bool immediate = false) {
        if (spriteRenderer == null && image == null) return;

        Sprite sprite = null;

        // precedence: disabled > selected > hovered > normal
        if (!IsEnabled) {
            sprite = disabledSprite;
        }
        else if (IsSelected) {
            sprite = selectedSprite;
        }
        else if (IsHovered) {
            sprite = hoverSprite;
        }
        if (sprite == null) {
            sprite = normalSprite;
        }

        if (immediate || sprite != lastAppliedSprite) {
            if (spriteRenderer != null) spriteRenderer.sprite = sprite;
            if (image.sprite != null) image.sprite = sprite;
            lastAppliedSprite = sprite;
        }
    }

    void PlayClip(AudioClip clip) {
        if (clip == null) return;

        if (Game.Audio != null) {
            Game.Audio.PlayOneShot(clip);
            return;
        }
    }
}