using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{   
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject bullet2;
    [SerializeField] GameObject bullet3;
    [SerializeField] Transform shootPosition;   

    // Start is called before the first frame update
    void Start()
    {        
        //WeaponInAction.OnBulletProjectile += OnBulletProjectile_Moving;
    }

    private void OnBulletProjectile_Moving()
    {
        Instantiate(bullet1, shootPosition.position, transform.rotation);     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
