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
    #endregion

    // Getters
    #region
    public KingChoiceDataSO[] AllChoices
    {
        get { return allChoices; }
        private set { allChoices = value; }
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
    public void FadeContinueButton(float endAlpha, float duration)
    {
        continueButtonCg.DOFade(endAlpha, duration);
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
    public void FadeOutChoiceButtons()
    {
        StartCoroutine(FadeOutChoiceButtonsCoroutine());
    }
    private IEnumerator FadeOutChoiceButtonsCoroutine()
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
            button.cg.DOFade(0, 0.25f);
        }

        yield return new WaitForSeconds(0.25f);
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
        }

        return choicePairing;
    }
    public void OnChoiceButtonClicked(KingsChoiceButton button)
    {
        HandleChoiceButtonClicked(button);
    }
    private void HandleChoiceButtonClicked(KingsChoiceButton button)
    {

    }
    private void TriggerKingsChoiceEffect(KingChoiceDataSO effect)
    {

    }
    #endregion

}
