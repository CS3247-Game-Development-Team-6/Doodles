using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Text text;
    public float lifetime = 0.5f;
    public float minDist = 2f;
    public float maxDist = 3f;

    private Vector3 initPos;
    private Vector3 targetPos;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        float direction = Random.rotation.eulerAngles.z;

        Vector3 offset = Vector3.zero + Vector3.up;
        initPos = transform.position + offset;
        float dist = Random.Range(minDist, maxDist);
        targetPos = initPos + (Quaternion.Euler(0, 0, direction) * new Vector3(dist, dist, 0f));
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        float fraction = lifetime / 2f;

        if (timer > lifetime) Destroy(gameObject);
        // fading out
        else if (timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime - fraction));

        transform.localPosition = Vector3.Lerp(initPos, targetPos, Mathf.Sin(timer / lifetime));

        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));

    }

    public void SetDamageText(int damage)
    {
        text.text = damage.ToString();
    }
}
