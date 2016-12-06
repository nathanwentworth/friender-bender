using UnityEngine;
using System.Collections;

public class PlayOnImpact : MonoBehaviour
{

    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!sound.isPlaying) sound.Play();
    }
}
