/*
    Author: Harry Tanama
    Edited by: Juan Contreras
    Date Created: 01/18/2025
    Date Updated: 02/02/2025
    Description: Script to handle all gun functionalities and store gun info from scriptables
                 **GUN CONTROLS, DOES NOT UPDATE**

    Dev Notes: Keep eye on ammo retention
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class WeaponInAction : MonoBehaviour
{
    [Header("WEAPON INFO")]
    [Header("Weapon Scriptable List")]
    [SerializeField] List<WeaponInformation> availableWeapons = new List<WeaponInformation>();   //player and enemy can use    (connect to player inventory)
    [SerializeField] GameObject gunModelPlaceHolder;
    [SerializeField] GameObject shootOrigin;

    [Header("UI")]
    [Header("Automatically found")]
    [SerializeField] Image gunImage;
    [SerializeField] TextMeshProUGUI gunName;
    [SerializeField] TextMeshProUGUI currentAmmoUI;
    [SerializeField] TextMeshProUGUI ammoStoredUI;
    [SerializeField] GameObject reloadMessage;
    [SerializeField] TextMeshProUGUI reloadText;

    //===========VARIABLES===========
    int playerLayer;
    int layerMask;

    WeaponInformation gunInfo;
    int currentAmmo = 0;
    int ammoStored = 0;

    bool isReloading;
    bool isShooting;
    bool isFlashing;

    //===========STORE WEAPON AMMO===========
    Dictionary<WeaponInformation, AmmoState> ammoStates = new Dictionary<WeaponInformation, AmmoState>();

    private class AmmoState
    {
        public int currentAmmo;
        public int ammoStored;
    }


    //===========GETTERS===========
    public int CurrentAmmo => currentAmmo;
    public GameObject GunModelPlaceHolder => gunModelPlaceHolder;
    public WeaponInformation GunInfo { get; set; }
    public GameObject ShootOrigin => shootOrigin;

    private void Start()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            if(InventoryManager.instance)
            {
                InventoryManager.instance.OnInventoryUpdated.AddListener(CheckAvailableWeapons);
            }

            //ignore player mask
            playerLayer = LayerMask.NameToLayer("Player");
            layerMask = ~(1 << playerLayer);

            //find UI components (reset every scene)
            gunImage = GameObject.Find("WeaponSprite").GetComponent<Image>();
            gunName = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
            currentAmmoUI = GameObject.Find("CurrentAmmo").GetComponent<TextMeshProUGUI>();
            ammoStoredUI = GameObject.Find("TotalAmmo").GetComponent<TextMeshProUGUI>();
            reloadMessage = GameObject.Find("ReloadMsgBackground");
            reloadText = GameObject.Find("ReloadMessageText").GetComponent<TextMeshProUGUI>();

            CheckAvailableWeapons();
            if(availableWeapons.Count > 0)
                EquipWeapon(0);
        }
    }

    private void Update()
    {
        //if(!CompareTag("Player"))
        //Debug.DrawRay(shootOrigin.transform.position, transform.forward * gunInfo.shootDistance, Color.yellow);

        if (Time.timeScale == 0)
            return;

        if (this.gameObject.CompareTag("Player"))
        {
            OnSwitchWeapon();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerFireGun();
            }

            if (Input.GetKeyDown(KeyCode.R))
                Reload();

            if (gunModelPlaceHolder != null && reloadMessage != null)
            {
                if ((gunModelPlaceHolder.GetComponent<MeshFilter>().sharedMesh == null) && (reloadMessage.GetComponent<Image>().enabled) ||
                    gunInfo != null && currentAmmo > (gunInfo.maxClipAmmo / 3))
                {
                    if (!isReloading)
                    {
                        reloadMessage.GetComponent<Image>().enabled = false;
                        reloadText.enabled = false;
                    }
                }
            }

        }
    }
    public void OnSwitchWeapon()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1) && availableWeapons.Count > 0)        //press 1 for primary
        {
            EquipWeapon(0);

            if (reloadMessage.activeSelf)
                reloadMessage.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && availableWeapons.Count > 0)
        {
            EquipWeapon(1);

            if (reloadMessage.activeSelf)
                reloadMessage.SetActive(false);
        }
        else if (availableWeapons.Count <= 0 && gunInfo != null)
            gunInfo = null;*/
        
        //USE IF ADDING MORE EQUIPABLE WEAPONS
        for (int i = 0; i < availableWeapons.Count; i++)
        {
            // Display the last weapon on the last array/list of availableWeapons.
            // EquipWeapon(availableWeapons.Count-1);

            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                EquipWeapon(i);
                break;
            }
        }

        if (availableWeapons.Count <= 0 && gunInfo != null) 
        {
            gunInfo = null;
        }

    }

    //PLAYER ONLY: updates weapons based on weapons in the inventory
    public void CheckAvailableWeapons()                 //called with Unity Event when updated
    {
        if (InventoryManager.instance)
        {
            bool newWeaponAdded = false;
            foreach (InventorySlot slot in InventoryManager.instance.InventorySlotsList)
            {
                if (slot.Item is WeaponInformation weapon)       //if they match, casts to WeaponInformation to add to list of weapons
                {
                    //avoids adding dupes
                    if (!availableWeapons.Contains(weapon))
                    {
                        availableWeapons.Add(weapon);

                        newWeaponAdded = true;
                    }
                    else
                    {
                        //add logic to restore ammo to max instead
                    }    
                }
            }

            //remove guns that are no longer in the player's inventory
            availableWeapons.RemoveAll(weapon =>
                !InventoryManager.instance.InventorySlotsList.Exists(slot => slot.Item == weapon));

            if (newWeaponAdded )
                EquipWeapon(availableWeapons.Count - 1);    //auto equip new gun
        }
            //Debug.Log("No Inventory Manager for weapons");
    }

    //equips the weapon based on the index
    public void EquipWeapon(int index)
    {
        if(gunInfo != null)
        {
            if(!ammoStates.ContainsKey(gunInfo))
            {
                ammoStates.Add(gunInfo, new AmmoState { currentAmmo = currentAmmo, ammoStored = ammoStored });
            }
            else
            {
                ammoStates[gunInfo].currentAmmo = currentAmmo;
                ammoStates[gunInfo].ammoStored = ammoStored;
            }
        }

        if (index >= 0 && index < availableWeapons.Count)
        {
            //currentWeaponIndex = index;
            gunInfo = availableWeapons[index];

            if (this.gameObject.CompareTag("Player"))
            {
                gunImage.sprite = gunInfo.Icon;

                //use previous gun ammo state if it exists
                if (ammoStates.ContainsKey(gunInfo))
                {
                    currentAmmo = ammoStates[gunInfo].currentAmmo;
                    ammoStored = ammoStates[gunInfo].ammoStored;
                }
                else
                {
                    currentAmmo = gunInfo.maxClipAmmo;
                    ammoStored = gunInfo.ammoStored;
                    ammoStates.Add(gunInfo, new AmmoState { currentAmmo = currentAmmo, ammoStored = ammoStored});
                }

                gunName.text = gunInfo.ItemName;
                currentAmmoUI.text = currentAmmo.ToString();
                ammoStoredUI.text = ammoStored.ToString();
            }
            else currentAmmo = gunInfo.maxClipAmmo;

            UpdateWeaponModel(gunInfo);
        }
    }

    //update gun model based on the equipped gun
    void UpdateWeaponModel(WeaponInformation _gunInfo)
    {
        gunModelPlaceHolder.GetComponent<MeshFilter>().sharedMesh =
            _gunInfo.ItemModel.GetComponent<MeshFilter>().sharedMesh;

        gunModelPlaceHolder.GetComponent<MeshRenderer>().sharedMaterial =
            _gunInfo.ItemModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void Reload()
    {
        if ((ammoStored > 0 && currentAmmo < gunInfo.maxClipAmmo) || 
            (this.GetComponentInParent<EnemyBase>()))
        {
            if(!isReloading)
                StartCoroutine(ReloadRoutine());
        }
        else if (ammoStored <= 0)
        {
            //Debug.Log("Out of Ammo");
        }
    }

    //coroutine for delaying reload
    IEnumerator ReloadRoutine()
    {
        isReloading = true;     //so enemy does not infinite reload

        reloadMessage.GetComponent<Image>().enabled = true;
        reloadText.enabled = true;
        reloadText.text = "Reloading...";


        yield return new WaitForSeconds(gunInfo.reloadRate);

        if (CompareTag("Player"))
        {
            //refill ammo
            int ammoToRefill = Mathf.Min(gunInfo.maxClipAmmo - currentAmmo, ammoStored);     //makes sure to not use more bullets than stored
            currentAmmo += ammoToRefill;
            ammoStored -= ammoToRefill;

            currentAmmoUI.text = currentAmmo.ToString();
            ammoStoredUI.text = ammoStored.ToString();

            reloadMessage.GetComponent<Image>().enabled = false;
            reloadText.text = "Reload";
            reloadText.enabled = false;

        }
        else currentAmmo = gunInfo.maxClipAmmo;

        isReloading = false;
    }

    public void FireGun()
    {
        //fire only if there is ammo in the gun
        if (currentAmmo > 0 && !isShooting)         //isShooting always false for player
        {
            //adjust ammo
            currentAmmo--;

            if (!this.gameObject.CompareTag("Player"))          //only enemy calls coroutine
            {
                StartCoroutine(EnemyShootRate(this.gameObject.GetComponent<EnemyBase>().EnemyShootRate));
            }

            //raycast to where the gun is aimed at
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance))
            {
                //Debug.Log($"WeaponInAction: Hit {hitInfo.transform.name}");

                //check if it has health to it                                                                                  //APPLY HEALTH/DAMAGE COMPONENT HERE
                HealthSystem targetHealth = hitInfo.transform.GetComponent<HealthSystem>();
                if (targetHealth != null)
                {
                    targetHealth.Damage(gunInfo.shootDamage);
                    //Debug.Log($"WeaponInAction: Hit {hitInfo.transform.name} for {gunInfo.shootDamage} damage");
                }

                if (gunInfo.hitEffect != null)
                {
                    Instantiate(gunInfo.hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                }
            }
            else { }
                //Debug.Log("WeaponInAction: Missed");

            //muzzle flash method
            //PlayMuzzleFlash();                                                                //UNDER MAINTAINANCE
        }
        else if (this.gameObject.CompareTag("Player"))
        {
            //Debug.Log("WeaponInAction: Gun out of ammo");
            reloadMessage.GetComponent<Image>().enabled = true;
            reloadText.enabled = true;
        }
    }

    public void PlayerFireGun()
    {
        // fire only if player has weapon equiped
        if (gunInfo != null && !isReloading)
        {
            //fire only if the gun has ammo
            if (currentAmmo > 0)
            {
                //adjust ammo
                currentAmmo--;

                currentAmmoUI.text = currentAmmo.ToString();
                AudioManager.instance.PlaySFX(gunInfo.shootSound);
                

                //raycast to where the player is looking
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, gunInfo.shootDistance, layerMask))
                {
                    //Debug.Log($"Player: Hit {hitInfo.transform.name}");

                    //check if the hit object has a HealthSystem
                    HealthSystem targetHealth = hitInfo.transform.GetComponent<HealthSystem>();
                    if (targetHealth != null)
                    {
                        targetHealth.Damage(gunInfo.shootDamage);
                        //Debug.Log($"Player: Dealt {gunInfo.shootDamage} damage to {hitInfo.transform.name}");
                    }

                    //hit effect for bullet impact
                    if (gunInfo.hitEffect != null)
                    {
                        Instantiate(gunInfo.hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    }
                }
                else
                {
                    //Debug.Log("Player: Missed");
                }

                if (currentAmmo < (gunInfo.maxClipAmmo / 3))
                {
                    reloadMessage.GetComponent<Image>().enabled = true;
                    reloadText.enabled = true;
                }
                //muzzle flash method
                //PlayMuzzleFlash();
            }
            else
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(AudioManager.instance.Empty_Clip[0]);
                //Debug.Log("Player: Gun out of ammo");
                //reloadMessage.SetActive(true);
            }
        }
    }

    public void EnemyFireGun(Transform target)
    {
        // Fire only if the gun has ammo
        if (currentAmmo > 0 && !isShooting)
        {
            //adjust ammo
            currentAmmo--;

            AudioManager.instance.PlaySFX(AudioManager.instance.ER_Sounds[3]);

            //adjusted shoot rate for enemies
            StartCoroutine(EnemyShootRate(this.gameObject.GetComponent<EnemyBase>().EnemyShootRate));

            //calculate the direction to the target
            Vector3 directionToTarget = (target.position - shootOrigin.transform.position).normalized;              //TODO: Add randomization to have them miss once in a while

            //Debug.DrawRay(transform.position, directionToTarget, Color.yellow);

            Debug.Log("Enemy Shooting...");

            //raycast towards the target
            if (Physics.Raycast(shootOrigin.transform.position, directionToTarget, out RaycastHit hitInfo, gunInfo.shootDistance))
            {
                //Debug.Log($"Enemy: Hit {hitInfo.transform.name}");
                shootOrigin.transform.LookAt(target.position);

                //check if the hit object has a HealthSystem
                HealthSystem targetHealth = hitInfo.transform.GetComponent<HealthSystem>();
                if (targetHealth != null)
                {
                    targetHealth.Damage((float)gunInfo.shootDamage);
                    Debug.Log($"Enemy: Dealt {gunInfo.shootDamage} damage to {hitInfo.transform.name}");
                }

                //hit effect for bullet impact
                if (gunInfo.hitEffect != null)
                {
                    Instantiate(gunInfo.hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                }
            }
            else
            {
                Debug.Log("Enemy: Missed");
            }

            //PlayMuzzleFlash();
        }
    }



    //method to create muzzle flash when shooting the weapon
    void PlayMuzzleFlash()
    {
        if(gunInfo.muzzleFlash != null)
        {
            if (!isFlashing)
            {
                GameObject gunFlash = Instantiate(gunInfo.muzzleFlash, gunInfo.muzzleFlashPos.position, gunModelPlaceHolder.transform.rotation);
                //gunInfo.muzzleFlash.SetActive(true);

                StartCoroutine(MuzzleFlashRoutine(gunFlash));
            }
            


            //Debug.Log("WeaponInAction: Muzzle Flash Instantiated");
        }
    }

    IEnumerator EnemyShootRate(int shootRate)
    {
        isShooting = true;

        yield return new WaitForSeconds(shootRate);     //to slow down enemy shoot rate

        isShooting = false;
    }

    IEnumerator MuzzleFlashRoutine(GameObject _gunFlash)
    {
        isFlashing = true;

        yield return new WaitForSeconds(1f);

        //gunInfo.muzzleFlash.SetActive(false);

        Destroy(_gunFlash);

        isFlashing = false;
    }

    public void DropEquippedGun()
    {
        if(gunInfo != null)
        {
            //drop weapon logic
            GameObject droppedGun = Instantiate(gunInfo.ItemModel, transform.position, transform.rotation);
            droppedGun.transform.SetParent(null);
        }
    }
}
