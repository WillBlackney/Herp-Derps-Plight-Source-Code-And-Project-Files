using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.Data;
using HeroEditor.Common;
using UnityEngine;

namespace Assets.HeroEditor.Common.CharacterScripts.Firearms
{
	/// <summary>
	/// Stores firearms and their params.
	/// </summary>
	[CreateAssetMenu(fileName = "FirearmCollection", menuName = "ScriptableObjects/FirearmCollection")]
	public class FirearmCollection : ScriptableObject
	{
        public string Id;
		public List<FirearmParams> Firearms;

		public static Dictionary<string, FirearmCollection> Instances = new Dictionary<string, FirearmCollection>();
		public static FirearmCollection Active;

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			Instances = Resources.LoadAll<FirearmCollection>("").ToDictionary(i => i.Id, i => i);
		}
		
		public void OnValidate()
        {
			var spriteCollection = FindObjectOfType<SpriteCollection>();

			if (spriteCollection == null) return;

			var entries = spriteCollection.Firearms1H.Union(spriteCollection.Firearms2H).ToList();

			foreach (var entry in entries)
			{
				if (Firearms.All(i => i.Name != entry.Name))
				{
					Debug.LogWarningFormat("Firearm params missed for: {0}", entry.Name);
				}
			}

			foreach (var p in Firearms)
			{
				if (entries.All(i => i.Name != p.Name))
				{
					Debug.LogWarningFormat("Excess params found: {0}", p.Name);
				}
			}

			foreach (var firearm in Firearms)
			{
				if (firearm.FirearmTexture != null)
				{
					firearm.Name = firearm.FirearmTexture.name;
				}
			}
		}

		public void UpdateNames()
		{
			foreach (var firearm in Firearms)
			{
				if (firearm.FirearmTexture == null)
				{
					Debug.LogWarningFormat("Please assign a texture for {0}", firearm.Name);
				}
				else
				{
					firearm.Name = firearm.FirearmTexture.name;
				}
			}
		}

		public void RemoveExcess()
		{
			var spriteCollection = FindObjectOfType<SpriteCollection>();
			var entries = spriteCollection.Firearms1H.Union(spriteCollection.Firearms2H).ToList();

			Firearms.RemoveAll(p => entries.All(i => i.Name != p.Name));
		}
	}
}