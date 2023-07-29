using UnityEngine;

[CreateAssetMenu(menuName = "TestAssignment/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    [SerializeField] private FactionType m_ObstacleType;
    public FactionType _EnemyType => m_ObstacleType;

    [SerializeField] private int m_DamageDealt;
    public int _DamageDealt => m_DamageDealt;
}