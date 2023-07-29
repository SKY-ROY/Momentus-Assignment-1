using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDefineMovement
{
    [SerializeField] private PlayerData m_PlayerData;
    public PlayerData _PlayerData => m_PlayerData;

    private List<Material> m_Materials => m_PlayerData._Materials;
    private int m_HP = 0, m_Score = 0;
    private MovementLane m_Lane;
    private FactionType m_PlayerFaction;
    private bool m_AllowMovement = false;
    private MeshRenderer m_MeshRenderer;

    public static PlayerController Instance;

    public static Action<int> OnHealthUpdate;
    public static Action<int> OnScoreUpdate;
    public static Action OnGameOver;

    #region Lifecycle Methods
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        m_PlayerFaction = m_PlayerData._InitialFaction;
        m_Lane = m_PlayerData._InitialLane;
        m_HP = m_PlayerData._MaxHealth;
        m_AllowMovement = true;
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        SwitchFaction(m_PlayerFaction);
    }

    void Update()
    {
        if (m_AllowMovement)
        {
            ProcessForwardMovement(m_PlayerData._ForwardMovementSpeed);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnMoveLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnMoveRight();
            }
        }
    }
    #endregion

    #region Physics Handling
    void OnTriggerEnter(Collider other)
    {
        IDefineObstacle obs = other.gameObject.GetComponent<IDefineObstacle>();
        if (obs != null)
        {

            if (obs.ObstacleFaction != m_PlayerFaction)
            {
                Debug.Log($"death-> {obs.ObstacleFaction} {m_PlayerFaction}");
                m_HP -= obs.InflictDamage();
                OnHealthUpdate?.Invoke(m_HP);

                if (m_HP <= 0)
                {
                    OnGameOver?.Invoke();
                    m_AllowMovement = false;
                }
            }
            else
            {
                Debug.Log($"award-> {obs.ObstacleFaction} {m_PlayerFaction}");
                OnScoreUpdate.Invoke(++m_Score);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        FactionType next = (FactionType)UnityEngine.Random.Range(0, m_Materials.Count);
        SwitchFaction(next);
    }
    #endregion

    #region Player Behavior Methods
    void SwitchFaction(FactionType faction)
    {
        m_PlayerFaction = faction;
        switch (faction)
        {
            case FactionType.Red:
                m_MeshRenderer.material = m_Materials[0];
                break;
            case FactionType.Green:
                m_MeshRenderer.material = m_Materials[1];
                break;
            case FactionType.Blue:
                m_MeshRenderer.material = m_Materials[2];
                break;
        }
    }
    #endregion

    #region Interface Implementation
    public void OnMoveLeft()
    {
        if (transform.position.x > m_PlayerData._LeftBinding.xPosition)
        {
            // Debug.Log("Left");
            ProcessSidewaysMovement(MovementLane.Left);
        }
    }

    public void OnMoveRight()
    {
        if (transform.position.x < m_PlayerData._RightBinding.xPosition)
        {
            // Debug.Log("Right");
            ProcessSidewaysMovement(MovementLane.Right);
        }
    }

    public void ProcessSidewaysMovement(MovementLane lane)
    {
        m_Lane = lane;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        switch (lane)
        {
            case MovementLane.Left:
                pos += new Vector3(m_PlayerData._LeftBinding.xPosition, 0f, 0f);
                break;
            case MovementLane.Mid:
                pos += new Vector3(m_PlayerData._MidBinding.xPosition, 0f, 0f);
                break;
            case MovementLane.Right:
                pos += new Vector3(m_PlayerData._RightBinding.xPosition, 0f, 0f);
                break;
        }

        transform.position = pos;
    }

    public void ProcessForwardMovement(float speed)
    {
        transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
    }

    #endregion
}
