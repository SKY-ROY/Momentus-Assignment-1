using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TestAssignment/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] int m_MaxHealth;
    public int _MaxHealth => m_MaxHealth;

    [SerializeField] MovementLane m_InitialLane;
    public MovementLane _InitialLane => m_InitialLane;

    [SerializeField] FactionType m_InitialFaction;
    public FactionType _InitialFaction => m_InitialFaction;

    [SerializeField] float m_ForwardMovementSpeed;
    public float _ForwardMovementSpeed => m_ForwardMovementSpeed;

    [SerializeField] List<Material> m_Materials;
    public List<Material> _Materials => m_Materials;

    [SerializeField] LaneTransversePositionBinding m_LeftBinding, m_MidBinding, m_RightBinding;
    public LaneTransversePositionBinding _LeftBinding => m_LeftBinding;
    public LaneTransversePositionBinding _MidBinding => m_MidBinding;
    public LaneTransversePositionBinding _RightBinding => m_RightBinding;
}
