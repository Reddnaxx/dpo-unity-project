using System;
using UniRx;
using UnityEngine;

namespace _00_Scripts.Game.Weapon
{
  public abstract class Weapon : MonoBehaviour
  {
    [SerializeField] protected Rigidbody2D projectilePrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float fireRate = 0.5f;
    [SerializeField] private float orderChangeAngle = 45f;

    [SerializeField] private SpriteRenderer body;

    private static Vector2 MousePosition =>
      Camera.main?.ScreenToWorldPoint(UnityEngine.Input.mousePosition) ??
      Vector2.zero;

    protected virtual void Start()
    {
      Observable.EveryUpdate()
        .Where(_ => UnityEngine.Input.GetButton("Fire1"))
        .ThrottleFirst(TimeSpan.FromSeconds(fireRate))
        .Subscribe(_ => Shoot())
        .AddTo(this);
    }

    protected virtual void Update()
    {
      var direction = MousePosition - (Vector2)transform.position;
      var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
      var orderMaxAngle = 90 + orderChangeAngle;
      var orderMinAngle = 90 - orderChangeAngle;

      body.flipY = Mathf.Abs(angle) > 90;
      body.sortingOrder = angle > orderMinAngle && angle < orderMaxAngle ? 0 : 1;
    }

    protected virtual void Shoot()
    {
      Instantiate(projectilePrefab, firePoint.position,
        transform.rotation);
    }
  }
}