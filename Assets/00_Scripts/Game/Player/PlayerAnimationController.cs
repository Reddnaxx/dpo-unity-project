using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Game.Player
{
  [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
  public class PlayerAnimationController : MonoBehaviour
  {
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private Animator _animator;

    private PlayerInput _input;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
      _input = FindFirstObjectByType<PlayerInput>();
      _animator = GetComponent<Animator>();
      _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
      _spriteRenderer.flipX = _input.actions["Move"].ReadValue<Vector2>().x < 0;
      _animator.SetBool(IsMoving, _input.actions["Move"].IsPressed());
    }
  }
}
