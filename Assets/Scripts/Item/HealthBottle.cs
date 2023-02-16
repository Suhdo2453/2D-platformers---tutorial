using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBottle : MonoBehaviour
{
    [SerializeField]
    private float healthValue = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().AddHealth(healthValue);
            Destroy(gameObject);
        }
    }
}
