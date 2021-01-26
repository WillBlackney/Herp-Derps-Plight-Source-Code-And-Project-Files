using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.HeroEditor.FantasyHeroes.TestRoom.Scripts
{
    /// <summary>
    /// This script can be used for swapping monster skins.
    /// </summary>
    public class MonsterVariation : MonoBehaviour
    {
        public List<Sprite> Sprites;

        public void Apply()
        {
            foreach (var sr in GetComponent<LayerManager>().Sprites)
            {
                var sprite = Sprites.SingleOrDefault(i => i.name == sr.sprite.name);

                if (sprite != null)
                {
                    sr.sprite = sprite;
                }
            }

            var monster = GetComponent<Monster>();

            monster.HeadSprites = new List<Sprite> { Sprites.Single(i => i.name == "HeadNormal"), Sprites.Single(i => i.name == "HeadAngry"), Sprites.Single(i => i.name == "HeadDead") };
        }
    }
}