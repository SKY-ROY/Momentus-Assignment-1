using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pooler
    {
        [HideInInspector] public string poolType => objectPrefab.gameObject.name;
        public int objCount;
        public GameObject objectPrefab;
    }
    public static ObjectPooler Instance { get; set; }

    public List<Pooler> mPool;

    public Dictionary<string, List<GameObject>> mPoolDictionary;

    [SerializeField] private bool canGrow = true;

    private void Awake()
    {
        Instance = this;

        mPoolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pooler pool in mPool)
        {
            List<GameObject> pooledObjectList = new List<GameObject>();
            for (int i = 0; i < pool.objCount; i++)
            {
                GameObject obj = Instantiate(pool.objectPrefab);
                obj.SetActive(false);
                pooledObjectList.Add(obj);
            }

            mPoolDictionary.Add(pool.poolType, pooledObjectList);
        }
    }

    void Start()
    {
        // mPoolDictionary = new Dictionary<string, List<GameObject>>();
        // foreach (Pooler pool in mPool)
        // {
        //     List<GameObject> pooledObjectList = new List<GameObject>();
        //     for (int i = 0; i < pool.objCount; i++)
        //     {
        //         GameObject obj = Instantiate(pool.objectPrefab);
        //         obj.SetActive(false);
        //         pooledObjectList.Add(obj);
        //     }

        //     mPoolDictionary.Add(pool.poolType, pooledObjectList);
        // }
    }

    public GameObject GetPooledObject(string objectType, Vector3 reqPosition, Quaternion reqRotation, bool isActive = false)
    {
        if (mPoolDictionary == null)
        {
            print("--------------------->>>>> mPoolDictionary is null");
            return null;
        }
        if (!mPoolDictionary.ContainsKey(objectType))
        {
            Debug.Log("The Object type not found");
            return null;
        }

        for (int i = 0; i < mPoolDictionary[objectType].Count; i++)
        {
            if (mPoolDictionary[objectType][i] == null)
            {
                print("mPoolDictionary[objectType][i] is null OBJ NAME = " + objectType);
            }
            if (!mPoolDictionary[objectType][i].activeInHierarchy)
            {
                mPoolDictionary[objectType][i].transform.position = reqPosition;
                mPoolDictionary[objectType][i].transform.rotation = reqRotation;
                mPoolDictionary[objectType][i].SetActive(isActive);

                return mPoolDictionary[objectType][i];
            }
        }

        if (canGrow)
        {
            foreach (Pooler pool in mPool)
            {
                if (pool.poolType == objectType)
                {
                    GameObject obj = (GameObject)Instantiate(pool.objectPrefab);

                    obj.transform.position = reqPosition;
                    obj.transform.rotation = reqRotation;

                    mPoolDictionary[objectType].Add(obj);
                    pool.objCount += 1;

                    return obj;
                }
            }
        }
        else
        {
            Debug.Log("Cannot generate New Objects as 'CanGrow' is set to false");
        }

        return null;
    }

    //public void GenerateObjectTest(string type)
    //{
    //    GameObject obj = GetPooledObject(type);
    //    obj.SetActive(true);
    //}

    //public int GetLiveObjects(List<string> objectTypeList){
    //    int count=0;
    //    foreach(string type in objectTypeList){
    //        for (int i=0;  i < mPoolDictionary[type].Count; i++){
    //            if(mPoolDictionary[type][i].activeSelf){
    //                if(mPoolDictionary[type][i].GetComponent<Orb>().GetIsPlaced())
    //                    count++;
    //            }
    //        }
    //    }
    //    return count;
    //}
}
