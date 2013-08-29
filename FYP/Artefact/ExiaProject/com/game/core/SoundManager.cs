using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ExiaProject.com.game.core
{
    class SoundManager
    {
        Dictionary<string, SoundEffectInstance> _soundboardInstance;
        Dictionary<string, SoundEffect> _soundboardEffect;

        private static SoundManager _soundManager;

        public static SoundManager getInstance()
        {
            if (_soundManager == null)
                _soundManager = new SoundManager();

            return _soundManager;
        }

        private SoundManager()
        {
            _soundboardEffect = new Dictionary<string, SoundEffect>();
            _soundboardInstance = new Dictionary<string, SoundEffectInstance>();
        }

        public void AddSound(string name, SoundEffect sound)
        {
            _soundboardEffect.Add(name, sound);
            _soundboardInstance.Add(name, sound.CreateInstance());
        }

        public void PlaySoundInstance(string name, float volume, float pitch)
        {
            if (volume > 1)volume = 1.0f;
            if (volume < 0.1) volume = 0.1f;
            if (pitch > 1) pitch = 1.0f;

            if (_soundboardInstance.ContainsKey(name))
            {
                _soundboardInstance[name].Volume = volume;
                _soundboardInstance[name].Pitch = pitch;

                if (_soundboardInstance[name].State != SoundState.Playing)
                {
                    _soundboardInstance[name].Play();
                }
            }
        }

        public void PlaySoundEffect(string name, float volume, float pitch)
        {
            if (volume > 1) volume = 1.0f;
            if (pitch > 1) pitch = 1.0f;

            if (_soundboardEffect.ContainsKey(name))
            {
                _soundboardEffect[name].Play(volume,pitch,0);
            }
        }
    }
}
