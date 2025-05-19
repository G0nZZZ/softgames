using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using MagicWords.Models;

namespace MagicWords.Services
{
    /// <summary>
    /// Concrete implementation of <see cref="IMagicWordsService"/> that fetches
    /// Magic Words data from the remote endpoint.
    /// </summary>
    public class MagicWordsService : IMagicWordsService
    {
        private const string Endpoint = "https://private-624120-softgamesassignment.apiary-mock.com/v3/magicwords";
        private const int RequestTimeoutSeconds = 10;

        /// <inheritdoc/>
        public virtual async Task<MagicWordsData> FetchMagicWordsAsync()
        {
            using (var request = CreateRequest(Endpoint))
            {
                request.timeout = RequestTimeoutSeconds;

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

#if UNITY_2020_1_OR_NEWER
                if (request.result != UnityWebRequest.Result.Success)
#else
                if (request.isNetworkError || request.isHttpError)
#endif
                    throw new System.Exception($"Error fetching MagicWords data: {request.error}");

                
                var json = request.downloadHandler.text;
                Debug.Log(json);
                return JsonUtility.FromJson<MagicWordsData>(json);
            }
        }

        /// <summary>
        /// Factory method for creating the UnityWebRequest. Exposed for unit testing.
        /// </summary>
        protected virtual UnityWebRequest CreateRequest(string url) =>
            UnityWebRequest.Get(url);
    }
}