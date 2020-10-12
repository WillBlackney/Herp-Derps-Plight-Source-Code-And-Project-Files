using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
#endif

public class SpriteLibrary : MonoBehaviour
    {
        // Singleton Pattern
        #region
        public static SpriteLibrary Instance;
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    #endregion

        // Talent School Badges
        #region
    [Header("Talent School Badges")]
    [PreviewField(75)]
    public Sprite neutralBadge;
    [PreviewField(75)]
    public Sprite warfareBadge;
        [PreviewField(75)]
        public Sprite guardianBadge;
        [PreviewField(75)]
        public Sprite scoundrelBadge;
        [PreviewField(75)]
        public Sprite rangerBadge;
        [PreviewField(75)]
        public Sprite pyromaniaBadge;
        [PreviewField(75)]
        public Sprite divinityBadge;
        [PreviewField(75)]
        public Sprite shadowCraftBadge;
        [PreviewField(75)]
        public Sprite corruptionBadge;
        [PreviewField(75)]
        public Sprite naturalismBadge;
        [PreviewField(75)]
        public Sprite manipulationBadge;
        #endregion

        // Intent Images
        #region
        [Header("Intent Images")]
        [PreviewField(75)]
        public Sprite attack;
        [PreviewField(75)]
        public Sprite attackAll;
        [PreviewField(75)]
        public Sprite attackDefend;
        [PreviewField(75)]
        public Sprite attackBuff;
        [PreviewField(75)]
        public Sprite attackDebuff;
        [PreviewField(75)]
        public Sprite defend;
        [PreviewField(75)]
        public Sprite buff;
        [PreviewField(75)]
        public Sprite defendBuff;
        [PreviewField(75)]
        public Sprite greenDebuff;
        [PreviewField(75)]
        public Sprite purpleDebuff;
        [PreviewField(75)]
        public Sprite unknown;
        [PreviewField(75)]
        public Sprite flee;
    #endregion

        // Card Type Images
        #region
    [Header("Card Type Images")]
    [PreviewField(75)]
    public Sprite meleeAttack;
    [PreviewField(75)]
    public Sprite rangedAttack;
    [PreviewField(75)]
    public Sprite skill;
    [PreviewField(75)]
    public Sprite power;
    #endregion

        // Logic 
        #region
    public Sprite GetTalentSchoolSpriteFromEnumData(TalentSchool data)
        {
            Sprite spriteReturned = null;

            if (data == TalentSchool.Scoundrel)
            {
                spriteReturned = scoundrelBadge;
            }
            else if (data == TalentSchool.Warfare)
            {
                spriteReturned = warfareBadge;
            }
            else if (data == TalentSchool.Corruption)
            {
                spriteReturned = corruptionBadge;
            }
            else if (data == TalentSchool.Divinity)
            {
                spriteReturned = divinityBadge;
            }
            else if (data == TalentSchool.Guardian)
            {
                spriteReturned = guardianBadge;
            }
            else if (data == TalentSchool.Manipulation)
            {
                spriteReturned = manipulationBadge;
            }
            else if (data == TalentSchool.Naturalism)
            {
                spriteReturned = naturalismBadge;
            }
            else if (data == TalentSchool.Pyromania)
            {
                spriteReturned = pyromaniaBadge;
            }
            else if (data == TalentSchool.Shadowcraft)
            {
                spriteReturned = shadowCraftBadge;
            }
            else if (data == TalentSchool.Neutral)
            {
                spriteReturned = neutralBadge;
            }

            return spriteReturned;
        }
    public Sprite GetIntentSpriteFromIntentEnumData(IntentImage data)
        {
            Sprite spriteReturned = null;

            if (data == IntentImage.Attack)
            {
                spriteReturned = attack;
            }
            else if (data == IntentImage.AttackAll)
            {
                spriteReturned = attackAll;
            }
            else if (data == IntentImage.AttackBuff)
            {
                spriteReturned = attackBuff;
            }
            else if (data == IntentImage.AttackDebuff)
            {
                spriteReturned = attackDebuff;
            }
            else if (data == IntentImage.AttackDefend)
            {
                spriteReturned = attackDefend;
            }
            else if (data == IntentImage.Buff)
            {
                spriteReturned = buff;
            }
            else if (data == IntentImage.Defend)
            {
                spriteReturned = defend;
            }
            else if (data == IntentImage.DefendBuff)
            {
                spriteReturned = defendBuff;
            }
            else if (data == IntentImage.GreenDebuff)
            {
                spriteReturned = greenDebuff;
            }
            else if (data == IntentImage.PurpleDebuff)
            {
                spriteReturned = purpleDebuff;
            }
            else if (data == IntentImage.Unknown)
            {
                spriteReturned = unknown;
            }
            else if (data == IntentImage.Flee)
            {
                spriteReturned = flee;
            }

            return spriteReturned;
        }
    public Sprite GetCardTypeImageFromTypeEnumData(CardType data)
    {
        Sprite spriteReturned = null;

        if (data == CardType.MeleeAttack)
        {
            spriteReturned = meleeAttack;
        }
        else if (data == CardType.RangedAttack)
        {
            spriteReturned = rangedAttack;
        }
        else if (data == CardType.Power)
        {
            spriteReturned = power;
        }
        else if (data == CardType.Skill)
        {
            spriteReturned = skill;
        }        

        return spriteReturned;
    }
    #endregion

}

#if UNITY_EDITOR
public class ColorFoldoutGroupAttribute : PropertyGroupAttribute
    {
        public float R, G, B, A;

        public ColorFoldoutGroupAttribute(string path) : base(path)
        {

        }

        public ColorFoldoutGroupAttribute(string path, float r, float g, float b, float a = 1f) : base(path)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            var otherAttr = (ColorFoldoutGroupAttribute)other;

            R = Mathf.Max(otherAttr.R, R);
            G = Mathf.Max(otherAttr.G, G);
            B = Mathf.Max(otherAttr.B, B);
            A = Mathf.Max(otherAttr.A, A);
        }

    }

public class ColorFoldoutGroupAttributeDrawer : OdinGroupDrawer<ColorFoldoutGroupAttribute>
    {
        private LocalPersistentContext<bool> isExpanded;

        protected override void Initialize()
        {
            this.isExpanded = this.GetPersistentValue<bool>("ColorFoldoutGroupAttributeDrawer.isExpaned",
                GeneralDrawerConfig.Instance.ExpandFoldoutByDefault);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUIHelper.PushColor(new Color(Attribute.R, Attribute.G, Attribute.B, Attribute.A));
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.BeginBoxHeader();
            GUIHelper.PopColor();

            isExpanded.Value = SirenixEditorGUI.Foldout(isExpanded.Value, label);
            SirenixEditorGUI.EndBoxHeader();


            if (SirenixEditorGUI.BeginFadeGroup(this, isExpanded.Value))
            {
                for (int i = 0; i < Property.Children.Count; i++)
                {
                    Property.Children[i].Draw();
                }
            }
            SirenixEditorGUI.EndFadeGroup();
            SirenixEditorGUI.EndBox();

        }
    }

#endif
