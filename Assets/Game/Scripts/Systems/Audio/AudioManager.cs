using FMOD.Studio;
using FMODUnity;
using LemonInc.Core.Utilities;
using UnityEngine;

namespace Game.Systems.Audio
{
	public class AudioManager : PersistentSingleton<AudioManager>
	{
		[Header("Volume")]
		[Range(0, 1), SerializeField] private float _masterVolume = 1;
		[Range(0, 1), SerializeField] private float _musicVolume = 1;
		[Range(0, 1), SerializeField] private float _ambienceVolume = 1;
		[Range(0, 1), SerializeField] private float _sfxVolume = 1;

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
		}

		private void OnValidate()
		{
			if (!Application.isPlaying) return;
			SetMasterVolume(_masterVolume);
			SetMusicVolume(_musicVolume);
			SetSfxVolume(_sfxVolume);
		}

		private void Start()
		{
			PlayMainMusic();
		}

		public void PlayMainMusic()
		{
			if (!_mainMusic.IsNull && !_isMainMusicPlaying)
			{
				RuntimeManager.PlayOneShot(_mainMusic);
				_isMainMusicPlaying = true;
			}
		}

		public void StopMainMusic()
		{
			// Implement logic to stop the main music if needed
			_isMainMusicPlaying = false;
		}

		public static void PlayOneShot(EventReference sound, Vector3 worldPos = default)
		{
			if (!sound.IsNull)
				RuntimeManager.PlayOneShot(sound, worldPos);
		}

		public static void SetMasterVolume(float arg0)
		{
			Instance._masterBus.setVolume(arg0);
			PlayerPrefs.SetFloat(MASTER_BUS, arg0);
		}

		public static void SetMusicVolume(float arg0)
		{
			Instance._musicBus.setVolume(arg0);
			PlayerPrefs.SetFloat(MUSIC_BUS, arg0);
		}

		public static void SetSfxVolume(float arg0)
		{
			Instance._sfxBus.setVolume(arg0);
			PlayerPrefs.SetFloat(SFX_BUS, arg0);
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