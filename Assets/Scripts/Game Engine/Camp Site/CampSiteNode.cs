using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampSiteNode : MonoBehaviour
{
    [SerializeField] LevelNode levelNode;

    public LevelNode LevelNode
    {
        get { return levelNode; }
        private set { levelNode = value; }
    }
}
