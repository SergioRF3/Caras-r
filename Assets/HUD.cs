using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public TextMeshPro timer;
    public TextMeshPro rounds;

    void Update()
    {
        timer.SetText(string.Format("{0}:{1:00}", Mathf.FloorToInt(Game.TimeUp / 60), Mathf.FloorToInt(Game.TimeUp % 60)));
        rounds.SetText("Ronda " + Game.Rounds);
    }
}
