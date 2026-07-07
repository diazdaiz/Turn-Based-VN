using System;
using System.Collections.Generic;
using UnityEngine;

//NOTE:
//PLEASE FUTURE DIAZ, DON'T TOUCH ANYTHING, THIS IS ALREADY WORK AS INTENDED AND THERE'S SERIOUSLY NO NEED FOR YOU TO CHANGE ANYTHING.
//THIS CODE IS VERY PROMPT TO ERROR AND ALL OVER THE PLACE. IF YOU NEED MORE ROBUST CODE, JUST CREATE A NEW ONE. AND ONLY DO THAT IF
//YOU REALLY NEED IT & HAVE A LOT OF SPARE TIME.
//Note for others: sorry, this one is not that robust xdd
namespace Dz.Audio {
    public partial class MusicPlayer : MonoBehaviour {
        public List<AudioClip> Playlist { get; set; }
        public AudioClip CurrentMusic { get; private set; }
        public int CurrentIndex { get; private set; }
        public enum RepeatMusic { Disable, Current, Playlist }
        public RepeatMusic Repeat { get; set; }

        public bool IsPlaying => targetState == MusicPlayerState.Playing;
        public bool IsPaused => targetState == MusicPlayerState.Pausing;
        public bool UseFade { get; private set; } = true;
        /// <summary>
        /// 0f - maxVolume (default 200f)
        /// </summary>
        public float Volume {
            get {
                return sourceTargetVolume;
            }
            set {
                sourceTargetVolume = value;
            }
        }
        float MaxVolume { get; set; } = 200f;
        public Action<AudioClip> Starting;
        public Action<AudioClip> Stopping;

        enum MusicPlayerState { NotPlaying, Starting, Playing, Pausing, Continuing, AutoChangingMusic, ManualChangingMusic, Stopping };
        MusicPlayerState targetState;
        MusicPlayerState currentState;
        AudioClip targetMusic;
        float sourceTargetVolume;
        float targetVolume;
        float volume = 100f;
        float fadeSpeed = 300f;
        List<AudioSource> audioSources;

        /// <summary>
        /// Repeat current music if currently playing a music
        /// </summary>
        public void Play() {
            if (Playlist == null || Playlist.Count == 0) {
                Debug.Log("No music to be played");
                return;
            }
            Play(CurrentIndex);
        }

        /// <summary>
        /// Play music at index from playlist
        /// </summary>
        /// <param name="index"></param>
        public void Play(AudioClip music) {
            if (Playlist.Contains(music)) {
                for (int i = 0; i < Playlist.Count; i++) {
                    if (music == Playlist[i]) {
                        CurrentIndex = i;
                    }
                }
            }
            else {
                Playlist = new() {
                     music
                 };
                CurrentIndex = 0;
            }
            targetMusic = music;
            if (targetState == MusicPlayerState.AutoChangingMusic) {
                return;
            }
            else if (currentState == MusicPlayerState.NotPlaying) {
                targetState = MusicPlayerState.Starting;
            }
            else {
                targetState = MusicPlayerState.ManualChangingMusic;
            }
        }

        public void Play(int index) {
            if (index < 0 || index >= Playlist.Count) {
                Debug.Log("index out of bounds, will not play music");
                return;
            }
            Play(Playlist[index]);
        }

        public void Play(List<AudioClip> playlist, int playAtIndex = 0) {
            Playlist = playlist;
            Play(playAtIndex);
        }

        public void PlayNext() {
            if (Playlist.Count > 0) {
                Play(((CurrentIndex + 1) + Playlist.Count) % Playlist.Count);
            }
        }

        public void PlayPrevious() {
            if (Playlist.Count > 0) {
                Play(((CurrentIndex - 1) + Playlist.Count) % Playlist.Count);
            }
        }

        public void Pause() {
            targetState = MusicPlayerState.Pausing;
        }

        public void Stop() {
            targetState = MusicPlayerState.Stopping;
        }

        public void Continue() {
            targetState = MusicPlayerState.Continuing;
        }

        //Note: kerasa yang umumnya kyk gini, bisa dikembangin/diubah tapi nanti aja
        /// <summary>
        /// set sound fading when paused, continue, stop and manually changing music. Will not affect repeat and auto play next music in list
        /// </summary>
        /// <param name="fade"></param>
        /// <param name="targetDuration">in second</param>
        public void SetFade(bool fade, float targetDuration = 0.3f) {
            UseFade = fade;
            fadeSpeed = 100f / targetDuration;
        }

        private void Awake() {
            audioSources = new List<AudioSource>();
            sourceTargetVolume = MaxVolume;
        }

        private void FixedUpdate() {
            //Starting, repeat, next music
            #region music stop playing
            if (audioSources.Count > 0) {
                audioSources[0].loop = (Repeat == RepeatMusic.Current) || (Repeat == RepeatMusic.Playlist && Playlist.Count == 1);

                //kalau audio sourcenya menyatakan engga play, tapi current & targetnya menyatakan play, berarti musicnya berhenti
                if (!audioSources[0].isPlaying && currentState == MusicPlayerState.Playing && targetState == MusicPlayerState.Playing) {
                    if (Repeat == RepeatMusic.Current) {
                        return; //dah ke loop dari audio source unitynya, nanti dengan sendirinya audioSources[0].isPlaying jadi true lagi
                    }
                    if (Repeat == RepeatMusic.Playlist) {
                        if (Playlist.Count == 1) {
                            return; //^same reasoning
                        }
                        else if (CurrentIndex == Playlist.Count - 1) {
                            CurrentIndex = 0;
                        }
                        else {
                            CurrentIndex += 1;
                        }
                        targetState = MusicPlayerState.AutoChangingMusic;
                        Play(CurrentIndex);
                    }
                    if (Repeat == RepeatMusic.Disable) {
                        if (CurrentIndex == Playlist.Count - 1) {
                            CurrentIndex = 0;
                            Stop();
                        }
                        else {
                            CurrentIndex += 1;
                            targetState = MusicPlayerState.AutoChangingMusic;
                            Play(CurrentIndex);
                        }
                    }
                    return;
                }
            }
            #endregion

            #region music & its audio source
            if (targetMusic != CurrentMusic) {
                GameObject audioSourceGameObjectContainer = new GameObject("Music Player", typeof(AudioSource));
                audioSourceGameObjectContainer.transform.SetParent(transform);
                audioSources.Insert(0, audioSourceGameObjectContainer.GetComponent<AudioSource>());
                CurrentMusic = targetMusic;
                audioSources[0].clip = CurrentMusic;
            }
            if (audioSources.Count == 0) {
                return;
            }

            //remove other audio source
            if (audioSources.Count > 1) {
                for (int i = audioSources.Count - 1; i >= 1; i--) {
                    if (UseFade) {
                        audioSources[i].volume = Mathf.MoveTowards(audioSources[i].volume, 0, fadeSpeed / MaxVolume * Time.fixedDeltaTime);
                    }
                    else {
                        audioSources[i].volume = 0;
                    }
                    if (audioSources[i].volume <= 0) {
                        GameObject audioSourceGameObject = audioSources[i].gameObject;
                        audioSources.RemoveAt(i);
                        Destroy(audioSourceGameObject);
                    }
                }
            }
            #endregion

            #region volume & fade
            //fade work on manual change music, continue, pause, & stop
            //fade not work on auto change music (next/repeat) & play music when no music is currently playing

            //buat target
            if (targetState == MusicPlayerState.NotPlaying || targetState == MusicPlayerState.Pausing || targetState == MusicPlayerState.Stopping) {
                targetVolume = 0;
            }
            else {
                targetVolume = sourceTargetVolume;
            }

            //menuju target
            if (UseFade) {
                if (targetState == MusicPlayerState.Starting || targetState == MusicPlayerState.AutoChangingMusic) {
                    volume = targetVolume;
                }
                else {
                    volume = Mathf.MoveTowards(volume, targetVolume, fadeSpeed * Time.fixedDeltaTime);
                }
            }
            else {
                volume = targetVolume;
            }

            //set
            audioSources[0].volume = volume / MaxVolume;

            //harus 0 dulu baru lanjut
            if ((targetState == MusicPlayerState.NotPlaying || targetState == MusicPlayerState.Pausing || targetState == MusicPlayerState.Stopping) && volume != 0) {
                return;
            }
            #endregion

            #region state
            //NotPlaying, Starting, Playing, Pausing, Continuing, AutoChangingMusic, ManualChangingMusic, Stopping
            if (currentState != targetState) {
                if (targetState == MusicPlayerState.NotPlaying) {
                    audioSources[0].Stop();
                    targetState = MusicPlayerState.NotPlaying;
                    currentState = MusicPlayerState.NotPlaying;
                }
                else if (targetState == MusicPlayerState.Starting) {
                    audioSources[0].Play();
                    targetState = MusicPlayerState.Playing;
                    currentState = MusicPlayerState.Playing;
                }
                else if (targetState == MusicPlayerState.Playing) {
                    audioSources[0].Play();
                    targetState = MusicPlayerState.Playing;
                    currentState = MusicPlayerState.Playing;
                }
                else if (targetState == MusicPlayerState.Pausing) {
                    audioSources[0].Pause();
                    targetState = MusicPlayerState.Pausing;
                    currentState = MusicPlayerState.Pausing;
                }
                else if (targetState == MusicPlayerState.Continuing) {
                    audioSources[0].UnPause();
                    targetState = MusicPlayerState.Playing;
                    currentState = MusicPlayerState.Playing;
                }
                else if (targetState == MusicPlayerState.AutoChangingMusic) {
                    audioSources[0].Play();
                    targetState = MusicPlayerState.Playing;
                    currentState = MusicPlayerState.Playing;
                }
                else if (targetState == MusicPlayerState.ManualChangingMusic) {
                    audioSources[0].Play();
                    targetState = MusicPlayerState.Playing;
                    currentState = MusicPlayerState.Playing;
                }
                else if (targetState == MusicPlayerState.Stopping) {
                    audioSources[0].Stop();
                    targetState = MusicPlayerState.NotPlaying;
                    currentState = MusicPlayerState.NotPlaying;
                }
            }
            #endregion
        }
    }
}
