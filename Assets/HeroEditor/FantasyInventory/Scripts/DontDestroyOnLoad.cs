using System.Linq;
using UnityEngine;

namespace Assets.HeroEditor.FantasyInventory.Scripts
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public string Id;

        public void Awake()
        {
            var existed = FindObjectsOfType<DontDestroyOnLoad>().SingleOrDefault(i => i.Id == Id && i != this);

            if (existed == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}