using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedInfoUI : MonoBehaviour {
    void Start() {
        Vector3 finalPos = transform.position;
        transform.position += Vector3.down * GetComponent<RectTransform>().sizeDelta.y;
        LeanTween.move(gameObject, finalPos, 0.8f).setEase(LeanTweenType.easeInCubic);
    }
}
