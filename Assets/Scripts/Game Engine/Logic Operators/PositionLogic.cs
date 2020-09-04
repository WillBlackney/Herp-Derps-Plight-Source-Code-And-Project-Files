using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLogic : Singleton<PositionLogic>
{
    // Calculate Direction + Visual
    #region
    public void FlipCharacterSprite(CharacterEntityView character, bool faceRight)
    {
        Debug.Log("PositionLogic.FlipCharacterSprite() called...");
        float scale = Mathf.Abs(character.ucmVisualParent.transform.localScale.x);

        if (faceRight == true)
        {
            if (character.ucmVisualParent != null)
            {
                character.ucmVisualParent.transform.localScale = new Vector3(scale, Mathf.Abs(scale));
            }
        }

        else
        {
            if (character.ucmVisualParent != null)
            {
                character.ucmVisualParent.transform.localScale = new Vector3(-scale, Mathf.Abs(scale));
            }
        }

    }
    public void SetDirection(CharacterEntityView character, string leftOrRight)
    {
        Debug.Log("PositionLogic. SetDirection() called, setting direction of " + leftOrRight);
        if (leftOrRight == "Left")
        {
            FlipCharacterSprite(character, false);
        }
        else if (leftOrRight == "Right")
        {
            FlipCharacterSprite(character, true);
        }
    }
    public void TurnFacingTowardsLocation(CharacterEntityView entity, Vector3 location)
    {
        if(entity.ucmMovementParent.transform.position.x < location.x)
        {
            SetDirection(entity, "Right");
        }
        else if (entity.ucmMovementParent.transform.position.x > location.x)
        {
            SetDirection(entity, "Left");
        }
    }
    #endregion
    

}
