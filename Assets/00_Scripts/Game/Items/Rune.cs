using UnityEngine;

namespace _00_Scripts.Game.Items
{
  public class Rune : MonoBehaviour
  {
    [SerializeField] private GameObject hintText;
    private bool isPlayerInRange = false;

    private void Start()
    {
      if (hintText != null)
      {
        Debug.Log("HintText assigned: " + hintText.name);
        hintText.SetActive(false);
      }
      else
      {
        Debug.LogWarning("HintText is not assigned in the Inspector!");
      }
    }

    private void Update()
    {
      if (isPlayerInRange && UnityEngine.Input.GetKeyDown(KeyCode.E))
      {
        Debug.Log("E key pressed, collecting rune");
        CollectRune();
      }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      Debug.Log("Trigger entered by: " + other.name);
      if (other.CompareTag("Player"))
      {
        Debug.Log("Player entered rune trigger");
        isPlayerInRange = true;
        if (hintText != null)
        {
          hintText.SetActive(true);
          Debug.Log("HintText activated");
        }
      }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      Debug.Log("Trigger exited by: " + other.name);
      if (other.CompareTag("Player"))
      {
        Debug.Log("Player exited rune trigger");
        isPlayerInRange = false;
        if (hintText != null)
        {
          hintText.SetActive(false);
          Debug.Log("HintText deactivated");
        }
      }
    }

    private void CollectRune()
    {
      Debug.Log("Rune collected!");
      Destroy(gameObject);
    }
  }
}
