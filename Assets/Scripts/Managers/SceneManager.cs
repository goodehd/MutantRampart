using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : IManagers
{
    public Scene Scene { get; set; }

    public bool Init()
    {
        return true;
    }

    public void ChangeScene<T>() where T : Scene
    {
        string sceneName = typeof(T).Name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
