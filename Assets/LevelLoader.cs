using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text facts;
    public String[] factLists;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        int rand = Random.Range(1, (factLists.Length-1));
        facts.text = factLists[rand];
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        slider.value = 0.12f;
        yield return new WaitForSeconds(2.5f);
        slider.value = 0.15f;
        yield return new WaitForSeconds(3.0f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            if (progress > 0.15f)
            {
                slider.value = progress;
            }

            if (progress == 1.0f)
            {
                operation.allowSceneActivation = false;
                yield return new WaitForSeconds(1.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
