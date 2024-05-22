using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAIBehaviour
{
    public virtual void EnterBehaviour(EnemyBehaviour enemy) { }
    public virtual void UpdateBehaviour(EnemyBehaviour enemy) { }
    public virtual void ExitBehaviour(EnemyBehaviour enemy) { }

}
