using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Anything that can  be damaged (players)
public interface IDamageable
{
    void takeDamage(); // Abstract method to take damage
}
