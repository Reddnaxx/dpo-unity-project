using _00_Scripts.Game.Player;

using UnityEngine;

namespace _00_Scripts.Input
{
  public class EnemyFollow : MonoBehaviour
  {
    [SerializeField] public Transform player;
    [SerializeField] public float speed = 2f;

    private void Start() => player = FindFirstObjectByType<PlayerCharacter>().transform;

    private void Update()
    {
      if (!player) return;

      var direction = (player.position - transform.position).normalized;

      transform.position += direction * (speed * Time.deltaTime);
    }
  }
}
