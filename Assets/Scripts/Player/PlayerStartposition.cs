using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartposition : MonoBehaviour
{
    private Vector3 startPosition;
    
    /*
     * Takes the base's position and the position of the first curve (from the base).
     * The start position of the player is then set to be one tile away from the base towards the
     * direction of the first turn.
     */
    public void MovePlayerStartPosition(Vector3 basePosition, Vector3 firstCurvePosition)
    {
        var delta = basePosition - firstCurvePosition;
        delta = delta.normalized;

        startPosition = basePosition - delta;

        var oldPos = gameObject.transform.position;

        startPosition.y += oldPos.y;


        gameObject.transform.position = startPosition;
    }
}
