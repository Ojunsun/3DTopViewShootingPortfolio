using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon
{
    public WeaponDataSO weaponData;
    public int bulletInMagazine;

    public Transform GunTrm { get; protected set; }
    public Transform GunPoint { get; protected set; } //�ѱ� ��ġ

    protected float _nextShootTime;
    protected float _currentSpread, lastSpreadTime; //ź ����

    public void Shooting()
    {
        bulletInMagazine--;
        _nextShootTime = Time.time + 1 / weaponData.fireRate;
    }

    public abstract bool CanShooting();
}
