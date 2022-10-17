using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SpellUI : MonoBehaviour {
    [SerializeField] private Image imageCooldown;
    [SerializeField] private Image effectCooldown;
    [SerializeField] private TMP_Text textCooldown;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text textCost;

    [Header("Spell Script [Must attach child class from editor]")]
    [SerializeField] public SpellInfo spellInfo;
    [SerializeField] public Spell spell;
    private bool isCooldown = false;
    private float cooldownTimer = 0.0f;
    public int Level { get; set; }

    private void Start() {
        if (spell == null) Debug.LogWarning($"No spell on {name}");
        spell.Init(spellInfo);
        textCost.text = spell.cost.ToString();
        textCooldown.gameObject.SetActive(false);
        imageCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
        gameObject.name = spellInfo.spellName;
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
        if (spell == null) {
            Debug.LogError($"No spell registered under {name}");
        } else if (!isCooldown) {
            spell.Init(spellInfo);
            StartCoroutine(spell.Activate(this));
        }
    }

    /*
    public void OnClick() {
        if (spell != null) UseSpell();
        else Debug.LogError($"No spell registered under {name}");
    }
    */

}
