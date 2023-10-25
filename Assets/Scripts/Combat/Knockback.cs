using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;
    [SerializeField] private float _knockbackTime = 0.2f;

    private Vector3 _hitDirection;
    private float _knockbackThrust;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackForce;
        OnKnockbackEnd += StopKnockROutine;
    }

    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKnockROutine;
    }

    public void GetKnockedBack(Vector3 hitdirection, float knockbackTHrust)
    {
        _hitDirection = hitdirection;
        _knockbackThrust = knockbackTHrust;

        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockbackForce()
    {
        Vector3 difference = (transform.position- _hitDirection).normalized * _knockbackThrust * _rigidBody.mass;
        _rigidBody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        OnKnockbackEnd?.Invoke();
    }

    private void StopKnockROutine()
    {
        _rigidBody.velocity = Vector2.zero;
    }
}
