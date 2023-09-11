using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuzzleTimer : MonoBehaviour
{
    private MgSettings _settings;
    private int _introTime;
    private int _puzzleTime;
    public float introTimeLeft;
    public float puzzleTimeLeft;   
    private bool isEnabled;
    private bool puzzleStarted;
    [FormerlySerializedAs("puzzleStarted")] public bool introEnded ;
    private Image _loadingbarTimer;

    public void Initialize(MgSettings settings, Image loadingBarTimer =null)
    {
        _settings = settings;
        _introTime = _settings.DefaultIntroTimer;
        _puzzleTime = _settings.DefaultPuzzleTimer;
        _loadingbarTimer = loadingBarTimer;
        reset_timer();
    }

    public void InitializeLoadingBar(Image loadingBarTimer)
    {
        _loadingbarTimer = loadingBarTimer;
    }

    public void reset_timer()
    {
        _introTime = _settings.CurrentIntroTimer;
        _puzzleTime = _settings.CurrentPuzzleTimer;
        introTimeLeft = _introTime; 
        puzzleTimeLeft = _puzzleTime;
        isEnabled = false;
        introEnded = false;
        puzzleStarted = false;

        if (_loadingbarTimer) {
            _loadingbarTimer.fillAmount = 0;
        }
        else {
            Debug.Log("No timer image(timer uninitialized)");
        }

    }
    public void startIntroTimer(){
        reset_timer();
        isEnabled = true;
    }
    public bool introFinished()
    {
        return introEnded;
    }
    public void startPuzzleTimer()
    {
        isEnabled = true;
        introEnded = true;
        puzzleStarted = true;
    }
    public bool IsGameOver()
    {
        return !isEnabled;
    }

    public void PauseTimer() {
        isEnabled = false;
    }

    public void ResumeTimer() {
        isEnabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(isEnabled){
            if(!introEnded){
                introTimeLeft -= Time.deltaTime;
                if(introTimeLeft < 0){
                    introEnded = true;
                    Debug.Log("Time's up!");
                    introTimeLeft = 0f;

                }
            }
            else if(puzzleStarted){
                if (_loadingbarTimer) {
                    _loadingbarTimer.fillAmount = 1 - puzzleTimeLeft/ _puzzleTime;

                }
                puzzleTimeLeft -= Time.deltaTime;
                if(puzzleTimeLeft < 0){
                    isEnabled = false;
                    introEnded = false;
                    puzzleStarted = false;
                    Debug.Log("Time's up2!");
                    puzzleTimeLeft = 0.0f;

                }
            }
        }
        
    }
}

