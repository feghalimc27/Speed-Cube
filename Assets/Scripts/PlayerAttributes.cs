using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "Options/Attributes")]
public class PlayerAttributes : ScriptableObject {

    public float speed, acceleration, friction, gravity, jumpStrength;
}
