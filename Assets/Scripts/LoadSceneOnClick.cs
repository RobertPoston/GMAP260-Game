using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    IEnumerator setactive(int sceneIndex)
    {
        yield return new WaitForSeconds(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }

	public void LoadByIndex (int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        StartCoroutine(setactive(sceneIndex));
    }
}