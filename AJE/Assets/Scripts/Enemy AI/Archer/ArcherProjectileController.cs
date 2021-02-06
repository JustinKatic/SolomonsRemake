﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherProjectileController : EnemyProjectileController
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagOfObjectCanHit.Value)
        {
            PlayerCurrentHp.RuntimeValue -= _damage.RuntimeValue;
            FloatingText(_damage.RuntimeValue, other.transform.position);
            UpdatePlayerHealthEvent.Raise();
        }

        if (other.gameObject.tag == "Wall")
        {
            SetUnActive();
        }
    }
}