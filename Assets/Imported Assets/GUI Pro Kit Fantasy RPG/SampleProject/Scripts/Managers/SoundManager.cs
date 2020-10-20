using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

namespace FantasyRPG
{

	public class SoundManager : Singleton<SoundManager>
	{
		public AudioSource activeEffectLoop;
		public AudioSource activeEffect;
		public AudioSource activeBGM;

		public AudioMixer mixer;

		private string mixerEffect = "Effect";
		private string mixerBgm = "Bgm";
		private string mixerMaster = "Master";

		private void Awake()
		{
			//믹서가져오기
			mixer = Resources.Load("MasterMixer") as AudioMixer;

			//반복 효과음
			activeEffectLoop = gameObject.AddComponent<AudioSource>();
			activeEffectLoop.playOnAwake = false;
			activeEffectLoop.loop = true;
			activeEffectLoop.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerEffect)[0];

			//효과음
			activeEffect = gameObject.AddComponent<AudioSource>();
			activeEffect.playOnAwake = false;
			activeEffect.loop = false;
			activeEffect.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerEffect)[0];

			//배경음
			activeBGM = gameObject.AddComponent<AudioSource>();
			activeBGM.playOnAwake = false;
			activeBGM.loop = true;
			activeBGM.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerBgm)[0];

			//믹서 초기화q
			mixer.SetFloat(mixerEffect, 0f);
			mixer.SetFloat(mixerBgm, 0f);
			mixer.SetFloat(mixerMaster, 0f);
		}


		#region //효과음 반복

		public void PlayEffectLoop(string soundName, float volume = 1f)
		{
			activeEffectLoop.volume = volume;
			activeEffectLoop.clip = Resources.Load(string.Format("{0}{1}", Data.path_sound, soundName)) as AudioClip;
			activeEffectLoop.Play();
		}

		public void StopEffectLoop()
		{
			if (activeEffectLoop == null) return;
			activeEffectLoop.Stop();
		}

		#endregion







		#region //효과음 재생

		public void PlayEffect(string effect, float volume = 1f)
		{
			AudioClip clip = Resources.Load(string.Format("{0}{1}", Data.path_sound, effect)) as AudioClip;
			activeEffect.PlayOneShot(clip, volume);
		}

		#endregion







		#region //배경음악 재생

		public void PlayBGM(string music, float volume = 1f)
		{
			if (!Data.IsBgm) return;

			// TODO: 이 조건은 지금 null 오류를 예방하는 것으로 임시방편 처리함, 꼭 수정해야 함
			if (activeBGM.clip)
			{
				if (activeBGM.clip.name == music) return;

				DOTween.To(() => activeBGM.volume, x => activeBGM.volume = x, 0f, 0.1f).SetEase(Ease.Linear).OnComplete(
					() =>
					{
						activeBGM.Stop();
						activeBGM.clip = null;
						PlayBGM(music);
					}).SetUpdate(true);
			}
			else
			{
				activeBGM.volume = volume;
				activeBGM.clip = Resources.Load(string.Format("{0}{1}", Data.path_sound, music)) as AudioClip;
				activeBGM.Play();
			}
		}

		#endregion





		#region //배경음악 재생

		public void PauseBGM()
		{
			if (activeBGM.clip == null) return;

			DOTween.To(() => activeBGM.volume, x => activeBGM.volume = x, 0f, 0.1f).SetEase(Ease.Linear)
				.SetUpdate(true);
			activeBGM.Pause();
		}

		#endregion





		#region //배경음악 이어듣기

		public void ResumeBGM()
		{
			if (activeBGM.clip == null) return;

			activeBGM.Play();
			DOTween.To(() => activeBGM.volume, x => activeBGM.volume = x, 1f, 0.1f).SetEase(Ease.Linear)
				.SetUpdate(true);
		}

		#endregion





		#region //배경음악 정지

		public void StopBGM()
		{
			if (activeBGM.clip == null) return;

			DOTween.To(() => activeBGM.volume, x => activeBGM.volume = x, 0f, 0.1f).SetEase(Ease.Linear).OnComplete(
				() =>
				{
					activeBGM.Stop();
					Destroy(activeBGM.gameObject);
					activeBGM = null;
				}).SetUpdate(true);
		}

		#endregion
	}
}