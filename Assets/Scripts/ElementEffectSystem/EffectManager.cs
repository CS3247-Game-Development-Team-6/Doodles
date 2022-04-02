using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour, IEffectable
{
    private Enemy enemy;
    
    // For Status Effects
    private StatusEffectData _data;
    private GameObject _effectParticles;
    private float _currentEffectTime = 0f;
    private float _nextTickTime = 0f;

    // For Special Effects
    [SerializeField] private StatusEffectData _scaldedData;
    [SerializeField] private StatusEffectData _frozenData;
    [SerializeField] private StatusEffectData _weakenedData;

    // For Burst DOT Effect
    private float _burstDotAmount;
    private float _burstTickSpeed;
    private float _burstLifetime;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_data != null) HandleEffect();
    }


    // Apply Status Effect on enemy
    public void ApplyEffect(StatusEffectData _data)
    {
        // Check if already has 2 elements
        if (_data != null) {
            if (this._data != null)     // Enemy is already inflicted with status effect
            {
                // Different element (Decide which elemental reaction data to use)
                if (this._data.Element != _data.Element)
                {
                    // Frozen
                    if ((this._data.Element == "Ice" && _data.Element == "Water") || (this._data.Element == "Water" && _data.Element == "Ice"))
                    {
                        enemy.setChilled(true);
                        enemy.setDrenched(true);
                        RemoveEffect();
                        enemy.setFrozen(true);
                        this._data = _frozenData;
                    }
                    // Scalded
                    else if (this._data.Element == "Fire" && _data.Element == "Water" || this._data.Element == "Water" && _data.Element == "Fire")
                    {                       
                        if (this._data.Element == "Fire")
                        {
                            _burstDotAmount = this._data.DOTAmount;
                            _burstTickSpeed = this._data.TickSpeed;
                            _burstLifetime = this._data.Lifetime;
                        }
                        else if (_data.Element == "Fire")
                        {
                            _burstDotAmount = _data.DOTAmount;
                            _burstTickSpeed = _data.TickSpeed;
                            _burstLifetime = _data.Lifetime;
                        }
                        enemy.setDrenched(true);
                        RemoveEffect();
                        enemy.setScalded(true);

                        this._data = _scaldedData;
                        // Info for burstDot effect
                        this._data.DOTAmount = _burstDotAmount;
                        this._data.TickSpeed = _burstTickSpeed;
                        this._data.Lifetime = _burstLifetime;
                    }
                    // Weakened
                    else if (this._data.Element == "Ice" && _data.Element == "Fire" || this._data.Element == "Fire" && _data.Element == "Ice")
                    {                       
                        enemy.setChilled(true);
                        RemoveEffect();
                        enemy.setWeakened(true);
                        this._data = _weakenedData;
                    }
                }
                // Same element
                else if (this._data.Element != "None")
                {
                    RemoveEffect();
                    this._data = _data;
                }
            }
            else this._data = _data;    // Enemy not inflicted with status effect yet

            _effectParticles = Instantiate(this._data.EffectParticles, transform);
            
        }
    }

    
    // Remove Status Effect on enemy
    public void RemoveEffect()
    {
        if (enemy.getChilled())
        {
            enemy.RestoreSpeed();
            enemy.setChilled(false);
        }
        if (enemy.getDrenched())
        {
            enemy.RestoreAttack();
            enemy.setDrenched(false);
        }
        if (enemy.getFrozen())
        {
            enemy.RestoreSpeed();
            enemy.setFrozen(false);
        }
        if (enemy.getWeakened())
        {
            enemy.RestoreDefense();
            enemy.setWeakened(false);
        }

        _data = null;
        _currentEffectTime = 0;
        _nextTickTime = 0;
        if (_effectParticles != null) Destroy(_effectParticles);
    }

    // Handle Status Effect Updates
    public void HandleEffect()
    {
        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime >= _data.Lifetime) RemoveEffect();

        if (_data == null) return;

        // Frozen Effect (Ice + Water)
        if (enemy.getFrozen() && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _data.TickSpeed;
            enemy.ReduceSpeed(0);
        }
        else if (enemy.getScalded() && _currentEffectTime > _nextTickTime)
        {
            // Only triggered once
            if (_nextTickTime == 0)
            {
                enemy.TakeDot(_data.DOTAmount * _data.TickSpeed * _data.Lifetime);
            }
            _nextTickTime += _data.TickSpeed;
        }
        else if (enemy.getWeakened() && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _data.TickSpeed;
            enemy.ReduceDefense(_data.DefDecreAmount);
        }
        // DOT Effect
        else if (_data.DOTAmount != 0 && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _data.TickSpeed;
            enemy.TakeDot(_data.DOTAmount);
        }
        // Slow Effect
        else if (_data.SlowAmount != 0 && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _data.TickSpeed;
            enemy.ReduceSpeed(_data.SlowAmount);
        }
        // Attack Decrease Effect
        else if (_data.AtkDecreAmount != 0 && _currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _data.TickSpeed;
            enemy.ReduceAttack(_data.AtkDecreAmount);
        }
    }
}
