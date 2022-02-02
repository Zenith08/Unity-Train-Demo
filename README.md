# Train System High Performance

This is a scratch project where I tried a variety of different approaches to implement trains in Unity. The one that we will use for FTC is in /Scenes/TestScene.scene.
When running the scene, press w to make the train move forwards, and s to slow the train down or make it go backwards. There is some parts where the train jumps, this is an issue with how the game objects are layed out, not with the scripts.
Ideally creating these routes becomes easier with new models, and improved editor scripts.

### Scripts Included
Below is a list of the most important scripts for this system. Anything not mentioned there, and not included in the scene can be safely ignored.
- Assets/Scripts/Train.cs
- Assets/Scripts/TrackPiece.cs
- Assets/Scripts/StraightTrack.cs
- Assets/Scripts/CornerTrack.cs
- Assets/Scripts/SmoothSwitchTrack.cs
- Assets/Scripts/TrackEditorHelper.cs
- Assets/Prefabs/StraightTrack.prefab
- Assets/Prefabs/TrackTurn.prefab
- Assets/Prefabs/EditorHelper.prefab

Rotation is currently not included, this is because the plan is to use "train" to represent wheels on an actual rolling stock. With a position at the front and back of the rolling stock, we can interpolate its actual rotation. We can also do this with wheel trucks at each end.
