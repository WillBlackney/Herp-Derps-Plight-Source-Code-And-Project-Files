using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

public class KingsBlessingController : Singleton<KingsBlessingController>
{
    // Properties + Component References
    #region
    [Header("Choice Data & Properties")]
    [SerializeField] private KingChoiceDataSO[] allChoices;
    [SerializeField] private KingsChoiceButton[] choiceButtons;
    [SerializeField] private GameObject choiceButtonsVisualParent;
    private List<KingsChoicePairingModel> currentChoices = new List<KingsChoicePairingModel>();

    [Header("UCM References")]
    [SerializeField] private UniversalCharacterModel playerModel;
    [SerializeField] private UniversalCharacterModel kingModel;
    [SerializeField] private Transform playerModelStartPos;
    [SerializeField] private Transform playerModelMeetingPos;
    [SerializeField] private Transform playerModelEnterDungeonPos;

    [Header("Component References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] GameObject uiVisualParent;
    [SerializeField] CanvasGroup continueButtonCg;

    [Header("King Model References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private Animator kingFloatAnimator;
    [SerializeField] private List<string> kingBodyParts = new List<string>();

    [Header("Environment References")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private GameObject gateParent;
    [SerializeField] private Transform gateStartPos;
    [SerializeField] private Transform gateEndPos;

    [Header("Sppech Bubble Components")]
    [PropertySpace(SpaceBefore = 20, SpaceAfter = 0)]
    [SerializeField] private CanvasGroup bubbleCg;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private string[] bubbleGreetings;
    [SerializeField] private string[] bubbleFarewells;

    [Header("Misc Choice Properties")]
    private bool awaitingCardUpgradeChoice = false;
    private bool awaitingCardTransformChoice = false;
    private bool awaitingCardDiscoveryChoice = false;
    [HideInInspector] public CardData selectedUpgradeCard = null;
    private KingsChoicePairingModel cachedPairing = null;

    #endregion

    // Getters
    #region
    public KingChoiceDataSO[] AllChoices
    {
        get { return allChoices; }
        private set { allChoices = value; }
    }
    public bool AwaitingCardUpgradeChoice
    {
        get { return awaitingCardUpgradeChoice; }
        private set { awaitingCardUpgradeChoice = value; }
    }
    public bool AwaitingCardDiscoveryChoice
    {
        get { return awaitingCardDiscoveryChoice; }
        private set { awaitingCardDiscoveryChoice = value; }
    }
    public bool AwaitingCardTransformChoice
    {
        get { return awaitingCardTransformChoice; }
        private set { awaitingCardTransformChoice = value; }
    }
    #endregion

    // Setup + Teardown
    #region
    public void BuildAllViews(CharacterData startingCharacter)
    {
        Debug.LogWarning("KingsBlessingController.BuildAllViews()");

        // Set view starting state
        ResetKingsBlessingViews();

        // Build player model
        CharacterModelController.BuildModelFromStringReferences(playerModel, startingCharacter.modelParts);
        CharacterModelController.ApplyItemManagerDataToCharacterModelView(startingCharacter.itemManager, playerModel);

        // Build skeleton king model
        // CharacterModelController.BuildModelFromStringReferences(kingModel, kingBodyParts);
    }
    private void ResetKingsBlessingViews()
    {
        playerModel.gameObject.transform.position = playerModelStartPos.position;
        gateParent.transform.position = gateStartPos.position;
    }
    #endregion

    // Button + GUI Logic
    #region
    public void OnContinueButtonClicked()
    {
        SetContinueButtonInteractions(false);
        EventSequenceController.Instance.HandleLoadNextEncounter();
    }
    public void SetContinueButtonInteractions(bool onOrOff)
    {
        continueButtonCg.interactable = onOrOff;
    }
    public void FadeContinueButton(float endAlpha, float duration, bool disableOnComplete = false)
    {
        continueButtonCg.gameObject.SetActive(true);
        Sequence s = DOTween.Sequence();
        s.Append(continueButtonCg.DOFade(endAlpha, duration));
        s.OnComplete(() =>
        {
            if (disableOnComplete)
            {
                continueButtonCg.gameObject.SetActive(false);
            }
        });
    }
    public void DisableUIView()
    {
        uiVisualParent.SetActive(false);
    }
    public void EnableUIView()
    {
        uiVisualParent.SetActive(true);
    }
    public void FadeInChoiceButtons()
    {
        StartCoroutine(FadeInChoiceButtonsCoroutine());
    }
    private IEnumerator FadeInChoiceButtonsCoroutine()
    {
        choiceButtonsVisualParent.SetActive(true);

        // make all buttons invisible
        foreach(KingsChoiceButton button in choiceButtons)
        {
            button.cg.alpha = 0;
        }

        // reveal each button sequencially
        foreach (KingsChoiceButton button in choiceButtons)
        {
            button.cg.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.25f);            
        }

        // enable button clickability
        foreach (KingsChoiceButton button in choiceButtons)
        {
            button.clickable = true;
        }

    }
    public void FadeOutChoiceButtons(float fadeSpeed = 0.25f)
    {
        StartCoroutine(FadeOutChoiceButtonsCoroutine(fadeSpeed));
    }
    private IEnumerator FadeOutChoiceButtonsCoroutine(float fadeSpeed)
    {
        choiceButtonsVisualParent.SetActive(true);

        // make all buttons visible
        foreach (KingsChoiceButton button in choiceButtons)
        {
            button.clickable = false;
            button.cg.alpha = 1;
        }

        // fade out all buttons 
        foreach (KingsChoiceButton button in choiceButtons)
        {
            button.cg.DOFade(0, fadeSpeed);
        }

        yield return new WaitForSeconds(fadeSpeed);
        choiceButtonsVisualParent.SetActive(false);
    }
    #endregion

    // Visual Events
    #region   
    public void DoKingGreeting()
    {
        StartCoroutine(DoKingGreetingCoroutine());
    }
    private IEnumerator DoKingGreetingCoroutine()
    {
        bubbleCg.DOKill();
        bubbleText.text = bubbleGreetings[RandomGenerator.NumberBetween(0, bubbleGreetings.Length - 1)];
        bubbleCg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(3.5f);
        bubbleCg.DOFade(0, 0.5f);
    }
    public void DoKingFarewell()
    {
        StartCoroutine(DoKingFarewellCoroutine());
    }
    private IEnumerator DoKingFarewellCoroutine()
    {
        bubbleCg.DOKill();
        bubbleText.text = bubbleFarewells[RandomGenerator.NumberBetween(0, bubbleFarewells.Length - 1)];
        bubbleCg.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1.75f);
        bubbleCg.DOFade(0, 0.5f);
    }
    public void DoPlayerModelMoveToMeetingSequence()
    {
        playerModel.gameObject.transform.position = playerModelStartPos.position;
        Sequence s = DOTween.Sequence();
        playerModel.GetComponent<Animator>().SetTrigger("Move");
        s.Append(playerModel.gameObject.transform.DOMove(playerModelMeetingPos.position, 2f));
        AudioManager.Instance.FadeInSound(Sound.Character_Footsteps, 1f);
        s.OnComplete(() =>
        {
            playerModel.GetComponent<Animator>().SetTrigger("Idle");
            AudioManager.Instance.StopSound(Sound.Character_Footsteps);

        });
    }
    public void DoDoorOpeningSequence()
    {
        // Reset gate position
        gateParent.transform.position = gateStartPos.position;

        // Play gate opening sound
        AudioManager.Instance.PlaySound(Sound.Environment_Gate_Open);

        // Move gate up
        Sequence s = DOTween.Sequence();
        s.Append(gateParent.transform.DOMove(gateEndPos.position, 1f));

        // Shake camera on finish
        s.OnComplete(() => CameraManager.Instance.CreateCameraShake(CameraShakeType.Small));
    }
    public void DoPlayerMoveThroughEntranceSequence()
    {
        // Reset position + setup
        playerModel.gameObject.transform.position = playerModelMeetingPos.position;
        Sequence s = DOTween.Sequence();

        // Play move animation
        playerModel.GetComponent<Animator>().SetTrigger("Move");
        s.Append(playerModel.gameObject.transform.DOMove(playerModelEnterDungeonPos.position, 1.5f));

        // Trigger footsteps sfx
        AudioManager.Instance.PlaySound(Sound.Character_Footsteps);
        AudioManager.Instance.FadeOutSound(Sound.Character_Footsteps, 1.5f);

        // On finish, stop footsteps
        s.OnComplete(() =>
        {
            playerModel.GetComponent<Animator>().SetTrigger("Idle");
            //AudioManager.Instance.FadeOutSound(Sound.Character_Footsteps, 1f);
        });

    }
    public void PlayKingFloatAnim()
    {
        kingFloatAnimator.SetTrigger("Float");
        kingModel.GetComponent<Animator>().SetTrigger("Slow Idle");
    }

    #endregion

    // Choices Logic
    #region
    public void BuildAllChoiceButtonViewsFromActiveChoices()
    {
        for(int i = 0; i < choiceButtons.Length; i++)
        {
            BuildChoiceButtonFromChoicePairingData(choiceButtons[i], currentChoices[i]);
        }
    }
    private void BuildChoiceButtonFromChoicePairingData(KingsChoiceButton button, KingsChoicePairingModel data)
    {
        button.myPairingData = data;
        button.descriptionText.text = "<color=#57FF34>" + data.benefitData.choiceDescription + "<color=#FFFFFF>";

        if(data.conseqenceData != null)
        {
            button.descriptionText.text += " <color=#FF4747>" + data.conseqenceData.choiceDescription + "<color=#FFFFFF>";
        }
    }
    public void GenerateAndSetCharacterChoices()
    {
        currentChoices = GenerateFourRandomChoices();
    }
    private List<KingsChoicePairingModel> GenerateFourRandomChoices()
    {
        List<KingsChoicePairingModel> startingChoices = new List<KingsChoicePairingModel>();

        // Get 1 of each choice type: low, medium, high and extreme impact
        startingChoices.Add(GenerateChoicePairing(KingChoiceImpactLevel.Low, AllChoices));
        startingChoices.Add(GenerateChoicePairing(KingChoiceImpactLevel.Medium, AllChoices));
        startingChoices.Add(GenerateChoicePairing(KingChoiceImpactLevel.High, AllChoices));
        startingChoices.Add(GenerateChoicePairing(KingChoiceImpactLevel.Extreme, AllChoices));

        return startingChoices;
    }
    private KingsChoicePairingModel GenerateChoicePairing(KingChoiceImpactLevel impactLevel, IEnumerable<KingChoiceDataSO> dataSet)
    {
        KingsChoicePairingModel choicePairing = new KingsChoicePairingModel();

        List<KingChoiceDataSO> validBenefits = new List<KingChoiceDataSO>();
        List<KingChoiceDataSO> validConsequences = new List<KingChoiceDataSO>();

        foreach(KingChoiceDataSO choiceData in dataSet)
        {
            if(choiceData.impactLevel == impactLevel)
            {
                if(choiceData.category == KingChoiceEffectCategory.Benefit)
                {
                    validBenefits.Add(choiceData);
                }
                else if (choiceData.category == KingChoiceEffectCategory.Consequence)
                {
                    validConsequences.Add(choiceData);
                }
            }
        }

        // Randomize valid collections
        validBenefits.Shuffle();
        validConsequences.Shuffle();

        // Pick random selections
        choicePairing.benefitData = validBenefits[0];
        if(validConsequences.Count > 0)
        {
            choicePairing.conseqenceData = validConsequences[0];

            int loops = 0;

            // Prevent matching effects between benefit and negetive
            // e.g. prevent something like this: 'Gain 20 max hp, lose 15 max hp.'
            while(choicePairing.conseqenceData.effect == choicePairing.benefitData.effect &&
                loops < 20)
            {
                loops++;
                validConsequences.Shuffle();
                choicePairing.conseqenceData = validConsequences[0];                
            }
        }

        return choicePairing;
    }
    public void OnChoiceButtonClicked(KingsChoiceButton button)
    {
        HandleChoiceButtonClicked(button);
    }
    private void HandleChoiceButtonClicked(KingsChoiceButton button)
    {
        cachedPairing = button.myPairingData;

        if (button.myPairingData.benefitData.effect == KingChoiceEffectType.ModifyMaxHealth)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);        
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);

            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.ModifyAttribute)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);
            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.GainPassive)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);
            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.GainRandomCard)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);

            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.GainRandomAffliction)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);

            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.UpgradeRandomCards)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);

            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.TransformRandomCard)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            TriggerKingsChoiceEffect(button.myPairingData.conseqenceData);
            FadeOutChoiceButtons();
            FadeContinueButton(1, 1);
            SetContinueButtonInteractions(true);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.TransformCard)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            FadeOutChoiceButtons(0f);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.UpgradeCard)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            FadeOutChoiceButtons(0f);
        }
        else if (button.myPairingData.benefitData.effect == KingChoiceEffectType.DiscoverCard)
        {
            TriggerKingsChoiceEffect(button.myPairingData.benefitData);
            FadeOutChoiceButtons(0f);
        }
    }
    private void TriggerKingsChoiceEffect(KingChoiceDataSO data)
    {
        if(data == null)
        {
            return;
        }

        // Get starting character data
        CharacterData startingCharacter = CharacterDataController.Instance.AllPlayerCharacters[0];

        // Modify Max Health
        if (data.effect == KingChoiceEffectType.ModifyMaxHealth)
        {
            CharacterDataController.Instance.SetCharacterMaxHealth(startingCharacter, startingCharacter.maxHealth + data.maxHealthGainedOrLost);

            string notifText = "";

            if(data.maxHealthGainedOrLost < 0)
            {
                notifText = "Max Health " + data.maxHealthGainedOrLost.ToString();

                // Blood Splatter VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateBloodExplosion(playerModel.transform.position));
            }
            else if (data.maxHealthGainedOrLost > 0)
            {
                notifText = "Max Health +" + data.maxHealthGainedOrLost.ToString();

                // Heal VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateHealEffect(playerModel.transform.position));

                // Create SFX
                VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
            }

            // Notification VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateStatusEffect(playerModel.transform.position, notifText));       
        }

        // Modify Health
        else if (data.effect == KingChoiceEffectType.ModifyHealth)
        {
            CharacterDataController.Instance.SetCharacterHealth(startingCharacter, startingCharacter.health + data.healthGainedOrLost);

            if (data.healthGainedOrLost < 0)
            {
                // Blood Splatter VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateBloodExplosion(playerModel.transform.position));
            }
            else if (data.healthGainedOrLost > 0)
            {
                // Heal VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                    VisualEffectManager.Instance.CreateHealEffect(playerModel.transform.position));

                // Create SFX
                VisualEventManager.Instance.CreateVisualEvent(() => AudioManager.Instance.PlaySound(Sound.Passive_General_Buff));
            }

            // Notification VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateDamageEffect(playerModel.transform.position, Mathf.Abs(data.healthGainedOrLost)));            
        }

        // Modify Attribute
        else if (data.effect == KingChoiceEffectType.ModifyAttribute)
        {
            string notifMessage = "";
            string notifStacks = "";

            // Modify character stats
            if(data.attributeModified == CoreAttribute.Power)
            {
                CharacterDataController.Instance.ModifyPower(startingCharacter, data.attributeAmountModified);
                notifMessage = "Power ";
            }
            else if (data.attributeModified == CoreAttribute.Dexterity)
            {
                CharacterDataController.Instance.ModifyDexterity(startingCharacter, data.attributeAmountModified);
                notifMessage = "Dexterity ";
            }
            else if (data.attributeModified == CoreAttribute.Stamina)
            {
                CharacterDataController.Instance.ModifyStamina(startingCharacter, data.attributeAmountModified);
                notifMessage = "Stamina ";
            }
            else if (data.attributeModified == CoreAttribute.Draw)
            {
                CharacterDataController.Instance.ModifyDraw(startingCharacter, data.attributeAmountModified);
                notifMessage = "Draw ";
            }
            else if (data.attributeModified == CoreAttribute.Initiative)
            {
                CharacterDataController.Instance.ModifyInitiative(startingCharacter, data.attributeAmountModified);
                notifMessage = "Initiative ";
            }

            // Set stacks text message + play buff or debuff VFX
            if (data.attributeAmountModified > 0)
            {
                notifStacks = "+" + data.attributeAmountModified.ToString();

                // Buff VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateGeneralBuffEffect(playerModel.transform.position));

            }
            else if (data.attributeAmountModified < 0)
            {
                notifStacks = data.attributeAmountModified.ToString();

                // Debuff VFX
                VisualEventManager.Instance.CreateVisualEvent(() =>
                VisualEffectManager.Instance.CreateGeneralDebuffEffect(playerModel.transform.position));
            }

            // Notification VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateStatusEffect(playerModel.transform.position, notifMessage + notifStacks));

        }

        // Gain Passive
        else if (data.effect == KingChoiceEffectType.GainPassive)
        {
            // Choose random passive to apply
            PassivePairingData pData = data.possiblePassives[RandomGenerator.NumberBetween(0, data.possiblePassives.Count - 1)];

            string passiveStringSplit = TextLogic.SplitByCapitals(pData.passiveData.ToString());

            // Apply passive to character data
            PassiveController.Instance.ModifyPassiveOnCharacterEntity(startingCharacter.passiveManager, passiveStringSplit, pData.passiveStacks, false);

            // Set up notification message
            string notifMessage = pData.passiveData.ToString();
            if(pData.passiveStacks > 0)
            {
                notifMessage += " +" + pData.passiveStacks.ToString();
            }
            else if (pData.passiveStacks < 0)
            {
                notifMessage += " " + pData.passiveStacks.ToString();
            }

            // Buff VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateGeneralBuffEffect(playerModel.transform.position));

            // Notification VFX
            VisualEventManager.Instance.CreateVisualEvent(() =>
            VisualEffectManager.Instance.CreateStatusEffect(playerModel.transform.position, notifMessage));
        }

        // Gain random card
        else if (data.effect == KingChoiceEffectType.GainRandomCard)
        {
            List<CardData> cardGained = new List<CardData>();
            List<CardData> allLootableCards = LootController.Instance.GetAllValidLootableCardsForCharacter(startingCharacter);
            List<CardData> validCards = new List<CardData>();

            foreach(CardData card in allLootableCards)
            {
                if(card.rarity == data.randomCardRarity)
                {
                    validCards.Add(card);
                }
            }

            // Choose random card
            validCards.Shuffle();
            cardGained.Add(validCards[0]);

            // Add new card to character deck
            CharacterDataController.Instance.AddCardToCharacterDeck(startingCharacter, cardGained[0]);

            // Create add card to character visual event
            CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, cardGained);
        }

        // Gain random curse
        else if (data.effect == KingChoiceEffectType.GainRandomAffliction)
        {
            // Get affliction card data
            List<CardData> afflictionsGained = new List<CardData>();
            List<CardData> validAfflictions = CardController.Instance.QueryByAffliction(CardController.Instance.AllCards);

            // Randomly choose afflictions added
            for(int i = 0; i < data.afflicationsGained; i++)
            {
                validAfflictions.Shuffle();
                afflictionsGained.Add(validAfflictions[0]);
            }

            // Add curses to character
            foreach(CardData affliction in validAfflictions)
            {
                // Add new card to character deck
                CharacterDataController.Instance.AddCardToCharacterDeck(startingCharacter, affliction);
            }

            // Create add card to character visual event
            CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, afflictionsGained);
        }

        // Upgrade random cards
        else if (data.effect == KingChoiceEffectType.UpgradeRandomCards)
        {
            List<CardData> validUpgradeChoices = new List<CardData>();
            List<CardData> chosenUpgradeCards = new List<CardData>();
            List<CardData> vEventCardData = new List<CardData>();

            // Populate valid upgradeable card choices
            foreach (CardData card in startingCharacter.deck)
            {
                if(card.upgradeable && card.upgradeLevel == 0)
                {
                    validUpgradeChoices.Add(card);
                }
            }

            // choose X random/different cards to upgrade
            for(int i = 0; i < data.randomCardsUpgraded; i++)
            {
                if(validUpgradeChoices.Count > 0)
                {
                    validUpgradeChoices.Shuffle();
                    chosenUpgradeCards.Add(validUpgradeChoices[0]);
                    validUpgradeChoices.Remove(validUpgradeChoices[0]);
                }               
            }

            // Add newly upgraded cards to character deck
            foreach(CardData card in chosenUpgradeCards)
            {
                vEventCardData.Add(CardController.Instance.HandleUpgradeCardInCharacterDeck(card, startingCharacter));
            }

            // Create add card to character visual event
            CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, vEventCardData);
        }

        // Transform random cards
        else if (data.effect == KingChoiceEffectType.TransformRandomCard)
        {
            List<CardData> charactersTransformableCards = new List<CardData>();
            List<CardData> chosenTransformCards = new List<CardData>();
            List<CardData> vEventCardData = new List<CardData>();

            // Populate valid transformable card choices
            foreach (CardData card in startingCharacter.deck)
            {
                charactersTransformableCards.Add(card);
            }

            // choose X random/different cards to transform
            for (int i = 0; i < data.randomCardsTransformed; i++)
            {
                charactersTransformableCards.Shuffle();
                chosenTransformCards.Add(charactersTransformableCards[0]);
                charactersTransformableCards.Remove(charactersTransformableCards[0]);
            }

            // Add newly transformed cards to character deck
            foreach (CardData card in chosenTransformCards)
            {
                vEventCardData.Add(CardController.Instance.HandleTransformCardInCharacterDeck(card, startingCharacter));
            }

            // Create add card to character visual event
            CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, vEventCardData);
        }

        // Upgrade a card
        else if (data.effect == KingChoiceEffectType.UpgradeCard)
        {
            AwaitingCardUpgradeChoice = true;
            CardController.Instance.CreateNewUpgradeCardInDeckPopup(startingCharacter, "Upgrade A Card!");
            CardController.Instance.DisableCardGridScreenBackButton();
        }

        // Transform a card
        else if (data.effect == KingChoiceEffectType.TransformCard)
        {
            AwaitingCardTransformChoice = true;
            CardController.Instance.CreateNewTransformCardInDeckPopup(startingCharacter, "Transform A Card!");
            CardController.Instance.DisableCardGridScreenBackButton();
        }

        // Discover a card
        else if (data.effect == KingChoiceEffectType.DiscoverCard)
        {
            AwaitingCardDiscoveryChoice = true;

            // Get valid discovery choices
            List<CardData> allLootableCards = LootController.Instance.GetAllValidLootableCardsForCharacter(startingCharacter);
            List<CardData> validCards = new List<CardData>();

            foreach (CardData card in allLootableCards)
            {
                if (card.rarity == data.discoveryCardRarity)
                {
                    validCards.Add(card);
                }
            }

            StartCoroutine(CardController.Instance.StartNewDiscoveryEvent(validCards));
        }

        // Delay at end of v events, to create time spacing between benefit and consequence v events
        VisualEventManager.Instance.InsertTimeDelayInQueue(0.5f);

    }
    public void HandleUpgradeCardChoiceMade(CardData card)
    {
        // Setup
        CharacterData character = CharacterDataController.Instance.AllPlayerCharacters[0];
        List<CardData> cList = new List<CardData>();
        cList.Add(CardController.Instance.FindUpgradedCardData(card));

        // Close Grid view Screen
        CardController.Instance.HideCardGridScreen();
        CardController.Instance.HideCardUpgradePopupScreen();

        // Create add card to character visual event
        CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, cList);

        // Add new upgraded card to character data deck
        CardController.Instance.HandleUpgradeCardInCharacterDeck(card, character);

        // Resolve consequence of pairing choice
        TriggerKingsChoiceEffect(cachedPairing.conseqenceData);

        // Finish
        AwaitingCardUpgradeChoice = false;
        selectedUpgradeCard = null;
        cachedPairing = null;

        // Fade in continue button
        FadeContinueButton(1, 0.5f);
    }
    public void HandleTransformCardChoiceMade(CardData card)
    {
        // Setup
        CharacterData character = CharacterDataController.Instance.AllPlayerCharacters[0];
        List<CardData> cList = new List<CardData>();

        // Close Grid view Screen
        CardController.Instance.HideCardGridScreen();        

        // Add new transformed card to character data deck
        cList.Add(CardController.Instance.HandleTransformCardInCharacterDeck(card, character));

        // Do transform visual event
        CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, cList);

        // Resolve consequence of pairing choice
        TriggerKingsChoiceEffect(cachedPairing.conseqenceData);

        // Finish
        AwaitingCardTransformChoice = false;
        cachedPairing = null;

        // Fade in continue button
        FadeContinueButton(1, 0.5f);
    }
    public void HandleDiscoverCardChoiceMade(CardData card)
    {
        // Setup
        CharacterData character = CharacterDataController.Instance.AllPlayerCharacters[0];
        List<CardData> cList = new List<CardData>();
        cList.Add(card);

        // Create add card to character visual event
        CardController.Instance.StartNewShuffleCardsScreenVisualEvent(playerModel, cList);

        // Add new upgraded card to character data deck
        CharacterDataController.Instance.AddCardToCharacterDeck(character, card);

        // Resolve consequence of pairing choice
        TriggerKingsChoiceEffect(cachedPairing.conseqenceData);

        // Finish
        AwaitingCardDiscoveryChoice = false;
        cachedPairing = null;

        // Fade in continue button
        FadeContinueButton(1, 0.5f);
    }
    #endregion

}
