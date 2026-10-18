using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] ParticleSystem VFXPrefab;
    [SerializeField] float MaxHp = 1;
    // Lasers damage every frame, so the hit VFX is throttled
    [SerializeField] float vfxInterval = 0.25f;
    float _currentHP;
    float _nextVfxTime;
    public bool isDead => _currentHP <= 0;

    void Start()
    {
        _currentHP = MaxHp;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        _currentHP -= damage;

        if (isDead || Time.time >= _nextVfxTime)
        {
            _nextVfxTime = Time.time + vfxInterval;
            if (VFXPrefab) Instantiate(VFXPrefab, transform.position, transform.rotation);
        }

        if (isDead) Destroy(gameObject);
    }
}
