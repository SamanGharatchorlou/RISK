using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {

	public AudioSource click;
	public AudioSource addTroopsAudio;

	// fades out an audio over time
	public IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
		float startVolume = audioSource.volume;
		// fade out audio volume
		while (audioSource.volume > 0) {
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
			yield return null;
		}
		audioSource.Stop ();
		audioSource.volume = startVolume;
	}

	// click audio and placing soldier audio
	public void MoreTroopsAudio(){
		if (!addTroopsAudio.isPlaying) {
			addTroopsAudio.Play ();
			StartCoroutine (FadeOut (addTroopsAudio, 1f));
		}
	}

	// click audio
	public void Click(){
		click.Play ();
	}

}
