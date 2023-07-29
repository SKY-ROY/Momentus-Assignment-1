using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentObjectType
{
    PATHWAY_BLOCK,
    COSMETIC_TERRAIN
}

public class EnvironmentManager : MonoBehaviour
{
    private static EnvironmentManager instance = null;
    public static EnvironmentManager Instance => instance;

    private GameObject latestSpawnObstacle = null;
    private GameObject latestSpawnEnvironment = null;

    [SerializeField] List<GameObject> pathwayBlocks;
    [SerializeField] List<GameObject> cosmeticBlocks;
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] int platformIndex = -1;
    private int safeBlockCount = 0;

    private Queue<GameObject> pathwayQueue, obstacleQueue;

    #region Lifecycle Methods
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);      // destroy this game object, if instance already exists.

        pathwayQueue = new Queue<GameObject>();
        obstacleQueue = new Queue<GameObject>();
    }

    void Start()
    {
        PlaceInitialObjects(EnvironmentObjectType.PATHWAY_BLOCK);
    }

    private void Update()
    {
        if ((PlayerController.Instance.transform.position.z - pathwayQueue.Peek().transform.position.z) > 20f)
        {
            GameObject firstObj = pathwayQueue.Dequeue();
            firstObj.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                GameObject obsObj = obstacleQueue.Dequeue();
                obsObj.SetActive(false);
            }

            PlaceEnvironmentObjectAtStart(EnvironmentObjectType.PATHWAY_BLOCK);
        }
    }
    #endregion

    #region Level Generation
    private void PlaceInitialObjects(EnvironmentObjectType type)
    {
        int objIndex;
        string objPoolID;
        Vector3 spawnPosition;

        for (int i = 0; i < 10; i++)
        {
            objIndex = 0;
            objPoolID = (type == EnvironmentObjectType.PATHWAY_BLOCK) ? pathwayBlocks[objIndex].gameObject.name : cosmeticBlocks[objIndex].gameObject.name;
            spawnPosition = (i == 0) ? Vector3.zero : GetLatestSpawnedPosition(type, i);

            GameObject obj = ObjectPooler.Instance.GetPooledObject(objPoolID, spawnPosition, Quaternion.identity, true);
            UpdateLatestSpawnedObject(type, obj);

            if (type == EnvironmentObjectType.PATHWAY_BLOCK)
            {
                SpawnObstacle();
            }
        }
    }

    public void PlaceEnvironmentObjectAtStart(EnvironmentObjectType type)
    {
        int objIndex = GetRandomObject(type);
        string objPoolID = (type == EnvironmentObjectType.PATHWAY_BLOCK) ? pathwayBlocks[objIndex].gameObject.name : cosmeticBlocks[objIndex].gameObject.name; ;

        Vector3 spawnPosition = GetLatestSpawnedPosition(type);

        GameObject obj = ObjectPooler.Instance.GetPooledObject(objPoolID, spawnPosition, Quaternion.identity, true);
        UpdateLatestSpawnedObject(type, obj);

        if (type == EnvironmentObjectType.PATHWAY_BLOCK)
        {
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        Vector3 pos = GetLatestSpawnedPosition(EnvironmentObjectType.PATHWAY_BLOCK);
        pos.y += 1;

        GameObject orbL = ObjectPooler.Instance.GetPooledObject(obstacles[Random.Range(0, obstacles.Count)].name, pos + new Vector3(PlayerController.Instance._PlayerData._LeftBinding.xPosition, 0f, 0f), Quaternion.identity, true);

        GameObject orbM = ObjectPooler.Instance.GetPooledObject(obstacles[Random.Range(0, obstacles.Count)].name, pos, Quaternion.identity, true);

        GameObject orbR = ObjectPooler.Instance.GetPooledObject(obstacles[Random.Range(0, obstacles.Count)].name, pos + new Vector3(PlayerController.Instance._PlayerData._RightBinding.xPosition, 0f, 0f), Quaternion.identity, true);

        obstacleQueue.Enqueue(orbL);
        obstacleQueue.Enqueue(orbM);
        obstacleQueue.Enqueue(orbR);
    }

    private int GetRandomObject(EnvironmentObjectType type)
    {
        int num = 0;

        if (platformIndex == -1)
        {
            num = (
                    (type == EnvironmentObjectType.PATHWAY_BLOCK) ?
                        (
                            (safeBlockCount-- >= 0) ?
                            0 :
                            Random.Range(0, pathwayBlocks.Count)
                        ) :
                        Random.Range(0, cosmeticBlocks.Count)
                    );
        }
        else
        {
            num = (type == EnvironmentObjectType.PATHWAY_BLOCK) ? platformIndex : Random.Range(0, cosmeticBlocks.Count);
        }

        return num;
    }

    private void UpdateLatestSpawnedObject(EnvironmentObjectType type, GameObject obj)
    {
        if (type == EnvironmentObjectType.PATHWAY_BLOCK)
        {
            latestSpawnObstacle = obj;
            pathwayQueue.Enqueue(latestSpawnObstacle);
        }
        else
        {
            latestSpawnEnvironment = obj;
        }
    }

    private Vector3 GetLatestSpawnedPosition(EnvironmentObjectType type, int counter = -1)
    {
        switch (type)
        {
            case EnvironmentObjectType.PATHWAY_BLOCK:
                return new Vector3(
                    0f,
                    0f,
                    (counter == -1) ? latestSpawnObstacle.transform.position.z + latestSpawnObstacle.transform.localScale.z : counter * latestSpawnObstacle.transform.localScale.z
                );
        }
        return Vector3.zero;
    }
    #endregion
}