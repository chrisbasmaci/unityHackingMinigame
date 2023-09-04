using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.Serialization;

/// <summary>
/// TODO MAKE THIS A SINGLETON
/// </summary>
public class ButtonManager : MonoBehaviour
{
    //Toggles
    /// TODO FIND A PLACE FOR THESE TOO, OR MAYBE MOVE THE BUTTONS HERE

    public void OnInvertToggleValueChanged(bool isOn){
        Debug.Log("Invert Toggle " + " isOn: " + isOn);
        Game.Instance.invertToggle = isOn;
    }

    public void OnQuestionFirstToggleValueChanged(bool isOn){
        Debug.Log("Invert Toggle " + " isOn: " + isOn);
        Game.Instance.questionFirstToggle = isOn;
    }
    




}
