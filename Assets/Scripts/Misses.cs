using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Misses : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.SetText("Misses: " + Game.Misses);
    }
}
