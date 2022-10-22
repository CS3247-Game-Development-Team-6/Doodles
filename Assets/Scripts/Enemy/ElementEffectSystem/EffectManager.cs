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
        if (enemy.GetStatus() == Status.INVULNERABLE) {
            return;
        }

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
                    // refresh the effect time
                    _currentEffectTime = 0;
                    _nextTickTime = 0;
                    this._data = _data;
                    return; // to prevent stacking effects VFX
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
        enemy.SetStatus(Status.WEAKEN);
        this._data = _weakenedData;
    }

    private void setScaldedData() {
        RemoveEffect();
        enemy.SetStatus(Status.SCALD);
        this._data = _scaldedData;

        // Info for burstDot effect
        this._data.DOTAmount = _burstDotAmount;
    }

    private void setFrozenData() {
        RemoveEffect();
        enemy.SetStatus(Status.FROZE);
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
        if (enemy.GetStatus() == Status.CHILL) {
            enemy.RestoreBaseSpeed();
            enemy.RemoveStatus();
        }
        if (enemy.GetStatus() == Status.DRENCH) {
            enemy.RestoreAttack();
            enemy.RemoveStatus();
        }
        if (enemy.GetStatus() == Status.FROZE) {
            enemy.RestoreBaseSpeed();
            enemy.RemoveStatus();
        }
        if (enemy.GetStatus() == Status.WEAKEN) {
            enemy.RestoreDefense();
            enemy.RemoveStatus();
        }
        if (enemy.GetStatus() == Status.SCALD) {
            enemy.RemoveStatus();
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
        if (_currentEffectTime >= _data.Lifetime * SpellManager.instance.GetElementEffectLifetimeFactor()) RemoveEffect();

        // Enemy is currently not affected by any status effects
        if (_data == null) return;

        // Update status effect timer accordingly
        // Frozen Effect (Ice + Water)
        if (enemy.GetStatus() == Status.FROZE && _currentEffectTime > _nextTickTime) {
            // Only triggered once
            if (_nextTickTime == 0) {
                enemy.ReduceBaseSpeed(0);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // BurstDOT Effect (Fire + Water)
        else if (enemy.GetStatus() == Status.SCALD && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.TakeDot(_data.DOTAmount * _data.TickSpeed * _data.Lifetime * (SpellManager.instance.GetElementAugmentationFactor()));
            }
            _nextTickTime += _data.TickSpeed;
        }
        // DefDecre Effect (Ice + Fire)
        else if (enemy.GetStatus() == Status.WEAKEN && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceDefense((int)(_data.DefDecreAmount * (SpellManager.instance.GetElementAugmentationFactor())));
            }
            _nextTickTime += _data.TickSpeed;
        }
        // DOT Effect (Fire)
        else if (_data.DOTAmount != 0 && _currentEffectTime > _nextTickTime) {
            _nextTickTime += _data.TickSpeed;
            enemy.TakeDot(_data.DOTAmount * SpellManager.instance.GetElementAugmentationFactor());
        }
        // Slow Effect (Ice)
        else if (_data.SlowAmount != 0 && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceBaseSpeed(_data.SlowAmount * SpellManager.instance.GetElementAugmentationFactor());
                enemy.SetStatus(Status.CHILL);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // Attack Decrease Effect (Water)
        else if (_data.AtkDecreAmount != 0 && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceAttack((int)(_data.AtkDecreAmount * SpellManager.instance.GetElementAugmentationFactor()));
                enemy.SetStatus(Status.DRENCH);
            }
            _nextTickTime += _data.TickSpeed;
        }
    }
}
