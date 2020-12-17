using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermanentSceneSwapper : MonoBehaviour
{
    void Awake()
    {
        if (MasterManager.sceneSwapper == null)
        {
            MasterManager.sceneSwapper = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(this);
        }
    }
    public void SetFinalScoreScene(Dictionary<string, int[]> data)
    {
        StartCoroutine(LoadScene(data));
    }

    private IEnumerator LoadScene(Dictionary<string, int[]> data)
    {
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();

        Debug.Log("Sigue haciendo!!!");
        GameObject.Find("FinalScoreCanvas").GetComponent<FinalScoreScene>().SetFinalResults(data);
    }
}