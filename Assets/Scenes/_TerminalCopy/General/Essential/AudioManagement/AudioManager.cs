using Dz.Audio;
using System.Collections.Generic;
using UnityEngine;

public partial class AudioManager : MonoBehaviour {
    [SerializeField] List<AudioClip> playlist;
    [SerializeField] List<AudioClip> Sfxs;

    MusicPlayer musicPlayer;
    OneTimePlayer oneTimePlayer;

    private void Awake() {
        musicPlayer = GetComponentInChildren<MusicPlayer>();
        oneTimePlayer = GetComponentInChildren<OneTimePlayer>();
        musicPlayer.Playlist = playlist;
        musicPlayer.Repeat = MusicPlayer.RepeatMusic.Playlist;
    }

    public void PlayMusic() {
        musicPlayer.Play();
    }

    public void PlayMusic(List<AudioClip> playlist, int playAtIndex) {
        musicPlayer.Play(playlist, playAtIndex);
    }

    public void PlayMusic(AudioClip music) {
        musicPlayer.Play(music);
    }

    public void PlayMusic(int index) {
        musicPlayer.Play(index);
    }

    public void PlayOneShot(AudioClip clip) {
        oneTimePlayer.Play(clip);
    }

    void TestControl() {
        //#region music
        //if (Input.GetKeyDown(KeyCode.Q)) musicPlayer.Play(playlist[0]);
        //if (Input.GetKeyDown(KeyCode.W)) musicPlayer.Play(playlist[1]);
        //if (Input.GetKeyDown(KeyCode.E)) musicPlayer.Play(playlist[2]);
        //if (Input.GetKeyDown(KeyCode.R)) musicPlayer.Play(playlist[3]);
        //if (Input.GetKeyDown(KeyCode.A)) musicPlayer.Play();
        //if (Input.GetKeyDown(KeyCode.S)) musicPlayer.Stop();
        //if (Input.GetKeyDown(KeyCode.D)) {
        //    if (musicPlayer.IsPaused) {
        //        musicPlayer.Continue();
        //    }
        //    else {
        //        musicPlayer.Pause();
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.F)) musicPlayer.SetFade(!musicPlayer.UseFade);
        //if (Input.GetKeyDown(KeyCode.LeftArrow)) musicPlayer.PlayPrevious();
        //if (Input.GetKeyDown(KeyCode.RightArrow)) musicPlayer.PlayNext();
        //#endregion
        //#region sfx
        //if (Input.GetKeyDown(KeyCode.Alpha1)) oneTimePlayer.Play(Sfxs[0]);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) oneTimePlayer.Play(Sfxs[1]);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) oneTimePlayer.Play(Sfxs[2]);
        //if (Input.GetKeyDown(KeyCode.Alpha4)) oneTimePlayer.Play(Sfxs[3]);
        //#endregion
    }

    private void Update() {
        TestControl();
    }
}
