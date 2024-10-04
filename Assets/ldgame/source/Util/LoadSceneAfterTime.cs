using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTime : MonoBehaviour
{
    public float time;
    public string scene;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(scene);
    }
}