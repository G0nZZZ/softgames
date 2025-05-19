using System;
using UnityEngine;
using System.Threading.Tasks;
using MagicWords.Models;
using MagicWords.Services;
using MagicWords.State;
using UnityEngine.SceneManagement;

namespace MagicWords.Controllers
{
    /// <summary>
    /// Orchestrates loading MagicWords data and populating state,
    /// including fallback on error.
    /// </summary>
    public class MagicWordsController : MonoBehaviour
    {
        private IMagicWordsService _service;
        private MagicWordsState    _state;

        private async void Start()
        {
            _service = new MagicWordsService();
            _state   = MagicWordsState.Instance;
            // 1) Enter loading state
            _state.SetLoading(true);

            try
            {
                // 2) Fetch remote data
                MagicWordsData data = await _service.FetchMagicWordsAsync();

                // 3) Populate state & fire OnDataLoaded
                _state.SetData(data);
            }
            catch
            {
                // 4) On any error, mark error and load fallback JSON
                _state.SetError();

                // Load missing_data.json from Resources/MagicWords/missing_data.json
                TextAsset fallback = Resources.Load<TextAsset>("MagicWords/missing_data");
                if (fallback != null)
                {
                    var data = JsonUtility.FromJson<MagicWordsData>(fallback.text);
                    _state.SetData(data);
                }
                else
                {
                    Debug.LogError("Missing fallback data at Resources/MagicWords/missing_data.json");
                }
            }
            
            _state.OnFinish += OnFinish;
        }

        private void OnDestroy()
        {
            _state.OnFinish -= OnFinish;
        }

        private void OnFinish()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}