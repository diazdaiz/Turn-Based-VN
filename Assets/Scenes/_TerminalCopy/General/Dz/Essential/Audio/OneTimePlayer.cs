using UnityEngine;

namespace Dz.Audio {
    public partial class OneTimePlayer : MonoBehaviour {
        public float Volume { get; set; } = 200f;

        public void Play(AudioClip audio) {
            GameObject sfxPlayer = new GameObject("Play " + audio.name, typeof(AudioSource));
            sfxPlayer.transform.parent = transform;
            AudioSource audioSource = sfxPlayer.GetComponent<AudioSource>();
            audioSource.volume = Volume / 200f;
            audioSource.clip = audio;
            audioSource.Play();
            Destroy(sfxPlayer, audio.length);
        }
    }
}
