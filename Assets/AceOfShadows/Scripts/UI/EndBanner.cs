using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AceOfShadows.UI
{
    public class EndBanner : MonoBehaviour
    {
        [SerializeField] private Button button;


        private void Awake()
        {
            button.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        }
    }

}
