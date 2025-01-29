using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectScreen : MonoBehaviour
{

    public void SelectCharacter(PlayerCharacterData charData)
    {
        LocalPlayerManager.instance.SelectCharacter(charData);

        this.gameObject.SetActive(false);
    }
}
