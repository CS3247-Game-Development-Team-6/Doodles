using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpellManager instance = null;
    public float DamageFactor = 1.0f;
    public float InkFactor = 1.0f;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public IEnumerator HandleEffect(GlobalEffect effect,float EffectTime)
    {
        
        // but the next line not works, don't know why!
        effect.Activate();
        yield return new WaitForSeconds(EffectTime);
        effect.Deactivate();
    }
    public void DeactivateEffect(GlobalEffect effect)
    {
        effect.Deactivate();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
