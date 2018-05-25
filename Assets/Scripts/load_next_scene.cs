using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_next_scene : MonoBehaviour
{

    IEnumerator setactive(int sceneIndex)
    {
        yield return new WaitForSeconds(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        StartCoroutine(setactive(sceneIndex));
    }
}