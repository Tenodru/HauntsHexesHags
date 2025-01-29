using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("SceneObjects")]
    [SerializeField] SceneObject mainMenuScene;
    [SerializeField] SceneObject level1Scene;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void LoadScene(SceneObject scene)
    {
        // Check if scene exists first
        if (scene == null || SceneUtility.GetBuildIndexByScenePath(scene.sceneName) == -1)
        {
            Debug.Log("Scene could not be found.");
            return;
        }

        SceneManager.LoadScene(scene.sceneName);
    }
}
