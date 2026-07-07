using Dz.SelectionManagement;

namespace Dz.SceneManagement.Example {
    public partial class Tab : Selection {
        // [SerializeField] TabContent tabContent;
        // [SerializeField] bool enable = true;

        // [SerializeField] SpriteRenderer spriteRenderer;
        // [SerializeField] Sprite normalSprite;
        // [SerializeField] Sprite hoverSprite;
        // [SerializeField] Sprite selectedSprite;
        // [SerializeField] Sprite disabledSprite;

        // [SerializeField] AudioSource audioSource;
        // [SerializeField] AudioClip hoverClip;
        // [SerializeField] AudioClip selectClip;
        // [SerializeField] AudioClip confirmClip;

        // // if true, play hover sound every time Hover() is called even when already hovered
        // [SerializeField] bool playHoverEveryTime = false;

        // Sprite lastAppliedSprite;

        // public override void Enable() {
        //     base.Enable();
        //     UpdateVisual();
        // }

        // public override void Disable() {
        //     base.Disable();
        //     UpdateVisual();
        // }

        // public override void Hover() {
        //     bool wasHovered = IsHovered;
        //     base.Hover();

        //     // play hover sound only on state change unless configured otherwise
        //     if (!wasHovered || playHoverEveryTime) {
        //         PlayClip(hoverClip);
        //     }

        //     UpdateVisual();
        // }

        // public override void Unhover() {
        //     base.Unhover();
        //     UpdateVisual();
        // }

        // public override void Select() {
        //     bool wasSelected = IsSelected;
        //     base.Select();

        //     if (!wasSelected) {
        //         PlayClip(selectClip);
        //     }

        //     UpdateVisual();
        // }

        // public override void Unselect() {
        //     base.Unselect();
        //     UpdateVisual();
        // }

        // public override void Confirm() {
        //     base.Confirm();
        //     PlayClip(confirmClip);
        // }

        // void UpdateVisual(bool immediate = false) {
        //     if (spriteRenderer == null) return;

        //     Sprite sprite = null;

        //     // precedence: disabled > selected > hovered > normal
        //     if (!IsEnabled) {
        //         sprite = disabledSprite;
        //     }
        //     else if (IsSelected) {
        //         sprite = selectedSprite;
        //     }
        //     else if (IsHovered) {
        //         sprite = hoverSprite;
        //     }
        //     if (sprite == null) {
        //         sprite = normalSprite;
        //     }

        //     if (immediate || sprite != lastAppliedSprite) {
        //         spriteRenderer.sprite = sprite;
        //         lastAppliedSprite = sprite;
        //     }
        // }

        // void PlayClip(AudioClip clip) {
        //     if (clip == null) return;

        //     if (audioSource != null) {
        //         audioSource.PlayOneShot(clip);
        //         return;
        //     }

        //     // fallback: play at this position
        //     AudioSource.PlayClipAtPoint(clip, transform.position);
        // }

        // private void Awake() {
        //     IsEnabled = enable;
        //     if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        //     UpdateVisual(true);
        //     tabContent = GetComponentInChildren<TabContent>(true);
        // }

        // private void Update() {
        //     if (IsSelected) {
        //         if (!tabContent.gameObject.activeInHierarchy) {
        //             tabContent.gameObject.SetActive(true);
        //         }
        //     }
        //     else {
        //         if (tabContent.gameObject.activeInHierarchy) {
        //             tabContent.gameObject.SetActive(false);
        //         }
        //     }
        // }
    }
}
