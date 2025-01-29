using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles local player data.
/// </summary>
public class LocalPlayerManager : MonoBehaviour
{
    public static LocalPlayerManager instance;

    [Header("Player Data")]
    public List<Player> playerList;
    public Player localPlayer;
    public Player guestPlayer;
    public PlayerController localPlayerObject;


    public List<PlayerInputManager> playerObjectList = new List<PlayerInputManager>();

    public Camera mainCamera;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void SelectCharacter(PlayerCharacterData charData)
    {
        localPlayer.ChangePlayerCharacter(charData.character);

        // Disable camera on other players
        foreach (PlayerInputManager playerObject in playerObjectList)
        {
            if (playerObject.GetPlayerCharacter() == charData.character)
            {
                Debug.Log("Found player to add cam to");
                mainCamera.transform.parent = playerObject.transform;
                StartCoroutine(MoveObject(mainCamera.gameObject, new Vector3(0, 1.5f, -10)));
                break;
            }
        }
    }


    public IEnumerator MoveObject(GameObject targetObject, Vector3 destination)
    {
        Debug.Log("Moving object.");
        float totalMoveTime = 1f;
        float currentMoveTime = 0f;
        Vector3 originalPosition = targetObject.transform.localPosition;

        while (Vector2.Distance(targetObject.transform.localPosition, destination) > 0)
        {
            currentMoveTime += Time.deltaTime;
            targetObject.transform.localPosition = Vector3.Lerp(originalPosition, destination, currentMoveTime / totalMoveTime);
            yield return null;
        }
    }
}
