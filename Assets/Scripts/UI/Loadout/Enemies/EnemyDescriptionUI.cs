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
            attack.text = "--";
            defense.text = "--";
            hp.text = "--";
            enemyImage.enabled = false;
            return;
        }
        enemyName.text = enemyInfo.enemyName;
        enemyDesc.text = enemyInfo.description;
        earn.text = enemyInfo.inkGained.ToString();
        attack.text = enemyInfo.shotInfo.bulletDamage.ToString();
        defense.text = enemyInfo.defense.ToString();
        hp.text = enemyInfo.health.ToString();
        enemyImage.sprite = enemyInfo.sprite;
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
