using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class KingsBlessingController : Singleton<KingsBlessingController>
{
    [Header("UCM References")]
    [SerializeField] private UniversalCharacterModel playerModel;
    [SerializeField] private UniversalCharacterModel kingModel;
    [SerializeField] private Transform playerModelStartPos;
    [SerializeField] private Transform playerModelMeetingPos;
    [SerializeField] private Transform playerModelEnterDungeonPos;

    [Header("Component References")]
    [SerializeField] GameObject uiVisualParent;
    [SerializeField] CanvasGroup continueButtonCg;

    [Header("King Model References")]
    [SerializeField] private Animator kingFloatAnimator;
    [SerializeField] private List<string> kingBodyParts = new List<string>();

    [Header("Environment References")]
    [SerializeField] private GameObject gateParent;
    [SerializeField] private Transform gateStartPos;
    [SerializeField] private Transform gateEndPos;

    [Header("Sppech Bubble Components")]
    [SerializeField] private CanvasGroup bubbleCg;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private string[] bubbleGreetings;
    [SerializeField] private string[] bubbleFarewells;

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
    public void PlayKingFloatAnim()
    {
        kingFloatAnimator.SetTrigger("Float");
        kingModel.GetComponent<Animator>().SetTrigger("Slow Idle");
    }
    public void FadeContinueButton(float endAlpha, float duration)
    {
        continueButtonCg.DOFade(endAlpha, duration);
    }
    public void OnContinueButtonClicked()
    {
        SetContinueButtonInteractions(false);
        //EventSequenceController.Instance.HandleLoadKingsBlessingContinueSequence();
        EventSequenceController.Instance.HandleLoadNextEncounter();
    }
    public void SetContinueButtonInteractions(bool onOrOff)
    {
        continueButtonCg.interactable = onOrOff;
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
    private void ResetKingsBlessingViews()
    {
        playerModel.gameObject.transform.position = playerModelStartPos.position;
        gateParent.transform.position = gateStartPos.position;
    }
    public void DisableUIView()
    {
        uiVisualParent.SetActive(false);
    }
    public void EnableUIView()
    {
        uiVisualParent.SetActive(true);
    }
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

}
