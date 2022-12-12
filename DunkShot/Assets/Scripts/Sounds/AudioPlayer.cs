using System;
using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    public class AudioPlayer : MonoBehaviour
    {
        [Serializable]
        private struct Clip
        {
            public SoundType SoundType;
            public AudioClip AudioClip;
        }

        private static readonly Dictionary<SoundType, AudioClip> _clipsDictionary = new Dictionary<SoundType, AudioClip>();

        [SerializeField] private List<Clip> ClipsList;

        private static AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (_clipsDictionary.Count != 0)
            {
                return;
            }
            
            foreach (Clip clip in ClipsList)
            {
                _clipsDictionary.Add(clip.SoundType, clip.AudioClip);
            }
        }

        public static void PlaySound(SoundType soundType)
        {
            _audioSource.clip = _clipsDictionary[soundType];
            _audioSource.Play();
        }
    }
}