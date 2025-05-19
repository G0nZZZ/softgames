using System.Threading.Tasks;
using MagicWords.Models;

namespace MagicWords.Services
{
    /// <summary>
    /// Defines a service for fetching Magic Words data from the remote endpoint.
    /// </summary>
    public interface IMagicWordsService
    {
        /// <summary>
        /// Asynchronously fetches the Magic Words dialogue and avatar data.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the <see cref="MagicWordsData"/> model
        /// containing dialogue entries and avatar entries.
        /// </returns>
        Task<MagicWordsData> FetchMagicWordsAsync();
    }
}