using System;
using UnityEngine;

namespace Assets.HeroEditor.Common.CommonScripts
{
	/// <summary>
	/// Item representation inside IconCollection.
	/// </summary>
	[Serializable]
	public class ItemIcon
	{
		public string Name;
	    public string Path;
        public Sprite Sprite;
    }
}