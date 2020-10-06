using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistencyManager : Singleton<PersistencyManager>
{
    private SaveGameData currentSaveFile;
    public SaveGameData CurrentSaveFile
    {
        get { return currentSaveFile; }
        private set { currentSaveFile = value; }
    }
    public void BuildNewSaveFileOnNewGameStarted(List<CharacterTemplateSO> characters)
    {
        CurrentSaveFile = new SaveGameData();
        CurrentSaveFile.characters = new List<CharacterData>();

        foreach(CharacterTemplateSO data in characters)
        {
            CurrentSaveFile.characters.Add(CharacterDataController.Instance.ConverCharacterTemplateToCharacterData(data));
        }
    }
    public void SetupFreshGameFromSaveFile(SaveGameData saveData)
    {
        // RUN OPENING ENCOUNTER LOGIC SHOULD GO HERE!!!
        // logic below doesnt consider the current world position or which encounter
        // shouild be loaded first, it just auto creates a combat event.


        // Play battle theme music
        AudioManager.Instance.PlaySound(Sound.Music_Battle_Theme_1);

        // Build character data
        CharacterDataController.Instance.BuildAllCharactersFromSaveFile(CurrentSaveFile);

        // Create player characters in scene
        CharacterEntityController.Instance.CreateAllPlayerCombatCharacters();

        // Spawn enemies
        EnemySpawner.Instance.SpawnEnemyWave("Basic", GlobalSettings.Instance.testingEnemyWave);

        // Start a new combat event
        ActivationManager.Instance.OnNewCombatEventStarted();
    }
}
