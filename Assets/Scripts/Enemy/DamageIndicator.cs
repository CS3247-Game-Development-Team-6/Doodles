using UnityEngine;
using UnityEngine.UI;

/**
 * Spawn damage text that look at the camera. Text will go from the initPos to targetPos using interpolation.
 */
public class DamageIndicator : MonoBehaviour {
    public Text text;
    public float lifetime = 0.5f;
    public float dist = 2f;

    private Vector3 initPos;
    private Vector3 targetPos;
    private float timer;

    private void Start() {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        Vector3 offset = Vector3.zero + Vector3.up;
        initPos = transform.position + offset;
        targetPos = initPos + new Vector3(dist, dist, 0f);

        transform.localScale = Vector3.zero;
    }

    private void Update() {
        timer += Time.deltaTime;

        float fraction = lifetime / 2f;

        if (timer > lifetime) Destroy(gameObject);
        // fading out
        else if (timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));

        transform.position = Vector3.Lerp(initPos, targetPos, Mathf.Sin(timer / lifetime));

        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));

    }

    public void SetDamageText(int damage) {
        text.text = damage.ToString();
    }

    public void SetDamageTextFromFloat(float damage) {
        text.text = damage.ToString();
    }
}
