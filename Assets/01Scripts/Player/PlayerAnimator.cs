using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class PlayerAnimator : MonoBehaviour, IPlayerComponent
{
    private Player _player;

    private readonly int _xVelocityHash = Animator.StringToHash("xVelocity");
    private readonly int _zVelocityHash = Animator.StringToHash("zVelocity");
    private readonly int _isRunningHash = Animator.StringToHash("isRunning");
    private readonly int _fireTriggerHash = Animator.StringToHash("fire");
    private readonly int _grapTypeHash = Animator.StringToHash("weaponGrabType");
    private readonly int _grabTriggerHash = Animator.StringToHash("weaponGrab");
    private readonly int _equipSpeedHsh = Animator.StringToHash("equipSpeed");

    private Animator _animator;

    private PlayerMovement _moveCompo;
    private bool _isPlayRunAnimation;
    private PlayerWeaponController _weaponController;

    public event Action<bool> GrabStatusChangeEvent; //무기 집기 상태 알림(집는 중인지, 집은 후인지)
    public event Action WeaponGrabTriggerEvent; //백업 웨폰을 집는 순간

    public void Initialize(Player player)
    {
        _player = player;
        _animator = GetComponent<Animator>();
        _moveCompo = player.GetCompo<PlayerMovement>();
        _moveCompo.OnMovement += HandleMovement;
        _moveCompo.OnRunning += HandleOnRunning;

        _weaponController = _player.GetCompo<PlayerWeaponController>();
        _weaponController.WeaponFireEvent += HandleWeaponFireEvent;
        _weaponController.WeaponChangeStartEvent += HandleWeaponChangeStart;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for(int i = 1; i < _animator.layerCount; i++) 
        {
            _animator.SetLayerWeight(i, 0);
        }
        _animator.SetLayerWeight(layerIndex, 1);
    }

    private void PlayerGrabAnimation(GrabType grabType, float equipSpeed)
    {
        GrabStatusChangeEvent?.Invoke(false);
        _animator.SetFloat(_equipSpeedHsh, equipSpeed);
        _animator.SetInteger(_grapTypeHash, (int)grabType);
        _animator.SetTrigger(_grabTriggerHash);
    }

    public void ChangeWeaponAnimation() => WeaponGrabTriggerEvent?.Invoke();
    public void GrabAnimationEnd() => GrabStatusChangeEvent?.Invoke(true);

    private void HandleWeaponChangeStart(PlayerWeapon before, PlayerWeapon next)
    {
        int layerIndex = next.weaponData.animationLayer;
        SwitchAnimationLayer(layerIndex);
        PlayerGrabAnimation(next.weaponData.grabType, next.weaponData.equipSpeed);
    }

    private void HandleWeaponFireEvent()
    {
        _animator.SetTrigger(_fireTriggerHash);
    }

    private void HandleOnRunning(bool isRunning)
    {
        _isPlayRunAnimation = isRunning;
    }

    private void HandleMovement(Vector3 movement)
    {
        float x = Vector3.Dot(movement, transform.right);
        float z = Vector3.Dot(movement, transform.forward);

        float dampTime = 0.1f;
        _animator.SetFloat(_xVelocityHash, movement.x, dampTime, Time.deltaTime);
        _animator.SetFloat(_zVelocityHash, movement.z, dampTime, Time.deltaTime);

        bool isPlay = movement.sqrMagnitude > 0 && _isPlayRunAnimation == true;

        _animator.SetBool(_isRunningHash, isPlay);
    }
}
