using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDescriptionUI : MonoBehaviour {
    [Header("Skill Descriptions")]
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI enemyDesc;
    [SerializeField] private Image enemyImage;

    [Header("Skill Attributes")]
    [SerializeField] private TextMeshProUGUI earn;
    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI hp;

    private EnemyInfo enemyInfo;
    private EnemyInfo lastClicked;

    private void Update() {
        if (enemyInfo == null) {
            enemyName.text = "Enemy";
            earn.text = "--";
            defense.text = "--";
            hp.text = "--";
            enemyImage.enabled = false;
            return;
        }
        enemyName.text = enemyInfo.name;
        earn.text = enemyInfo.inkGained.ToString();
        defense.text = enemyInfo.defense.ToString();
        hp.text = enemyInfo.health.ToString();
        enemyImage.enabled = true;
    }

    public void SetInfo(EnemyInfo enemyInfo) {
        this.enemyInfo = enemyInfo;
    }

    public void SelectInfo(EnemyInfo enemyInfo) {
        this.enemyInfo = enemyInfo;
        this.lastClicked = enemyInfo;
    }

    public void ResetInfo() {
        if (!lastClicked) {
            enemyInfo = null;
        } else {
            enemyInfo = lastClicked;
        }
    }
}
