using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is sample script
/// </summary>
public class SampleEggDestroyScript : MonoBehaviour
{
    [SerializeField] private EggController[] _eggControllers;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DestroyEggs());
        }
    }

    IEnumerator DestroyEggs()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < _eggControllers.Length; i++)
        {
            _eggControllers[i].Destroy();
            yield return new WaitForSeconds(Random.value);
            
        }
    }
}