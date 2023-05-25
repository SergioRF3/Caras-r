using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.CameraSystem;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.XRSDK.WindowsMixedReality;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;


public class EndMenu : MonoBehaviour
{
    public TextMeshPro hitsText;
    public TextMeshPro missesText;
    public TextMeshPro netHitsText;
    public TextMeshPro impulsivityText;
    public TextMeshPro durationText;

    void Start()
    {
        var hits = Game.Hits;
        var misses = Game.Misses;
        var netHits = (Game.Hits - Game.Misses);
        var impulsivity = ((double)netHits / (Game.Hits + Game.Misses)) * 100;
        var duration = string.Format("{0}:{1:00}", Mathf.FloorToInt(Game.TimeUp / 60), Mathf.FloorToInt(Game.TimeUp % 60));

        hitsText.SetText("Aciertos: " + hits.ToString());
        missesText.SetText("Fallos: " + misses.ToString());
        netHitsText.SetText("Aciertos netos: " + netHits.ToString());
        impulsivityText.SetText("Impulsividad: " + impulsivity.ToString("F0"));
        durationText.SetText("Duración: " + duration);

        var text = "Aciertos;Fallos;Aciertos netos;Impulsividad;Duracion" + "\n"
            + hits.ToString() +";" + misses.ToString()+ ";" + netHits.ToString()+ ";" + impulsivity.ToString("F0") + ";" + duration;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        Directory.CreateDirectory(Path.Combine(path, "Caras-r"));
        File.WriteAllText(Path.Combine(path, "Caras-r", DateTime.Now.ToString("yyyyMMdd'T'HHmmss") + ".csv"), text);     
    }


    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
