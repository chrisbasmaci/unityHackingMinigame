using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuzzleTimer : MonoBehaviour
{
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
        Debug.Log("Initialize called on timer");
        _introTime = settings.DefaultIntroTimer;
        _puzzleTime = settings.DefaultPuzzleTimer;
        _loadingbarTimer = loadingBarTimer;
    }

    public void InitializeLoadingBar(Image loadingBarTimer)
    {
        _loadingbarTimer = loadingBarTimer;
    }

    public void reset_timer(MgSettings settings)
    {
        Debug.Log("Puzzle time" +_puzzleTime);
        _introTime = settings.CurrentIntroTimer;
        _puzzleTime = settings.CurrentPuzzleTimer;
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

