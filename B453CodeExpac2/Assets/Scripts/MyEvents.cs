using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyEvents
{
    public static UnityEvent OnPlayerDeath = new UnityEvent();
    public static UnityEvent<byte> OnPlayerDamaged = new UnityEvent<byte>();

    public static UnityEvent<byte> OnHealthPickUp = new UnityEvent<byte>();
    public static UnityEvent OnMovementPickUp = new UnityEvent();
    public static UnityEvent OnGravityPickUp = new UnityEvent();
    public static UnityEvent OnKillAllPickUp = new UnityEvent();
}
