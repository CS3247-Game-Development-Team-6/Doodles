using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellDescriptionUI : MonoBehaviour {
    [Header("Skill Descriptions")]
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellDesc;
    [SerializeField] private Image spellImage;

    [Header("Skill Attributes")]
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI cooldown;

    private SpellInfo spellInfo;
    private SpellInfo lastClicked;

    private void Update() {
        if (spellInfo == null) {
            spellName.text = "Spell";
            cost.text = "--";
            level.text = "--";
            cooldown.text = "--";
            spellImage.enabled = false;
            return;
        }

        spellName.text = spellInfo.name;
        spellDesc.text = spellInfo.description;
        cost.text = spellInfo.cost.ToString();
        level.text = spellInfo.level.ToString();
        cooldown.text = spellInfo.cooldownTime.ToString();
        spellImage.sprite = spellInfo.sprite;
        spellImage.enabled = true;
    }

    public void SetInfo(SpellInfo spellInfo) {
        this.spellInfo = spellInfo;

    }

    public void SelectInfo(SpellInfo spellInfo) {
        this.spellInfo = spellInfo;
        this.lastClicked = spellInfo;
    }

    public void ResetInfo() {
        if (!lastClicked) {
            spellInfo = null;
        } else {
            spellInfo = lastClicked;
        }
    }

}
