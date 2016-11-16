using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    private static System.Random rng = new System.Random();
    private int playlistIndex;

    [Header("Impact Audio Source on Car")]
    [SerializeField]
    private AudioSource impactSource;
    [SerializeField]
    private AudioSource powerupSource;

    [Header("Low-Speed Impact Sounds")]
    [SerializeField]
    private AudioClip[] lowSpeedImpacts;
    [SerializeField]
    private AudioClip highSpeedImpact;

    [Header("Powerups")]
    [SerializeField]
    private AudioClip randomLargeObject;
    [SerializeField]
    private AudioClip speedBoost;
    [SerializeField]
    private AudioClip distraction;

    [Header("Music")]
    [SerializeField]
    private AudioClip[] trekkieTrax;
    [SerializeField]
    private AudioClip[] mainPlaylist;
    [SerializeField]
    private AudioSource music;
    private AudioClip[] playlist;

    private void Awake() {
        StartCoroutine(ShuffleMusic());
        playlistIndex = -1;
    }

    private void Update() {
        if (!music.isPlaying) {
            if (playlistIndex == playlist.Length - 1) {
                playlistIndex = 0;
            } else {
                playlistIndex++;
            }
            music.clip = playlist[playlistIndex];
            music.Play();
        }
    }

    private IEnumerator ShuffleMusic() {
        playlist = DataManager.IsTrekkieTraxOn ? trekkieTrax : mainPlaylist;

        int n = playlist.Length;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            AudioClip value = playlist[k];
            playlist[k] = playlist[n];
            playlist[n] = value;
        }
        yield return null;
    }


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

    public void SetMusicVolume(float volume) {
        music.volume = volume;
    }

}