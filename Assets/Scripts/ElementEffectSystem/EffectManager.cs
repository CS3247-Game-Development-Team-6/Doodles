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
                if (this._data.Element == "Combined") {
                    return;
                }

                // Different element is inflicted on enemy
                if (this._data.Element != _data.Element) {
                    // Frozen
                    if ((this._data.Element == "Ice" && _data.Element == "Water") || (this._data.Element == "Water" && _data.Element == "Ice")) {
                        setFrozenData();
                    }
                    // Scalded
                    else if (this._data.Element == "Fire" && _data.Element == "Water" || this._data.Element == "Water" && _data.Element == "Fire") {
                        setBurstDOTAmount(_data);
                        setScaldedData();
                    }
                    // Weakened
                    else if (this._data.Element == "Ice" && _data.Element == "Fire" || this._data.Element == "Fire" && _data.Element == "Ice") {
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
        enemy.setEffectStatus(Enemy.EffectStatus.weaken);
        this._data = _weakenedData;
    }

    private void setScaldedData() {
        RemoveEffect();
        enemy.setEffectStatus(Enemy.EffectStatus.scald);
        this._data = _scaldedData;

        // Info for burstDot effect
        this._data.DOTAmount = _burstDotAmount;
    }

    private void setFrozenData() {
        RemoveEffect();
        enemy.setEffectStatus(Enemy.EffectStatus.froze);
        this._data = _frozenData;
    }

    private void setBurstDOTAmount(ElementEffectInfo _data) {
        if (this._data.Element == "Fire") {
            _burstDotAmount = this._data.DOTAmount * this._data.TickSpeed * this._data.Lifetime;
        } else if (_data.Element == "Fire") {
            _burstDotAmount = _data.DOTAmount * _data.TickSpeed * _data.Lifetime;
        }
    }

    public void RemoveEffect() {
        if (enemy.getEffectStatus() == Enemy.EffectStatus.chill) {
            enemy.RestoreSpeed();
            enemy.removeEffectStatus();
        }
        if (enemy.getEffectStatus() == Enemy.EffectStatus.drench) {
            enemy.RestoreAttack();
            enemy.removeEffectStatus();
        }
        if (enemy.getEffectStatus() == Enemy.EffectStatus.froze) {
            enemy.RestoreSpeed();
            enemy.removeEffectStatus();
        }
        if (enemy.getEffectStatus() == Enemy.EffectStatus.weaken) {
            enemy.RestoreDefense();
            enemy.removeEffectStatus();
        }
        if (enemy.getEffectStatus() == Enemy.EffectStatus.scald) {
            enemy.removeEffectStatus();
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
        if (enemy.getEffectStatus() == Enemy.EffectStatus.froze && _currentEffectTime > _nextTickTime) {
            // Only triggered once
            if (_nextTickTime == 0) {
                enemy.ReduceSpeed(0);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // BurstDOT Effect (Fire + Water)
        else if (enemy.getEffectStatus() == Enemy.EffectStatus.scald && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.TakeDot(_data.DOTAmount * _data.TickSpeed * _data.Lifetime);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // DefDecre Effect (Ice + Fire)
        else if (enemy.getEffectStatus() == Enemy.EffectStatus.weaken && _currentEffectTime > _nextTickTime) {
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
                enemy.setEffectStatus(Enemy.EffectStatus.chill);
            }
            _nextTickTime += _data.TickSpeed;
        }
        // Attack Decrease Effect (Water)
        else if (_data.AtkDecreAmount != 0 && _currentEffectTime > _nextTickTime) {
            if (_nextTickTime == 0) {
                enemy.ReduceAttack(_data.AtkDecreAmount);
                enemy.setEffectStatus(Enemy.EffectStatus.drench);
            }
            _nextTickTime += _data.TickSpeed;
        }
    }
}
