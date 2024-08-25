using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon
{
    public WeaponDataSO weaponData;
    public int bulletInMagazine;

    public Transform GunTrm { get; protected set; }
    public Transform GunPoint { get; protected set; } //ÃÑ±¸ À§Ä¡

    protected float _nextShootTime;
    protected float _currentSpread, lastSpreadTime; //Åº ÆÛÁü

    public void Shooting()
    {
        bulletInMagazine--;
        _nextShootTime = Time.time + 1 / weaponData.fireRate;
    }

    public abstract bool CanShooting();
}
