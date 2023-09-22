using UnityEngine;
using System.Collections;

public class ActiveStateToggler : MonoBehaviour {

	public MonoBehaviour componentToToggle; // Drag the component you want to toggle here in the Inspector

	public void ToggleComponent()
	{
		if (componentToToggle != null)
		{
			componentToToggle.enabled = !componentToToggle.enabled;
		}
	}
	public void ToggleActive () {
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
