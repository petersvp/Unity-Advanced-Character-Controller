# SceneLoader Tutorial

## Reference video

[![SceneLoader tutorial video](https://img.youtube.com/vi/Dp1vMD3wCTw/0.jpg)](https://www.youtube.com/watch?v=Dp1vMD3wCTw)

## 1. Create a new scene

- From Unity's main menu, choose `Assets > Create > Scene`. Name the new scene `SubScene`.
- Open `SubScene` and delete `DirectionalLight` and `MainCamera`.
- Create a cube named `Cube_1` and set its Transform position to `(3, 1, 3)`.
- Enable `Is Trigger` on the cube's `BoxCollider`.

## 2. Open the scene through SceneLoader

- Open the original scene, `NewScene.unity`.
- From the Hierarchy menu, create a `SceneLoader` object.
- Drag the `SubScene.unity` you created into `Scene Asset` on the `SceneLoader` component. This automatically registers the scene with Addressables.
- Open the registered scene by clicking `Edit Settings > Open` on `SceneLoader`, or by enabling its checkbox in the Hierarchy.
- To open the scene automatically when you open it in the Editor, change `Edit Settings > Scene Open Behaviour` on `SceneLoader` to `LoadAndEdit`.

> **Tip:** In the Editor, expanding a scene works regardless of whether the GameObject is active.

## 3. Load and unload the scene at runtime

Add behavior that loads the scene automatically when the player enters a trigger and unloads it when the player leaves.

- Set the position of the object holding `SceneLoader` to `(3, 1, 3)`.
- Add a `BoxCollider` to that object. Enable `Is Trigger` and set its size to `(5, 2, 5)`.
- Disable the `SceneLoader` component by clearing its checkbox so that the scene does not load automatically.
- Add a `ScriptMachine`. In its graph, enable `SceneLoader` when the player enters the trigger and disable it when the player exits:
  - Connect `OnTriggerEnter` to `SceneLoader.OnEnable`, with the value set to `True`.
  - Connect `OnTriggerExit` to `SceneLoader.OnEnable`, with the value set to `False`.

> **Tip:** With SceneLoader installed, starting Play mode unloads scenes other than the parent scene. You can change this behavior in the Scene View's `SceneLoaderOverlay`:
>
> - `RootOnly`: Keep the parent scene and unload the others. This matches normal game behavior.
> - `UseStart`: Start in the specified scene.
> - `KeepAll`: Keep the current scenes. This is useful when checking components or `ScriptMachine` behavior.

## 4. Pass information to the scene

Use information from the scene's loader to change how the loaded scene behaves.

- Add `FlagCollectionContainer` and `SaveDataControl` to the object holding `SceneLoader`.
- Add an entry to `FlagCollectionContainer.Values`. Set its checkbox to `False` and its flag name to `Cube_1`.
- Select `Cube_1` in `SubScene`. Make a graph that changes the cube according to a flag on its owner (the `SceneLoader` that loaded the scene):
  - Add a `ScriptMachine` to `Cube_1` and click `Edit Graph` to open Visual Scripting.
  - Get the owner's `SceneLoader` by adding `SceneLoaderManager.GetOwner` and passing `This` as its argument.
  - Connect the result of `SceneLoaderManager.GetOwner` to `FlagCollectionContainer.GetFlag("Cube_1")` to read the owner's flag. Connect this flow from `OnStart`.
  - Add an `If` node using the result of `GetFlag`. The original guide describes connecting the `False` branch to `MeshRenderer.SetMaterial` and assigning a material there. Its wording about which flag value changes the material is inconsistent, so follow the graph branch you actually wire.
- Reflect changes in the loaded scene back onto its owner. For example, update the owner's flag when the character interacts with `Cube_1`:
  - Add an `OnTriggerEnter` node.
  - Connect the result of `SceneLoaderManager.GetOwner` to `FlagCollectionContainer.SetFlag("Cube_1", True)`.
