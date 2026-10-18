# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Unity **6000.2.9f1** (Unity 6) 3D story/puzzle adventure game using URP. Primary target is Android (`com.ALittleBitMOre.EmitCycleStory`, Mono backend) with on-screen touch controls; Standalone is also configured. The README is in Russian and is written as a portfolio piece; its `/Src/Scripts/...` paths actually live under `Assets/Scripts/`.

## Building / running

There is no CLI build script, test suite, or linter. Work is done in the Unity Editor:
- Open the project folder in Unity Hub with editor 6000.2.9f1 (must match `ProjectSettings/ProjectVersion.txt`).
- The `*.csproj` / `.sln` files at the root are Unity-generated (regenerate via Edit > Preferences > External Tools); don't edit them by hand.
- Compile check from the command line (approximate, uses generated projects): `dotnet build Assembly-CSharp.csproj`. The Unity Editor's console is authoritative.
- `com.unity.test-framework` is installed but no project tests or `.asmdef` files exist — all game code compiles into `Assembly-CSharp`.

Scene build order (`ProjectSettings/EditorBuildSettings.asset`) matters because progress is stored as a build index: MainMenu → LvL_1 → LvL_2 → LvL_2.5 → SC3 → LvL_3 → LvL_4 → ToBeContinue → GameOver → SC1 → LvL_6 → SC4. Levels are in `Assets/Scenes/`, cutscene scenes in `Assets/Cutscene/`.

## Architecture

Game code is in `Assets/Scripts/`. Most other folders under `Assets/` (`Plugins`, `TextMesh Pro`, `Visual/...` incl. Joystick Pack and nuclear model scripts, `Scripts/Light/VolumetricLights-master`, `ExternalDependencyManager`) are third-party assets — avoid modifying them.

**Signal graph (core puzzle mechanic).** `ISignal` (`Assets/Scripts/Objects/Logic Connections/ISignal.cs`) is an *abstract MonoBehaviour* (not an interface, despite the name) with `bool Signal()`. It's pull-based: consumers poll `Signal()` every frame, and producers are wired together in the Inspector by serialized `ISignal` references. Producers/combinators include `LaserReceiver`, `Wire` (passthrough/invert), `LogicAnd` (class name in `LogicGate.cs`; AND/OR/NOR/NAND over a list of sources), `LogicTimer`, `LogicActivateForever`, `TouchButton`, `Lock`, and `DialogController` (true once its dialog finishes). Consumers (`LaserEmitter`, `Bulb`, `DeadlyObject`, `ParticleActivator`, `SceneLoader`, `DialogController`, etc.) typically have a `needSignal` flag + `signalSource` field. Adding a new puzzle element usually means subclassing `ISignal` or consuming one.

**Lasers.** `LaserEmitter` recursively raycasts each `Update`, reflecting off objects on `mirrorLayer` up to `maxBounces`, and on hit calls `LaserReceiver.Power(true)`, damages/knocks back `PlayerController`, or damages `DestroyableObject`. `LaserReceiver.Signal()` stays true for a 0.15s grace period after the last hit, so receivers must be re-powered each frame.

**Player.** `PlayerController` (largest file, a god object) drives movement via a `PlayerState` enum (`Walking`, `Dead`, `MovingObject`, `RotatingObject`, `Stunned`) switched in `Update`. Interactables are detected by trigger enter/exit, collected into a list of `IInteractable` (`Interact` / `StopInteraction(GameObject, Animator)`), and fired from the UI interaction button. Mobile input comes from UI scripts in `Assets/Scripts/UI/` (joystick, jump/slide/interaction buttons).

**Singletons / global state.** Static instances are used throughout: `PlayerController.instance`, `LevelManager.Instance`, `KarmaManager.Instance` (`DontDestroyOnLoad`), `FireManager.Instance` (in `FireDurationManager.cs`), `GameController.instance`, `LocalizationSystem.instance`. Persistent data uses `PlayerPrefs` with keys defined as statics on `PlayerController` (`NEXT_LEVEL_KEY`, `TOTAL_GAME_TIME`, `PLAYER_CARMA_KEY`). On death, `LevelManager.FinishLevel(false)` resets karma, destroys all `DontDestroyOnLoad` objects, and loads the death cutscene; on win it stores `buildIndex + 1` as the next level.

**Dialog & karma.** `Assets/DialogNodeBasedSystem/` is a node-graph dialog system (namespace `cherrydev`; `DialogBehaviour`, `DialogNodeGraph`, sentence/answer nodes, graphs in `Assets/Dialogs/`). `DialogController` starts a graph when karma is within `carmaBorders` (and optionally a signal is true) and adds the chosen answer's `answerCarmaValue` to `KarmaManager`. `GameController` (also in namespace `cherrydev`) listens for dialog-finished to activate objects. "Carma" is the codebase's spelling of karma — keep it consistent with existing members.

**Name collision to watch.** `Assets/Scripts/Levels/LevelManager.cs` is the real `LevelManager`; `Assets/Scripts/Objects/LevelManager.cs` defines `LevelManager2` (lock/password puzzle).

**Localization.** Unity Localization package plus `LocalizationSystem`, which queues `(TMP_Text, key, args)` jobs via static `AddLocalizationJob` and re-applies them on locale change.

## Conventions / gotchas

- Many older scripts (e.g. `Levels/LevelManager.cs`, `Objects/SceneLoader.cs`) contain Russian comments saved in Windows-1251, which appear garbled as UTF-8. Recently refactored files are UTF-8 with BOM. Don't mass-re-encode files unless asked; when editing a 1251 file, avoid corrupting the existing comments.
- Code style in refactored files: `[SerializeField] private` fields in camelCase, private runtime fields `_camelCase`, `const` for magic numbers, null-guards on serialized references, `TryGetComponent`.
- Renaming a serialized field or MonoBehaviour class breaks scene/prefab references (use `[FormerlySerializedAs]`), and moving/renaming a script must keep its `.meta` file to keep its GUID.
## Strict Limits
- NEVER read/search/index: `Library/`, `Temp/`, `Logs/`, `Obj/`, `Builds/`, `.git/`, or `.unity`/`.prefab`/`.asset` YAML files.
- Do not modify `.meta` files manually.
- Fix Windows-1251 garbled comments to UTF-8 only in files you are actively refactoring.
- Keep responses concise. No fluff.