using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMethods : MonoBehaviour
{
    public static LevelMethods instance;

    void Awake()
    {
        if (instance != null) Debug.LogError("More than one instance of LevelMethods found!");

        instance = this;
    }

    public void Restart()
    {
        Play(SceneManager.GetActiveScene().name);
    }

    public void Play(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
