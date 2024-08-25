using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour, IPlayerComponent
{
    public delegate void WeaponChange(PlayerWeapon before, PlayerWeapon next);

    [Header("Weapon data")]
    [SerializeField] private List<PlayerWeapon> _weaponSlots;
    [HideInInspector] public PlayerWeapon currentWeapon;
    [SerializeField] private int _maxSlotAllowed = 2;
    public Transform GunPointTrm => currentWeapon.GunPoint;

    [Header("WeaponHolder")]
    [SerializeField] private Transform _weaponHolderTrm;
    [SerializeField] private Transform _backHolderTrm;
    [SerializeField] private Transform _sideHolderTrm;

    private Player _player;
    public event Action<float> ReloadEvent;
    public event Action WeaponFireEvent;
    public event WeaponChange WeaponChangeStartEvent;

    private bool _isShooting, _weaponReady;

    public void Initialize(Player player)
    {
        _player = player;
        _player.GetCompo<InputReaderSO>().FireEvent += HandleFireInputEvent;
        _player.GetCompo<InputReaderSO>().ChangeWeaponSlotEvent += HandleChangeWeaponSlot;
        _player.GetCompo<PlayerAnimator>().GrabStatusChangeEvent += HandleGrabStatusChange;

        foreach(PlayerWeapon weapon in _weaponSlots)
        {
            weapon.SetUpGun(_weaponHolderTrm, _backHolderTrm, _sideHolderTrm);
            weapon.bulletInMagazine = weapon.weaponData.maxAmmo; //최대치로 채워줌.
        }

        _weaponReady = true;
    }

    private void HandleGrabStatusChange(bool isGrabWeapon)
    {
        _weaponReady = isGrabWeapon;
    }

    private void HandleChangeWeaponSlot(int slotIndex)
    {
        if (slotIndex >= _weaponSlots.Count || slotIndex < 0) return;
        if (_weaponSlots[slotIndex].weaponData == null) return; //슬롯이 비어있다면 교체 불가
        if (_weaponReady == false) return;

        PlayerWeapon before = currentWeapon;
        currentWeapon = _weaponSlots[slotIndex];

        WeaponChangeStartEvent?.Invoke(before, currentWeapon);
    }

    private void HandleFireInputEvent(bool isFire)
    {
        _isShooting = isFire;
    }

    private void Update()
    {
        if(_isShooting)
        {
            Shooting();
        }
    }

    private void Shooting()
    {
        if (_weaponReady == false || currentWeapon.CanShooting() == false)
            return;

        WeaponFireEvent?.Invoke();
    }
}
