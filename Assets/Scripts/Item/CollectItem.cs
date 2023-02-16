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
    [SerializeField]
    private GameObject paticleItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            itemCountTxt.text = ++itemCount + "/10";
            audioSource.Play();
            Instantiate(paticleItem, collision.transform.position, Quaternion.identity);
        }else if (collision.CompareTag("HealthReponse"))
        {
            audioSource.Play();
            Instantiate(paticleItem, collision.transform.position, Quaternion.identity);
        }
    }
}
