using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour, IPlayerComponent
{
    [Header("Aim Control")]
    [SerializeField] private Transform _aimTrm;

    public event Action<Quaternion> OnLookDirectionChange;

    private Player _player;
    private Vector3 _mousePos;
    private Vector3 _beforeLookDirection;

    private InputReaderSO _inputCompo;

    public void Initialize(Player player)
    {
        _player = player;
        _inputCompo = _player.GetCompo<InputReaderSO>();
    }

    private void Update() {
        UpdateAimPosition();
        UpdateLookDirection();
    }

    private void UpdateAimPosition()
    {
        _mousePos = _player.GetCompo<InputReaderSO>().GetWorldMousePosition();
        _aimTrm.position = _mousePos;
    }

    private void UpdateLookDirection()
    {
        Vector3 lookDirection = _mousePos - transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();
        if(_beforeLookDirection != lookDirection)
        {
            OnLookDirectionChange?.Invoke(Quaternion.LookRotation(lookDirection));
            _beforeLookDirection = lookDirection;
        }
    }
}
