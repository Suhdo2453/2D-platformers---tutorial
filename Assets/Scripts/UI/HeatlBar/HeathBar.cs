using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerState;

    private float currentHealth;


    private void Start()
    {
        
        Debug.Log(playerState.currentHealth);
        Demo();
    }

    private void Update()
    {
        Demo();
    }

    private void Demo()
    {
        int i = 0;
        currentHealth = playerState.currentHealth / 10;
        foreach (Image g in transform.GetComponentsInChildren<Image>())
        {
            if(i >= currentHealth)
            {
                g.fillAmount = 0;
            }
            else
            {
                g.fillAmount = 1;
            }
            ++i;
        }
    }
}
