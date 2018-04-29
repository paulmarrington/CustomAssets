# Custom Assets
[TOC]

> Read the code in the Examples Folder and run the Example scene

## Introduction
Unity provides a base class called [ScriptableObject](https://docs.unity3d.com/ScriptReference/ScriptableObject.html). Derive from it to create objects or assets that don't need to be attached to game objects.

In short, a `ScriptableObject` is a class that contains serialisable data and functionality. Each instance of a class that derives from `ScriptableObject` has representation on disk as an asset. Each asset is a source for data and actions not coupled to a scene. The decoupling makes for functionality that is easy to test. It also provides modules to be shared across and between projects.

Custom assets are scriptable objects with benefits.
* They have a description associated with them that make editor usage easier. The creator can provide more information than just a name on the why and wherefore of an asset.
* All custom assets can have listeners registered against them that get informed of changes in value. In may, new cases components can react to or create changes without additional code.
* Storage can be of anything serialisable - from primitives like float to complex objects, or even MonoBehaviours.
* Custom assets can be saved on program exit and reloaded at startup, providing a clean and straightforward persistence mechanism. Assets marked as critical will persist immediately, and every time they change.
* Basic types offered include Float, Integer, Boolean, Trigger, String and Set.

### Custom Assets - the new Singleton
Static variables are not evil, just inflexible. Singleton MonoBehaviour instances are not corrupt either. However, both encourage tight coupling between interested parties.

And now for a bit of mild blasphemy. Assets created from scriptable objects, and hence custom assets are Unity supported *singletons*. Create `[SerializeField]` fields and drag an asset of the correct type onto them in the editor. All will reference the same in-memory instance.

Using custom assets over traditional singletons provide some benefits:

* Code is less coupled - or is that more decoupled?
* Custom assets can be tested in isolation.
* Alternative custom assets can be injected into objects that are expecting them. An inventory may depend on location or whether the player is in training mode.
* It is less error prone to pass custom assets between scenes and projects.
* Functionality can be more generalised for direct reuse from within the editor without writing as much scaffolding code. A `Float` custom asset, for example, can have listeners that hook into display objects. It can also be updated by sliders and scroll-bars without additional code by adding it to the *On Value Changed* field.

### Custom Assets as Game Managers
Managers are a favourite Unity pattern. As well as a system-wide master manager, many games have them for player, enemies, sound and more. They have some standard features:

* They are accessed by a static `Instance` variable.
* They are a MonoBehaviour that sets `Instance` in `Awake()`.
* They will call `DontDestroyOnLoad(gameObject)` if they are to be common across scenes.
* There is often one manager to rule them all.

It is not uncommon to see code like:

```C#
health = GameManager.Instance.PlayerManager.MaxHealth;
```

Try testing that component in isolation.

By contrast, a custom asset approach would be more like:

```C#
[SerializeField] PlayerManager;
...
health = PlayerManager.MaxHealth;
```

To work with this version create a test scene and put in an individual PlayerManager custom asset. It might have different data, or it might include mock methods.

### Custom Assets as Configuration
The most common use for scriptable objects is to ignore the scriptable part and use them as configuration containers. A Custom Asset is a file within the project. This file contains a reference to the script and serialised copies of all the data as added in the Unity editor.

You can safeguard the serialisable data by making it a private `[SerializeField]` and using accessors to allow reading. Alternatively, you can use them as seed data and change them during program execution.

```C#
class MyData : CustomAsset {
[SerializeField] private int maxHealth;
[SerializeField] private float timeOfDay;

// maxHealth cannot be changed outside this class
public int MaxHealth { get { return maxHealth; } };
// timeOfDay can be updated
public float TimeOfDay { get { return timeOfDay; } set { timeOfDay = value; } };
```
Later I will introduce better and more granular ways to handle data.

### Read-only Custom Assets
The custom asset inspector allows a designer to mark the asset read-only For serialisable classes as values, protection of internal data is still code bases. Make the fields private and serialisable so that the editor can change them. Then use accessors without `set` to only allow for reading. If you can't trust the code accessing data in a complex object graph, either clone the custom asset or lock down access at all levels.

```C#
var clone = Object.Instantiate(myCustomAsset).Value;
```

Cloning is much more expensive at runtime than baking in protection during the compile phase.

### Custom Assets and Persistence
Custom Assets adds optional persistence to scriptable objects. Persistent assets must be read/write and have the `persistent` field set in the Unity Editor.

Each persistent object is serialised to JSON and written as a `PlayerPref` entity. For this reason, the total storage is about one megabyte.For more massive storage needs, use a database.

The key is made up of the name of the asset and the class name, making it unique to the application.

## Accessing Custom Assets

A custom asset is like any other Unity asset. Create a field for it in a MonoBehaviour or other CustomAsset class and drop in the reference.

```C#
public sealed class CustomAssetsExample: MonoBehaviour {
  [SerializeField] private Float             maxFloat;
  [SerializeField] private Float             currentFloat;
  // ...
}
```
![Sample Custom Asset](SampleCustomAsset.png)

Custom assets aid decoupling. Many components can operate without directly knowing each other.

Access custom asset values by either casting or using the `Value` getter. ToString() willways call ToString() on the Value field.
```C#
Debug.LogFormat("{0} == {1}",maxFloat.Value, ((float) maxFloat);
Debug.LogFormat("{0} == {1}",maxFloat.Value.ToString(), maxFloat.ToString());
```

## Creating Custom Assets
Custom Assets are ScriptableObjects serialised and written to disk as an asset in the project.

```YAML
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 32
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 0}
  m_GameObject: {fileID: 0}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: 22f206729bb7e417e9b12649707e941e, type: 3}
  m_Name: SampleFloatVariable
  m_EditorClassIdentifier: 
  Description: 
  value: 0
  readWrite: 1
  persistent: 0
  critical: 0
```
Each custom asset type has an entry on the ***Create / CustomAssets / asset name***. Use it, select the resulting file and fill in the fields. If you want to load it from disk using `Resources.Load(pathFromResources)` you will need to place it in a ***Resources*** folder.

### OfType&lt;T>
`CustomAsset.OfType<T>` is the base type for all custom assets except `Trigger`. Functionality includes being able to register events on change, persistence and some read-only protection.
```C#
[CreateAssetMenu(menuName = "Examples/LargerAssetSample")]
public class LargerAssetSample : CustomAsset.OfType<LargerAssetContents> { }

[Serializable]
  public class LargerAssetContents {
    public int    Order;
    public float  Limit;
    public string Name;
  }
```

### Primative Custom Assets
```C#
  [SerializeField] private Float             currentFloat;
  [SerializeField] private Integer           integer;
  [SerializeField] private String            str;
  [SerializeField] private Boolean           boolean;
```
Each if these custom assets can in a project with or without supporting code. It is possible, for example, to have a `Float` value set in the ***On Value Changed*** field of a Slider or Scrollbar, then displayed using listener like `CustomAsset.IOImageFillListener()` to set the fill amount on a health bar,

### Integer
### String
### Trigger
### Boolean
### Custom Asset Sets
#### Enum Replacements
#### Sound Clips
## Editing Custom Assets
## Custom Assets as Resources
## Custom Assets as Event Sources
## Custom Assets as Event Listeners
### Component Listeners
#### ComponentListener (string)
#### Float
#### Integer
#### Boolean
#### UIImageFillListener
#### UITextListener
### Animation Listeners
#### Trigger
#### Float
#### Integer
#### Boolean
### Unity Event Listeners
## Custom Asset Persistence
## Asset Support
### Components
`Components` is a static helper class with functions to create and find Components.

#### Components.Find<T>(name)
Search the current scene for components of type `T`, then return the one with the name supplied. For a call with no name, we use the name of T.

If there are no matching objects in the scene, `Find` will try to load a resource of the supplied type and name. The name can be any path inside a ***Resources*** directory.

#### Components.Create<T>(gameObject, name)
Will create a component of type T inside the provided game object.  The instance of T is given the name supplied or the type name if the former is null.

#### Components.Create<T>(name)
The overload that does not supply a gameObject will create a new one and name the same as the component. The new gameObject is attached to the root of the current hierarchy.

### Asset Pooling
Unity3D games can run on lightweight platforms such as phones, tablets and consoles. Virtual or augmented reality games are immersive, and the least stutter in frame-rate is evident and annoying. Two of the most prominent culprits during gameplay are instantiating many complex objects and garbage collection.

`Instantiate()` and `Destroy()` aren't evil, but using them on game objects that are regularly needed is an overhead that is better overcome. The solution is to use a pool of these objects. It minimises expensive instantiation, and the garbage collector does not have to reclaim the memory for every usage.

A GameObject becomes a pool of it has the `Pool` script attached. Any child object becomes candidates for pooling. Alternatively, you can drag the ***Askowl/Assets/Prefabs/Pools*** prefab into the hierarchy. You can have as many pools as you wish and they may reside in any scene. The names have to be unique.

![Pool in Scene](PoolInScene.png)

In this example, three GameObjects are pooling aware. ***Scene GameObject*** has been created within the scene, while the other two copies of the same prefab with differing values in editor-available fields. They represent two different characters or effects that differ only in detail.

To retrieve a clone from the pool, use `Acquire()`. A new GameObject is cloned from the master if the pool is empty.

To release an object back to the pool, just disable it.

```C#
myClone.gameObject.SetActive(false);
```
Never call `Destroy()` unless you don't want to reused the GameObject. It cannot be returned to the pool after `Destroy`.

#### Acquire GameObject by Name
Calling `Acquire` with the name of the GameObject will retrieve a clone from the pool, instantiating a new one if necessary. `Pool.Acquire("Scene GameObject")` does the trick, or returns null if the Pool did not contain an original by that name.

Seeding some of the GameObject information using optional parameters is possible.

#### Transform parent
An effect will have the target as the parent, while a character may have a spawn point or a team leader. Position and rotation below are relative to that of the parent.

#### Vector3  position
The location where the clone will spawn relative to the parent. It defaults to (0, 0, 0).

#### Quaternion rotation
The facing direction, relative to the parent.

#### bool enable
Defaults to true so that the clone is enabled when it is taken from the pool or created.

#### bool poolOnDisable
PoolOnDisable also defaults to true. Using `SetActive(false) to disable a component will cause it to return to the pool. For situations where you want to enable and disable and as part of game processing, set `poolOnDisable` to false. Use `Pool.Return(clone)` to release the GameObject to the pool for reuse.

```C#
    for (int i = 0; i < 21; i++) {
      prefab1[i] =
        Pool.Acquire<PoolPrefabScriptSample>("PoolSamplePrefab",
                                             parent: FindObjectOfType<Canvas>().transform,
                                             position: new Vector3(x: i * 60, y: i * 60));
    }
```

#### Acquire<T>
The generic form of `Acquire` is a shortcut to get a component.
```C#
PoolPrefabScriptSample script = Pool.Asquire<PoolPrefabScriptSample>();
// is the same as
script = Pool.Asquire<PoolPrefabScriptSample>("PoolPrefabScriptSample");
// is the same as
GameObject clone = Acquire(typeof(T).Name);
script = (clone == null) ? null : clone.GetComponent<T>();
```
The same optional parameters are available as the non-generic game object - with the addition of name so that you can return a prefab with a different name to the MonoBehaviour inside.

### Quotes
`Quotes` is a C# class that if given a list of lines or a text file in a Resources folder will return a line randomly using the `Pick` interface. A quote is formatted as a ***body of the quote (attribution)*** where the attribution is optional. The attribution is surrounded in brackets and must be at the end of the line. RTF is acceptable in the quote.

```C#
Quotes QuotesA = new Quotes();
Quotes QuotesB = new Quotes("path-to-a-text-file-in-Resources");
Quotes QuotesC = new Quotes(new string[]{
    "The trouble with having an open mind, of course, is that people will insist on coming along and trying to put things in it (Terry Pratchett)",
    "Never say, 'oops'. Always say, 'Ah, interesting'.",
    "Hints on how to play <b>the game</b> are rarely attributed."
    "Success does not consist in never making mistakes but in never making the same one a second time. (George Bernard Shaw)");

string aQuote = QuotesC.Pick();
```

If there are less than one hundred options, they are cycled through sequentially rather than chose randomly.

### Selector
It is useful to select one item from a list as needed.

```C#
Selector<int> selector = new Selector<int> (new int[] { 0, 1, 2, 3, 4 });

for (int idx = 0; idx < 100; idx++) {
  int at = selector.Pick();
  Use(at);
}
```

The magic is in having different pickers for different requirements. The constructor allows for three types. Add more by overriding `Pick()`.

```C#
Selector(T[] choices = null, bool isRandom = true, int exhaustiveBelow = 0);
```

* ***Sequential***: `isRandom` is false or;
* ***Random***: `exhaustiveBelow` is less than the number of choices.
* ***Exhaustive Random***: `exhaustiveBelow` is greater than the number of choices.

In ***Exhaustive Random*** mode items are returned in a random order, but no entry shows up a second time until all are exhausted.

#### Choices
If the list of items to choose from changes, update the selector with `Choices`. The same picker will be reset and used.

```C#
Selector<int> selector = new Selector<int> (new int[] { 0, 1, 2, 3, 4 });
selector.Choices = new int[] { 5, 6, 7, 8 };
```

### Pick&lt;T>
`Random` is the default picker. In small lists is may appear to be favouring one or another asset.

There is are NUnit Editor tests in ***Examples/Scripts*** that demonstrate all the pickers.




==========================================================

## CustomAsset


For more detailed examples of all uses, view ***Askowl-Lib/Examples/Editor/TestCustomAsset.cs***.

A script asset is a file in the Unity project used to contain data and functionality. When defining a custom asset, use the [CreateMenuAsset](https://docs.unity3d.com/ScriptReference/CreateAssetMenuAttribute.html) Attribute. Selecting the specified item from Assets/Create writes the asset. Move it to a directory named ***Resources*** anywhere in your project.

Fill the public data in the asset for use in methods to provide functionality. The classic example is a custom asset with an array of audio clips. A `Play` method can select a clip to play randomly. In this way, sound effects would be less monotonous.

```C#
[CreateAssetMenu(menuName = "Examples/Sound Clips", fileName = "Clips", order = 1)]
public class ClipsExample: CustomAsset<ClipsExample> {
  public AudioClip[] clips;

  public void Play() {
    AudioClip clip = clips [Random.Range(0, clips.Length)];
    AudioSource.PlayClipAtPoint(clip, new Vector3 (0, 0, 0));
  }
}
```

From any ***Resources*** directory create an asset by selecting Examples / Sounds. Rename the asset to ***Birds***. Select the asset and add bird sounds to the list. A sample already exists in this package. Running the ***Askowl-Lib*** scene displays a ***Bird Sounds*** button that runs this script asset. The same technique could be used to select a prefab from a list to provide a variety of hazards.

### Asset Selector

Many assets are plug-and-play. We looked at sound clips earlier, but what about projectiles, opponents or even the cloths a hero is going to wear. It is easy to make a game more interesting with mix-and-match. A prefab is an asset allowing for dynamic behaviour.

We can rewrite the `Clips` class above to use an `AssetSelector`.

```C#
[CreateAssetMenu(menuName = "Custom/Sound Clips", fileName = "Clips")]
public class Clips: AssetSelector<AudioClip> {
  public void Play() {
    AudioSource.PlayClipAtPoint(Pick(), new Vector3 (0, 0, 0));
  }
}
```
Create a clip from the menu. Remember to put it in a ***Resources*** folder. Then fill the assets list with the clips to select from.

The default picker chooses a random asset from the list. By overriding `Pick()`. An interesting variant would be a pick that chooses different items until all are exhausted. For a simpler example, cycle through the assets in order.

```C#
  int idx = 0;

  public override T Pick() {
    return assets [idx++ % assets.Length];
  }
```

Meet `Selector.Cycle()`, one of AssetSelector optional pickers. Pickers can be selected in `OnEnable` or by code that has a reference to the Custom Asset. `Selector.Random()` is the default. `Exhaustive()` is like random but it guarantees not to repeat an item until all the other options are exhausted. See `Selector` for more details.

If you need another way of choosing your item, subclass `AssetSelector` and override the `Pick()` method.

### Objects Helpers

### Play Mode Test Runner Support