using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour, ICollectable
{
    [SerializeField] private int _healAmount = 1;

    public void Collect(PlayerController player)
    {
        player.Heal(_healAmount);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Collect(player);
        }
    }
}
