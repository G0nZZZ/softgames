using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace App
{
    /// <summary>
    /// Hooks up menu buttons to load feature scenes.
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        [Header("Assign buttons in Inspector")]
        [SerializeField] private Button btnAce;
        [SerializeField] private Button btnMagicWords;
        [SerializeField] private Button btnPhoenix;

        private void Awake()
        {
            if (btnAce == null || btnMagicWords == null || btnPhoenix == null)
            {
                Debug.LogError("MenuController: button references missing.");
                return;
            }

            btnAce.onClick.AddListener(() => LoadScene("AceOfShadows"));
            btnMagicWords.onClick.AddListener(() => LoadScene("MagicWords"));
            btnPhoenix.onClick.AddListener(() => LoadScene("PhoenixFlame"));
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}