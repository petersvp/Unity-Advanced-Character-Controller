# Project_TCC System Installation Guide

This guide explains how to install the systems included with Project_TCC into your own Unity project.

## Installation steps

1. **Download Project_TCC.** Download the [Project_TCC ZIP archive](https://github.com/unity3d-jp/Project_TCC/archive/refs/heads/main.zip).
2. **Extract the ZIP archive.** Unpack the downloaded file.
3. **Place the packages in your project.** From the extracted folder, put the following packages under your project's `Packages` folder, or reference them as packages:
   - `com.utj.charactercontroller` — character control
   - `com.utj.gameobjectfolder` — treats GameObjects as folders
   - `com.utj.savedata` — saves game progress
   - `com.utj.scenarioimporter` — extracts game-ready scripts from text
   - `com.utj.sceneloader` — loads and unloads scenes like GameObjects
4. **Restart the Unity Editor** after the import finishes.

> **Note:** Immediately after importing, you may see errors related to `UIElements`, such as `TypeLoadException: Could not load type 'Utj.SceneManagement.SceneLoadType' from assembly 'GOSubSceneEditor'.` The original guide says these errors disappear after restarting the Editor and can be ignored until then.

## Using Visual Scripting

If you intend to use Visual Scripting, set it up as follows:

1. **Initialize Visual Scripting.** Open `Edit > Project Settings > Visual Scripting`, select the `Visual Scripting` tab, and click `Initialize Visual Scripting`.

   ![Visual Scripting settings](./Images/SysInst_Image01.png)

2. **Make the package components available.** In `Generate Nodes`, open `Node Library`.

   ![Node Library settings](./Images/SysInst_Image02.png)

3. Click the `+` button at the far right to add slots, then register these assemblies:
   - `Utj.SceneLoader`
   - `Utj.GameDataSave`
   - `Utj.ScenarioImporter`
   - `Utj.TinyCharacterController`

   ![Assemblies added to the Node Library](./Images/SysInst_Image03.png)

## Using SceneLoader

Addressables must be initialized before you use SceneLoader. If they are not initialized, you cannot register scenes with SceneLoader.

1. **Enable Addressables.** Open `Window > Asset Management > Addressable > Group`, then click `Create Addressable Settings`.

   ![Create Addressable Settings](./Images/SysInst_Image04.png)
