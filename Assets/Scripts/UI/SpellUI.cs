using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    [SerializeField] private Image imageCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text textCost;
    [SerializeField] private TMP_Text notification;

    [SerializeField] private GlobalEffect spell;
    public bool isCooldown = false;
    public float cooldownTime = 10.0f;
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
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
    }

    private IEnumerator sendNotification(string text, int time) {
        notification.text = text;
        yield return new WaitForSeconds(1);
        notification.text = "";
    }

    public void UseSpell() {
        if (isCooldown) {
            StartCoroutine(sendNotification("Spell is in cooldown", 1));
        } else if (!InkManager.instance.hasEnoughInk(spell.cost)) {
            StartCoroutine(sendNotification("Not enough ink", 1));
        } else {
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);
            imageCooldown.gameObject.SetActive(true);
            imageCooldown.fillAmount = 1.0f;
            cooldownTimer = cooldownTime;
            InkManager.instance.ChangeInkAmount(-spell.cost);
            SpellManager.HandleEffect(spell);
            spell.Activate();
        }
    }
}
