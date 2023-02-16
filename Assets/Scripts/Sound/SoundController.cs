using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource meleeAttack;
    [SerializeField]
    private AudioSource bow;
    [SerializeField]
    private AudioSource jump;
    [SerializeField]
    private AudioSource hurt;
    [SerializeField]
    private AudioSource rangeAttack;

    private void MeleeAttack()
    {
        if (!meleeAttack.isPlaying)
        {
            meleeAttack.Play();
        }
        
    }

    private void Bow()
    {
        bow.Play();
    }

    private void Jump()
    {
        jump.Play();
    }

    private void Hurt()
    {
        hurt.Play();
    }

    private void RangeAttack()
    {
        rangeAttack.Play();
    }
}
