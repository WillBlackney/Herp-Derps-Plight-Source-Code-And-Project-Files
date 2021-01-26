using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using Assets.HeroEditor.Common.CommonScripts;
using Assets.HeroEditor.FantasyInventory.Scripts.Data;
using Assets.HeroEditor.FantasyInventory.Scripts.Interface.Elements;
using Assets.HeroEditor4D.SimpleColorPicker.Scripts;
using HeroEditor.Common;
using HeroEditor.Common.Enums;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.HeroEditor.Common.EditorScripts
{
    /// <summary>
    /// Character editor UI and behaviour.
    /// </summary>
    public class CharacterEditor : CharacterEditorBase
    {
        [Header("Public")]
        public Transform Tabs;
        public ScrollInventory Inventory;
        public Text ItemName;

        [Header("Other")]
        public List<string> PaintParts;
        public Button PaintButton;
        public ColorPicker ColorPicker;
        public string PrefabFolder;

        public Action<Item> EquipCallback;

        public static string CharacterJson;

        private Toggle ActiveTab => Tabs.GetComponentsInChildren<Toggle>().Single(i => i.isOn);
        private FirearmCollection FirearmCollection => FirearmCollection.Instances[Character.SpriteCollection.Id];

        /// <summary>
        /// Called automatically on app start.
        /// </summary>
        public void Awake()
        {
            RestoreTempCharacter();
        }

        public new void Start()
        {
            base.Start();

            if (Tabs.gameObject.activeSelf)
            {
                Tabs.GetComponentInChildren<Toggle>().isOn = true;
            }
        }

        /// <summary>
        /// This can be used as an example for building your own inventory UI.
        /// </summary>
        public void OnSelectTab(bool value)
        {
            if (value)
            {
                Refresh();
            }
        }

        public void Refresh(int defaultIndex = 0)
        {
            Item.GetParams = null;

            Dictionary<string, SpriteGroupEntry> dict;
            Action<Item> equipAction;
            int equippedIndex;
            var tab = ActiveTab;
            var spriteCollection = Character.SpriteCollection;

            switch (tab.name)
            {
                case "Helmet":
                {
                    dict = spriteCollection.Helmet.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Helmet);
                    equippedIndex = spriteCollection.Helmet.FindIndex(i => i.Sprites.Contains(Character.Helmet));
                    break;
                }
                case "Armor":
                {
                    dict = spriteCollection.Armor.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Armor);
                    equippedIndex = Character.Armor == null ? -1 : spriteCollection.Armor.FindIndex(i => i.Sprites.SequenceEqual(Character.Armor));
                    break;
                }
                case "Pauldrons":
                case "Vest":
                case "Gloves":
                case "Belt":
                case "Boots":
                {
                    string part;

                    switch (tab.name)
                    {
                        case "Pauldrons": part = "ArmR"; break;
                        case "Vest": part = "Torso"; break;
                        case "Gloves": part = "SleeveR"; break;
                        case "Belt": part = "Pelvis"; break;
                        case "Boots": part = "Shin"; break;
                        default: throw new NotSupportedException(tab.name);
                    }

                    dict = spriteCollection.Armor.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], tab.name.ToEnum<EquipmentPart>());
                    equippedIndex = Character.Armor == null ? -1 : spriteCollection.Armor.FindIndex(i => i.Sprites.Contains(Character.Armor.SingleOrDefault(j => j.name == part)));
                    Item.GetParams = item => new ItemParams { Id = item.Id, Path = dict[item.Id] == null ? null : dict[item.Id].Path.Replace("Armor/", $"{tab.name}/") + $".{tab.name}", Meta = dict[item.Id] == null ? null : JsonConvert.SerializeObject(dict[item.Id].Tags) };
                    break;
                }
                case "Shield":
                {
                    dict = spriteCollection.Shield.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Shield);
                    equippedIndex = spriteCollection.Shield.FindIndex(i => i.Sprites.Contains(Character.Shield));
                    break;
                }
                case "Melee1H":
                {
                    dict = spriteCollection.MeleeWeapon1H.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.MeleeWeapon1H);
                    equippedIndex = spriteCollection.MeleeWeapon1H.FindIndex(i => i.Sprites.Contains(Character.PrimaryMeleeWeapon));
                    break;
                }
                case "Melee2H":
                {
                    dict = spriteCollection.MeleeWeapon2H.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.MeleeWeapon2H);
                    equippedIndex = spriteCollection.MeleeWeapon2H.FindIndex(i => i.Sprites.Contains(Character.PrimaryMeleeWeapon));
                    break;
                }
                case "MeleePaired":
                {
                    dict = spriteCollection.MeleeWeapon1H.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.MeleeWeaponPaired);
                    equippedIndex = spriteCollection.MeleeWeapon1H.FindIndex(i => i.Sprites.Contains(Character.SecondaryMeleeWeapon));
                    break;
                }
                case "Bow":
                {
                    dict = spriteCollection.Bow.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Bow);
                    equippedIndex = Character.Bow == null ? -1 : spriteCollection.Bow.FindIndex(i => i.Sprites.SequenceEqual(Character.Bow));
                    break;
                }
                case "Firearm1H":
                {
                    dict = spriteCollection.Firearms1H.ToDictionary(i => i.FullName, i => i);
                    equipAction = item =>
                    {
                        Character.GetFirearm().Params = item.Id == "Empty" ? null : FirearmCollection.Firearms.Single(i => i.Name == item.Id.Split('/')[1]);
                        Character.Equip(dict[item.Id], EquipmentPart.Firearm1H);
                        Character.Animator.SetBool("Ready", true);
                    };
                    equippedIndex = Character.Firearms == null ? -1 : spriteCollection.Firearms1H.FindIndex(i => i.Sprites.SequenceEqual(Character.Firearms));
                    break;
                }
                case "Firearm2H":
                {
                    dict = spriteCollection.Firearms2H.ToDictionary(i => i.FullName, i => i);
                    equipAction = item =>
                    {
                        Character.GetFirearm().Params = item.Id == "Empty" ? null : FirearmCollection.Firearms.Single(i => i.Name == item.Id.Split('/')[1]);
                        Character.Equip(dict[item.Id], EquipmentPart.Firearm2H);
                        Character.Animator.SetBool("Ready", true);
                    };
                    equippedIndex = Character.Firearms == null ? -1 : spriteCollection.Firearms2H.FindIndex(i => i.Sprites.SequenceEqual(Character.Firearms));
                    break;
                }
                case "Cape":
                {
                    dict = spriteCollection.Cape.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Cape);
                    equippedIndex = spriteCollection.Cape.FindIndex(i => i.Sprites.Contains(Character.Cape));
                    break;
                }
                case "Back":
                {
                    dict = spriteCollection.Back.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Back);
                    equippedIndex = spriteCollection.Back.FindIndex(i => i.Sprites.Contains(Character.Back));
                    break;
                }
                case "Body":
                {
                    dict = spriteCollection.Body.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Body);
                    equippedIndex = Character.Body == null ? -1 : spriteCollection.Body.FindIndex(i => i.Sprites.SequenceEqual(Character.Body));
                    break;
                }
                case "Head":
                {
                    dict = spriteCollection.Head.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Head);
                    equippedIndex = spriteCollection.Head.FindIndex(i => i.Sprites.Contains(Character.Head));
                    break;
                }
                case "Ears":
                {
                    dict = spriteCollection.Ears.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Ears);
                    equippedIndex = spriteCollection.Ears.FindIndex(i => i.Sprites.Contains(Character.Ears));
                    break;
                }
                case "Eyebrows":
                {
                    dict = spriteCollection.Eyebrows.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Eyebrows);
                    equippedIndex = spriteCollection.Eyebrows.FindIndex(i => i.Sprites.Contains(Character.Expressions[0].Eyebrows));
                    break;
                }
                case "Eyes":
                {
                    dict = spriteCollection.Eyes.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Eyes);
                    equippedIndex = spriteCollection.Eyes.FindIndex(i => i.Sprites.Contains(Character.Expressions[0].Eyes));
                    break;
                }
                case "Hair":
                {
                    dict = spriteCollection.Hair.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Hair);
                    equippedIndex = spriteCollection.Hair.FindIndex(i => i.Sprites.Contains(Character.Hair));
                    break;
                }
                case "Beard":
                {
                    dict = spriteCollection.Beard.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Beard);
                    equippedIndex = spriteCollection.Beard.FindIndex(i => i.Sprites.Contains(Character.Beard));
                    break;
                }
                case "Mouth":
                {
                    dict = spriteCollection.Mouth.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Mouth);
                    equippedIndex = spriteCollection.Mouth.FindIndex(i => i.Sprites.Contains(Character.Expressions[0].Mouth));
                    break;
                }
                //case "Makeup":
                //    {
                //        dict = SpriteCollection.Makeup.ToDictionary(i => i.FullName, i => i);
                //        equipAction = item => Character.SetBody(dict[item.Id], BodyPart.Makeup);
                //        equippedIndex = SpriteCollection.Makeup.FindIndex(i => i.Sprites.Contains(Character.Makeup));
                //        break;
                //    }
                case "Earrings":
                {
                    dict = spriteCollection.Earrings.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Earrings);
                    equippedIndex = spriteCollection.Earrings.FindIndex(i => i.Sprites.Contains(Character.Earrings));
                    break;
                }
                case "Glasses":
                {
                    dict = spriteCollection.Glasses.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Glasses);
                    equippedIndex = spriteCollection.Glasses.FindIndex(i => i.Sprites.Contains(Character.Glasses));
                    break;
                }
                case "Mask":
                {
                    dict = spriteCollection.Mask.ToDictionary(i => i.FullName, i => i);
                    equipAction = item => Character.Equip(dict[item.Id], EquipmentPart.Mask);
                    equippedIndex = spriteCollection.Mask.FindIndex(i => i.Sprites.Contains(Character.Mask));
                    break;
                }
                default:
                    throw new NotImplementedException(tab.name);
            }

            var items = dict.Values.Select(i => new Item(i.FullName)).ToList();

            items.Insert(0, new Item("Empty"));
            dict.Add("Empty", null);

            if (Item.GetParams == null)
            {
                Item.GetParams = item => new ItemParams { Id = item.Id, Path = dict[item.Id]?.Path, Meta = dict[item.Id] == null ? null : JsonConvert.SerializeObject(dict[item.Id].Tags) }; // We override GetParams method because we don't have a database with item params.
            }

            IconCollection.Active = IconCollection.Instances[Character.SpriteCollection.Id];

            if (equippedIndex == -1) equippedIndex = defaultIndex;

            Inventory.OnLeftClick = item =>
            {
                equipAction(item);
                EquipCallback?.Invoke(item);
                ItemName.text = item.Params.Id ?? "Empty";
                SetPaintButton(tab.name, item);
            };

            var equipped = items.Count > equippedIndex + 1 ? items[equippedIndex + 1] : null;

            Inventory.Initialize(ref items, equipped, reset: true);
            SetPaintButton(tab.name, equipped);
        }

        private void SetPaintButton(string tab, Item item)
        {
            var tags = item?.Params.MetaToList() ?? new List<string>();

            PaintButton.interactable = PaintParts.Contains(tab) && !tags.Contains("NoPaint") || tags.Contains("Paint");
        }

        /// <summary>
        /// Remove all equipment.
        /// </summary>
        public void Reset()
        {
            Character.ResetEquipment();
            Refresh(-1);
        }

        /// <summary>
        /// Randomize character.
        /// </summary>
        public void Randomize()
        {
            Character.Randomize();
            OnSelectTab(true);
        }

        #if UNITY_EDITOR

        /// <summary>
        /// Save character to prefab.
        /// </summary>
        public void Save()
        {
            PrefabFolder = UnityEditor.EditorUtility.SaveFilePanel("Save character prefab", PrefabFolder, "New character", "prefab");

	        if (PrefabFolder.Length > 0)
	        {
		        Save("Assets" + PrefabFolder.Replace(Application.dataPath, null));
	        }
		}

	    /// <summary>
		/// Load character from prefab.
		/// </summary>
		public void Load()
        {
	        PrefabFolder = UnityEditor.EditorUtility.OpenFilePanel("Load character prefab", PrefabFolder, "prefab");

            if (PrefabFolder.Length > 0)
            {
                Load("Assets" + PrefabFolder.Replace(Application.dataPath, null));
            }

			//FeatureTip();
		}

	    /// <summary>
	    /// Save character to json.
	    /// </summary>
	    public void SaveToJson()
	    {
		    PrefabFolder = UnityEditor.EditorUtility.SaveFilePanel("Save character to json", PrefabFolder, "New character", "json");

		    if (PrefabFolder.Length > 0)
		    {
			    var path = "Assets" + PrefabFolder.Replace(Application.dataPath, null);
			    var json = Character.ToJson();

			    System.IO.File.WriteAllText(path, json);
			    Debug.LogFormat("Json saved to {0}: {1}", path, json);
		    }
		}

		/// <summary>
		/// Load character from json.
		/// </summary>
		public void LoadFromJson()
	    {
		    PrefabFolder = UnityEditor.EditorUtility.OpenFilePanel("Load character from json", PrefabFolder, "json");

		    if (PrefabFolder.Length > 0)
		    {
				var path = "Assets" + PrefabFolder.Replace(Application.dataPath, null);
			    var json = System.IO.File.ReadAllText(path);

				Character.FromJson(json);
			}
	    }

		public override void Save(string path)
		{
			Character.transform.localScale = Vector3.one;

			#if UNITY_2018_3_OR_NEWER

			UnityEditor.PrefabUtility.SaveAsPrefabAsset(Character.gameObject, path);

			#else

			UnityEditor.PrefabUtility.CreatePrefab(path, Character.gameObject);

			#endif

            Debug.LogFormat("Prefab saved as {0}", path);
        }

        public override void Load(string path)
        {
			var character = UnityEditor.AssetDatabase.LoadAssetAtPath<Character>(path);

            Character.GetFirearm().Params = character.Firearm.Params; // TODO: Workaround
			Load(character);
            Character.GetComponent<CharacterBodySculptor>().OnCharacterLoaded(character);
        }

	    #else

        public override void Save(string path)
        {
            throw new NotSupportedException();
        }

        public override void Load(string path)
        {
            throw new NotSupportedException();
        }

        #endif

        /// <summary>
        /// Load a scene by name.
        /// </summary>
        public void LoadScene(string sceneName)
        {
            #if UNITY_EDITOR

            if (!UnityEditor.EditorBuildSettings.scenes.Any(i => i.path.Contains(sceneName) && i.enabled))
            {
	            UnityEditor.EditorUtility.DisplayDialog("Hero Editor", $"Please add '{sceneName}.scene' to Build Settings!", "OK");
				return;
            }

            #endif

            CharacterJson = Character.ToJson();
            SceneManager.LoadScene(sceneName);
		}

        /// <summary>
		/// Navigate to URL.
		/// </summary>
		public void Navigate(string url)
        {
            #if UNITY_WEBGL && !UNITY_EDITOR

            Application.ExternalEval($"window.open('{url}')");

            #else

			Application.OpenURL(url);

            #endif
        }

		protected override void SetFirearmParams(SpriteGroupEntry entry)
        {
            if (entry == null) return;

            if (FirearmCollection.Firearms.Count(i => i.Name == entry.Name) != 1) throw new Exception("Please check firearms params for: " + entry.Name);

            Character.GetFirearm().Params = FirearmCollection.Firearms.Single(i => i.Name == entry.Name);
		}

        private Color _color;

        public void OpenColorPicker()
        {
            var currentColor = ResolveParts(ActiveTab.name).FirstOrDefault()?.color ?? Color.white;

            ColorPicker.Color = _color = currentColor;
            ColorPicker.OnColorChanged = Paint;
            ColorPicker.SetActive(true);
        }

        public void CloseColorPicker(bool apply)
        {
            if (!apply) Paint(_color);

            ColorPicker.SetActive(false);
        }

        public void Paint(Color color)
        {
            foreach (var part in ResolveParts(ActiveTab.name))
            {
                part.color = color;
                part.sharedMaterial = color == Color.white ? DefaultMaterial : ActiveTab.name == "Eyes" ? EyesPaintMaterial : EquipmentPaintMaterial;
            }
        }

        private void RestoreTempCharacter()
        {
            if (CharacterJson != null)
            {
                Character.FromJson(CharacterJson);
            }
        }

	    protected override void FeedbackTip()
	    {
			#if UNITY_EDITOR

		    var success = UnityEditor.EditorUtility.DisplayDialog("Hero Editor", "Hi! Thank you for using my asset! I hope you enjoy making your game with it. The only thing I would ask you to do is to leave a review on the Asset Store. It would be awesome support for my asset, thanks!", "Review", "Later");
			
			RequestFeedbackResult(success);

			#endif
	    }
    }
}