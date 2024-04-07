using System;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleMan
{
    public delegate void GameEventHandler(UnityEngine.Object sender);
    public delegate void GameEventHandler<TArg1>(UnityEngine.Object sender, TArg1 arg1);

    [Serializable]
    public class UnityEventHandler : UnityEvent<Component>
    {

    }

    [Serializable]
    public class UnityEventHandler<T1> : UnityEvent<Component, T1>
    {

    }
}