using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to pass in PlayerCharacter data to anything that doesn't accept enums (i.e. button functions)
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Player/PlayerCharacterData")]
public class PlayerCharacterData : ScriptableObject
{
    public PlayerCharacter character;
    public PauseMenu pauseMenu;
}
