using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelsMenu : MonoBehaviour
{
    public LevelInfoScriptableObject[] levels;
    public TMP_Dropdown levelsDropdown;
    public LevelInfoScriptableObject levelToPlay;

    private void Start() {
        if (levelsDropdown != null && levels.Length != 0) {
            List<string> options = new List<string>();
            foreach (LevelInfoScriptableObject level in levels) {
                options.Add(level.levelName);
            }
            levelsDropdown.ClearOptions();
            levelsDropdown.AddOptions(options);
            levelsDropdown.SetValueWithoutNotify(0);
            levelsDropdown.RefreshShownValue();
            SelectLevel(0);
        }
    }

    public void SelectLevel(int index) {
        if (index >= levels.Length || index < 0) return;
        levelToPlay.name = levels[index].name;
        levelToPlay.waves = levels[index].waves;
        levelToPlay.startingInkPercentage = levels[index].startingInkPercentage;
    }

}
