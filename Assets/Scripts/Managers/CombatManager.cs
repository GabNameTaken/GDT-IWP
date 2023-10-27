using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<PlayableCharacter> playableCharactersOnField;

    void StartBattle()
    {
        foreach (PlayableCharacter playableCharacter in playableCharactersOnField)
            playableCharacter.turnMeter = 0;

    }
}
