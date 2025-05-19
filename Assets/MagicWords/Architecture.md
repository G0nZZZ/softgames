Magic Words System Architecture
Below is the proposed full architecture for the Magic Words feature, implemented in Unity (C#).

📂 File & Folder Structure

Assets/
├── Scenes/
│   └── MagicWords.unity                      # Scene entry point for Magic Words
│
├── Scripts/
│   ├── Common/                               # Shared utilities & helpers
│   │   ├── JsonUtilityExtensions.cs          # JSON parsing helpers
│   │   └── UnityWebRequestExtensions.cs      # HTTP helpers
│   │
│   ├── MagicWords/                           # All feature-specific code
│   │   ├── Models/
│   │   │   ├── DialogueEntry.cs              # Data class for one line of dialogue
│   │   │   ├── AvatarEntry.cs                # Data class for one avatar
│   │   │   └── MagicWordsData.cs             # Root data object (dialogue + avatars)
│   │   │
│   │   ├── Services/
│   │   │   ├── IMagicWordsService.cs         # Interface for data fetching
│   │   │   └── MagicWordsService.cs          # HTTP client implementation
│   │   │
│   │   ├── State/
│   │   │   └── MagicWordsState.cs            # Holds current data & loading state
│   │   │
│   │   ├── Controllers/
│   │   │   └── MagicWordsController.cs       # Orchestrates loading & view updates
│   │   │
│   │   ├── Views/
│   │   │   ├── DialogueView.cs               # Renders text + emojis to UI
│   │   │   └── AvatarView.cs                 # Renders avatar images & positions
│   │   │
│   │   └── UI/
│   │       ├── Prefabs/
│   │       │   ├── DialogueLine.prefab       # Text + emoji layout
│   │       │   └── AvatarImage.prefab        # Image + name layout
│   │       └── MagicWordsCanvas.prefab       # Root canvas for the feature
│   │
│   └── App/
│       └── MenuController.cs                 # In-game menu to launch tasks
│
└── Resources/
    └── MagicWords/                           # Local fallbacks & placeholders
        ├── default_avatar.png
        └── missing_data.json
🏷 What Each Part Does
Scenes/MagicWords.unity

Entry point. Contains the root MagicWordsController GameObject with child canvases for UI.

Common/

Reusable helper extensions (e.g., JSON parsing, HTTP request wrappers).

MagicWords/Models/

DialogueEntry.cs

csharp
Copiar
Editar
public class DialogueEntry {
    public string name;
    public string text;
}
AvatarEntry.cs

csharp
Copiar
Editar
public class AvatarEntry {
    public string name;
    public string url;
    public string position;
}
MagicWordsData.cs

csharp
Copiar
Editar
public class MagicWordsData {
    public List<DialogueEntry> dialogue;
    public List<AvatarEntry> avatars;
}
MagicWords/Services/

IMagicWordsService.cs

csharp
Copiar
Editar
public interface IMagicWordsService {
    Task<MagicWordsData> FetchMagicWordsAsync();
}
MagicWordsService.cs

Implements IMagicWordsService using UnityWebRequest to GET the endpoint.

Handles timeouts, retries, JSON deserialization via JsonUtilityExtensions.

MagicWords/State/

MagicWordsState.cs

A ScriptableObject or MonoBehaviour singleton holding:

MagicWordsData Data

bool IsLoading / bool HasError

Event hooks (OnDataLoaded, OnError).

MagicWords/Controllers/

MagicWordsController.cs

On Start(), sets State.IsLoading = true, calls Service.FetchMagicWordsAsync().

On success: stores data in State.Data, fires OnDataLoaded.

On failure: sets State.HasError = true, uses fallback JSON from Resources/MagicWords/missing_data.json.

MagicWords/Views/

DialogueView.cs

Subscribes to State.OnDataLoaded.

Iterates State.Data.dialogue, instantiates DialogueLine.prefab, sets <name> and interprets inline {emojis} into Unicode characters.

Handles scrolling or pagination.

AvatarView.cs

Reads State.Data.avatars, instantiates AvatarImage.prefab per entry.

Uses UnityWebRequestTexture.GetTexture() to load from url; on failure, uses default_avatar.png.

Positions each image (left / right).

MagicWords/UI/Prefabs/

DialogueLine.prefab

Contains TextMeshProUGUI components for speaker name and dialogue text.

AvatarImage.prefab

Contains Image component + Text for name, laid out for left/right anchoring.

App/MenuController.cs

In-game menu to switch between “Ace of Shadows”, “Magic Words”, and “Phoenix Flame” scenes.

Resources/MagicWords/

default_avatar.png — fallback image.

missing_data.json — minimal JSON for error cases.

🔄 Where State Lives & How Services Connect
Startup

MagicWordsController.Start() → MagicWordsState.IsLoading = true

Data Fetch

Controller → calls IMagicWordsService.FetchMagicWordsAsync() (in MagicWordsService)

Service → performs UnityWebRequest.Get("https://…/v3/magicwords") → parses JSON → returns MagicWordsData

State Population

Controller → on success: MagicWordsState.Data = data, IsLoading = false, invokes OnDataLoaded

on error: HasError = true, loads missing_data.json from Resources, populates state

View Rendering

DialogueView & AvatarView subscribe to OnDataLoaded

They read MagicWordsState.Data, instantiate prefabs under the root Canvas

Error Handling & Fallbacks

Avatar load failures → fallback to default_avatar.png

Missing fields in JSON → string defaults or skip entries

🔗 Endpoint Response Format
json
Copiar
Editar
{
  "dialogue": [
    { "name": "Sheldon", "text": "I admit {satisfied} the design of Cookie Crush is quite elegant in its simplicity." },
    { "name": "Leonard", "text": "That’s practically a compliment, Sheldon. {intrigued} Are you feeling okay?" },
    { "name": "Penny",   "text": "Don’t worry, Leonard. He’s probably just trying to justify playing it himself." },
    { "name": "Sheldon", "text": "Incorrect. {neutral} I’m studying its mechanics. The progression system is oddly satisfying." },
    { "name": "Penny",   "text": "It’s called fun, Sheldon. You should try it more often." },
    { "name": "Leonard", "text": "She’s got a point. Sometimes, a simple game can be relaxing." },
    { "name": "Neighbour", "text": "I fully agree {affirmative}" },
    { "name": "Sheldon", "text": "Relaxing? I suppose there’s merit in low-stakes gameplay to reduce cortisol levels." },
    { "name": "Penny",   "text": "Translation: Sheldon likes crushing cookies but won’t admit it. {laughing}" },
    { "name": "Sheldon", "text": "Fine. I find the color-matching oddly soothing. Happy?" },
    { "name": "Leonard", "text": "Very. Now we can finally play as a team in Wordscapes." },
    { "name": "Penny",   "text": "Wait, Sheldon’s doing team games now? What’s next, co-op decorating?" },
    { "name": "Sheldon", "text": "Unlikely. But if the design involves symmetry and efficiency, I may consider it." },
    { "name": "Penny",   "text": "See? Casual gaming brings people together!" },
    { "name": "Leonard", "text": "Even Sheldon. That’s a win for everyone. {win}" },
    { "name": "Sheldon", "text": "Agreed. {neutral} Though I still maintain chess simulators are superior." },
    { "name": "Penny",   "text": "Sure, Sheldon. {intrigued} You can play chess *after* we beat this next level." }
  ],
  "avatars": [
    {
      "name": "Sheldon",
      "url":  "https://api.dicebear.com:81/timeout",
      "position": "right"
    },
    {
      "name": "Sheldon",
      "url":  "https://api.dicebear.com/9.x/personas/png?body=squared&clothingColor=6dbb58&eyes=open&hair=buzzcut&hairColor=6c4545&mouth=smirk&nose=smallRound&skinColor=e5a07e",
      "position": "left"
    },
    {
      "name": "Penny",
      "url":  "https://api.dicebear.com/9.x/personas/png?body=squared&clothingColor=f55d81&eyes=happy&hair=extraLong&hairColor=f29c65&mouth=smile&nose=smallRound&skinColor=e5a07e",
      "position": "right"
    },
    {
      "name": "Leonard",
      "url":  "https://api.dicebear.com/9.x/personas/png?body=checkered&clothingColor=f3b63a&eyes=glasses&hair=shortCombover&hairColor=362c47&mouth=surprise&nose=mediumRound&skinColor=d78774",
      "position": "right"
    },
    {
      "name": "Nobody",
      "url":  "https://api.dicebear.com/5.x/personas/",
      "position": "right"
    }
  ]
}
This architecture emphasizes modularity, testability, and clear separation of concerns: services fetch data, state holds it, controllers orchestrate, and views render. Error paths fall back gracefully using Unity’s Resources system.