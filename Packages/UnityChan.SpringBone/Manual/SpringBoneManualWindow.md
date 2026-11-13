# Unity-Chan SpringBone manual: editor windows

## SpringBone window

The SpringBone window provides tools for setup. Open it with **Show Window** in the `SpringManager` Inspector or **UTJ > SpringBone Window**.

### Display

- **Show Selected Bones Only:** Show gizmos only for selected SpringBones. This makes scenes with many bones easier to read and faster to display.
- **Show Selected Colliders Only:** Show gizmos only for selected colliders.
- **Show Bone Collisions:** Show or hide the collision spheres of SpringBones.
- **Show Bone Names:** Display bone names in the Scene view. Hide them when unnecessary because displaying many can be slow.

### Dynamics CSV

- **Load:** Read a CSV file and set up the character's dynamics.
- **Save:** Write the character's dynamics settings to CSV.

### SpringBones

- **Add SpringBones:** Add `SpringBone` components to the selection and its children. Leaf objects are skipped because a SpringBone needs a child.
- **Create Pivots:** Create a pivot for each selected SpringBone. A `SpringBonePivot` component is added at the same hierarchy level as its SpringBone, as a child of the SpringBone's parent.
- **Create/Update Manager:** Add a `SpringManager` to the selected object if needed, then find and register the SpringBones beneath it again.
- **Mirror SpringBones:** Open the mirror window described below to copy settings from one side to the other.
- **Select SpringBones in Selection and Children:** Select all SpringBones under the selected objects.
- **Delete Managers and Bones in Selection and Children:** Remove all SpringBones and SpringManagers under the selected objects.

### Collision

- **Sphere:** Create a `SpringSphereCollider` as a child of the selected object.
- **Capsule:** Create a `SpringCapsuleCollider` as a child. If the target bone has a child, the new capsule is positioned to fit between the bone and its child.
- **Panel:** Create a `SpringPanelCollider` as a child.
- **Align Capsule with Parent:** Position a selected capsule between its parent and another child of that parent, using the same alignment as when creating a capsule.
- **Remove Collisions from SpringBones:** Unregister all colliders from selected SpringBones.
- **Delete Colliders in Selection and Children:** Delete all colliders under the selected objects.
- **Cleanup:** Delete colliders not registered to any SpringBone and remove `None` entries from every SpringBone collision list.

## SpringBone mirror window

This window mirrors SpringBone settings from left to right or right to left across the X axis. Place the character at the origin in a symmetrical T or A pose, with its front/back direction along the Z axis.

- **Get from Selection:** Use selected SpringBones as sources and automatically find corresponding bones on the opposite side of the X axis.
- **Use Bones at X < 0 as Sources:** Find all SpringBones in the scene at negative X positions and match opposite-side destinations.
- **Use Bones at X > 0 as Sources:** Do the same with bones at positive X positions.
- **Select All Sources:** Select all source SpringBones.
- **Select All Destinations:** Select all destination SpringBones.
- **Select All:** Select both source and destination SpringBones.
- **Source and Destination List:** Sources are shown on the left and destinations on the right. The Select button detects destinations automatically; use the picker if a destination is missing or incorrect.
- **Mirror:** Copy source settings to destinations while reflecting them. Force parameters are copied unchanged. The Y-angle limit bounds swap, while Z-angle limits are copied without inversion, because Z normally points outward from the body. A collider is replaced by its counterpart on the opposite side when one is found; otherwise the source collider is copied.

## Select by Name window

This is a general selection tool that is useful during SpringBone setup. Open it through **UTJ > Select by Name Window**. It selects scene objects by part of their name or by attached component type.

- **Name:** Search for objects containing the given text, ignoring case. `*` is a wildcard. Space-separated terms after the first narrow the results: objects must match the first term and at least one later term. For example, if `bone_a`, `bone_b`, and `bone_c` exist, `bone _a _b` selects only `bone_a` and `bone_b`.
- **Ignore:** Exclude names containing any of the space-separated terms.
- **Component:** Of the names matching the fields above, select only objects with a component whose class name matches this text. `SpringBon` matches both `SpringBone` and `SpringBonePivot`; `SpringBone` matches only the exact `SpringBone` class.

## Parenting window

This general tool is useful for parenting pivots and colliders when a large skeleton makes hierarchy edits difficult. Set the desired **New Parent**, select objects to reparent, and click **Parent**. For example, you can reparent skirt pivots to leg bones or change the parents of multiple colliders at once.
