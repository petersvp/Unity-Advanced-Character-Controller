# Creating your own controls

In TCC, you add functionality by implementing the following interfaces.

| Interface | Purpose |
|---|---|
| `IMove` | Specifies the character's movement vector. |
| `ITurn` | Specifies the character's facing direction and turn speed. |
| `IPriorityLifecycle<IMove>` | Defines behavior when this `IMove` has the highest priority. |
| `IPriorityLifecycle<ITurn>` | Defines behavior when this `ITurn` has the highest priority. |
| `IEarlyUpdateComponent` | Updates a component according to the Brain's timing, before `Update` at `ExecuteOrder = 0`. Stops updating while the Brain is stopped. |
| `IUpdateComponent` | Updates a component according to the Brain's timing, after `Update` at `ExecuteOrder = 0`. Stops updating while the Brain is stopped. |


## Character movement

This section shows how to control character movement with priorities.

Create a `Control` that moves the character horizontally with WASD and vertically with Q and E.


First, create a `MyControl` class.

```cs
public class MyControl : MonoBehaviour
{
    public float Speed = 4;

    public void MoveHorizontal(Vector2 input)
    {
    }

    public void MoveVertical(float input)
    {
    }
}
```

Implement `IMove` to control movement. This interface specifies movement priority and the velocity used when this component has the highest priority.

`MoveHorizontal()` and `MoveVertical()` receive input for the `MoveVelocity` calculation. `CharacterSettings.CameraYawRotation` rotates the movement vector to account for the camera's direction.

With this component attached, the character follows the input while its `Priority` is higher than that of other movement components.

```cs
public class MyControl : MonoBehaviour, IMove
{
    public float Speed = 4;
    
    public int Priority = 0;

    private Vector3 _inputVelocity;
    private CharacterSettings _settings;

    private void Awake()
    {
        TryGetComponent(out _settings);
    }

    // Horizontal movement input
    public void MoveHorizontal(Vector2 input)
    {
        _inputVelocity = new Vector3(input.x, _inputVelocity.y, input.y);
    }

    // Vertical movement input
    public void MoveVertical(float input)
    {
        _inputVelocity.y = input;
    }

    
    int IPriority<IMove>.Priority => Priority;

    Vector3 IMove.MoveVelocity => _settings.CameraYawRotation * _inputVelocity * Speed;
}
```

Next, ignore gravity while this component controls movement. Implement `IPriorityLifecycle<IMove>` to receive callbacks when it gains or loses the highest movement priority.

Get the `Gravity` component, disable it in `OnAcquireHighestPriority`, and re-enable it in `OnLoseHighestPriority`.

```cs
public class MyControl : MonoBehaviour, IMove, IPriorityLifecycle<IMove>
{
    public float Speed = 4;    
    public int Priority = 0;
    private Vector3 _inputVelocity;

    private Gravity _gravity; // New

    private CharacterSettings _settings;

    private void Awake()
    {
        TryGetComponent(out _settings);
        TryGetComponent(out _gravity); // New
    }

    public void MoveHorizontal(Vector2 input)
    {
        _inputVelocity = new Vector3(input.x, _inputVelocity.y, input.y);
    }

    public void MoveVertical(float input)
    {
        _inputVelocity.y = input;
    }
    
    int IPriority<IMove>.Priority => Priority;

    Vector3 IMove.MoveVelocity => _settings.CameraYawRotation * _inputVelocity * Speed;
    
    
    void IPriorityLifecycle<IMove>.OnUpdateWithHighestPriority(float deltaTime) { }

    void IPriorityLifecycle<IMove>.OnAcquireHighestPriority()
    {
        _gravity.enabled = false;
    }

    void IPriorityLifecycle<IMove>.OnLoseHighestPriority()
    {
        _gravity.enabled = true;
    }
}
```

## Character dash

This section shows how to use priorities to update facing direction and how to change a priority over time.

Create a dash `Control` that moves quickly in a fixed direction without interference from other controls.

Use `Input(...)` to set the intended dash direction; call `Dash()` to dash in the character's current input direction.

```cs
public class DashControl : MonoBehaviour
{
    [Header("Settings")]
    public float DashSpeed = 14;        // Dash movement speed
    public float DashDuration = 0.5f;   // Dash duration
    
    public void Input(Vector2 input)
    {
    }

    public void Dash()
    {
    }
}
```

Implement the dash with `IMove`, so it supplies movement only when its priority is highest. Make that priority active only while dashing.

Implement `IUpdateComponent` and slow the dash in `IUpdateComponent.OnUpdate(...)`. Unlike Unity's standard `Update`, this callback runs in `FixedUpdate` or `Update` according to the Brain's timing.
Set its execution order with `IUpdateComponent.Order`. For example, increase `Order` if this component must use the result of another `IUpdateComponent`.


```cs
public class DashControl : MonoBehaviour, IMove, IUpdateComponent
{
    [Header("Priority")]
    public int MovePriority = 0;

    [Header("Settings")]
    public float DashSpeed = 14;
    public float DashDuration = 0.5f;
    
    private IWallCheck _wallCheck; // Wall contact check
    private CharacterSettings _settings;

    private Vector3 _direction; // Dash direction
    private float _currentSpeed; // Current dash speed
    private float _dashEndTime; // Time when the dash ends
    private Vector2 _input; // Cached input

    private void Awake()
    {
        TryGetComponent(out _wallCheck);
        TryGetComponent(out _settings);
    }

    public void Input(Vector2 input)
    {
        _input = input;
    }

    public void Dash()
    {
        // Do not dash without a movement direction
        if (_input.sqrMagnitude < 0.5f)
            return;

        // Set the dash direction
        _direction = _settings.CameraYawRotation * new Vector3(_input.x, 0, _input.y);

        // Set dash speed
        _currentSpeed = DashSpeed;
        _dashEndTime = Time.time + DashDuration;
    }

    // Enable this priority only while dashing
    int IPriority<IMove>.Priority => _dashEndTime > Time.time ? MovePriority : 0;

    Vector3 IMove.MoveVelocity => _direction * _currentSpeed;
    
    void IUpdateComponent.OnUpdate(float deltaTime)
    {
        // Run only while dashing
        if (_dashEndTime < Time.time)
            return;

        // Stop on contact with a wall
        if (_wallCheck.IsContact)
            Stop();
        
        // Slow down so the dash reaches zero speed after DashDuration.
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0, deltaTime * DashSpeed / DashDuration);
    }

    int IUpdateComponent.Order => Order.Control;

    private void Stop()
    {
        _currentSpeed = 0;
        _dashEndTime = 0;
    }
}
```

Finally, make the character face the dash direction while dashing.

Set facing direction through `ITurn`. Like `IMove`, it works by priority. `YawAngle` gives the target direction; `TurnSpeed` specifies how quickly the character turns. With `TurnSpeed` set to `-1`, the character faces the target direction immediately.


```cs
public class DashControl : MonoBehaviour, IMove, IUpdateComponent, ITurn
{
    [Header("Priority")]
    public int MovePriority = 0;
    public int TurnPriority = 0;

    [Header("Settings")]
    [Range(-1, 50)]
    public int TurnSpeed = -1;
    public float DashSpeed = 14;
    public float DashDuration = 0.5f;
    
    private Vector3 _direction;
    private IWallCheck _wallCheck;
    private CharacterSettings _settings;

    private float _currentSpeed;
    private float _dashEndTime;
    private float _yawAngle;
    private Vector2 _input;

    private void Awake()
    {
        TryGetComponent(out _wallCheck);
        TryGetComponent(out _settings);
    }

    public void Input(Vector2 input)
    {
        _input = input;
    }

    public void Dash()
    {
        if (_input.sqrMagnitude < 0.5f)
            return;

        _direction = _settings.CameraYawRotation * new Vector3(_input.x, 0, _input.y);
        _currentSpeed = DashSpeed;
        _dashEndTime = Time.time + DashDuration;
        _yawAngle = Vector3.SignedAngle(Vector3.forward, _direction, Vector3.up);
    }

    int IPriority<IMove>.Priority => _dashEndTime > Time.time ? MovePriority : 0;

    Vector3 IMove.MoveVelocity => _direction * _currentSpeed;
    
    void IUpdateComponent.OnUpdate(float deltaTime)
    {
        if (_dashEndTime < Time.time)
            return;
        
        if (_wallCheck.IsContact)
            Stop();
        
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0, deltaTime * DashSpeed / DashDuration);
    }

    private void Stop()
    {
        _currentSpeed = 0;
        _dashEndTime = 0;
    }

    int IUpdateComponent.Order => Order.Control;

    int IPriority<ITurn>.Priority => _dashEndTime > Time.time ? TurnPriority : 0;

    int ITurn.TurnSpeed => TurnSpeed;

    float ITurn.YawAngle => _yawAngle;
}
```

## Movement to a specified position

This section introduces `IWarp` movement, which is unaffected by other Effects or Controls.
`IWarp` is useful when the character must reach a precise position. `IWarp.Warp(...)` ignores obstacles, while `IWarp.Move(...)` considers intermediate positions; the exact behavior depends on the Brain.
Positions set through `IWarp` take precedence over other Controls and Effects, and reset the Effects' state.

`ITransform` caches Transform data at the start of each frame and exposes those cached values. Values assigned through it take effect immediately.

The following component moves between two positions over a fixed duration.

```cs
public class WarpControl : MonoBehaviour, IMove, IPriorityLifecycle<IMove>
{
    public int Priority = 15;
    public float Duration = 5;
    public Transform Start, End;

    private IWarp _warp;
    private ITransform _transform;
    private float _timeAmount = 0;
    private Vector3 _velocity;

    private void Awake()
    {
        TryGetComponent(out _warp);
        TryGetComponent(out _transform);
    }

    int IPriority<IMove>.Priority => Priority;

    Vector3 IMove.MoveVelocity => _velocity;

    void IPriorityLifecycle<IMove>.OnUpdateWithHighestPriority(float deltaTime)
    {
        _timeAmount += deltaTime /  Duration;
        _timeAmount %= 1;
        
        var pos = Vector3.Lerp(Start.position, End.position, _timeAmount);

        _velocity = (_transform.Position - pos) / deltaTime;
        _warp.Move(pos);
    }

    void IPriorityLifecycle<IMove>.OnAcquireHighestPriority()
    {
        _timeAmount = 0;
    }

    void IPriorityLifecycle<IMove>.OnLoseHighestPriority() { }
}

```
