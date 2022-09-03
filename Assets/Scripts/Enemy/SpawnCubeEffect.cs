using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffects/SpawnCubeEffect")]
public class SpawnCubeEffect : CardEffect
{
    public override void TriggerEffect()
    {
        GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        /*if you need add position, scale and color to the cube*/
        cubeObject.transform.localPosition = new Vector3(0, 0, 0);
        cubeObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
