📂 Project Folder Structure

AceOfShadows/
├── Assets/
│   ├── Scenes/
│   │   └── AceOfShadows.unity
│   ├── Sprites/
│   │   └── Cards/           # 144 card sprites (card_000.png … card_143.png)
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── GameManager.cs
│   │   │   ├── ServiceLocator.cs
│   │   │   └── IUpdatableService.cs
│   │   ├── Models/
│   │   │   ├── CardModel.cs
│   │   │   └── StackModel.cs
│   │   ├── Services/
│   │   │   ├── ICardService.cs
│   │   │   ├── CardService.cs
│   │   │   ├── IAnimationService.cs
│   │   │   └── AnimationService.cs
│   │   ├── Controllers/
│   │   │   ├── DeckController.cs
│   │   │   └── StackController.cs
│   │   └── UI/
│   │       ├── StackCounterUI.cs
│   │       └── EndGameMessageUI.cs
│   ├── Prefabs/
│   │   ├── Card.prefab
│   │   └── Stack.prefab
│   └── Materials/
│       └── CardMaterial.mat
├── ProjectSettings/
└── README.md
📝 What Each Part Does
1. Assets/Scenes/AceOfShadows.unity
The main scene containing:

An empty root GameObject (GameManager)

4× empty Stack GameObjects (instances of Stack.prefab) positioned in the scene

UI Canvas for counters and end‐game message

2. Assets/Sprites/Cards/
Holds the 144 card textures.

Named consistently (e.g. card_000.png … card_143.png) for dynamic loading.

3. Assets/Scripts/Core/
ServiceLocator.cs

A simple registry to register and resolve services (ICardService, IAnimationService, etc.).

GameManager.cs

Bootstraps the scene:

Registers services

Instantiates DeckController

Starts the shuffle/move loop

Tracks global state (e.g. total ongoing animations, finished flag).

IUpdatableService.cs

Interface for services that need periodic updates (e.g. animation ticking).

4. Assets/Scripts/Models/
CardModel.cs

Data class representing a single card:

int Id

Sprite Sprite

StackModel CurrentStack

StackModel.cs

Holds a list of CardModel in LIFO order for that stack.

Exposes methods to Push(CardModel), Pop(), and query Count.

5. Assets/Scripts/Services/
All services are registered in ServiceLocator by interface.

ICardService.cs / CardService.cs

Loads the 144 sprites at startup

Creates CardModel instances

Distributes them evenly into StackModels

IAnimationService.cs / AnimationService.cs

Smoothly animates card GameObjects from one stack to another over time

Maintains a list of active tweens; notifies when each completes

6. Assets/Scripts/Controllers/
DeckController.cs

High‐level orchestrator: every 1 second, picks the top card from a random non‐empty stack and orders the AnimationService to move it to another random stack.

Increments/decrements a global “animations in progress” counter in GameManager.

When no more moves remain (all 144 have moved once), signals end-of-game.

StackController.cs

Attached to each Stack.prefab instance.

Mirrors the underlying StackModel by instantiating/destroying Card.prefab children in a stacked layout (z-offset or y-offset).

Notifies StackCounterUI when its Count changes.

7. Assets/Scripts/UI/
StackCounterUI.cs

Displays the .Count for its associated StackModel above the stack.

Subscribes to stack events to update the label in real time.

EndGameMessageUI.cs

Listens to GameManager for the “all animations done” event and shows a “Done!” banner.

8. Assets/Prefabs/
Card.prefab

A simple SpriteRenderer + CardModel reference + AnimationTarget component.

Stack.prefab

Empty GameObject with StackController, placeholder child for counters.

🔄 Where State Lives
Global Game State

GameManager:

Total animations in progress (int activeAnimations)

End‐of‐game flag / event

Domain Models

CardModel

StackModel (contains its list of cards)

Service State

CardService: loaded sprites & list of all CardModels

AnimationService: list of running tweens, timing data

🔗 How Services Connect
Startup (GameManager)

Registers CardService and AnimationService in ServiceLocator.

Calls CardService.Initialize(numberOfStacks = 4).

Creates 4 StackController instances; each reads its StackModel from CardService.

Move Loop (DeckController)

On a 1 s timer:

Queries all non‐empty StackModels from CardService.

Picks sourceStack → targetStack.

Pops CardModel from source; pushes it to target.

Tells AnimationService.Move(cardGameObject, targetTransform, onComplete).

Increments GameManager.activeAnimations.

Animation Service

Runs tweens over time (via Update() if it implements IUpdatableService).

On each tween completion, invokes callback:

Decrements GameManager.activeAnimations.

DeckController checks if all cards have moved once → fires end‐game event.

UI Updates

StackController listens to its StackModel changes → repositions children + updates StackCounterUI.

At end, EndGameMessageUI shows “All animations finished!”

🛠️ Notes & Extensions
Scalability: Swap out AnimationService for DOTween or Unity’s built-in tweening.

Persistence: If you wanted to persist state, stash StackModel lists in ScriptableObjects or JSON.

Testing: You can inject mocks for IAnimationService to skip real time delays.

Responsiveness: For mobile vs. desktop, adjust camera orthographic size or Canvas Scaler settings in the UI Canvas.

