using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public MixedRealityToolkitConfigurationProfile profileTouch;
    public MixedRealityToolkitConfigurationProfile profileEye;
    public MixedRealityToolkitConfigurationProfile profileVoice;

    private const string modelsFolder = "Assets/Resources/Models/";

    public  AudioSource source;
    public  AudioClip clip;

    private static int hits = 0;
    private static int misses = 0;
    private static int rounds = 0;
    private const int maxRounds = 30;

    private static bool eyeMode = false;
    private static bool touchMode = true;
    private static bool voiceMode = false;

    private static bool startGrowth = false;
    private static bool startShrink = false;

    private static GameObject currentFace1;
    private static GameObject currentFace2;
    private static GameObject currentFace3;

    private static int duration = 185;
    private static float timeUp = 0;

    float smoothTime = 0.3f;
    float yVelocity = 0.0f;

    public static int Hits { get => hits; set => hits = value; }
    public static int Misses { get => misses; set => misses = value; }
    public static int Rounds { get => rounds; set => rounds = value; }
    public static bool EyeMode { get => eyeMode; set => eyeMode = value; }
    public static bool TouchMode { get => touchMode; set => touchMode = value; }
    public static bool StartGrowth { get => startGrowth; set => startGrowth = value; }
    public static GameObject CurrentFace1 { get => currentFace1; set => currentFace1 = value; }
    public float SmoothTime { get => smoothTime; set => smoothTime = value; }
    public float YVelocity { get => yVelocity; set => yVelocity = value; }
    public static GameObject CurrentFace2 { get => currentFace2; set => currentFace2 = value; }
    public static GameObject CurrentFace3 { get => currentFace3; set => currentFace3 = value; }
    public static bool StartShrink { get => startShrink; set => startShrink = value; }
    public static int Duration { get => duration; set => duration = value; }
    public static float TimeUp { get => timeUp; set => timeUp = value; }
    public  AudioSource Source { get => source; set => source = value; }
    public  AudioClip Clip { get => clip; set => clip = value; }
    public static bool VoiceMode { get => voiceMode; set => voiceMode = value; }

    public static void addHit()
    {
        Hits++;
    }

    public static void addMiss()
    {
        Misses++;
    }

    public static int GetNumberOfPrefabsIn(string folderName)
    {
        var files = Resources.LoadAll("Models/" + folderName);
        return files.Length;
    }

    public static int[] GenerateRandomArray()
    {
        var numAdded = false;
        int numToAdd;
        int[] randomArray = new int[] { -1, -1, -1 };
        for (int i = 0; i < randomArray.Length; i++)
        {
            numAdded = false;
            while (!numAdded)
            {
                numToAdd = Random.Range(0, 3);
                if (!randomArray.Contains(numToAdd))
                {
                    randomArray[i] = numToAdd;
                    numAdded = true;
                }
            }
        }
        return randomArray;
    }

    public static void NextRound(GameObject face)
    {
        GameObject face0 = GameObject.Find("OriginalFace");
        GameObject face1 = GameObject.Find("AlternativeFace");
        GameObject face2 = GameObject.Find("CopyFace");
        Rounds++;
        if (face.name == "AlternativeFace")
        {
            addHit();
        }
        else
        {
            addMiss();
        }
        Face.DestroySetOfFaces(face0, face1, face2);
        if(Rounds < maxRounds)
        {
            Init();
        }
        else
        {
            SceneManager.LoadScene("EndScene");
        }
    }


    public static int RandomNumberExcept(int minInclusive, int maxExclusive, int except)
    {
        int result;
        do
        {
            result = Random.Range(minInclusive, maxExclusive);
        } while (result == except);
        return result;
    }

    public static void SetDefaultValues()
    {
        Hits = 0;
        Misses = 0;
        Rounds = 0;
        TimeUp = 0;
    }

    private void Awake()
    {
        if (eyeMode)
        {
            MixedRealityToolkit.SetProfileBeforeInitialization(profileEye);
        }
        else if(touchMode)
        {
            MixedRealityToolkit.SetProfileBeforeInitialization(profileTouch);
        }
        else
        {
            MixedRealityToolkit.SetProfileBeforeInitialization(profileVoice);
        }
    }
    void Start()
    {
        var speech = GameObject.Find("GameObject").GetComponent<SpeechInputHandler>();
        speech.AddResponse("centro", delegate () { NextRound(currentFace1); });
        speech.AddResponse("derecha", delegate () { NextRound(currentFace3); });
        speech.AddResponse("izquierda", delegate () { NextRound(currentFace2); });
        Init();
    }

    public static GameObject setUpFace(GameObject face)
    {
        ObjectManipulator prueba = face.AddComponent<ObjectManipulator>();
        Rigidbody body = face.AddComponent<Rigidbody>();
        var script = face.AddComponent<Face>();
        var eye = face.AddComponent<EyeTrackingTarget>();
        eye.OnLookAtStart = new UnityEvent();
        eye.OnLookAtStart.AddListener(delegate () { script.onFocusEnter(); }); 
        eye.OnLookAway= new UnityEvent();
        eye.OnLookAway.AddListener(delegate () { script.onFocusExit(); });
        body.useGravity = false;
        body.isKinematic = true;
        face.transform.localScale = new Vector3 { x = 0.0f, y = 0.0f, z = 0.0f };
        return face;
    }
        public static void Init()
    {
        var faces = Face.GenerateSetOfFaces();
        faces[0].transform.position = new Vector3 { x = 0, z = 1f };
        faces[1].transform.position = new Vector3 { x = -0.4f, z = 1f };
        faces[2].transform.position = new Vector3 { x = 0.4f, z = 1f };

        setUpFace(faces[0]);
        setUpFace(faces[1]);
        setUpFace(faces[2]);

        faces[0].GetComponent<Face>().Circle =  GameObject.Find("centerCircle").GetComponent<Image>();
        faces[1].GetComponent<Face>().Circle = GameObject.Find("rightCircle").GetComponent<Image>();
        faces[2].GetComponent<Face>().Circle = GameObject.Find("leftCircle").GetComponent<Image>();

        CurrentFace1 = faces[0];
        CurrentFace2 = faces[1];
        CurrentFace3 = faces[2];

        StartGrowth = true;
    }

    void Update()
    {

        if(rounds < maxRounds)
        {
            TimeUp += Time.deltaTime;
        }

        if (StartGrowth)
        {
            float increase = Mathf.SmoothDamp(CurrentFace1.transform.localScale.x, 1f, ref yVelocity, SmoothTime);
            CurrentFace1.transform.localScale = new Vector3 { x = increase, y = increase, z = increase };
            CurrentFace2.transform.localScale = new Vector3 { x = increase, y = increase, z = increase };
            CurrentFace3.transform.localScale = new Vector3 { x = increase, y = increase, z = increase };
            if(increase > 0.99f)
            {
                StartGrowth = false;
            }
        }

        if (StartShrink)
        {
            float decrease = Mathf.SmoothDamp(CurrentFace1.transform.localScale.x, 0f, ref yVelocity, SmoothTime);
            CurrentFace1.transform.localScale = new Vector3 { x = decrease, y = decrease, z = decrease };
            CurrentFace2.transform.localScale = new Vector3 { x = decrease, y = decrease, z = decrease };
            CurrentFace3.transform.localScale = new Vector3 { x = decrease, y = decrease, z = decrease };
            if(decrease > 0.1f)
            {
                StartShrink = false;
            }
        }
    }
}
