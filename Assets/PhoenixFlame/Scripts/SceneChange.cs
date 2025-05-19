using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string SceneName;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
    }
}
