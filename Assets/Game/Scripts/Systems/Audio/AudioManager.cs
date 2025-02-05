using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Game.Systems.Audio
{
	public class AudioManager : PersistentSingleton<AudioManager>
	{
		[Header("Volume")]
		private float _masterVolume;
		private float _musicVolume;
		private float _ambienceVolume;
		private float _sfxVolume;

		[SerializeField] private EventReference _mainMusic;

		private Bus _masterBus;
		private Bus _musicBus;
		private Bus _ambienceBus;
		private Bus _sfxBus;

		private const string MASTER_BUS = "bus:/";
		private const string MUSIC_BUS = "bus:/Music";
		private const string SFX_BUS = "bus:/SFX";

		private bool _isMainMusicPlaying = false;

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);

			_masterBus = RuntimeManager.GetBus(MASTER_BUS);
			_musicBus = RuntimeManager.GetBus(MUSIC_BUS);
			_sfxBus = RuntimeManager.GetBus(SFX_BUS);

			_masterVolume = PlayerPrefs.GetFloat(MASTER_BUS, 1);
			_musicVolume = PlayerPrefs.GetFloat(MUSIC_BUS, 1);
			_sfxVolume = PlayerPrefs.GetFloat(SFX_BUS, 1);
			
			SetMasterVolume(_masterVolume);
			SetMusicVolume(_musicVolume);
			SetSfxVolume(_sfxVolume);
		}

		private void Start()
		{
			PlayMainMusic();
		}

		private void Update()
		{
			RuntimeManager.StudioSystem.setParameterByName("time_scale", Time.timeScale);
		}

		public void PlayMainMusic()
		{
			if (!_mainMusic.IsNull && !_isMainMusicPlaying)
			{
				RuntimeManager.PlayOneShot(_mainMusic);
				_isMainMusicPlaying = true;
			}
		}

		public void StopAllSFX()
		{
			_sfxBus.stopAllEvents(STOP_MODE.IMMEDIATE);
		}
		
		public void StopMainMusic()
		{
			// Implement logic to stop the main music if needed
			_isMainMusicPlaying = false;
		}
		
		public static EventInstance? PlayOneShot(EventReference sound, Vector3 worldPos = default, float volume = 1f)
		{
			if (sound.IsNull)
				return null;
			
			var instance = RuntimeManager.CreateInstance(sound.Guid);
			instance.set3DAttributes(worldPos.To3DAttributes());
			instance.setVolume(volume);
			instance.start();
			instance.release();
			return instance;
		}

		public static void StopSound(EventInstance? instance, STOP_MODE stopMode = STOP_MODE.IMMEDIATE)
		{
			if (instance?.isValid() == true)
			{
				instance.Value.stop(stopMode);
				instance.Value.release();
			}
		}
		
		public static void SetMasterVolume(float arg0)
		{
			var value = Mathf.Clamp01(arg0);
			
			Instance._masterVolume = value;
			Instance._masterBus.setVolume(value);
			PlayerPrefs.SetFloat(MASTER_BUS, value);
		}

		public static void SetMusicVolume(float arg0)
		{
			var value = Mathf.Clamp01(arg0);

			Instance._musicVolume = value;
			Instance._musicBus.setVolume(value);
			PlayerPrefs.SetFloat(MUSIC_BUS, value);
		}

		public static void SetSfxVolume(float arg0)
		{
			var value = Mathf.Clamp01(arg0);

			Instance._sfxVolume = value;
			Instance._sfxBus.setVolume(value);
			PlayerPrefs.SetFloat(SFX_BUS, value);
		}

		public static float GetMasterVolume()
		{
			return Instance._masterVolume;
		}

		public static float GetMusicVolume()
		{
			return Instance._musicVolume;
		}

		public static float GetAmbianceVolume()
		{
			return Instance._ambienceVolume;
		}

		public static float GetSfxVolume()
		{
			return Instance._sfxVolume;
		}
	}
}