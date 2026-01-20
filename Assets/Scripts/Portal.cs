using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private AudioSource victoryAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                victoryAudio.Play();
                player.Win();
            }
            else
            {
                Debug.LogError("No PlayerController on colliding object!");
            }
        }
    }
}