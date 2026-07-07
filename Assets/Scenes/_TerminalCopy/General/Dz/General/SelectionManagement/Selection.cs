using UnityEngine;

namespace Dz.SelectionManagement {
    public partial class Selection : MonoBehaviour {
        /// <summary>
        /// use this for checking if this selection is enabled instead of unity "enabled"
        /// </summary>
        public bool IsEnabled { get; protected set; }
        public bool IsHovered { get; protected set; }
        public bool IsSelected { get; protected set; }

        public System.Action OnHovered { get; set; }
        public System.Action OnSelected { get; set; }
        public System.Action OnConfirmed { get; set; }

        public virtual void Enable() {
            IsEnabled = true;
        }

        public virtual void Disable() {
            IsEnabled = false;
        }

        public virtual void Hover() {
            OnHovered?.Invoke();
            IsHovered = true;
        }

        public virtual void Unhover() {
            IsHovered = false;
        }

        public virtual void Select() {
            OnSelected?.Invoke();
            IsSelected = true;
        }

        public virtual void Unselect() {
            IsSelected = false;
        }

        //ada banyak kasus dimana selection harus diconfirm walau sudah di select
        public virtual void Confirm() {
            OnConfirmed?.Invoke();
        }
    }
}