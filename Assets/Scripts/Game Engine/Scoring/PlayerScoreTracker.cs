using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreTracker
{
    public int roomsCleared;
    public int basicEnemiesDefeated;
    public int miniBossEnemiesDefeated;
    public int bossEnemiesDefeated;
    public int basicNoDamageTakenTicks;
    public int miniBossNoDamageTakenTicks;
    public int totalGoldGained;
    public int trinketsCollected;

    // scoring elements done at run end
    /*
     * Librarian: 4 copies of a card
     * Purist: for each character without a rare or epic card
     * Highlander: for each character withou a duplicate card
     * Cursed: for each character with 3 or more afflictions in deck
     * Muscles/Big Brain / Fat Boi: for eeach character with 30+ strength/intelligence/constitution
     * Specialized: for each talent type with 2 points leveled.
     * Porky: for each 10 max health a character has above 100.
     * 
     */
}
public class ScoreElementData
{
    public int totalScore;
    public ScoreElementType type;

    public ScoreElementData()
    {
        
    }
    public ScoreElementData(int totalScore, ScoreElementType type)
    {
        this.totalScore = totalScore;
        this.type = type;
    }
}
public enum ScoreElementType
{
    None = 0,
    RoomsCleared = 1,
    MonsterSlayer = 2,
    GiantSlayer = 3,
    KingSlayer = 8,
    Finesse = 4,
    ProfessionalKiller = 5,
    Riches = 6, 
    Curator = 7,
    Porky = 9,
    Purist = 10,
    ShrugItOff = 11,
    Muscles = 12,
    BigBrain = 13,
    FatBoi = 14,

}
