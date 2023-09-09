using UnityEngine;

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
