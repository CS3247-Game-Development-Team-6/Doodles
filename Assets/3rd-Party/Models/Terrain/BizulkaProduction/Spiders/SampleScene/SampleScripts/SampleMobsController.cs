using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// This is sample script
/// </summary>
public class SampleMobsController : MonoBehaviour
{
    [SerializeField] private AnimationsController[] _animationsControllers;
    [SerializeField] private Text _text;


    IEnumerator ShowText(string text, float time)
    {
        _text.text = text;
        yield return new WaitForSeconds(time);
        _text.text = "Idle";
    }
    IEnumerator DoAttack()
    {
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.Attack();
            yield return new WaitForSeconds(Random.value * 0.1f);
        }
    }

    IEnumerator DoHit()
    {
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.Hit();
            yield return new WaitForSeconds(Random.value * 0.1f);
        }
    }

    IEnumerator DoMove()
    {
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.SetMovingState(true);
        }

        yield return new WaitForSeconds(4.2f);
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.SetMovingState(false);
        }
    }

    IEnumerator DoDeath()
    {
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.ClearDead();
            animationsController.SetDead();
            yield return new WaitForSeconds(Random.value * 0.1f);
        }

        yield return new WaitForSeconds(1.2f);
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.ClearDead();
        }
    }

    void ClearAll()
    {
        StopAllCoroutines();
        foreach (var animationsController in _animationsControllers)
        {
            animationsController.ClearDead();
            animationsController.SetMovingState(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClearAll();
            StartCoroutine(ShowText("Run", 4));
            StartCoroutine(DoMove());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClearAll();
            StartCoroutine(ShowText("Hit",2));
            StartCoroutine(DoHit());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClearAll();
            StartCoroutine(ShowText("Death",2));
            StartCoroutine(DoDeath());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ClearAll();
            StartCoroutine(ShowText("Attack",2));
            StartCoroutine(DoAttack());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearAll();
            StartCoroutine(ShowText("Egg explosions",4));
        }
    }
}