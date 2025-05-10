using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Input
{
  public class PlayerMovement : MonoBehaviour
  {
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private AudioSource plAudioSource;

    [SerializeField] private float speed = 3;
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private float footstepDelay = 0.5f;
        
    private float _timeSinceLastStep;
    private bool _wasMoving;

    private void Awake()
    {
      plAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
      _playerInput = GetComponent<PlayerInput>();
      _moveAction = _playerInput.actions.FindAction("Move");
    }

    private void Update()
    {
      MovePlayer();
      HandleFootsteps();
    }

    private void MovePlayer()
    {
      var direction = _moveAction.ReadValue<Vector2>();
      transform.position += new Vector3(direction.x, direction.y, 0) * 
                            (speed * Time.deltaTime);
            
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
      if (footstepSound != null && plAudioSource != null && !plAudioSource.isPlaying)
      {
        plAudioSource.PlayOneShot(footstepSound);
      }
    }
  }
}
