#if UNITY_EDITOR && CustomAssets
//- We are going to create a code-free health resource for your game. First let's create a test scene. [[Folder Examples/Editor/Example. Context Create/Scene. Name Health Double-click]]
//- Next create a custom asset to store character health. [[Folder Examples/Editor/Example. Context Create/Custom Assets/Mutable/Float. Name Health.]]
//- For convenience we will start with full health [[Health Value: 1]]
//- Now for the visual.
/*Hierarchy
Create/UI/Canvas
Create/Create Empty Child and call HealthBar
Create/UI/Image and call Background
Set colour to red, position and size to 0
Set X/Y Anchors to Min 0, Max 1
Change X pivot to 0
Duplicate for Foreground, but make green
*/
#endif