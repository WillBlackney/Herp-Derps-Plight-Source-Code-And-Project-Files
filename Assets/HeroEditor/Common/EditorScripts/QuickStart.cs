using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.ExampleScripts;
using UnityEngine;

namespace Assets.HeroEditor.Common.EditorScripts
{
    /// <summary>
    /// A small helper used in Quick Start scene.
    /// </summary>
    public class QuickStart : MonoBehaviour
    {
        public List<Character> CharacterPrefabs;
        public MovementExample MovementExample;
        public AttackingExample AttackingExample;
        public BowExample BowExample;
        public EquipmentExample EquipmentExample;
        public void Awake()
        {
            var character = Instantiate(CharacterPrefabs.First(i => i != null));

            character.transform.position = Vector2.zero;

            MovementExample.Character = character;
            AttackingExample.Character = character;
            BowExample.Character = character;
            EquipmentExample.Character = character;
        }
    }
}