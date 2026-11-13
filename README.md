# Composable Character Controller

**Composable Character Controller** is a maintained fork of [Tiny Character Controller (Project_TCC)](https://github.com/unity3d-jp/Project_TCC), the Unity-Chan! project by Unity Technologies Japan. It builds on TCC's packages and examples to provide composable character movement and gameplay features. This fork will continue to be maintained and developed independently of the upstream project.

While te public name of this fork is **Composable Character Controller**, its controller packages retains the upstream IDs, and its C# namespaces and assemblies still use `TinyCharacterController` for compatibility reasons. The bundled examples, Visual Scripting graphs, and tests refer to these identifiers, so retaining them keeps the existing project content working. `TCC` and `Project_TCC` names in the samples also identify their upstream origin.

We decided to rebrand this project's public facing name, because "Tiny" led some developers to underestimate the asset’s quality.

## Upstream release history

- 2024-02-21: v1.0.2 — bug fixes; merged [PR #1](https://github.com/unity3d-jp/Project_TCC/pull/1).
- 2024-02-19: v1.0.1 — bug fixes.
- 2024-02-09: v1.0.0 — first release.

## License notice

- ComposableCharacterController is licensed under the [Unity Companion License v1.3](https://unity.com/ja/legal/licenses/unity-companion-license), same as the upstream project.
- Unity-Chan! character assets are licensed under [Unity-Chan License 2.0](https://unity-chan.com/contents/guideline_en/).
- Some sound effects were made with [GameSynth](http://tsugi-studio.com/web/jp/products-gamesynth.html).
- The Source Han Sans font is licensed by Adobe under the [SIL Open Font License v1.1](https://github.com/adobe-fonts/source-han-sans?tab=License-1-ov-file#License-1-ov-file).

---

# ComposableCharacterController guide

## Unity version used to develop the upstream project

- Unity 2023.2.0f1

## Overview

Tiny Character Controller (TCC) builds character behavior from many small components. It provides movement, jumping, camera control, and other features. Assigning priorities to components lets you change which behavior controls a character in a given situation.

TCC accepts external input and supports Visual Scripting. The project also includes UI display, object pooling, a scenario importer, game data saving, scene switching, and a Scene Loader that supports layered scenes and staged loading.

## Basic TCC concepts

TCC uses four main kinds of component to control a character:

1. **[Brain](./Documentations/Componentlist_en.md#brain)** updates the character's final position. It combines the results of Checks, Effects, and Controls, writes them to the Transform, and manages movement vectors and sensor processing.
2. **[Check](./Documentations/Componentlist_en.md#check)** gathers information from the surroundings, such as ground contact, head contact, and visibility. It caches results during updates for other components to use.
3. **[Control](./Documentations/Componentlist_en.md#control)** controls movement in response to player input, including movement, jumping, and camera behavior. It sets movement direction and speed, jump height, and similar values.
4. **[Effect](./Documentations/Componentlist_en.md#effect)** adds influences such as gravity, moving-platform motion, and extra force (`AddForce`).

Combining these components makes character behavior customizable. They exchange sensor results and instructions for updating position and behavior:

![The four basic TCC component types](./Documentations/Images/TccBasic4Components.png)

Control components have priorities that determine their influence on a character's position (Transform) and rotation. You can attach several Controls to one character GameObject and switch between them according to the situation.

## Component groups included in this project

The [complete runtime component list](./Documentations/Componentlist_en.md) links to individual component pages. The main groups are:

### 1. [Brain](./Documentations/Componentlist_en.md#brain)

Brains combine information from Controls and Effects and update the character's position.

- **CharacterBrain** updates position with `CharacterController`; useful for action games.
- **RigidbodyBrain** updates position using `Rigidbody` and physics simulation.
- **NavmeshBrain** uses a `NavMeshAgent` to move within a NavMesh, for example in an RPG.
- **TransformBrain** updates the Transform directly, without physics or collider checks; useful for strategy games or characters with little physical interaction.

### 2. [Check](./Documentations/Componentlist_en.md#check)

Checks sense the character's surroundings.

- **Ground Check** detects ground contact and reports distance, slope, and ground object type.
- **Head Collision Check** tests for objects above the character at its current height and raises an event on contact.

### 3. [Control](./Documentations/Componentlist_en.md#control)

Controls govern character movement and facing direction.

- **Move Control** provides ground movement in a specified screen-relative direction.
- **Ladder Move Control** moves the character within the area defined by a `Ladder` component.

### 4. [Effect](./Documentations/Componentlist_en.md#effect)

Effects add motion or other influences.

- **Gravity** accelerates the character downward while airborne and resets downward acceleration on ground contact.
- **MoveWithPlatform** makes the character follow moving terrain by adding the acceleration of a platform with a `MovePlatform` component.

### 5. Utility

These components support common game-development tasks.

- **Swing Hit Detector** detects contact between a moving attack or swing region and other Colliders, including the path traveled by the contact point, and raises a contact event.
- **Sequential Collision Detector** enables multiple hitboxes in sequence, for example to align attack ranges with an animation.
- **GameObjectPool** pools GameObjects made from a specified Prefab to reduce repeated creation and destruction.
- **Indicator** displays an icon tracking a target and adjusts its position and state depending on whether the target is on-screen.
- **IndicatorPin** aligns a UI element with a position in 3D space.

Other features include **Scene Loader** for loading and unloading scenes, tracking progress, and reporting completion; **Game Object Folder** for organizing the Unity hierarchy with folder-like GameObjects and comments; and **Save System** for saving and loading `IDataContainer` components on a GameObject and its children as one JSON document.

These components provide a base for adding your own character behavior and game systems.

## Guides and tutorials

- [Install the character controller in your own Unity project](./Documentations/SystemInstallGuide.md)
- [Create your own Controls](./Documentations/Create_your_own_ctrl.md)
- [My First TCC tutorial](./Documentations/My_First_TCC.md)
- [SceneLoader tutorial](./Documentations/SceneLoader_Tutorial.md)

## Bundled SpringBone manuals

- [Components and setup](./Packages/UnityChan.SpringBone/Manual/SpringBoneManualComponents.md)
- [Loading and saving CSV](./Packages/UnityChan.SpringBone/Manual/SpringBoneManualLoadAndSave.md)
- [Editor windows](./Packages/UnityChan.SpringBone/Manual/SpringBoneManualWindow.md)
- [Existing English SpringBone readme (PDF)](./Packages/UnityChan.SpringBone/ReadMe/ReadMe_en.pdf)

---

# Step By Step (SBS) series

The SBS series consists of sample scenes in `Assets/TCC/Scenes`. The scenes generally build on one another from 01-01 onward, moving from walking and running through jumping and other character actions. You can also start with whichever sample resembles the game you want to build. The series is a starting route through TCC, not a demonstration of every feature.

### 01. Move — basic actions

- **UC-01-01 Walk**: Make a character walk.
- **UC-01-02 Walk and Run**: Make a character run.
- **UC-01-03 Move and Jump**: Add jumping.
- **UC-01-04 Move and DoubleJump**: Add a double jump.
- **UC-01-05 Move Jump Crouch**: Crouch and walk while crouching.
- **UC-01-06 TwinStickControl**: Add twin-stick movement and gaze control.

### 02. Camera — camera movement

- **UC-02-01A SimpleLookAt**: Use `LookAtConstraint` to keep the camera looking at the character.
- **UC-02-01B SimpleLookAt and Trace**: Add `PositionConstraint` so the camera follows the character.
- **UC-02-02 Cinemachine LookAt**: Add smooth look-at behavior with Cinemachine.
- **UC-02-03 Cinemachine TPS Control**: Build a third-person camera.
- **UC-02-04 Cinemachine 2D Control**: Build a side-view camera.

### 03. Action — game actions

- **UC-03-01 Dash**: Add a burst of movement.
- **UC-03-02 Rolling**: Add an evasive roll.
- **UC-03-03 Attack and Guard**: Add sword attacks and guarding.
- **UC-03-04 Damage and Dodge**: Take damage and dodge.
- **UC-03-05 Down and Wakeup**: Fall down and get back up.
- **UC-03-06 Sliding**: Add sliding.
- **UC-03-07 AerialKick**: Dive rapidly while jumping.
- **UC-03-08 Wall Jump**: Cling to a wall and jump from it.
- **UC-03-09 Elude and Climb**: Hang from a ledge and move sideways.
- **UC-03-10 Ambrella**: Hold the jump button while falling to slow the descent.

### 04. Shooter — third-person shooter features

- **UC-04-01 Rifle Control**: Use `IKBrain` to hold a gun.
- **UC-04-02 Fire**: Fire projectiles with `PooledGameObject`.
- **UC-04-03 Hit**: Make projectiles hit walls and targets.
- **UC-04-04 WeaponChange and Reload**: Switch among weapons and reload.

### 05. Mechanics — gameplay systems

- **UC-05-01A Platform**: Add a moving platform with `CharacterBrain`.
- **UC-05-01B Platform**: Add a moving platform with `RigidbodyBrain`.
- **UC-05-02 CheckpointSystem**: Add checkpoints.
- **UC-05-03 ScoringSystem**: Add scoring.
- **UC-05-04 FlagSystem**: Add a simple flag system.
- **UC-05-05 MobSystem**: Add roaming NPCs.
- **UC-05-06 MobSystem with Vision**: Give roaming NPCs vision and behavior branches.
- **UC-05-07 MobSystem3(ManualControl)**: Build general NPC behavior for a 2D platform game.
- **UC-05-08 SequentialCollisionDetector**: Build hit detection with `SequentialCollisionDetector`.

### 06. UI — interface features

- **UC-06-01 SimpleText**: Display text.
- **UC-06-02A MessageBox**: Add NPC dialogue.
- **UC-06-02B MessageBoxEX**: Add longer NPC dialogue.
- **UC-06-03 LifeBar**: Build a health bar.
- **UC-06-04 StaminaBar**: Add stamina.
- **UC-06-05 Minimap**: Build a minimap.
- **UC-06-06 Indicator**: Track an off-screen target with an indicator.
- **UC-06-07 PopupMessage Indicator**: Display popup messages using an indicator.
- **UC-06-08 ShowInputKey**: Visualize keyboard and mouse input.
- **UC-06-09 IndicatorPin**: Display damage dealt during an attack.
- **UC-06-10 LifeBar(ObjectPooling)**: Build a segmented HP gauge.

### 07. Scene — scene switching and multi-scene management

- **UC-07-01 SceneLoader Introduction**: Introduce the advantages of multiple scenes.
- **UC-07-02 SceneLoader Function**: Switch interactively among several scenes.

### 08. Cycle — game loop

- **UC-08-01A Save and Load Basic**: Learn the basics of TCC's `SaveDataControl`.
- **UC-08-01B Save and Load Function**: Try the full DataSave feature.
- **UC-08-02 Save and Load Advanced**: Build an RPG field using multiple scenes and DataSave.

### 09. Additional playable samples

- **UC-09-01 MaterialPropertyBlock**: Make individual roofs transparent while sharing a material.
- **UC-09-02A DungeonCrawl(Cycle)**: Explore a dungeon split into scenes by area.
- **UC-09-02B DungeonCrawl(Advanced)**: Play a top-down dungeon exploration game.
- **UC-09-03 MetroidVania**: Play a 2D platform action game.
- **UC-09-04 2D 3D**: Control a 2D sprite character in a 3D world.
- **UC-09-05 Drone**: Play a drone-racing sample.
- **UC-09-07 CubePuzzle**: Move cubes to make a path in a puzzle game.

---

2024-02-09 Unity Technologies Japan

**Unity-Chan! Team:** Tatsuhiko Yamamura, Kohei Kyono, Nobuyuki Kobayashi, Hiroki Omae.
