/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{

    /// <summary>
    /// Child class interacting and managed by the PoolManager.
    /// Handles all internal spawning/despawning of active/inactive instances.
    /// </summary>
    public class Pool : MonoBehaviour
    {
        /// <summary>
        /// Prefab to instantiate for pooling.
        /// </summary>
        public GameObject prefab;

#if UNITY_EDITOR
        public string info;
#endif

        /// <summary>
        /// Amount of instances to create at game start.
        /// </summary>
        public int preLoad = 0;

        /// <summary>
        /// Whether the creation of new instances should be limited at runtime.
        /// </summary>
        public bool limit = false;

        /// <summary>
        /// Maximum amount of instances created, if limit is enabled.
        /// </summary>
        public int maxCount;

        /// <summary>
        /// List of active prefab instances for this pool.
        /// </summary>  
        [HideInInspector] public List<GameObject> active = new List<GameObject>();

        /// <summary>
        /// List of inactive prefab instances for this pool.
        /// </summary>
        [HideInInspector] public List<GameObject> inactive = new List<GameObject>();


        /// <summary>
        /// Initialization called by PoolManager on runtime created pools.
        /// </summary>
        public void Awake()
        {

            //can't initialize without prefab
            if (prefab == null) return;

            //add this pool to the PoolManager dictionary
            PoolManager.Add(this);
            PreLoad();
        }


        /// <summary>
        /// Loads specified amount of objects before playtime.
        /// </summary>
        public void PreLoad()
        {
            if (prefab == null)
            {
                Debug.LogWarning("Prefab in pool empty! No Preload happening. Please check references.");
                return;
            }

            //instantiate defined preload amount but don't exceed the maximum amount of objects
            for (int i = totalCount; i < preLoad; i++)
            {
                //instantiate new instance of the prefab
                GameObject obj = (GameObject) Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
                //parent the new instance to this transform
                obj.transform.SetParent(transform);

                //rename it to an unique heading for easier editor overview
                Rename(obj.transform);
                //deactivate object including child objects
                obj.SetActive(false);
                //add object to the list of inactive instances
                inactive.Add(obj);
            }
        }


        /// <summary>
        /// Activates (or instantiates) a new instance for this pool.
        /// </summary>
        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            //init variables
            GameObject obj;
            Transform trans;

            //there are inactive objects available for activation
            if (inactive.Count > 0)
            {
                //get first inactive object in the list
                obj = inactive[0];
                //we want to activate it, remove it from the inactive list
                inactive.RemoveAt(0);

                //get instance transform
                trans = obj.transform;
            }
            else
            {
                //we don't have any inactive objects available,
                //we have to instantiate a new one
                //check if the limited count allows new instantiations
                //if not, return nothing
                if (limit && active.Count >= maxCount)
                    return null;

                //instantiation possible, instantiate new instance of the prefab
                obj = (GameObject) Object.Instantiate(prefab);
                //get instance transform
                trans = obj.transform;
                //rename it to an unique heading for easier editor overview
                Rename(trans);
            }

            //set position and rotation passed in
            trans.position = position;
            trans.rotation = rotation;
            //in case it wasn't parented to this transform, reparent it now
            if (trans.parent != transform)
                trans.SetParent(transform);

            //add object to the list of active instances
            active.Add(obj);
            //activate object including child objects
            obj.SetActive(true);
            //call the method OnSpawn() on every component and children of this object
            obj.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

            //submit instance
            return obj;
        }


        /// <summary>
        /// Deactivates an instance of this pool for later use.
        /// </summary>
        public void Despawn(GameObject instance)
        {
            //search in active instances for this instance
            if (!active.Contains(instance))
            {
                Debug.LogWarning("Can't despawn - Instance not found: " + instance.name + " in Pool " + this.name);
                return;
            }

            //in case it was unparented during runtime, reparent it now
            if (instance.transform.parent != transform)
                instance.transform.SetParent(transform);

            //we want to deactivate it, remove it from the active list
            active.Remove(instance);
            //add object to the list of inactive instances instead
            inactive.Add(instance);
            //call the method OnDespawn() on every component and children of this object
            instance.BroadcastMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);
            //deactivate object including child objects
            instance.SetActive(false);
        }


        /// <summary>
        /// Timed deactivation of an instance of this pool for later use.
        /// </summary>
        public void Despawn(GameObject instance, float time)
        {
            //create new class PoolTimeObject to keep track of the instance
            PoolTimeObject timeObject = new PoolTimeObject();
            //assign time and instance variable of this class
            timeObject.instance = instance;
            timeObject.time = time;

            //start timed deactivation using the created properties
            StartCoroutine(DespawnInTime(timeObject));
        }


        //coroutine which waits for 'time' seconds before deactivating the instance
        IEnumerator DespawnInTime(PoolTimeObject timeObject)
        {
            //cache instance to deactivate
            GameObject instance = timeObject.instance;

            //wait for defined seconds
            float timer = Time.time + timeObject.time;
            while (instance.activeInHierarchy && Time.time < timer)
                yield return null;

            //the instance got deactivated in between already
            if (!instance.activeInHierarchy) yield break;
            //despawn it now
            Despawn(instance);
        }


        /// <summary>
        /// Destroys all inactive instances of this pool (garbage collector heavy). The
        /// parameter determines if only instances above the preLoad value should be destroyed.
        /// </summary>
        public void DestroyUnused(bool limitToPreLoad)
        {
            //only destroy instances above the limit amount
            if (limitToPreLoad)
            {
                //start from the last inactive instance and count down
                //until the index reached the limit amount
                for (int i = inactive.Count - 1; i >= preLoad; i--)
                {
                    //destroy the object at 'i'
                    Object.Destroy(inactive[i]);
                }

                //remove the range of destroyed objects (now null references) from the list
                if (inactive.Count > preLoad)
                    inactive.RemoveRange(preLoad, inactive.Count - preLoad);
            }
            else
            {
                //limitToPreLoad is false, destroy all inactive instances
                for (int i = 0; i < inactive.Count; i++)
                {
                    Object.Destroy(inactive[i]);
                }

                //reset the list
                inactive.Clear();
            }
        }


        /// <summary>
        /// Destroys a specific amount of inactive instances (garbage collector heavy).
        /// </summary>
        public void DestroyCount(int count)
        {
            //the amount which was passed in exceeds the amount of inactive instances
            if (count > inactive.Count)
            {
                Debug.LogWarning("Destroy Count value: " + count + " is greater than inactive Count: " +
                                 inactive.Count + ". Destroying all available inactive objects of type: " +
                                 prefab.name + ". Use DestroyUnused(false) instead to achieve the same.");
                DestroyUnused(false);
                return;
            }

            //starting from the end, count down the index and destroy each inactive instance
            //until we destroyed the amount passed in
            for (int i = inactive.Count - 1; i >= inactive.Count - count; i--)
            {
                Object.Destroy(inactive[i]);
            }

            //remove the range of destroyed objects (now null references) from the list
            inactive.RemoveRange(inactive.Count - count, count);
        }


        //create an unique name for each instance at instantiation
        //to differ them from each other in the editor
        private void Rename(Transform instance)
        {
            //count total instances and assign the next free number
            //convert it in the range of hundreds:
            //there shouldn't be thousands of instances at any time
            //e.g. TestEnemy(Clone)001
            instance.name += (totalCount + 1).ToString("#000");
        }


        //count all instances of this pool option
        private int totalCount
        {
            get
            {
                //initialize count value
                int count = 0;
                //add active and inactive count
                count += active.Count;
                count += inactive.Count;
                //return final count
                return count;
            }
        }


        //when this pool gets destroyed,
        //clear instances lists
        void OnDestroy()
        {
            active.Clear();
            inactive.Clear();
        }
    }
}

namespace Pool
{
    /// <summary>
    /// Stores properties used on timed deactivation of instances.
    /// </summary>
    [System.Serializable]
    public class PoolTimeObject
    {
        /// <summary>
        /// Instance to deactivate.
        /// </summary>
        public GameObject instance;

        /// <summary>
        /// Delay until deactivation.
        /// </summary>
        public float time;
    }
}



