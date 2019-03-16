using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] float startForce;
    [SerializeField] float maxVerocity;
    [SerializeField] PhysicsMaterial2D physicsMaterialSpeedUpOnBound;
    [SerializeField] PhysicsMaterial2D physicsMaterialFixedSpeed;
    [SerializeField] SpriteRenderer boundEffect;

    public int BoundCount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody.velocity.magnitude >= maxVerocity)
        {
            rigidBody.sharedMaterial = physicsMaterialFixedSpeed;
        }
        else
        {
            rigidBody.sharedMaterial = physicsMaterialSpeedUpOnBound;
        }
    }

    public void StartMove()
    {
        var vec = new Vector2(startForce, 0f);
        vec = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * vec;
        GetComponent<Rigidbody2D>().AddForce(vec, ForceMode2D.Impulse);
    }

    public void Bound()
    {
        BoundCount++;

        var seq = DOTween.Sequence();
        seq.Append(boundEffect.transform.DOScale(Vector3.one, 0f));
        seq.Join(boundEffect.DOFade(1f, 0f));
        seq.Append(boundEffect.transform.DOScale(Vector3.one * 4, 1f));
        seq.Join(boundEffect.DOFade(0f, 1f));
        seq.Play();
    }
}
