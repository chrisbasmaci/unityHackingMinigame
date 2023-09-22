using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler {
	
	public enum Corner { BottomRight, BottomLeft, TopRight, TopLeft }
	public Corner resizeCorner = Corner.BottomRight;

	public Vector2 minSize = new Vector2 (100, 100);
	public Vector2 maxSize = new Vector2 (400, 400);
	public int nestedness = 1;

	private RectTransform panelRectTransform;
	private Vector2 originalLocalPointerPosition;
	private Vector2 originalSizeDelta;
	
	void Awake () {
		Transform currentTransform = transform;
		for (int i = 0; i < nestedness; i++) {
			if (currentTransform.parent != null) {
				currentTransform = currentTransform.parent;
			}
		}
		panelRectTransform = currentTransform.GetComponent<RectTransform>();
	}

	public void OnPointerDown (PointerEventData data) {
		originalSizeDelta = panelRectTransform.sizeDelta;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
	}
	
	public void OnDrag (PointerEventData data) {
		if (panelRectTransform == null)
			return;

		Vector2 localPointerPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out localPointerPosition);
		Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;

		Vector2 sizeDelta = originalSizeDelta;

		switch (resizeCorner) {
			case Corner.BottomRight:
				sizeDelta += new Vector2(offsetToOriginal.x, -offsetToOriginal.y);
				break;
			case Corner.BottomLeft:
				sizeDelta += new Vector2(-offsetToOriginal.x, -offsetToOriginal.y);
				break;
			case Corner.TopRight:
				sizeDelta += new Vector2(offsetToOriginal.x, offsetToOriginal.y);
				break;
			case Corner.TopLeft:
				sizeDelta += new Vector2(-offsetToOriginal.x, offsetToOriginal.y);
				break;
		}

		sizeDelta = new Vector2 (
			Mathf.Clamp (sizeDelta.x, minSize.x, maxSize.x),
			Mathf.Clamp (sizeDelta.y, minSize.y, maxSize.y)
		);
		
		panelRectTransform.sizeDelta = sizeDelta;
	}
}
