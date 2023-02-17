using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectItem : MonoBehaviour
{
    [SerializeField]
    private Text itemCountTxt;

    private int itemCount = 0;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameObject paticleItem;
    [SerializeField]
    private string scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            itemCountTxt.text = ++itemCount + "/10";
            audioSource.Play();
            Instantiate(paticleItem, collision.transform.position, Quaternion.identity);
            if(itemCount == 10)
            {
                Invoke("LoadScene", 2);
                
            }
        }else if (collision.CompareTag("HealthReponse"))
        {
            audioSource.Play();
            Instantiate(paticleItem, collision.transform.position, Quaternion.identity);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }
}
