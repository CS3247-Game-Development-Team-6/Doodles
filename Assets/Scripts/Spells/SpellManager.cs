using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager instance = null;
    public int doodleDamageIncreasing=0;
    private float elementEffectLifeTimeFactor = 1.0f;
    private float elementEffectAugmentationFactor = 1.0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float GetElementEffectLifetimeFactor()
    {
        return elementEffectLifeTimeFactor;
    }

    public float GetElementAugmentationFactor()
    {
        return elementEffectAugmentationFactor;
    }
    public void ActivateElementBurst(float lifeTimeFactor,float augmentationFactor)
    {
        elementEffectLifeTimeFactor = lifeTimeFactor;
        elementEffectAugmentationFactor = augmentationFactor;
    }

    public void DeActivateElementBurst()
    {
        elementEffectLifeTimeFactor = 1.0f;
        elementEffectAugmentationFactor = 1.0f;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
