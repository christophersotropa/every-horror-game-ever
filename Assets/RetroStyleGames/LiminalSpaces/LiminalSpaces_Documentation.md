# Liminal Spaces - Yellow Rooms (Free)

## Overview

A free modular backrooms kit: the endless mono-yellow office maze - wallpapered walls (plain and worn), inner and outer corners, a door wall with its door leaf, carpet floors and drop ceilings with recessed fluorescent panels. The modules snap to one 2 m grid and every model has a ready prefab with colliders. The demo is a large walkable yellow-rooms level lit by its ceiling panels, with a first-person bodycam controller.

This is the free sample of **Liminal Spaces - Modular Kit** (16 themes, hundreds of modules, more demo levels and the Faceless Man antagonist). The free pack uses the same folders, names and GUIDs, so importing the full kit later updates it in place.

## Render Pipeline Compatibility

| Pipeline | Supported | Notes |
|---|---|---|
| Built-in | Yes | RSG Shader Graph: install **Shader Graph** from the Package Manager (setup step 0). Demo scene `*_BRP`, no post-processing |
| URP | Yes | same materials, no conversion. Demo scene `*_URP` |
| HDRP | Yes | same materials, no conversion. Demo scene `*_HDRP` |

One material set works in all three pipelines: every material uses the cross-pipeline Shader Graph `RSG_Lit` (shared with the other RetroStyleGames packs, `RetroStyleGames/Base/Shaders`).

## Technical Details

| Item | Value |
|---|---|
| Models | 15 FBX (15 meshes) |
| Prefabs | 15 (one per model) |
| Materials | 14 (RSG_Lit; 1 transparent, 0 alpha-clipped, 1 emissive) |
| Textures | 29 PNG |
| Texture resolution | 21 x 2048x2048, 7 x 1024x1024, 1 x 4x4 |
| Texture maps | Base Color `_BaseColor` (sRGB), `_MAS` Metallic R / AO G / B 1 / Smoothness A (linear), `_Normal` (OpenGL) |
| Total triangles | 5,389 (sum of unique meshes) |
| Triangles per model | 32 (SM_RSG_YellowRooms_Floor_2x2m_A) - 1,460 (SM_RSG_YellowRooms_DoorWall_4m_A) |
| LODs | No |
| UV channels | UV0 (tileable / atlas); UV1 on 15 of 15 meshes |
| Rigging / animation | No (the door leaf swings by script in the demo) |
| Colliders | 14 MeshCollider, 2 MeshCollider convex on the prefabs |
| Grid / scale | 1 unit = 1 m; 2 m grid (2 / 4 m modules), walls 3 m high; prefab roots at position 0 / rotation 0 / scale 1 |
| Scenes | 3 demo scenes (YellowRooms_Large for Built-in, URP and HDRP) |
| Scripts | 5 C# demo scripts (namespace `RetroStyleGames.LiminalSpaces`), no render-pipeline code in the models / prefabs |
| Unity version | exported from 6000.0.59f2, minimum Unity 6.0 |
| Dependencies | Shader Graph 17.0+ on Built-in (URP / HDRP include it); Input System optional (the demo falls back to the Input Manager) |

## Modules

| Folder | Models | Prefabs | Materials | Textures | Triangles |
|---|---|---|---|---|---|
| YellowRooms | 15 | 15 | 5 | 12 | 5,389 |
| Shared | 0 | 0 | 9 | 17 | 0 |

YellowRooms: Wall 2 m / 4 m x 3 m (Plain, Worn), Corner 2 x 2 m (Inner, Outer), DoorWall 4 m, DoorLeaf 0.79 x 2.03 m, Floor 2 x 2 / 2 x 4 / 4 x 4 m (A, B), Ceiling 2 x 2 / 2 x 4 m (Plain) and 4 x 4 m with recessed light panels. `Shared` holds the materials the yellow modules share with the full kit (skirting trim, light panel, door frame, metal, grime atlas).

## Included Content

```
Assets/RetroStyleGames/
  Base/Shaders/RSG_Lit.shadergraph        shared RetroStyleGames cross-pipeline shader
  LiminalSpaces/
    LiminalSpaces_Documentation.md
    YellowRooms/Models  Materials  Textures  Prefabs
    Shared/Materials  Textures                 materials the yellow modules share with the full kit
    Demo/Scenes/LiminalSpaces_YellowRooms_Large_{BRP,URP,HDRP}.unity
    Demo/Scenes/Profiles/                    volume profiles of the demo (URP + HDRP copies _HDRP)
    Demo/Profiles/                           bodycam lens volume profiles
    Demo/Settings/URP, Demo/Settings/HDRP     pipeline assets of the demo look
    Demo/Scripts/                            player, door, room navigator, ambient
    Documentation/                           AI disclosure, third-party notices
```

## Demo Scene

Open `LiminalSpaces/Demo/Scenes/LiminalSpaces_YellowRooms_Large_<Pipeline>` (BRP = Built-in) and press Play: WASD move, Shift faster, Ctrl crouch, mouse look, F flashlight, E open / close a door, H hide the help line, Esc release the cursor. The level is a large maze of yellow rooms, door walls and corridors under drop ceilings, lit only by the ceiling light panels.

## Setup Instructions

0. **Built-in only:** install *Shader Graph* (Window > Package Manager > Unity Registry > Shader Graph, 17.0 or newer). URP and HDRP include it.
1. Keep *Project Settings > Player > Color Space* on **Linear** (the default of the Unity 6 templates; the lighting was authored in Linear).
2. Import the package.
3. Open `LiminalSpaces/Demo/Scenes/LiminalSpaces_YellowRooms_Large_<Pipeline>`. For the delivered look assign `Demo/Settings/URP/RP_RSG_Demo_URP` or `Demo/Settings/HDRP/RP_RSG_Demo_HDRP` in *Project Settings > Graphics* and *Quality* (optional: the scene also runs on the template pipeline assets).
4. **HDRP:** the scene has many ceiling lights; set *Max Shadows on Screen* to 256 in your HDRP asset (the included `RP_RSG_Demo_HDRP` has it) or Unity drops shadow requests with a Console warning.
5. Optional: install *Input System*; without it the demo player uses the legacy Input Manager (*Active Input Handling* = *Input Manager (Old)* or *Both*).
6. Build your own levels from the prefabs in `LiminalSpaces/YellowRooms/Prefabs`: snap to a 2 m grid (Edit > Grid and Snap settings, move snap 2) and rotate in 90 degree steps.

## Notes

- The demo level is lit with real-time lights only (no lightmap bake), so it opens and plays without baking. UV1 is included on the modules for your own lightmap bakes.
- Built-in demo scene: deferred camera, no post-processing; light intensities and the ambient are re-balanced for Built-in light falloff and the Built-in default *Lights Use Linear Intensity = off*. Without tonemapping it reads flatter than URP / HDRP.
- HDRP demo scene: fixed exposure (volume profiles `*_HDRP`) and the gradient sky.
- Files of the other pipelines (the `_HDRP` volume profiles, URP volume profiles) stay in every project; Unity lists them with missing-type warnings when that pipeline is not installed. In a fresh URP template project the first Play also shows Unity's one-time *URP Material upgrade* prompt; later runs are clean.
- GPU instancing is off on the materials: on Built-in, Shader Graph's instanced path draws a whole instanced batch at one object's position. Keep it off if you duplicate materials for Built-in (URP / HDRP use the SRP Batcher).
- Doors: the door leaf is a separate prefab with the pivot on the hinge; in the demo `LSDoor` swings it (E). An open leaf stops blocking the passage.

## AI-Generated Content

Disclosed per Asset Store guideline 1.6: the models, textures and demo scripts were created with an AI-assisted workflow and then edited, checked and prepared for real-time use by RetroStyleGames artists. Details: `Documentation/AI_CONTENT_DISCLOSURE.md`.

## Support

RetroStyleGames - https://retrostylegames.com

## License

Standard Unity Asset Store EULA. No third-party content is bundled; see `Documentation/THIRD_PARTY_NOTICES.md`.

