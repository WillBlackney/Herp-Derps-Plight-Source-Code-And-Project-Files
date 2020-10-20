/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections.Generic;
using UnityEngine;

namespace Pool
{

    public class PoolManager : MonoBehaviour
    {
        //mapping of prefab to Pool container managing all of its instances
        private static Dictionary<GameObject, Pool> Pools = new Dictionary<GameObject, Pool>();


        /// <summary>
        /// Called by each Pool on its own, this adds it to the dictionary.
        /// </summary>
        public static void Add(Pool pool)
        {
            //check if the Pool does not contain a prefab
            if (pool.prefab == null)
            {
                Debug.LogError("Prefab of pool: " + pool.gameObject.name +
                               " is empty! Can't add pool to Pools Dictionary.");
                return;
            }

            //check if the Pool has been added already
            if (Pools.ContainsKey(pool.prefab))
            {
                Debug.LogError("Pool with prefab " + pool.prefab.name + " has already been added to Pools Dictionary.");
                return;
            }

            //add it to dictionary
            Pools.Add(pool.prefab, pool);
        }


        /// <summary>
        /// Creates a new Pool at runtime. This is being called for prefabs which have not been linked
        /// to a Pool in the scene in the editor, but are called via Spawn() nonetheless.
        /// </summary>
        public static void CreatePool(GameObject prefab, int preLoad, bool limit = false, int maxCount = 0)
        {
            //debug error if pool was already added before 
            if (Pools.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager already contains Pool for prefab: " + prefab.name);
                return;
            }


            //create new gameobject which will hold the new Pool component
            GameObject newPoolGO = new GameObject("Pool " + prefab.name);
            
            // if (CtrPlay.instance != null) newPoolGO.transform.SetParent(CtrPlay.instance.transform);



            //add Pool component to the new gameobject in the scene
            Pool newPool = newPoolGO.AddComponent<Pool>();
            //assign default parameters
            newPool.prefab = prefab;
            newPool.preLoad = preLoad;
            newPool.limit = limit;
            newPool.maxCount = maxCount;
            //let it initialize itself after assigning variables
            newPool.Awake();

        }


        /// <summary>
        /// Activates a pre-instantiated instance for the prefab passed in, at the desired position.
        /// </summary>
        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            //debug a Log entry in case the prefab was not found in a Pool
            //this is not critical as then we create a new Pool for it at runtime
            if (!Pools.ContainsKey(prefab))
            {
                // Debug.Log("Prefab not found in existing pool: " + prefab.name + " New Pool has been created.");
                CreatePool(prefab, 0, false, 0);
            }

            //spawn instance in the corresponding Pool
            return Pools[prefab].Spawn(position, rotation);
        }


        /// <summary>
        /// Disables a previously spawned instance for later use.
        /// Optionally takes a time value to delay the despawn routine.
        /// </summary>
        public static void Despawn(GameObject instance, float time = 0f)
        {
            if (time > 0) GetPool(instance).Despawn(instance, time);
            else GetPool(instance).Despawn(instance);
        }


        /// <summary>
        /// Convenience method for quick lookup of an pooled object.
        /// Returns the Pool component where the instance has been found in.
        /// </summary>
        public static Pool GetPool(GameObject instance)
        {
            //go over Pools and find the instance
            foreach (GameObject prefab in Pools.Keys)
            {
                if (Pools[prefab].active.Contains(instance))
                    return Pools[prefab];
            }

            //the instance could not be found in a Pool
            Debug.LogError("PoolManager couldn't find Pool for instance: " + instance.name);
            return null;
        }


        /// <summary>
        /// Despawns all instances of a Pool, making them available for later use.
        /// </summary>
        public static void DeactivatePool(GameObject prefab)
        {
            //debug error if Pool wasn't already added before
            if (!Pools.ContainsKey(prefab))
            {
                Debug.LogError("PoolManager couldn't find Pool for prefab to deactivate: " + prefab.name);
                return;
            }

            //cache active count
            int count = Pools[prefab].active.Count;
            //loop through each active instance
            for (int i = count - 1; i > 0; i--)
            {
                Pools[prefab].Despawn(Pools[prefab].active[i]);
            }
        }


        /// <summary>
        /// Destroys all despawned instances of all Pools to free up memory.
        /// The parameter 'limitToPreLoad' decides if only instances above the preload
        /// value should be destroyed, to keep a minimum amount of disabled instances
        /// </summary>
        public static void DestroyAllInactive(bool limitToPreLoad)
        {
            foreach (GameObject prefab in Pools.Keys)
                Pools[prefab].DestroyUnused(limitToPreLoad);
        }


        /// <summary>
        /// Destroys the Pool for a specific prefab.
        /// Active or inactive instances are not available anymore after calling this.
        /// </summary>
        public static void DestroyPool(GameObject prefab)
        {
            //debug error if Pool wasn't already added before
            if (!Pools.ContainsKey(prefab))
            {
                Debug.LogError("PoolManager couldn't find Pool for prefab to destroy: " + prefab.name);
                return;
            }

            //destroy pool gameobject including all children. Our game logic doesn't reparent instances,
            //but if you do, you should loop over the active and deactive instances manually to destroy them
            Destroy(Pools[prefab].gameObject);
            //remove key-value pair from dictionary
            Pools.Remove(prefab);
        }


        /// <summary>
        /// Destroys all Pools stored in the manager dictionary.
        /// Active or inactive instances are not available anymore after calling this.
        /// </summary>
        public static void DestroyAllPools()
        {
            //loop over dictionary and destroy every pool gameobject
            //see DestroyPool method for further comments
            foreach (GameObject prefab in Pools.Keys)
                DestroyPool(Pools[prefab].gameObject);
        }


        //static variables always keep their values over scene changes
        //so we need to reset them when the game ended or switching scenes
        void OnDestroy()
        {
            Pools.Clear();
        }
    }
}
