using System;
using System.Collections.Generic;

namespace MagicWords.Models
{
    [Serializable]
    public class MagicWordsData
    {
        public List<DialogueEntry> dialogue;
        public List<AvatarEntry> avatars;
    }
}