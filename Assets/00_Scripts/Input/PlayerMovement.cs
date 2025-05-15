using UnityEngine;
using UnityEngine.InputSystem;

namespace _00_Scripts.Input
{
  public class PlayerMovement : MonoBehaviour
  {
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    [SerializeField] private float speed = 3;

    private void Start()
    {
      _playerInput = GetComponent<PlayerInput>();

      _moveAction = _playerInput.actions.FindAction("Move");
    }

    private void Update()
    {
      MovePlayer();
    }

    private void MovePlayer()
    {
      var direction = _moveAction.ReadValue<Vector2>();
      transform.position += new Vector3(direction.x, direction.y, 0) *
                            (speed * Time.deltaTime);
    }
  }
}
