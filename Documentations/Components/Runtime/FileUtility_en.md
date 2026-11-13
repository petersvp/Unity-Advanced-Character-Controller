# FileUtility

#### **Namespace**: Unity.SaveData.Core
---

## Overview
`FileUtility` saves and loads data in a platform-appropriate way. It handles JSON data and persists it in the local file system or `PlayerPrefs`.

## Features and operations
- **Path generation**: Builds a file path suitable for the current platform from the specified path and file name.
- **Data removal**: Deletes the data file with the specified path and name.
- **Data saving**: Saves a value as JSON at the specified path and file name.
- **Data loading**: Reads data from the specified path and file name and deserializes it to the requested type. Returns `true` when the data exists.

## Methods
| Name | Function |
|------|----------|
| `Save<TValue>` | Saves the specified value as JSON. |
| `Load<TValue>` | Loads JSON data and deserializes it to the specified type. |
| `Remove` | Deletes the data file with the specified path and name. |
| `IsExists` | Checks whether data exists at the specified path and name. |

## Usage examples
- Use `Save` to store game settings or player progress.
- Use `Load` to restore settings or progress at startup or at a particular point in the game.
- Use `Remove` to delete data that is no longer needed.
- Use `IsExists` to check whether data is present.

---
## Additional notes
- `UNITY_WEBGL` and `UNITY_EDITOR` directives select the storage method for each platform. WebGL uses `PlayerPrefs`; other platforms use the local file system.
- The `DataStore<T>` structure works around `JsonUtility`'s inability to serialize lists directly. It allows data of arbitrary types to be serialized to and deserialized from JSON.

`FileUtility` provides a convenient way to save and load persistent information such as game settings and player progress.
