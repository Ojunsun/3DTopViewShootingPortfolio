using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerWeapon : Weapon
{
    public int reservedAmmo;
    public Transform BackUpGunTrm { get; protected set; }

    public override bool CanShooting()
    {
        bool isCoolTime = _nextShootTime > Time.time;
        bool isEmptyClip = bulletInMagazine <= 0;

        if (isCoolTime || isEmptyClip) return false;

        return true;
    }

    public void SetUpGun(Transform weaponHolder, Transform backHolder, Transform sideHolder)
    {
        GunTrm = weaponHolder.Find(weaponData.gunTrmName);
        if(GunTrm == null)
        {
            Debug.LogWarning($"{weaponData.gunTrmName} is not exist on {weaponData.name}");
            return;
        }

        Transform backupHolder = weaponData.grabType == GrabType.Back ? backHolder : sideHolder;
        BackUpGunTrm = backupHolder.Find($"Backup_{weaponData.gunTrmName}");

        if(BackUpGunTrm == null)
        {
            Debug.LogWarning($"backup {weaponData.gunTrmName} is not exist on {backupHolder.name}");
            return;
        }

        ActiveGun(false);

        GunPoint = GunTrm.Find("GunPoint");
        if (GunPoint == null)
        {
            Debug.LogWarning($"GunPoint {weaponData.gunTrmName} is no exist on {GunTrm.name}");
        }
    }

    public void ActiveGun(bool isActive)
    {
        GunTrm.gameObject.SetActive(isActive);
        BackUpGunTrm.gameObject.SetActive(!isActive);
    }
}
