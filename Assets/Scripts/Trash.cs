using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public void playSound()
    {
        source.volume = 0.1f;
        source.PlayOneShot(clip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.volume = 0.1f;
        source.PlayOneShot(clip);
        Game.NextRound(collision.gameObject);
    }
}
