//- Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages
//- Custom assets are an extension to the unity ScriptableObject classes to provide containers for logic and data isolated from the interactive I/O elements of a game.
//- So, what are the benefits [[https://docs.google.com/presentation/d/1DaXU2w91cnFZjshvLlB_lQKfEJ1I_ewFkK_Gqs09XdE/edit#slide=id.p]] [[click slide]]
//- I will be showing the refactoring of a game object into custom asset managers later in this presentation [[click slide]]
//- By keeping all data used by multiple components in individual custom assets, changes and interactions are limited to loaded managers [[click slide]]
//- This makes component and multiple component testing extremely simple [[click slide]]
//- Changes in data custom assets raise events. These can be used to communicate between manager as well as driving visual changes [[click slide]]
//- By setting a custom asset as persistent it can be used to keep data between runs - such as the player level or location on a map [[click slide]]
//- Custom assets can be made to change GameObject Components directly using Custom Asset Drivers. When the component does not expose the needed public variable, use one of he provided connectors [[click slide]]
//- There are drivers for triggers and all basic data forms - from integer to string. There is a named version for Animator components [[switch to Rider Example/Example.cs]]

//- Now we will refactor a real game manager from a 2D game. The original code is to the left while we will build up the custom asset managers on the right

#if !ExcludeAskowlTests
using System;
using CustomAsset;
using CustomAsset.Mutable;
using UnityEngine;
using GameObject = CustomAsset.GameObject;

// ReSharper disable MissingXmlDoc

namespace Askowl.Transcripts {
  //- Because a custom asset exists on disk as part of the project, we need to create a concrete form. We could have more than one for a multi-player game
  [CreateAssetMenu(menuName = "Managers/Player Death"), Serializable]
  public class PlayerDeathManager : Manager {
    //- We load up all the data custom assets in the inspector. All of these are mutable assets that change during the course of the game.
    [SerializeField] private Float   health = default;
    [SerializeField] private Integer coins  = default;
    [SerializeField] private Integer lives  = default;
    [SerializeField] private Integer scene  = default;
    //- This is the only data custom asset that could be CustomAsset.Constant.Integer, but we may want to change it based on user level. It could also be persistent.
    [SerializeField] private Integer maximumLives = default;
    //- These two are sent with a GameObjectConnector MonoBehaviour. Add it to the relevant game object by selecting it and using the menu option Component/CustomAssets/GameObject Connnector
    [SerializeField] private GameObject player     = default;
    [SerializeField] private GameObject checkpoint = default;

    //- Initialise is called for a custom asset on the first FixedUpdate after the asset is enabled (OnEnable)
    protected override void Initialise() => health.Emitter.Listen(OnHealthChange);

    //- Called every time health changes. Not that frequently in the scheme of things.
    private void OnHealthChange(Emitter emitter) {
      if (health <= 0) KillPlayer();
    }

    //- In this game the player goes back to the last checkpoint if they have lives left.
    private void KillPlayer() {
      if (lives <= 1) {
        RestartGame();
      } else {
        SendPlayerToCheckpoint();
      }
    }

    private void RestartGame() {
      lives.Value = maximumLives;
      coins.Value = 0;
      //- THe SceneManager shown below interfaces the scene to Unity management
      scene.Value = 0;
    }

    //- The custom assets are filled with the GameObject containing the custom asset connector
    private void SendPlayerToCheckpoint() => player.Value.transform.position = checkpoint.Value.transform.position;
  }

  //- The scene manager would normally be in separate file
  [CreateAssetMenu(menuName = "Managers/Scene"), Serializable] public class SceneManager : Manager {
    [SerializeField] private Integer scene = default;

    //- Action triggered when scene changes
    protected override void Initialise() => scene.Emitter.Listen(OnSceneChange);

    //- That connects to Unity to change the display. This could be a string if you were to change scenes by name - my preferred option.
    private void OnSceneChange(Emitter emitter) =>
      UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex: scene);

    //- MoveToNextLevel() can be replaced by level.Value += 1
    //- AddCoin is replaced by coin.Value += 1
    //- OnLivesChanged and OnCoinsChanges are replaced by lives.Subscribe() and coins.Subscribe()
    //- Now we can delete the original GameManager.cs
    //- and write a few tests :)
  }
}
#endif