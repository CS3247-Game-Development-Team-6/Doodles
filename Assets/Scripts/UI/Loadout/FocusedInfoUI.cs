using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedInfoUI : MonoBehaviour {
    void Start() {
        Vector3 finalPos = transform.position;
        transform.position += Vector3.down * GetComponent<RectTransform>().sizeDelta.y;
        LeanTween.move(gameObject, finalPos, 0.8f).setEase(LeanTweenType.easeInCubic);
    }

    public void MoveToHUDPos() {
        float scale = 0.8f;
        float width = GetComponent<RectTransform>().sizeDelta.x * scale;
        float height = GetComponent<RectTransform>().sizeDelta.y * scale;
        Vector3 finalPos = transform.position + Vector3.down * (height * 0.1f) + Vector3.left * (width * 0.25f);
        LeanTween.scale(gameObject, Vector3.one * scale, 1f).setEaseInOutCubic();
        LeanTween.move(gameObject, finalPos, 1f);

    }
}
