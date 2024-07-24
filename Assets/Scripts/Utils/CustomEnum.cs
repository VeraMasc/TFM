using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Linq;
using System;

//TODO: Finish

/// <summary>
/// Custom enum that accepts any kind of type
/// </summary>
/// <typeparam name="T"></typeparam>
public struct CustomEnum<T>
{
    public T value;
}