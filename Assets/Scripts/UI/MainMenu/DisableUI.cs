using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour {
    public bool dontShow { get; private set; }
    public GameObject target;
    public string Key => $"{((target != null) ? target.name : name)}-hidden";

    private void Start() {
        dontShow = PlayerPrefs.GetInt(Key) > 0;
        if (dontShow && target != null) {
            target.SetActive(false);
        }
    }

    public void toggleDontShow() {
        dontShow = !dontShow;
        int val = (dontShow ? 1 : 0);
        Debug.LogWarning($"{Key} has been set to {val}.");

        PlayerPrefs.SetInt(Key, val);
    }
}
