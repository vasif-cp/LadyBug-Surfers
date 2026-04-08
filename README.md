# LadyBug Surfers

A mobile arcade sledding game built in **Unity 6** as a case study project. The player launches a ladybug character with a slingshot, steers down snowy/icy slopes collecting coins, and spends earned coins on upgrades between runs.

**[Download Build (Android)](https://drive.google.com/file/d/1GALZjDtSNQqhyj10y6_QN6iT-tdUrvzv/view?usp=sharing)**

---

## Core Systems

### Dependency Injection

The project uses a lightweight custom DI system instead of singletons. `GameContext` is the entry point — it runs first (execution order -100) and wires everything together:

- Discovers all `IContextService` components in the scene and registers them in a `ServiceRegistry` (a simple type-to-instance dictionary).
- Discovers all `IInjectable` components and passes the context to them.

This means systems never search for each other directly. A movement controller, input provider, save system, or upgrade manager — each one registers itself as a service and gets resolved through the context. Adding a new system is straightforward: implement `IContextService` to be available to others, or `IInjectable` to receive access to existing services.

### Event-Driven Communication

All cross-system communication goes through `GameEvents` — a static class holding ~14 Action delegates. Systems subscribe to the events they care about and fire events when something happens. No system holds a direct reference to another unrelated system.

Events are grouped by domain:
- **Lifecycle** — game start requested, camera transition complete, session started/ended
- **Slingshot** — pull started/updated/ended, launch requested
- **Items** — collectible collected, coins collected, obstacle hit
- **Meta** — upgrade purchased, coin balance updated, upgrade modifiers applied
- **Gameplay** — character distance updated

All events are cleared when `GameContext` is destroyed, preventing memory leaks on scene reload.

### Slingshot Launch

The launch mechanic is split into two parts:

**SlingshotController** handles input and visuals. It reads vertical joystick input (pulling down), interpolates the character's position backward with a line renderer showing the slingshot band, and fires pull events for the UI indicator.

**SlingshotEngine** is a pure state machine (Idle → Pulling → Launched) that handles the math. On release, it applies a power curve: `curvedPull = Pow(normalizedPull, exponent)`, then interpolates between min and max force. The exponent (default 2.0) gives a non-linear feel — small pulls are weak, full pulls are powerful. The final impulse is multiplied by any launch power upgrade bonus.

### Physics

`CoreSledPhysics` runs every FixedUpdate and computes forces applied to the character's Rigidbody:

1. **Steering** — perpendicular to velocity, scaled by input. Dampened at high speed so the sled feels heavier as it goes faster.
2. **Speed Boost** — constant forward force from the Speed upgrade.
3. **Friction** — opposes velocity. Ice surfaces have low friction (2.5), snow has higher (5.0).
4. **Slope Gravity** — gravity projected along the slope plane, pushing the sled downhill.
5. **Ground Stick** — downward force along surface normal to keep the sled on terrain.
6. **Air Gravity / Air Drag** — applied when airborne to bring the sled back down naturally.

Surface type is detected by `TerrainSurfaceDetector`, which samples the terrain's alphamap at the contact point and maps the dominant texture layer to Snow or Ice. Results are cached with a distance threshold to avoid sampling every frame.

### Collectibles & Obstacles

`CollectibleDetector` uses `Physics.OverlapSphereNonAlloc` each FixedUpdate to find nearby coins within a configurable radius. Collected coins are tracked per-session via a HashSet of instance IDs to avoid double-collection. `CollectibleManager` listens to collection events, deactivates the coin object, and persists the collected state to the save system when the session ends.

Obstacles use trigger colliders. `BaseObstacle` detects the character entering its trigger and fires `OnObstacleHit`. `ObstacleEffectHandler` listens and applies the effect:
- **SlowingDown** — multiplies current velocity by 0.5
- **Stop** — zeroes velocity completely

### Upgrade System

Four upgrades persist between runs: **Launch Power**, **Speed**, **Steering**, and **Collectible Value**. Each has a max level, base price, and price-per-level increase.

`UpgradeManager` loads saved levels from PlayerPrefs on construction. When the player starts a run, it computes an `UpgradeModifiers` struct (`ValuePerLevel × currentLevel` for each type) and broadcasts it. The slingshot controller applies the launch power bonus, the physics engine applies speed boost and steering bonuses, and the session uses the collectible value bonus in the final coin calculation.

The upgrade UI is generated dynamically — `MainMenuView` instantiates one `UpgradeSlotView` per upgrade type, each showing icon, level, and price with a purchase button.

### Economy & Session

When a run ends, `GameplaySession` calculates earned coins:
- **Distance coins** = `floor(distance × (BaseCoinsPerMeter + CollectibleValueBonus))`
- **Collectible coins** = `collectedCount × CollectibleBonusCoins`

`MetaGameController` adds the total to the player's balance via the save system. If the distance exceeds the saved best, it's flagged as a new record and displayed on the results screen.

### Save System

`PlayerPrefsSaveSystem` handles all persistence with simple key patterns:
- `Resource:Coins` — coin balance
- `Upgrade:Level:{index}` — level per upgrade type
- `GameplayData:Collectibles:{resourceID}:{itemID}` — collected items
- `GameplayData:BestDistance` — personal best

### Camera

`CameraTransitionController` uses Cinemachine's `CinemachinePositionComposer` to animate from a menu camera position to a gameplay follow camera. The transition uses a DOTween sequence that interpolates both the target offset and camera distance over 1 second, then fires `OnCameraTransitionComplete` to activate the slingshot.

### UI

All views extend `UIViewBase`, which provides DOTween-based fade in/out through a CanvasGroup (0.25s duration). Views subscribe to game events to show/hide themselves at the right moments — no view directly references another view or gameplay system beyond what it receives through injection.

---

## What I Would Improve With More Time

1. **State reset instead of scene reload** — Currently the game reloads the entire scene after each run. Resetting game state and re-enabling components in place would be more efficient and eliminate the loading pause between runs.

2. **Visual polish** — Better particle effects, smoother animations, environment variety, and overall presentation to offer a more engaging player experience.

3. **Editor tooling for level design** — Custom editor tools for placing coins, obstacles, and shaping terrain would make level creation faster and allow non-programmers to design levels easily.

4. **Shader-based optimizations** — Effects like coin rotation could be handled entirely in shaders instead of Transform manipulation, reducing CPU overhead especially with many coins on screen.

5. **Level completion logic** — Currently runs end only when speed drops to zero. A proper level system with defined start/end points, distance goals, or checkpoints would add structure and progression to the gameplay.