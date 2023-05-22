## To run project:
clone and open in Unity. currently running on version 2021.3.21f1. select a scene and hit play in editor.

## To build project:

In Unity editor: 
File > Build Settings > (select your platform in dropdown) > Build (button) 

## Project Structure:
Most everything touched by us lives in the Assets folder.

Assets root:
PlayerInput has the input configurations for Player and UI. Keyboard, controller etc. 

Animation:
animations and animation controllers live here

Graphics:
all art asset .pngs live here, such as tilesets, character sprites, etc. 
save tile palettes in Graphics/Palettes folder.
save tiles in Graphics/Tiles folder.

Prefabs:
self-explanatory

Scenes:
Unity Scenes are saved here.

ScriptableObjects:
Scriptable Objects created live here. Once you've created a new Scriptable Object script, if configured properly, you should be able to create new instances with right click menu.

Scripts:
C# scripts live here. we aren't leveraging namespaces for now but to keep things neat, we can divide stuff up into separate folders.

## Practices
Create branches off of main and PR to allow reviews. 
For Graphics all files are 16 pixels per unit. 

It's good when testing new functionality to create a test scene.

### Packages to note
This project is using Cinemachine for camera mechanics, TextMesh Pro for GUI stuff, and TileMap 2D extras. 


