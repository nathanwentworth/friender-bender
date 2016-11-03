using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Impact Audio Source on Car")]
    public AudioSource impactSource;
    public AudioSource powerupSource;

    [Header("Low-Speed Impact Sounds")]
    public AudioClip[] lowSpeedImpacts;
    public AudioClip highSpeedImpact;

    [Header("Powerups")]
    public AudioClip randomLargeObject;
    public AudioClip speedBoost;
    public AudioClip distraction;

    public IEnumerator Impact(bool highSpeed)
    {
        if (highSpeed)
        {
            Debug.Log("High-Speed Impact");
            impactSource.PlayOneShot(highSpeedImpact);
        }
        else {
            Debug.Log("Low-Speed Impact");
            impactSource.PlayOneShot(lowSpeedImpacts[DataManager.RandomVal(0, 1)]);
        }
        yield return null;
    }

    public IEnumerator PowerupSounds(string powerupName)
    {
        switch (powerupName)
        {
            case "speedBoost":
                Debug.Log("Playing Speedboost");
                powerupSource.PlayOneShot(speedBoost);
                break;
            //   case "randomLargeObject":
            // powerupSource.PlayOneShot(randomLargeObject);
            //     break;
            case "distraction":
                Debug.Log("Playing distraction");
                powerupSource.PlayOneShot(distraction);
                break;
            default:
                Debug.LogError("No sound for this powerup yet :^)");
                break;
        }
        yield return null;
    }

}