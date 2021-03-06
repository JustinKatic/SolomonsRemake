﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using TMPro;


public class TowersDefault : MonoBehaviour
{
    private float _timer;
    [SerializeField] public LayerMask EnemyLayerMask;
    [SerializeField] public float ActivateEveryX;
    [SerializeField] public float TowerRadius;
    [SerializeField] public float TowerDamage;

    [SerializeField] protected GameObject floatingDmg;

    [SerializeField] protected ParticleSystem pulseFX;


    public void SetTowerEffects()
    {
        ParticleSystem.EmissionModule emission = pulseFX.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 1, 1000, ActivateEveryX) });
        ParticleSystem.MainModule main = pulseFX.main;
        main.startSize = TowerRadius * 2;
        main.startLifetime = ActivateEveryX;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= ActivateEveryX)
        {
            MyCollisions();
            _timer = 0;
        }
    }

    protected virtual void MyCollisions()
    {
        Debug.Log("Select a specifc tower script");
    }

    public void FloatingTxt(float damage, Transform transformToSpawnTxtAt)
    {
        GameObject points = ObjectPooler.SharedInstance.GetPooledObject("FloatingTxt");
        points.transform.position = transformToSpawnTxtAt.position;
        points.transform.rotation = Quaternion.identity;
        points.transform.GetChild(0).GetComponent<TextMeshPro>().text = "-" + damage.ToString();
        points.SetActive(true);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, TowerRadius);
    //}
}
