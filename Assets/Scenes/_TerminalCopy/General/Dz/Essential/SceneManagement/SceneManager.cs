using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dz.SceneManagement {
    public partial class SceneManager<T> : MonoBehaviour {
        public virtual Dictionary<T, string> MainScenesPath { get; set; }
        public virtual Dictionary<T, Type> MainScenesType { get; set; }
        public GameObject MainScene { get; private set; }
        public Subscene DefaultSubscene { get; set; }
        public List<Subscene> Subscenes {
            get {
                if (subscenes == null) {
                    subscenes = new();
                }
                return subscenes;
            }
            set {
                subscenes = value;
                subscenesAndParent = new();
                for (int i = 0; i < subscenes.Count; i++) {
                    subscenesAndParent.Add(subscenes[i], subscenes[i].transform.parent.parent.gameObject);
                }
            }
        }
        List<Subscene> subscenes;
        Dictionary<Subscene, GameObject> subscenesAndParent;

        public virtual void OpenMainScene(T mainScene) {
            if (MainScene != null) {
                DestroyImmediate(MainScene);
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(MainScenesPath[mainScene]);
        }

        public virtual void OpenSubscene(Subscene subscene, bool UnfocusOthersSubscene = true, bool focusOpenedSubscene = true) {
            if (!subscene.Scene.activeInHierarchy) {
                subscene.Scene.transform.parent = subscenesAndParent[subscene].transform;
            }
            subscene.IsOpened = true;
            subscene.Scene.SetActive(true);

            if (focusOpenedSubscene) {
                subscene.IsFocused = true;
            }
            if (UnfocusOthersSubscene) {
                foreach (Subscene _subscene in subscenesAndParent.Keys) {
                    if (_subscene == subscene || _subscene.KeepFocused) {
                        continue;
                    }
                    _subscene.IsFocused = false;
                }
            }
        }

        public virtual void Focus(Subscene subscene, bool UnfocusOthersSubscene = true) {
            subscene.IsFocused = true;
            if (UnfocusOthersSubscene) {
                foreach (Subscene _subscene in subscenesAndParent.Keys) {
                    if (_subscene == subscene) {
                        continue;
                    }
                    _subscene.IsFocused = false;
                }
            }
        }

        public virtual void Focus(List<Subscene> subscenes, bool UnfocusOthersSubscene = true) {
            for (int i = 0; i < subscenes.Count; i++) {
                subscenes[i].IsFocused = true;
            }
            if (UnfocusOthersSubscene) {
                foreach (Subscene subscene in subscenesAndParent.Keys) {
                    if (subscenes.Contains(subscene)) {
                        continue;
                    }
                    subscene.IsFocused = false;
                }
            }
        }

        public virtual void Unfocus(Subscene subscene) {
            subscene.IsFocused = false;
        }

        public virtual void Unfocus(List<Subscene> subscenes) {
            for (int i = 0; i < subscenes.Count; i++) {
                subscenes[i].IsFocused = false;
            }
        }

        public virtual void MinimizeSubscene(Subscene subscene) {
            Unfocus(subscene);
            subscene.Scene.SetActive(false);
        }

        public virtual void CloseSubscene(Subscene subscene) {
            if (subscene.KeepOpened) {
                return;
            }
            subscene.IsOpened = false;
            subscene.Scene.SetActive(false);
        }

        public virtual void Start() {
            MainScenesPath = new();
            subscenesAndParent = new();
        }

        public void Update() {
            if (subscenesAndParent == null) {
                return;
            }
            foreach (Subscene subscene in subscenesAndParent.Keys) {
                if (subscene.KeepOpened && !subscene.IsOpened) {
                    OpenSubscene(subscene, false);
                }
                if (subscene.KeepFocused && !subscene.IsFocused) {
                    Focus(subscene, false);
                }
            }

            //Kalau gak ada yang subscene yang difokuskan, fokus default subscene
            if (DefaultSubscene != null) {
                foreach (Subscene subscene in subscenesAndParent.Keys) {
                    if (subscene.IsFocused) {
                        return;
                    }
                }
                Focus(DefaultSubscene);
            }
        }
    }
}