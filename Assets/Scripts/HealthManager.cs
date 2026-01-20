using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Heart UI")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private PlayerController player;

    private void Start()
    {
        player = PlayerController.Instance;
        if (player == null)
        {
            Debug.LogError("no PlayerController");
            return;
        }

        player.OnHealthChanged += UpdateHearts;
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHearts;
        }
    }

    // called only when health changes (not every frame)
    private void UpdateHearts(int currentHealth)
    {
        int healthToShow = Mathf.Clamp(currentHealth, 0, hearts.Length);

        // set all hearts to empty first
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        // set full hearts from left
        for (int i = 0; i < healthToShow; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }
}