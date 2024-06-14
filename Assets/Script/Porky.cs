using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porky : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private float _damageThreshold = 0.2f;
    [SerializeField] private GameObject _baddieDeathParticle;

    private float _currentHealth;

    private void Awake() {
        _currentHealth = _maxHealth;
    }

    public void DamagePorkie(float dmg) {
        _currentHealth -= dmg;
        if(_currentHealth <= 0f) {
            Die();
        }
    }

    private void Die() {
        GameManager.instance.RemovePorky(this);

        Instantiate(_baddieDeathParticle, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float impactVelocity = collision.relativeVelocity.magnitude;
        if(impactVelocity > _damageThreshold) {
            DamagePorkie(impactVelocity);
        }
    }
}
