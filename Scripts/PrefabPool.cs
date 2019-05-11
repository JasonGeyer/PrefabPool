/* Written By Jason R. Geyer.
 * jason.r.geyer@gmail.com
 * https://github.com/JasonGeyer
 */
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class PrefabPool : MonoBehaviour
{
    /* This class spawns prefabs and pools them. The core way to access this will be to call ActivateObject() or DeactivateObject()
     * If extra objects need to be spawned, a prefab reference can be passed to ActivateObject. If the object does
     * not yet exist, it will be instantiated. Example use: if your 'Shooter' shoots 'Bullet01', reference the
     * the 'Bullet01' prefab as a public GameObject and call ActivateObject(Bullet01Prefab);
     * Note: This needs to be attached to an object in a scene.
     */

    //objects that will pre-spawn upon Awake()
    public List<GameObject> PrespawnPrefabs;
    public List<int> PrespawnCounts;

    public List<GameObject> ActiveGameObjects = new List<GameObject>();
    private List<GameObject> InactiveGameObjects = new List<GameObject>();

    private void Awake()
    {
        if(PrespawnPrefabs.Count > 0)
            for(int i = 0; i < PrespawnPrefabs.Count; i++)
            {
                int spawnCount = 0;
                if(PrespawnCounts.Count > i) spawnCount = PrespawnCounts[i];
                for (int j = 0; j < spawnCount; j++)
                {
                    Debug.Log("spawning: " + PrespawnPrefabs[i]);
                    GameObject newObj = SpawnObject(PrespawnPrefabs[i], Vector3.zero, Quaternion.identity);
                    DeactivateObject(newObj);
                }
            }
    }

    /*Activates a game object. If one is not in the pool already, it will spawn a new one and add it to the pool.
     *If adding the object to the pool is not desired, set addToPool to false. Otherwise, it defaults to true
     */
    public GameObject ActivateObject(GameObject go, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), int activateCount = 1, bool addToPool = true)
    {
        int numberToActiavte = activateCount;
        GameObject newGo = null;
        while(numberToActiavte > 0)
        {
            //find an inactive gameobject and activate it.
            for (int i = 0; i < InactiveGameObjects.Count; i++)
            {
                if (go.Compare(InactiveGameObjects[i]))
                {
                    newGo = InactiveGameObjects[i];
                    InactiveGameObjects[i].SetActive(true);
                    InactiveGameObjects[i].transform.position = position;
                    InactiveGameObjects[i].transform.rotation = rotation;
                    numberToActiavte--;
                }
            }
            //if an inactive object isnt found, spawn a new one
            if (newGo == null)
            {
                if (SpawnObject(go, position, rotation)) numberToActiavte--;
                else
                    break;
            }
        }
        return newGo;
    }

    /* If this function is supplied a prefab (not instantiated), it will
     * remove the first one in the list (presumeably the oldest). If a
     * an instanced object is provided, it will deactivate that specific
     * object.
     */
    public GameObject DeactivateObject(GameObject go)
    {
        GameObject deactivatedObject = null;
        for (int i = 0; i < ActiveGameObjects.Count; i++)
        {
            if (go.transform.parent == null)
            {
                //match with the prefab's name only
                if (go.Compare(ActiveGameObjects[i], false, false))
                {
                    deactivatedObject = ActiveGameObjects[i];
                    ActiveGameObjects[i].SetActive(false);
                    ActiveGameObjects.Remove(deactivatedObject);
                    break;
                }
            }
            else
            {
                //exact match with the gameobject
                if(ActiveGameObjects[i].GetInstanceID() == go.GetInstanceID())
                {
                    deactivatedObject = ActiveGameObjects[i];
                    ActiveGameObjects[i].SetActive(false);
                    ActiveGameObjects.Remove(deactivatedObject);
                    break;
                }
            }
        }
        if (deactivatedObject == null) Debug.Log("didnt find it.");
        else
            Debug.Log("found this: " + deactivatedObject);

        return deactivatedObject;
    }

    //Note: Instantiated objects will lack (Clone) for name matching with prefab name
    private GameObject SpawnObject(GameObject go, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), bool addToPool = true)
    {
        GameObject spawnedObject = null;
        spawnedObject = Instantiate(go, position, rotation, this.transform) as GameObject;
        spawnedObject.name = go.name;
        if (addToPool) ActiveGameObjects.Add(spawnedObject);
        return spawnedObject;
    }
}
