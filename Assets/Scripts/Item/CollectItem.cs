using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectItem : MonoBehaviour
{
    [SerializeField]
    private Text itemCountTxt;

    private int itemCount = 0;
    [SerializeField]
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            itemCountTxt.text = ++itemCount + "/10";
            audioSource.Play();
        }else if (collision.CompareTag("HealthReponse"))
        {
            audioSource.Play();

        }
    }
}
