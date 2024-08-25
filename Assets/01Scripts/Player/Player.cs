using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputReaderSO _inputReader;

    private Dictionary<Type, IPlayerComponent> _components;

    private void Awake()
    {
        _components = new Dictionary<Type, IPlayerComponent>();

        IPlayerComponent[] compoArr = GetComponentsInChildren<IPlayerComponent>();
        foreach (var component in compoArr)
        {
            _components.Add(component.GetType(), component);
        }
        _components.Add(_inputReader.GetType(), _inputReader);

        foreach (IPlayerComponent compo in _components.Values)
        {
            compo.Initialize(this);
        }
    }

    public T GetCompo<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out IPlayerComponent compo))
            return compo as T;
        return default;
    }
}