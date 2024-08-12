# 3D Mesh Procedural Generation
## Summary

An infinite 3D world generated using procedural generation and optimized with LODs.

## Development

This project was made with [Unity](https://unity.com/fr) and [Shader Graph](https://unity.com/fr/features/shader-graph).\ 
The following videos were used as references :\
[TPLF](https://www.youtube.com/playlist?list=PL4wvE9SgBKlfxsV1upO26FrRNoomVuikC).

## Demo

TO DO

## TDL

- [x] TERRAIN
- - [x] Preview mesh generation
- - [x] Procedural endless terrain generation
- - [x] LODS
- - [x] Flatshading
- - [x] Mesh coloration

- [x] IMPLEMENTATION AND DATA
- - [x] Optimization
- - [x] Data storage
- - [x] Refactoring
- - [x] Threading
- - - [x] Fix threads getting the wrong minHeight and maxHeight values due to multiple requests at the same time

- [x] SKYBOX
- - [x] Day shader
- - [x] Night shader
- - [x] Sun

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
