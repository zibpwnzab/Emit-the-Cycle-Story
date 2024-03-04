using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] ParticleSystem VFXPrefab;
    [SerializeField] float MaxHp = 1;
    float _currentHP;
    public bool isDead => _currentHP <= 0;

    // Start is called before the first frame update
    void Start()
    {
        _currentHP = MaxHp;
    }


    public void TakeDamage(float damage)
    {
        if (VFXPrefab) Instantiate(VFXPrefab, transform.position, transform.rotation);
        _currentHP -= damage;
        if (isDead) Destroy(gameObject);

    }

}
