using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
static public class AIBehaviour
{   
    static public MoveToTarget moveToTarget = new MoveToTarget();
    static public MoveAwayFromTarget moveAwayFromTarget = new MoveAwayFromTarget();

    static public Idle idle = new Idle();
    static public CirclePlayer circlePlayer = new CirclePlayer();
    static public Strafe strafe = new Strafe();
    static public ShootProjectileAtTarget shootProjectileAtTarget = new ShootProjectileAtTarget();
}
