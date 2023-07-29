using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour, IDefineObstacle
{
    [SerializeField] ObstacleData obstacleData;

    public FactionType ObstacleFaction { get => obstacleData._EnemyType; }
    public int Damage { get => obstacleData._DamageDealt; }

    #region Interface Implementation
    public int InflictDamage()
    {
        return Damage;
    }
    #endregion
}
