2. Define Data Models
Start: Empty Assets/Scripts/MagicWords/Models/.
Done: Create three C# files with public classes matching the JSON schema:

DialogueEntry.cs → string name; string text;

AvatarEntry.cs → string name; string url; string position;

MagicWordsData.cs → List<DialogueEntry> dialogue; List<AvatarEntry> avatars;

Include a simple unit test (e.g. using NUnit) that deserializes a small hard-coded JSON snippet into MagicWordsData to prove the model shapes match.

3. Build HTTP Service Interface
Start: Empty Assets/Scripts/MagicWords/Services/IMagicWordsService.cs.
Done:

Write an interface IMagicWordsService declaring Task<MagicWordsData> FetchMagicWordsAsync();.

Add XML doc comments.

✔️ Verify compilation.

4. Implement HTTP Service
Start: Empty MagicWordsService.cs in the same folder.
Done:

Implement IMagicWordsService using UnityWebRequest.Get().

Parse JSON via JsonUtility.FromJson<MagicWordsData>().

On HTTP error or timeout, throw a descriptive exception.

Add a unit test that mocks a successful JSON response and a failed request.

5. Create State Holder
Start: Empty Assets/Scripts/MagicWords/State/MagicWordsState.cs.
Done:

Implement a ScriptableObject or singleton MonoBehaviour with:

MagicWordsData Data

bool IsLoading, bool HasError

event Action OnDataLoaded, event Action OnError

Write a small script that toggles IsLoading and fires events; verify by subscribing and logging to the console.

6. Scaffold the Controller
Start: Empty Assets/Scripts/MagicWords/Controllers/MagicWordsController.cs; Scene not yet created.
Done:

In Start(), set State.IsLoading = true, call FetchMagicWordsAsync().

On success: assign State.Data, set IsLoading=false, invoke OnDataLoaded.

On catch: set State.HasError=true, load fallback JSON (from Resources—see Task 10), invoke OnError.

✔️ Test by simulating both paths.

7. Create Scene & Hook Controller
Start: No MagicWords scene.
Done:

Add new scene Scenes/MagicWords.unity.

In it, create an empty GameObject named MagicWordsController, attach your controller script.

Ensure the scene builds and plays without errors (controller will run but views are still missing).

8. Dialogue View Component
Start: Empty Assets/Scripts/MagicWords/Views/DialogueView.cs.
Done:

Subscribe to State.OnDataLoaded.

On event, iterate State.Data.dialogue, instantiate a placeholder GameObject under a public Transform contentParent.

For now, just log each name: text to verify wiring.

9. Avatar View Component
Start: Empty Assets/Scripts/MagicWords/Views/AvatarView.cs.
Done:

Subscribe to State.OnDataLoaded.

On event, iterate State.Data.avatars, instantiate a placeholder GameObject.

Log each name, url, and position to verify.

10. Fallback Resources
Start: Empty Assets/Resources/MagicWords/.
Done:

Add a minimal missing_data.json with an empty dialogue and avatars array.

Add a default_avatar.png (can be a 1×1 transparent).

Verify Resources.Load<TextAsset>("MagicWords/missing_data") and Resources.Load<Sprite>("MagicWords/default_avatar") both succeed.

11. Prefab: Dialogue Line
Start: Empty Assets/Scripts/MagicWords/UI/Prefabs/DialogueLine.prefab.
Done:

In a UI Canvas, create a HorizontalLayoutGroup with two TextMeshProUGUI children: one for speaker name, one for dialogue.

Save as prefab.

In DialogueView, swap placeholder instantiation for this prefab; assign texts.

Play and verify that you see lines appear in the Canvas when data loads.

12. Prefab: Avatar Image
Start: Empty Assets/Scripts/MagicWords/UI/Prefabs/AvatarImage.prefab.
Done:

Create a UI/Image plus a TextMeshProUGUI under a HorizontalLayoutGroup.

Save as prefab.

In AvatarView, instantiate it, download the texture with UnityWebRequestTexture.GetTexture, assign to Image or else use default_avatar.

Verify left/right anchoring according to position field.

13. Emoji Parsing in DialogueView
Start: DialogueView is showing raw text with {emojiName}.
Done:

Implement a helper to replace {intrigued} → Unicode emoji (e.g. 🤔) inline.

Hook it before assigning the TextMeshProUGUI.text.

Add unit tests for several mappings.

14. In-Game Menu Integration
Start: No menu.
Done:

In Assets/Scripts/App/MenuController.cs, build a simple UI menu with 3 buttons: “Ace of Shadows”, “Magic Words”, “Phoenix Flame”.

Wire the “Magic Words” button to SceneManager.LoadScene("MagicWords").

Verify clicking it loads your MagicWords scene.

15. Responsive UI Settings
Start: Scenes may not adapt to screen sizes.
Done:

On the root Canvas of both Menu and MagicWords scenes, set Canvas Scaler to “Scale With Screen Size.”

Use anchors on your prefabs so elements reposition correctly on mobile vs desktop aspect ratios.

Manually test at two resolutions (e.g. 1920×1080 & 800×600).

16. FPS Display
Start: No FPS counter.
Done:

Create a MonoBehaviour that in Update() computes fps = 1/Time.deltaTime and writes to a small TextMeshProUGUI in the top-left.

Add it to each scene’s Canvas.

Verify the number updates in play mode.

17. Build & Host WebGL
Start: No build.
Done:

In Build Settings, add both scenes, switch target to WebGL, build.

Host on a simple static server (e.g. GitHub Pages) and provide the URL.

Verify the Magic Words feature works in the hosted build.