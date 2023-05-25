using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using JetBrains.Annotations;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;
using UnityEngine.UI;
using TMPro;

public class Face : MonoBehaviour
{
    private bool looking;
    private double timeLeft = 0;

    private Image circle;

    const float selectTime = 1f;

    public Image Circle { get => circle; set => circle = value; }

    public static GameObject GenerateAttribute(GameObject face, string attributeName) {
        var attribute = (GameObject)Instantiate(Resources.Load("Models/"+attributeName+"/"+attributeName+"_" + UnityEngine.Random.Range(1, Game.GetNumberOfPrefabsIn(attributeName) + 1)), face.transform);
        attribute.name = attribute.name.Replace("(Clone)", "");
        return attribute;
    }
    public static GameObject GenerateAlternativeAttribute(GameObject face, GameObject originalAttribute, string attributeName)
    {
        var except = originalAttribute.name.Split('_')[1];
        var attribute = (GameObject)Instantiate(Resources.Load("Models/" + attributeName + "/" + attributeName + "_" + Game.RandomNumberExcept(1, Game.GetNumberOfPrefabsIn(attributeName) + 1, int.Parse(except))), face.transform);
        attribute.name = attribute.name.Replace("(Clone)", "");
        return attribute;
    }
    public static GameObject CopyAttribute(GameObject face, GameObject originalAttribute)
    {
        var attribute = Instantiate(originalAttribute, face.transform);
        attribute.name = attribute.name.Replace("(Clone)", "");
        return attribute;
    }
    public static GameObject GenerateFace()
    {
            GameObject face = new GameObject();
            var temp = (GameObject)Instantiate(Resources.Load("Models/Base"), face.transform);
            temp.name = "Base";
            temp.AddComponent<NearInteractionGrabbable>();

            GenerateAttribute(face, "Hair");
            GenerateAttribute(face, "Eyebrows");
            GenerateAttribute(face, "Eyes");
            GenerateAttribute(face, "Mouth");
            return face;
    }


    public static GameObject GenerateAlternativeFace(GameObject righFace)
    {
        Transform[] attributes = new Transform[righFace.transform.childCount];
        var index = 0;
        foreach(Transform child in righFace.transform)
        {
            attributes[index] = child;
            index++;
        }
        var randomAttribute = UnityEngine.Random.Range(1, attributes.Length);
        GameObject face = new GameObject();
        var temp = (GameObject)Instantiate(Resources.Load("Models/Base"), face.transform);
        temp.name = "Base";
        temp.AddComponent<NearInteractionGrabbable>();
        for (int i = 1; i < attributes.Length; i++)
        {
            if (randomAttribute == i)
            {
                GenerateAlternativeAttribute(face, attributes[i].gameObject, attributes[i].name.Split('_')[0]);
            }
            else
            {
                CopyAttribute(face, attributes[i].gameObject);
            }
        }
        return face;
    }
    public static GameObject[] GenerateSetOfFaces()
    {
        GameObject[] faces = new GameObject[3];
        var originalFace = GenerateFace();
        originalFace.name = "OriginalFace";
        var copyFace = Instantiate(originalFace);
        copyFace.name = "CopyFace";
        var alternativeFace = GenerateAlternativeFace(originalFace);
        alternativeFace.name = "AlternativeFace";
        var randomArray = Game.GenerateRandomArray();
        faces[randomArray[0]] = originalFace;
        faces[randomArray[1]] = copyFace;
        faces[randomArray[2]] = alternativeFace;
        return faces;
    }

    public static void DestroySetOfFaces(GameObject face1, GameObject face2, GameObject face3)
    {
        Destroy(face1);
        Destroy(face2);
        Destroy(face3);
    }
    /*
    void IMixedRealityFocusHandler.OnFocusEnter(FocusEventData eventData)
    {
        if (Game.EyeMode)
        {
            looking = true;
            timeLeft += selectTime;
        }
    }


    void IMixedRealityFocusHandler.OnFocusExit(FocusEventData eventData)
    {
        if(Game.EyeMode)
        {
            looking = false;
        }
    }
    */
    public void onFocusEnter()
    {
        if (Game.EyeMode)
        {
            looking = true;
            timeLeft += selectTime;
        }
    }

    public void onFocusExit()
    {
        if (Game.EyeMode)
        {
            looking = false;
        }
    }

    public void playSound()
    {
        Trash trash = GameObject.Find("Trash").GetComponent<Trash>();
        trash.playSound();
    }

    void Update()
    {
        if (Game.EyeMode && looking)
        {
            if(timeLeft> 0)
            {
                Circle.fillAmount = (float)(selectTime - timeLeft) / selectTime;
                timeLeft -= Time.deltaTime;
            }
            else
            {
                playSound();
                Game.NextRound(this.gameObject);
            }
        }
        else
        {
            Circle.fillAmount = 0;
            timeLeft= 0;
        }
    }
}
