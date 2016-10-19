using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Header("Impact Audio Source on Car")]
	public AudioSource ImpactSource;

	[Header("Low-Speed Impact Sounds")]
	public AudioClip LowSpeedImpact1;
	public AudioClip LowSpeedImpact2;
	public AudioClip LowSpeedImpact3;
	private AudioClip[] Impacts;

/*	public void Start(){
		Impacts = new AudioClip[]{LowSpeedImpact1, LowSpeedImpact2, LowSpeedImpact3};
	}

	public void LowImpact(){
		ImpactSource.clip = Impacts [Random.Range(0, Impacts.Length)];
		ImpactSource.Play ();
		Debug.Log ("Low-Speed Impact");
	}*/

}