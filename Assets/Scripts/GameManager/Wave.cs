using UnityEngine;
using System.Collections; // IEnumerator


[System.Serializable]
public class Wave 
{
    public GameObject enemy;
    public int count;
    public float rate;

    // optional
    public GameObject enemy2;
    public int count2;
    public float rate2;

    public GameObject enemy3;
    public int count3;
    public float rate3;

    public readonly int maxEnemyVariant = 3;

    public bool isEnemySpawnable(GameObject enemy, int count, float rate)
    {
        bool isValidEnemy = enemy != null;
        bool isValidCount = count > 0;
        bool isValidRate = rate > 0;
        return isValidEnemy && isValidCount && isValidRate;
    }
    public int getTotalEnemy()
    {
        GameObject _enemy = null;
        int _count = 0;
        float _rate = 0;

        int res = 0;

        // loop every enemy type in this wave
        for (int i = 0; i < maxEnemyVariant; i++)
        {
            switch (i)
            {
                case 0:
                    _enemy = enemy;
                    _count = count;
                    _rate = rate;
                    break;
                case 1:
                    _enemy = enemy2;
                    _count = count2;
                    _rate = rate2;
                    break;
                case 2:
                    _enemy = enemy3;
                    _count = count3;
                    _rate = rate3;
                    break;
                default:
                    Debug.LogError("something is wrong in Wave.cs");
                    break;

            }

            if (isEnemySpawnable(_enemy, _count, _rate))
            {
                res += _count;
            }

        }

        return res;
    }

    public IEnumerator StartWave(WaveSpawner waveSpawner)
    {
        GameObject _enemy = null;
        int _count = 0;
        float _rate = 0;

        // loop every enemy type in this wave
        for (int i = 0; i < maxEnemyVariant; i++)
        {          
            switch (i)
            {
                case 0:
                    _enemy = enemy;
                    _count = count;
                    _rate = rate;
                    break;
                case 1:
                    _enemy = enemy2;
                    _count = count2;
                    _rate = rate2;
                    break;
                case 2:
                    _enemy = enemy3;
                    _count = count3;
                    _rate = rate3;
                    break;
                default:
                    Debug.LogError("something is wrong in Wave.cs");
                    break;

            }

            if (isEnemySpawnable(_enemy, _count, _rate))
            {
                for (int j = 0; j < _count; j++)
                {
                    waveSpawner.SpawnEnemy(_enemy);
                    yield return new WaitForSeconds(1f / _rate);
                }
            }

        }

        WaveSpawner.isSpawningEnemy = false;
    }  


}
