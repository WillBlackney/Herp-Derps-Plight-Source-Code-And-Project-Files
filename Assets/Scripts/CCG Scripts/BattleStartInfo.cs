using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStartInfo
{
    public List<CardAsset> deckList;
    public CharacterAsset playerClass;
}

public static class BattleStartInfo  
{
    public static PlayerStartInfo[] PlayerInfos; 
}
