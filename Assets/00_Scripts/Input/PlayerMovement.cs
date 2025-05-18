using _00_Scripts.Game.Entity;
using _00_Scripts.Game.Player;

using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Input
{
  [RequireComponent(typeof(AudioSource), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
  public class PlayerMovement : MonoBehaviour
  {
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private float footstepDelay = 0.5f;
    private InputAction _moveAction;
    private AudioSource _plAudioSource;
    private PlayerInput _playerInput;

    private IStats _playerStats;
    private Rigidbody2D _rigidbody2D;

    private float _timeSinceLastStep;
    private bool _wasMoving;
    private float Speed => _playerStats.Speed;

    private void Awake()
    {
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _plAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
      _playerStats = PlayerCharacter.Stats;
      _playerInput = GetComponent<PlayerInput>();
      _moveAction = _playerInput.actions.FindAction("Move");
    }

    private void Update() => HandleFootsteps();

    private void FixedUpdate() => MovePlayer();

    private void MovePlayer()
    {
      var direction = _moveAction.ReadValue<Vector2>();
      var movement = new Vector2(direction.x, direction.y).normalized;

      _rigidbody2D.MovePosition(_rigidbody2D.position + movement * (Speed * Time.deltaTime));

      // Обновляем состояние движения
      _wasMoving = direction.magnitude > 0.1f;
    }

    private void HandleFootsteps()
    {
      if (!_wasMoving)
      {
        _timeSinceLastStep = 0;
        return;
      }

      _timeSinceLastStep += Time.deltaTime;

      if (_timeSinceLastStep >= footstepDelay)
      {
        PlayFootstep();
        _timeSinceLastStep = 0;
      }
    }

    private void PlayFootstep()
    {
      if (footstepSound != null && _plAudioSource != null && !_plAudioSource.isPlaying)
        _plAudioSource.PlayOneShot(footstepSound);
    }
  }
}
