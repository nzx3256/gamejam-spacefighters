using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out audioSource))
        {
            ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ;
    }
    public void PlayExplosion()
    {
        audioSource.Play();
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
