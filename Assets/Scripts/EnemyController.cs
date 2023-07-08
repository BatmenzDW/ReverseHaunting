using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //public NavMesh agent;

    public Transform player;

    public LayerMask whatIsGround, WhatIsPlayer;

    //Patroling
    public Vector3 walkPoint;

}
