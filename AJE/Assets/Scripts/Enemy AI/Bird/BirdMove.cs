﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMove : EnemyMove
{
    [SerializeField] float ChargeSpeed;
    [SerializeField] float MyChargeRange;

    Vector3 chargePos;

    private float chargeUptimer;
    [SerializeField] float chargeUpTime;

    private float fleeTimer;
    public float fleeForX;

    bool posFound = false;

    bool canCharge;
    bool charge;
    bool fleeing;
    bool hoverToPlayer;

    [SerializeField] GameObject trail;

    public float fleeMultipler;
    public float ChargeMultipler;

    private void OnEnable()
    {
        fleeTimer = fleeForX;
        hoverToPlayer = true;
    }
    public override void Move()
    {
        //move away from player if dist is less then charge distance
        float dist = Vector3.Distance(transform.position, targetDestination.position);

        if (hoverToPlayer)
        {
            HoverTowardsPlayerState();
            if (dist < MyChargeRange)
            {
                hoverToPlayer = false;
                canCharge = true;
            }
        }

        //If dist is greater then charge distance and not already charging stop agent and set charging to true
        if (canCharge)
        {
            GetChargeTarget();
        }

        //if chargeing is true look for pos to charge to. then wait till timer is higher then charge up time
        if (charge)
        {
            ChargeState();
        }
        if (fleeing)
        {
            FleeState();
        }
    }

    private void HoverTowardsPlayerState()
    {
        navMeshAgent.SetDestination(targetDestination.position);

        if (!_slowDebuff)
            SetEnemyMoveSpeed(MyMoveSpeed);
        else
            SetEnemyMoveSpeed(MyMoveSpeed / _slowAmount);
    }
    private void FleeState()
    {
        fleeTimer -= Time.deltaTime;
        trail.SetActive(false);
        navMeshAgent.isStopped = false;
        Vector3 fleePos = transform.position + ((transform.position - targetDestination.transform.position) * fleeMultipler);
        fleePos.y = 1;
        navMeshAgent.SetDestination(fleePos);
        if (fleeTimer <= 0)
        {
            fleeing = false;
            hoverToPlayer = true;
            fleeTimer = fleeForX;
        }
    }

    private void GetChargeTarget()
    {

        if (!posFound)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            trail.SetActive(true);
            chargePos = targetDestination.transform.position + ((targetDestination.transform.position - transform.position) * ChargeMultipler);
            chargePos.y += 4;
            posFound = true;
        }

        Vector3 lookPos = chargePos - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * LookTowardsSpeed);

        chargeUptimer += Time.deltaTime;
        if (chargeUptimer >= chargeUpTime)
        {
            charge = true;
            canCharge = false;
            posFound = false;
        }
    }

    private void ChargeState()
    {
        float distToChargePos = Vector3.Distance(transform.position, chargePos);
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(chargePos);
        if (!_slowDebuff)
            SetEnemyMoveSpeed(ChargeSpeed);
        else
            SetEnemyMoveSpeed(ChargeSpeed / _slowAmount);

        //if object reached charge position reset speed back to normal and restart ai loop from start.
        if (distToChargePos <= 3f)
        {
            if (!_slowDebuff)
                SetEnemyMoveSpeed(MyMoveSpeed);
            else
                SetEnemyMoveSpeed(MyMoveSpeed / _slowAmount);

            chargeUptimer = 0;
            charge = false;
            fleeing = true;
        }
    }
}