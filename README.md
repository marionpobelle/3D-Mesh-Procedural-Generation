# TrashPandaLikesFlowers
3D World with Procedural Generation and Shaders

# Presentation

Trashpanda Likes Flowers is a 3D world/model built with procedural generation.

# Development

This model was made with [Unity](https://unity.com/fr) and [Shader Graph](https://unity.com/fr/features/shader-graph).

# Demo

### Terrain

https://user-images.githubusercontent.com/112869026/208720348-400e7628-e449-4740-9cae-b4bf669162ec.mp4

### Water

https://user-images.githubusercontent.com/112869026/208720370-65e088af-8a52-4caa-b5d2-27a2f5a12c3c.mp4

### Skybox

https://user-images.githubusercontent.com/112869026/208720393-ca75d5f7-af6c-4e68-aa9a-6c3fb92b1011.mp4

# TDL

- [x] TERRAIN
- - [x] Preview mesh generation
- - [x] Procedural endless terrain generation
- - [x] LODS
- - [x] Flatshading
- - [x] Mesh coloration

- [ ] IMPLEMENTATION AND DATA
- - [x] Optimization
- - [x] Data storage
- - [x] Refactoring
- - [ ] Threading
- - - [ ] Fix threads getting the wrong minHeight and maxHeight values due to multiple requests at the same time

- [ ] SKYBOX
- - [x] Day shader
- - [x] Night shader
- - [x] Sun
- - [ ] Day/Night circle

- [x] WATER
- - [x] Planes generation
- - [x] Shader
- - [x] Foam
- - [x] Movement
- - [x] Optimization

- [x] GRASS
- - [x] Movement
- - [x] Shader

- [x] OBJECTS
- - [x] Random spawn on each chunk
