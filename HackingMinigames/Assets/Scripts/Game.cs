using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public enum ShapeBundle{circle=0, triangle, square, diamond, rectangle}

public static class ColorHex
{
    public static Dictionary<int, (string colorName, string colorHex)> colorMap = new Dictionary<int, (string, string)>()
    {
        { 0, ("red", "#D7263D") },
        { 1, ("blue", "#51D6FF") },
        { 2, ("green", "#14CC60") },
        { 3, ("yellow", "#FFE381") },
    };
    public static readonly int color_total = colorMap.Count;
}
public class Game : MonoBehaviour
{
    // public static string arr;

    private static Game instance;
    [NonSerialized]public MinigameType currentMg;
    [NonSerialized]public GameObject currentSettingsPrefab;
    [NonSerialized]private GameWindow _currentGameWindow;


    public  int defaultIntroTime = 3;
    public  int defaultPuzzleTime = 10;
    //set in unity
    [SerializeField] public int defaultTileAmount = 4;
    [SerializeField] public Sprite cardBack;
    [SerializeField] public Sprite cardFace;
    [SerializeField] public Sprite[] cardOrderSheet;

    [SerializeField] public GameObject cardBackPrefab;
    [SerializeField] public AnimationClip orderRevealAnimation;
    [SerializeField] public RuntimeAnimatorController curtainController;

    [SerializeField] public Sprite[] shapeSheet;
    [SerializeField] public Sprite[] shapeTextSheet;
    [SerializeField] public Sprite[] colorTextSheet;
    
    
    //UI PREFABS
    [SerializeField] public GameObject upperHackPrefab;
    [SerializeField] public GameObject upperUntanglePrefab;
    [SerializeField] public GameObject bottomHackPrefab;
    [SerializeField] public GameObject bottomUntanglePrefab;

    [SerializeField] public GameObject highscoreBoardPrefab;
    // Public property to access the singleton instance
    
    //Toggles
    public bool invertToggle;
    public bool questionFirstToggle;

    public static Game Instance
    {
        get
        {
            // If the instance is null, try to find an existing instance in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<Game>();
                
                // If no instance exists in the scene, create a new GameObject and attach the singleton script to it
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Game).Name);
                    instance = singletonObject.AddComponent<Game>();
                }
            }

            return instance;
        }
    }
    private void Awake()
    {
        // Ensure only one instance of the singleton exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Don't destroy the singleton object when loading new scenes
        DontDestroyOnLoad(gameObject);
    }
    public GameWindow CurrentGameWindow
    {
        get{
            if (_currentGameWindow) {
                return _currentGameWindow;
            }
            Debug.Assert(false, "_currentGameWindow is null. No game is in process.");
            throw new InvalidOperationException("_currentGameWindow is null");
        }
        set{
        _currentGameWindow = value;
        }
    }

}


