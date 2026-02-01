using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyMethods
{
    public static Vector2 ClosestPlayerPosition(ref List<Transform> playerTransforms, Vector2 position)
    {
        float minDist = 100f;
        int index = 0;
        for(int i=0; i<playerTransforms.Count; i++)
        {
            if(playerTransforms[i] != null) {
                float temp = Vector2.Distance(playerTransforms[i].position, position);
                if(temp < minDist)
                {
                    minDist = temp;
                    index = i;
                }
            }
            else
            {
                playerTransforms.RemoveAt(index);
            }
        }
        return playerTransforms[index].position;
    }
}