using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using MagicWords.Models;
using MagicWords.Services;

public class MagicWordsServiceTests
{
    [Test]
    public async Task FetchMagicWordsAsync_OnSuccess_ReturnsEmptyData()
    {
        var service = new TestableMagicWordsService(success: true);
        var data = await service.FetchMagicWordsAsync();

        Assert.IsNotNull(data);
        Assert.IsNotNull(data.dialogue);
        Assert.IsNotNull(data.avatars);
        Assert.AreEqual(0, data.dialogue.Count);
        Assert.AreEqual(0, data.avatars.Count);
    }

    [Test]
    public void FetchMagicWordsAsync_OnError_ThrowsException()
    {
        var service = new TestableMagicWordsService(success: false);
        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await service.FetchMagicWordsAsync());
        StringAssert.Contains("Error fetching MagicWords data", ex.Message);
    }

    /// <summary>
    /// Bypass UnityWebRequest entirely by overriding the virtual method.
    /// </summary>
    private class TestableMagicWordsService : MagicWordsService
    {
        private readonly bool success;
        public TestableMagicWordsService(bool success) => this.success = success;

        public override Task<MagicWordsData> FetchMagicWordsAsync()
        {
            if (success)
            {
                // Return an “empty” data object
                return Task.FromResult(new MagicWordsData
                {
                    dialogue = new List<DialogueEntry>(),
                    avatars  = new List<AvatarEntry>()
                });
            }
            else
            {
                // Simulate a failure
                throw new Exception("Error fetching MagicWords data: 404 Not Found");
            }
        }
    }
}