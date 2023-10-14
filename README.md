Basic 2D-pixel, Terranigma-inspired ARPG Unity project. 

![seaside-1](https://github.com/michellegramolini/arpg/assets/112978754/a2d41396-c704-4dee-9905-8d5d221b2477)

Run, jump, fight and swim your way through a hostile island landscape.

![seaside-2](https://github.com/michellegramolini/arpg/assets/112978754/bbf8bddc-9a98-4b21-a245-cd476d0e59cb)

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

We’re using a shared repository model for our game. Meaning collaborators all have push access to a single shared repository and feature development branches are created when changes need to be made. PRs are used in this model to initiate peer code review and discussion about a set of changes before the changes are merged into the remote main branch.

Branches are created in order to develop features, fix bugs, and safely experiment in a contained area of the repo. Branches are always created off of an existing branch (typically main). Once you are happy with your changes on your branch, you can open a PR to merge the current branch to another branch.

Create branches off of main and PR to allow reviews. 
For Graphics all files are 16 pixels per unit. 

It's good when testing new functionality to create a test scene.

### Packages to note
This project is using Cinemachine for camera mechanics, TextMesh Pro for GUI stuff, and TileMap 2D extras.

### How to commit changes to our project

#### Create a branch

Create a local branch to do work on (usually off of remote main)

#### Make changes

Develop and test in your local branch using a new scene in Unity
Make, commit, and push changes from your local branch to your remote branch until you are ready to share with the team

#### Create a PR

Once you’re satisfied with your changes and want to share them with the team, ensure you’ve tested your changes on your local branch (open Unity and check there are no errors and test the game)
Commit your changes from your local branch to your remote branch. Do not make changes to remote branches unless they are yours
Create a PR to merge your remote feature development branch to remote main, add a summary of your changes here

#### Address review comments
Team reviewers may leave questions, comments, and suggestions on your PR. You can respond to these and continue to commit and push changes based on team feedback

#### Merge your PR
Once PR is approved (currently not in place bc we don’t pay for GitHub), you may merge the PR to remote main

We’ll keep old branches around for awhile once they’ve been merged to remote main as a failsafe and clean up intermittently.


