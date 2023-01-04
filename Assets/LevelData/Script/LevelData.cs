using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "LevelDesign/NewLevel")]
public class LevelData : ScriptableObject
{
    public Vector2Int PlayerSpawnPos;

    public List<LevelCharacter> enemyList;

    public List<LevelObstacle> obstacleList;
}
