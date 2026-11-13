# Unity-Chan SpringBone manual: components and setup

## Overview

Unity-Chan SpringBone is a bone-based dynamics system for secondary character motion. Use it when cloth simulation is too expensive or when you need per-bone control.

The system consists mainly of these components:

- **SpringManager:** Manages the dynamic bones for one character. Add one to each character, usually on its root object.
- **SpringBone:** Holds parameters for one bone or joint. A SpringBone does not move by itself; it must be registered with a SpringManager. It treats a child position as its tip, calculates the tip's dynamics, and converts the result to bone rotation. Consequently, only bones with children can receive a SpringBone. If there are multiple children, their initial average position becomes the tip; children with `SpringBonePivot` are excluded. The vector from parent to child is normally considered the bone's negative X axis.
- **SpringSphereCollider, SpringCapsuleCollider, SpringPanelCollider:** Sphere, capsule, and panel collision components. Attach them to bones and register each collider with the SpringBones it should affect. Standard Unity/PhysX Colliders cannot currently be used for SpringBones.
- **SpringBonePivot:** Marks a newly created pivot object. Use **Create Pivot** in the SpringBone window to make one. Any object can serve as a pivot; the component itself does no special processing, but a newly created pivot is recorded in dynamics CSV and recreated when that CSV is loaded.

Angle limits and collisions constrain SpringBone motion. Angle limits need a pivot object as their frame of reference. By default, the SpringBone's parent is its pivot. You can choose another object, such as a leg bone when you want a skirt to follow the leg.

## Setup procedure

### Add SpringManager and SpringBone components

Place the character in the scene and add a `SpringManager` to its root, either through **Add Component > Spring Manager** in the Inspector or **Create/Update Manager** in the SpringBone window.

Select all bones that should use dynamics and click **Add SpringBones** in the SpringBone window. This adds a `SpringBone` to each eligible bone and registers the bones in the root `SpringManager` in hierarchy order, from parents to children.

SpringBones must be processed in parent-to-child order, so they do not work individually. The SpringManager processes entries in its Spring Bones list; unregistered SpringBones do not move. Enter Play mode and move the character to see the dynamics. With default settings, gravity will probably make everything hang straight down.

### Set angle limits

Add angle limits to SpringBones that must not enter the body. Select them and click **Create Pivot**. Align the pivot's X axis with the bone direction, point Z outward from the body, and point Y sideways. Then a minimum Z-angle limit of zero prevents movement into the body.

The pivot can be a child of any bone. For example, parent a front-hair pivot to the head bone and a skirt-root pivot to a leg bone.

### Add collisions

Use colliders where angle limits alone do not prevent intersection. For example, add a capsule under a leg bone and register it to skirt SpringBones, or add a panel under a spine bone and register it to long rear-hair SpringBones. Angle limits generally work best near the root of a bone chain; colliders are useful farther out. Combining both can give the best result.

### Adjust while playing motion

Play character animations and tune the parameters while observing the secondary motion. Changes to SpringBone parameters made during Play mode are retained afterward.

### Save data

Use **Save** under **Dynamics CSV** in the SpringBone window to write a text CSV that can be reloaded. It can also be loaded onto another character if bone names match. Saving the configured character as a prefab is also recommended.

## Setup advice

- Parent each pivot under the object that its SpringBone should follow: skirt pivots under leg bones, chest ornaments under an upper-spine bone, and so on.
- Use pivots and angle limits mainly for bones near the root of a chain; use colliders mainly for bones farther away. Combine both where useful.
- Collisions yield to angle limits. If a bone does not avoid a collider properly, check whether its angle range is too narrow.
- Test as many animations as possible; a SpringBone can behave unexpectedly in a motion you have not tested.
- Make colliders a little larger than the visible body. A collider that fits a leg exactly may send a skirt bone in an unexpected direction.
- Skirts are difficult. For front skirt bones, parent pivots to the corresponding left or right leg. A center skirt bone has no obvious leg to follow and may sink into the body while running or crouching. Colliders help, but balancing the behavior across crouching and standing takes experimentation. The `HighLeg` component described below may help.

## SpringManager

`SpringManager` controls secondary motion for one character.

### Buttons

- **Show Window:** Open the SpringBone window.
- **Select All SpringBones:** Select every SpringBone.
- **Update SpringBone List:** Find all SpringBones beneath this manager and register them in the Spring Bones list.

### Properties

- **Automatic Updates:** Normally on. Programmers can turn it off and call `SpringManager.UpdateDynamics()` manually to control simulation timing.
- **Is Paused:** Pause dynamics, for example while the game is paused.
- **Simulation Frame Rate:** Set a fixed simulation interval for stability. `-1` uses `Time.deltaTime`, which can produce somewhat unstable motion if the frame rate varies. A setting of 60 is recommended.
- **Dynamic Ratio (experimental):** Blend between animation and dynamics. Animation normally does not animate secondary bones, so call `SpringManager.UpdateBoneIsAnimatedStates()` when playing an animation to identify which SpringBones the animation moves.
- **Gravity:** Global gravity added to each SpringBone's gravity setting.
- **Bounce (experimental):** Amount of bounce on collision. This is currently experimental and is intended eventually to be a per-bone parameter.
- **Friction (experimental):** Amount of friction on collision, also currently experimental and intended eventually to be per-bone.

### Constraints

These settings are mainly for debugging and disable features for the whole character:

- **Enable Angle Limits:** Toggle angle limits.
- **Enable Collision:** Toggle collisions.
- **Enable Length Limits:** Toggle distance limits.

### Ground Collision

The ground collision surface is a horizontal plane at the specified height. The Scene gizmo shows it as a rectangular outline at the feet. It cannot currently be tilted or vertical.

- **Collide With Ground:** Enable or disable ground collision.
- **Ground Height:** Ground Y coordinate.

### Bones

- **Spring Bones:** List of registered SpringBones. Normally you need not edit it manually if you use the SpringBone window or **Update SpringBone List**.

### Gizmos

Editor-only settings for gizmo and handle colors and angle-limit display size:

- **Bone Color:** SpringBone color.
- **Collision Color:** Sphere, capsule, and panel collision color.
- **Ground Collision Color:** Ground collision color.
- **Angle Limit Draw Scale:** Size of angle-limit axis display.

## SpringBone

- **Select Pivot:** Select the object used as this SpringBone's pivot.
- **Manager:** The SpringManager in which this bone is registered; read-only.
- **Enabled:** Whether dynamics are active for this bone.

### Forces

- **Stiffness:** The force returning the bone to its initial position. Higher values give a stronger return; lower values allow softer movement or gravity-driven droop. No fixed upper bound is specified, but the Inspector slider stops at 5000.
- **Drag:** Air resistance or damping, in the range 0–1. Higher values slow motion; lower values allow faster, springier motion.
- **Gravity:** Bone-specific gravity. A negative Y value pulls downward; a positive Y value lifts upward.
- **Wind Influence:** Effect of wind, from 0 (none) to 1 (full). See `WindVolume` below.

You can select and adjust multiple SpringBones together. `SpringBoneEditorBackgroundTasks` retains parameter changes made during Play mode, which helps when tuning by observation.

### Angle limits

Angle limits restrict a bone's movement. Because the bone's direction is treated as the X axis, limits are set around Y and Z.

- **Pivot:** The rotation of a pivot object defines the angle-limit frame. For example, to keep a skirt bone from dropping below a leg, parent its pivot to that leg bone. This resembles a parent constraint in a 3D tool, but uses rotation only and applies specifically to SpringBone.

By default the SpringBone's parent is its pivot. To change the reference or orientation, use **Create Pivot** in the SpringBone window. This creates a GameObject with `SpringBonePivot`; that component does no special processing, but the object is saved in dynamics CSV and recreated when loaded.

- **Rotational Stiffness:** When angle limits are enabled, the force returning the bone toward zero degrees. It lets the bone slow into the limit rather than abruptly stopping at its bound.
- **Y and Z limits:** Enable each axis with its checkbox and set its minimum and maximum. In the Inspector, **Change Together** moves sliders together; **Match Minimum** sets the maximum to the minimum; **Match Maximum** sets the minimum to the maximum; **Invert** swaps or reverses upper and lower values.

The Inspector bottom and Scene view display the limits. Rotate the pivot object to see their orientation.

### Distance limits (experimental)

These designate targets from which a bone tries to maintain distance, such as adjacent skirt-bone chains. The feature currently behaves erratically and is not recommended.

### Collision

Each SpringBone's own collision shape is a sphere; the colliders it can collide with are spheres, capsules, and panels.

- **Radius:** Radius of the SpringBone's own collision sphere.
- **Spheres, Capsules, Panels:** Lists of colliders against which this bone checks collisions.

### Angle-limit display

At the bottom of the Inspector, Y limits appear on the left and Z limits on the right. Colored arrows mark minimum and maximum; the gray downward arrow is zero degrees. In Play mode, a white arrow shows the bone's current direction.

### Standard Inspector display

A checkbox shows the conventional Inspector view if needed.

## Collider types

Create colliders with the corresponding buttons in the SpringBone window. A GameObject with the selected collider component is added as a child of the current selection. Each collider has a `LinkedRenderer` parameter: when its assigned Renderer is disabled, the collider is disabled too. For example, an animation that hides a sword sheath can hide its collider at the same time.

### Sphere (`SpringSphereCollider`)

- **Radius:** Sphere radius.
- **Typical uses:** Head and waist. Front hair and hair roots are often more stable with angle limits instead.

### Capsule (`SpringCapsuleCollider`)

A capsule extends along its local Y axis according to `Height`. When created with the SpringBone window, its length and direction are fitted between the parent bone and a child.

- **Radius:** Capsule radius.
- **Height:** Capsule length.
- **Typical uses:** Arms, legs, and torso.

### Panel (`SpringPanelCollider`)

A panel has width and height, with its local Z axis as its normal. A colliding SpringBone is pushed toward the front, local positive Z, and cannot move behind the panel. Motion can be unstable at panel edges, so make the panel wider than the bone's movement range.

- **Width:** Horizontal extent.
- **Height:** Vertical extent.
- **Typical uses:** Prevent long rear hair, ponytails, or capes from passing through the back.

## Extension components

### HighLeg

`HighLeg` helps a center skirt bone avoid dipping below the legs when neither left nor right leg is the obvious pivot. Create an empty object, add `HighLeg`, and assign the hip and the start/end bones of each leg. The component adopts the rotation of whichever leg is higher relative to the hip. Use it, or one of its children, as the center skirt bone's pivot. Its information is saved in dynamics CSV.

- **hipPivot:** Hip bone.
- **legs:** Start (parent) and end (child) bone of each leg.

## External forces

Objects in the scene derived from `ForceProvider` are detected and apply force to SpringBones. To make a custom force, inherit from `ForceProvider` and implement `GetForceOnBone`. Two examples are included:

- **ForceVolume:** Applies a fixed force to all SpringBones.
- **WindVolume:** Applies a wave-like wind force that varies with SpringBone position. Wind direction follows the object's Transform. This is experimental and not optimized. Each SpringBone's Wind Influence parameter scales the result: 1 receives the full effect; 0 receives none.

`WindVolume` settings:

- **weight:** Value from 0 to 1 multiplied by `strength`.
- **strength:** Overall wind force.
- **amplitude:** Outward spread of wind relative to its direction.
- **period:** Currently has no effect, except that 0 stops wind motion.
- **spinPeriod:** Time in seconds for one wind rotation. As the wind rotates, `amplitude` changes the spread direction.
- **peakDistance:** Distance between points of peak wind strength; strength varies with SpringBone position.

## Retaining parameter changes after Play mode

Keeping values adjusted while watching motion is useful during setup. `SpringBoneEditorBackgroundTasks` preserves SpringBone parameters when Play mode ends. To disable this, remove `SpringBoneEditorBackgroundTasks.cs` or change the condition in `PlaymodeStateChanged`, for example to retain changes only in a dedicated setup scene.
