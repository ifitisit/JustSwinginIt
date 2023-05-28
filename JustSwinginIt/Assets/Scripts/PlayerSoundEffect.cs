using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip shootSFX;

    private GameObject grapplingGun;

    private void Awake()
    {
        grapplingGun = GameObject.Find("GrapplingGun");
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GetComponent<AudioSource>().PlayOneShot(shootSFX);
        }

        if (grapplingGun.GetComponent<GrapplingGun>().onGround())
        {
            if (!Input.GetKey(KeyCode.Mouse0)){
                GetComponent<AudioSource>().Stop();
            }
        }
            
    }
}

    
