using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ManaItem : MonoBehaviour ,ICollectable
{
    [SerializeField] private int _manaAmount = 1;

    public void Collect(PlayerController player)
    {
        player.RecoveryMana(_manaAmount);
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
