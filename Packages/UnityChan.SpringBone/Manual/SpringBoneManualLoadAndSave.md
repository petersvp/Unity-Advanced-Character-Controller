# Unity-Chan SpringBone manual: loading and saving

Dynamics setup data can be loaded from and saved to CSV. You can load it into a different model if bone names match. You can also edit the CSV in a spreadsheet application, change many values at once, and reload it into the character. Use the SpringBone window to load and save.

Loading and saving identify bones by name. Do not give multiple bones in the same character identical names.

## Loading

Select the character in the scene and click **Load** in the SpringBone window to open the load window.

- **SpringBone Root:** The root object of the character. A `SpringManager` is added to it.
- **Get Root from Selection:** Use the currently selected object as the root.
- **SpringBones:** Load SpringBone data.
- **Colliders:** Load collision data. Both options are normally enabled, but either can be loaded alone.
- **Load CSV and Set Up:** Choose a CSV file and load it into the character specified by SpringBone Root.

If the CSV does not match the character's bones, an error window offers to set up only the matching entries or cancel. A partial match can be useful, for example when models share hair but have different costumes. Check the result carefully. Before loading mismatched data, back up the prefab or scene, or save current dynamics to a separate CSV.

## Saving

Select the character in the scene and click **Save** in the SpringBone window.

- **SpringBone Root:** The character root, normally the object with `SpringManager`.
- **Get Root from Selection:** Use the current selection as the root.
- **SpringBones:** Save SpringBone data.
- **Colliders:** Save collision data. Both are normally enabled, but either can be saved alone.
- **Save CSV:** Choose a path and write the character's dynamics data to CSV.

## Dynamics CSV format

The currently supported format version is **4**.

### Version line

The first line always contains version information: `version` in the first column and the version number in the second.

### Sections

From the second line onward, data is divided into sections. Like an INI file, each section starts with a title in square brackets and continues to the next bracketed title or end of file. Empty lines and lines beginning with `//` are ignored.

### `[SpringBones]`

Each row records one SpringBone. Columns, in order:

1. **Bone name:** Name of an existing character bone to which `SpringBone` is added.
2. **Radius:** SpringBone collision sphere radius.
3. **Stiffness:** SpringBone stiffness.
4. **Drag:** SpringBone air resistance.
5. **Gravity X:** X component of gravity.
6. **Gravity Y:** Y component of gravity.
7. **Gravity Z:** Z component of gravity.
8. **Wind influence:** Effect of wind on this SpringBone.
9. **Pivot object name:** Name of an existing character bone or an object listed in `[Pivots]`.
10. **Enable Y-angle limits:** `TRUE` enables the limit; other values disable it.
11. **Minimum Y angle:** Lower bound in degrees, zero or negative.
12. **Maximum Y angle:** Upper bound in degrees, zero or positive.
13. **Enable Z-angle limits:** `TRUE` enables the limit; other values disable it. The original Japanese manual repeats “Y” in this label, but its description says Z.
14. **Minimum Z angle:** Lower bound in degrees, zero or negative.
15. **Maximum Z angle:** Upper bound in degrees, zero or positive.
16. **Rotational stiffness:** SpringBone rotational stiffness.
17. **Distance-limit target count:** Number of distance-limit targets. This is experimental and currently not recommended.
18. **Distance-limit target names:** As many existing bone names as given by the target count.
19. **Collider names:** Names of all colliders registered to this SpringBone, continuing to the end of the row.

### `[Pivots]`

Each row describes a new pivot object with a `SpringBonePivot` component:

1. **Pivot name:** Name for the new GameObject. It must not duplicate an existing bone or another pivot.
2. **Parent name:** Name of an existing bone that will parent the pivot.
3. **Local rotation X.**
4. **Local rotation Y.**
5. **Local rotation Z.**

### `[Colliders]`

Each row describes one collider:

1. **Collider name:** Name for the new collider; it must be unique.
2. **Parent name:** Name of an existing bone.
3. **Local position X.**
4. **Local position Y.**
5. **Local position Z.**
6. **Local rotation X.**
7. **Local rotation Y.**
8. **Local rotation Z.**
9. **Local scale X.**
10. **Local scale Y.**
11. **Local scale Z.**
12. **Collider type:** Two-character code: `sp` for sphere, `cp` for capsule, or `pa` for panel.

The remaining columns depend on type:

- **Sphere (`sp`):** Radius.
- **Capsule (`cp`):** Radius, then height along the Y axis.
- **Panel (`pa`):** Width, then height.
