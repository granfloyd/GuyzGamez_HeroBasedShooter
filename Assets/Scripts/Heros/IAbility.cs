using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility 
{
    void Use();
    bool IsReady();
    void AbilityUpdate();
}
