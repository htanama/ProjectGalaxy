using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // Bullet can damage player and enemy
    //[SerializeField] private Rigidbody rigidbody;
    [SerializeField] private int bulletDamageAmount;
    [SerializeField] private int speed; // how fast the bullet travel
    [SerializeField] private WeaponInformation weaponInformation;


    [Header("Shotgun")]
    [SerializeField][Range(0f, 1f)] float spreadArea;
    void Start()
    {       
        bulletDamageAmount = weaponInformation.shootDamage;      
        Projectiles_OnShootingBullet();
    }

    private void Projectiles_OnShootingBullet()
    {
        #if UNITY_EDITOR
                Debug.Log("Bullet - start moving");
        #endif

        int destroyTime = 10;
        
        float x = UnityEngine.Random.Range(-spreadArea, spreadArea);
        float y = UnityEngine.Random.Range(-spreadArea, spreadArea);

        Vector3 directionWithSpread = new Vector3(x, y, 0);
        this.GetComponent<Rigidbody>().AddForce(directionWithSpread);

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        
        Debug.Log($"Bullet position: {this.transform.position}");

        Destroy(gameObject, destroyTime);        
    }

    private void OnTriggerEnter(Collider other)
    {       
        //IDamage dmg = other.GetComponent<IDamage>();

        //if (dmg != null)
        //{
        //    dmg.TakeDamage(bulletDamageAmount);
        //}

        //#if UNITY_EDITOR
        //        Debug.Log($"{this.name} collide with {other.name}");
        //#endif
    }

    public int GetBulletDamageAmount()
    {
        return bulletDamageAmount;
    }

    // still thinking on the direction of projectile for shotgun spread
    public void ShotgunOnBulletSpread()
    {
        #if UNITY_EDITOR
                Debug.Log("Bullet - start moving");
        #endif

        int destroyTime = 10;

        float x = UnityEngine.Random.Range(-spreadArea, spreadArea);
        float y = UnityEngine.Random.Range(-spreadArea, spreadArea);

        Vector3 directionWithSpread = new Vector3(x, y, 0);
        this.GetComponent<Rigidbody>().AddForce(directionWithSpread);

        GetComponent<Rigidbody>().velocity = transform.forward * speed;

        Debug.Log($"Bullet position: {this.transform.position}");

        Destroy(gameObject, destroyTime);
    }
}