using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text textCost;
    [SerializeField] private int healthNeed=0;
    [SerializeField] private Spell spell;
    private bool isCooldown = false;
    private float cooldownTimer = 0.0f;

    private void Start() {
        textCost.text = spell.cost.ToString();
        textCooldown.gameObject.SetActive(false);
        imageCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    private void Update() {
        if (isCooldown) {
            ApplyCooldown();
        }
    }

    private void ApplyCooldown() {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0.0f) {
            isCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        } else {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / spell.cooldownTime;
        }
    }

    public void ResetCooldownTimer() {
        isCooldown = true;
        textCooldown.gameObject.SetActive(true);
        imageCooldown.gameObject.SetActive(true);
        imageCooldown.fillAmount = 1.0f;
        cooldownTimer = spell.cooldownTime;
    } 

    public void UseSpell() {
        if (healthNeed>0)
        {
            Transform player = FindObjectOfType<PlayerMovement>().transform;
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health.GetHealth()<=healthNeed)
            {
                return;
            }
        }
        if (!isCooldown) {
            StartCoroutine(spell.Activate(this));
        }
    }

}
