# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**LadyBug Surfers** is a mobile arcade sledding game built in Unity 6 (6000.2.6f2). Players launch a ladybug character via a slingshot mechanic, steer down slopes collecting coins, and upgrade between runs. Single scene (`Assets/Project/Scenes/GameScene.unity`), single assembly (no `.asmdef` files in Assets/).

## Build & Run

This is a standard Unity project — open in Unity Editor 6000.2.6f2. No custom build scripts or CI/CD. No automated tests exist yet (test framework package is installed but unused).

## Architecture

### Dependency Injection (`LS.Core`)

`GameContext` (MonoBehaviour, execution order -100) is the DI container. On Awake it:
1. Finds all `IContextService` implementations in the scene, registers them in `ServiceRegistry`
2. Finds all `IInjectable` implementations and injects the context into them

Services resolve each other through `IGameContext`. Key services: `IInputProvider`, `ICharacterMovementController`, `IUpgradeManager`, `ISaveSystem`.

### Event System (`LS.Events.GameEvents`)

Static pub/sub hub with ~14 Action delegates grouped by domain: lifecycle, slingshot, meta-progression, items, gameplay. All events are cleared in `OnDestroy` of `GameContext` to prevent leaks.

### Game Flow

MainMenuView (upgrades) → `OnGameStartRequested` → Camera transition → Slingshot pull/release → Physics-driven sledding → Coins/obstacles → Speed drops below threshold → `OnSessionEnded` → Results screen → Scene reload.

### Namespace Map

| Namespace | Responsibility |
|---|---|
| `LS.Core` | DI container, service registry, context interfaces |
| `LS.CharacterController.Core` | Movement controller, input handling, joystick provider |
| `LS.CharacterController.Physics.Core` | Force calculations, ground/surface detection, collectible detection |
| `LS.CharacterController.Physics.Data` | ScriptableObject settings (physics, slingshot, surface, collectibles) |
| `LS.Items.Slingshot` | Slingshot state machine (Idle/Pulling/Launched) and impulse math |
| `LS.Items.Collectibles` | Base collectible, coin implementation, collection tracking |
| `LS.Items.Obstacles` | Obstacle types (SlowingDown, Stop) and effect handler |
| `LS.Events` | Static event hub |
| `LS.Gameplay` | Session lifecycle, session state, upgrade modifier application |
| `LS.Meta` | Upgrade system (4 types: LaunchPower, Speed, Steering, CollectibleValue), economy settings |
| `LS.Save` | PlayerPrefs-based persistence (coins, upgrade levels, collectibles, best distance) |
| `LS.UI.View` | DOTween-animated views with CanvasGroup fade, upgrade slots |
| `LS.Camera` | Cinemachine camera transitions |

### Physics System

`CoreSledPhysics` computes forces each FixedUpdate: steering, speed boost, surface-dependent friction (Snow vs Ice), slope gravity, ground stick, air drag. Surface type is resolved by `TerrainSurfaceDetector` sampling terrain alphamaps at the contact point (cached with resample threshold).

### Data Configuration

All tunable parameters live in ScriptableObjects under `PhysicsSettings` (aggregates `CharacterPhysicsSettings`, `SlingshotPhysicsSettings`, `SurfacePhysicsSettings`, `CollectiblePhysicsSettings`), `UpgradeSettings`, and `EconomySettings`.

### Persistence

`PlayerPrefsSaveSystem` stores: coin balance (`Resource:Coins`), upgrade levels (`Upgrade:Level:{index}`), collected items (`GameplayData:Collectibles:{resourceID}:{itemID}`), best distance (`GameplayData:BestDistance`).

## Key Third-Party Dependencies

- **DOTween** — UI animations, camera transitions
- **Cinemachine 3** (com.unity.cinemachine 3.1.6) — Camera follow/positioning
- **Input System** (com.unity.inputsystem 1.14.2) — Joystick input
- **Joystick Pack** (ThirdParty) — FloatingJoystick UI element
- **URP** (com.unity.render-pipelines.universal 17.2.0) — Dual renderers: PC and Mobile

## Conventions

- All game scripts live under `Assets/Project/Scripts/` organized by namespace
- New systems should implement `IContextService` to register with DI, or `IInjectable` to receive context
- Communication between systems uses `GameEvents` static delegates — avoid direct references between unrelated systems
- Configuration goes in ScriptableObjects, not hardcoded values
- Commit messages follow `[Category] Description` format (e.g., `[Core Gameplay] Fixing rotation issue`)