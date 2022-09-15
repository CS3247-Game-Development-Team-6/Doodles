using UnityEngine;

public class EffectManager : MonoBehaviour, IEffectable {
    private Enemy enemy;

    // For Status Effects
    private ElementEffectInfo _data;
    private GameObject _effectParticles;
    private float _currentEffectTime = 0f;
    private float _nextTickTime = 0f;

    // For Combined Effects
    [SerializeField] private ElementEffectInfo _scaldedData;
    [SerializeField] private ElementEffectInfo _frozenData;
    [SerializeField] private ElementEffectInfo _weakenedData;

    // For Burst DOT Effect
    private float _burstDotAmount;

    void Start() {
        enemy = GetComponentInParent<Enemy>();
    }

    void Update() {
        if (_data != null) HandleEffect();
    }

    public void ApplyEffect(ElementEffectInfo _data) {
        // Check if already has 2 elements
        if (_data != null) {
            // Enemy is already inflicted with a status effect
            if (this._data != null) {
                // Enemy is already inflicted with elemental reaction effect
                if (this._data.Element == ElementEffectType.COMBINED) {
                    return;
                }

                // Different element is inflicted on enemy
                if (this._data.Element != _data.Element) {
                    // Frozen
                    if ((this._data.Element == ElementEffectType.ICE && _data.Element == ElementEffectType.WATER)
                        || (this._data.Element == ElementEffectType.WATER && _data.Element == ElementEffectType.ICE)) {
                        setFrozenData();
                    }
                    // Scalded
                    else if (this._data.Element == ElementEffectType.FIRE && _data.Element == ElementEffectType.WATER
                        || this._data.Element == ElementEffectType.WATER && _data.Element == ElementEffectType.FIRE) {
                        setBurstDOTAmount(_data);
                        setScaldedData();
                    }
                    // Weakened
                    else if (this._data.Element == ElementEffectType.ICE && _data.Element == ElementEffectType.FIRE
                        || this._data.Element == ElementEffectType.FIRE && _data.Element == ElementEffectType.ICE) {
                        setWeakenedData();
                    }
                }
                // Same element is inflicted on enemy
                else {
                    RemoveEffect();
                    this._data = _data;
                }
            }
            // Enemy not inflicted with status effect yet
            else
                this._data = _data;

            // Spawn particle effects for elements and elemental reactions
            _effectParticles = Instantiate(this._data.EffectParticles, transform);

        }
    }

    private void setWeakenedData() {
        RemoveEffect();
        enemy.setStatus(Status.WEAKEN);
        this._data = _weakenedData;
    }

    private void setScaldedData() {
        RemoveEffect();
        enemy.setStatus(Status.SCALD);
        this._data = _scaldedData;

        // Info for burstDot effect
        this._data.DOTAmount = _burstDotAmount;
    }

    private void setFrozenData() {
        RemoveEffect();
        enemy.setStatus(Status.FROZE);
        this._data = _frozenData;
    }

    private void setBurstDOTAmount(ElementEffectInfo _data) {
        if (this._data.Element == ElementEffectType.FIRE) {
            _burstDotAmount = this._data.DOTAmount * this._data.TickSpeed * this._data.Lifetime;
        } else if (_data.Element == ElementEffectType.FIRE) {
            _burstDotAmount = _data.DOTAmount * _data.TickSpeed * _data.Lifetime;
        }
    }

    public void RemoveEffect() {
        if (enemy.getStatus() == Status.CHILL) {
            enemy.RestoreSpeed();
            enemy.removeStatus();
        }
        if (enemy.getStatus() == Status.DRENCH) {
            enemy.RestoreAttack();
            enemy.removeStatus();
        }
        if (enemy.getStatus() == Status.FROZE) {
            enemy.RestoreSpeed();
            enemy.removeStatus();
        }
        if (enemy.getStatus() == Status.WEAKEN) {
            enemy.RestoreDefense();
            enemy.removeStatus();
        }
        if (enemy.getStatus() == Status.SCALD) {
            enemy.removeStatus();
        }

        _data = null;
        _currentEffectTime = 0;
        _nextTickTime = 0;
        if (_effectParticles != null) Destroy(_effectParticles);
    }

    // Handle Status Effect Updates
    public void HandleEffect() {
        // Update current effect's time
        _currentEffectTime += Time.deltaTime;

        // Remove status effect if lifetime is reached
        if (_currentEffectTime >= _data.Lifetime) RemoveEffect();

        // Enemy is currently not affected by any status effects
        if (_data == null) return;

        // Update status effect timer accordingly
        // Frozen Effect (Ice + Water)
        if (enemy.getStatus() == Status.FROZE && _currentEffectTime > _nextTickTime) {
            // Only triggered once
            if (_nextTickTime == 0) {
                enemy.ReduceSpeed(0);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // BurstDOT Effect (Fire + Water)
        else if (enemy.getStatus() == Status.SCALD && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.TakeDot(_data.DOTAmount * _data.TickSpeed * _data.Lifetime);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // DefDecre Effect (Ice + Fire)
        else if (enemy.getStatus() == Status.WEAKEN && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceDefense(_data.DefDecreAmount);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // DOT Effect (Fire)
        else if (_data.DOTAmount != 0 && _currentEffectTime > _nextTickTime) {
            _nextTickTime += _data.TickSpeed;
            enemy.TakeDot(_data.DOTAmount);
        }
        // Slow Effect (Ice)
        else if (_data.SlowAmount != 0 && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceSpeed(_data.SlowAmount);
                enemy.setStatus(Status.CHILL);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // Attack Decrease Effect (Water)
        else if (_data.AtkDecreAmount != 0 && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceAttack(_data.AtkDecreAmount);
                enemy.setStatus(Status.DRENCH);
            }
            _nextTickTime += _data.TickSpeed;
        }
    }
}
