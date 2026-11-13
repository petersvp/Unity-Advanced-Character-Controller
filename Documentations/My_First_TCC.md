# My First TCC Tutorial

This tutorial covers the basics of TCC: setting up a game scene, controlling a player, adding gravity and jumping, and changing behavior by switching the priority of `Control` components.

## Reference video

[![My First TCC video](https://img.youtube.com/vi/qSAUngXAQkI/0.jpg)](https://www.youtube.com/watch?v=qSAUngXAQkI)

## 1. Create a new scene

- Choose `File > New Scene` from Unity's main menu. Name the new scene `NewScene`.
- Right-click in the Hierarchy and choose `Folder`. Rename the new empty object `Level`; it will contain the objects that make up the world.
- Add a `Plane` under `Level` to serve as the ground. Set its Transform position to `(0, 0, 0)`.
- Move `DirectionalLight` under `Level`.

> Objects placed under a `Folder` are moved back to the root when the game is built or played.

## 2. Create the player

- Right-click in the Hierarchy, choose `Create Empty`, name the new GameObject `Player`, and set its Transform position to `(0, 0, 0)`.
- Add a `Capsule` as a child of `Player` to serve as the character model. Set its Transform position to `(0, 1, 0)`.
- Add a `Sphere` as another child, for a head or visual accent. Set its position to `(0, 1.4, 0.5)`.
- Add `PlayerInput` to `Player`. Click `Create Actions` to create an `InputActionAsset`, then configure the player's controls.
- Add `CharacterBrain`. This makes the `CharacterController` the target that TCC uses to apply movement vectors.
- Add `CharacterSettings` to manage the character's size. Set `Radius` to `0.6` and `Height` to `2` to match the model.

## 3. Add character movement

Add the simplest form of character movement:

- Add `MoveControl` to `Player`. This component controls the player's movement.
- Add a `ScriptMachine` to `Player` and click `Edit Graph` to open Visual Scripting.
- In the graph, capture input with `OnInputSystemEventVector2` and pass it to `MoveControl.Move`:
  - Add `OnInputSystemEventVector2` with timing `OnHold` and Input Action `Move`.
  - Connect its flow and input value to `MoveControl.Move`.
  - Add another `OnInputSystemEventVector2` with timing `OnRelease` and Input Action `Move`.
  - Connect it to another `MoveControl.Move` node so that movement updates when the input is released.

> **Tip:** Character movement changes relative to the direction of the screen.

## 4. Add gravity

Add effects such as gravity:

- Add `Gravity` to `Player`.
- Add `GroundCheck` or `GroundHeightCheck` to `Player` so that `Gravity` can detect the ground:
  - `GroundCheck` uses physics to detect ground contact.
  - `GroundHeightCheck` compares the character's position with an assumed ground height.

> **Tip:** If you use `GroundHeightCheck` instead of `GroundCheck`, the character will not fall when moving beyond the edge of the terrain.

## 5. Add jumping

Add another `Control` component alongside `MoveControl`:

- Add `JumpControl` to `Player`.
- Add `HeadContactCheck` or `ReachHeightCheck` to detect obstacles above the player.
- In the Visual Scripting graph, capture the input with `OnInputSystemEventButton` and connect it to `JumpControl.Jump`:
  - Add `OnInputSystemEventButton` with timing `OnPressed` and Input Action `Fire`.
  - Connect it to a `JumpControl.Jump` node.

> **Tip:** TCC is not an all-in-one behavior. Combine its components to build the behavior you need.

## 6. Make the player face a target at all times

Add a `Target` object to the scene and make the player always face it:

- Add a `Sphere` at `(0, 0.5, 3)`. Enable `Is Trigger` on its `SphereCollider` and set the collider radius to `5`.
- Add `LookTargetControl` to `Player` and assign the sphere as its `Target`.
- Set the priority of `LookTargetControl` to `10`.

> **Tip:** Priority lets one component override another component's movement or facing direction.

## 7. Face the target only when nearby

Change the behavior so that the player faces the target only while inside its trigger:

- In the Visual Scripting graph, change the priority of `LookTargetControl` on `OnTriggerEnter` and `OnTriggerExit`:
  - Connect `OnTriggerEnter` to `LookTargetControl.SetPriority` with a value of `10`.
  - Connect `OnTriggerExit` to `LookTargetControl.SetPriority` with a value of `0`.

> **Tip:** Switching priority changes which component controls the character in each situation.
