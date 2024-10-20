using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectScreen : MonoBehaviour
{
    public void SelectCharacter(PlayerCharacterData charData)
    {
        GlobalPlayerManager.instance.localPlayer.ChangePlayerCharacter(charData.character);
        this.gameObject.SetActive(false);
    }
}
