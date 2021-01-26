using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using UnityEngine;

namespace Assets.HeroEditor.FantasyInventory.Scripts
{
	public class ItemCollection : MonoBehaviour
	{
		[Header("Required")]
		public List<ItemParams> UserItems;
		public List<ItemParams> GeneratedItems;
        public Dictionary<string, ItemParams> Dict;

		public static ItemCollection Instance;

        public void Awake()
        {
            Instance = this;

            if (Dict == null)
            {
                InitializeDict();
            }
        }

        public void InitializeDict()
        {
            Dict = UserItems.Union(GeneratedItems).ToDictionary(i => i.Id, i => i);
        }
	}
}