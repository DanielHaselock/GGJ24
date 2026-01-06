using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/// <summary>
/// Manages player instances, handles player joining, scene transitions, and player list updates.
/// Implements a singleton pattern to persist across scenes.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public List<PlayerController> players = new List<PlayerController>();

    private List<string> names = new List<string>
    {
        "ACHOO",
        "ADAM",
        "APPLES",
        "ARIA",
        "ANDY",
        "AUGUST",
        "BABY",
        "BABY",
        "BUHLOU",
        "BEETLE",
        "BELLY",
        "BINKY",
        "BODIES",
        "BOING",
        "BOOBOO",
        "BOOHOO",
        "BOOMER",
        "BOOTY",
        "BOWWOW",
        "BOZO",
        "BRONZE",
        "BUDGIE",
        "BUFFY",
        "BUGGS",
        "BUMBUM",
        "BUNNY",
        "BUSTER",
        "BUTTER",
        "BUTTON",
        "CACKLE",
        "CANCER",
        "CANDY",
        "CATTY",
        "CHOCCY",
        "CHOCO",
        "CHORT",
        "CHUBBY",
        "CHUCHU",
        "CHUCK",
        "COCO",
        "COOKIE",
        "DAISY",
        "DANIEL",
        "DEEDEE",
        "DEXTER",
        "DENNY",
        "DINK",
        "DIDDLY",
        "DOINK",
        "DOO",
        "DOODLE",
        "DREW",
        "DR. RUG",
        "EARS",
        "FACES",
        "FART",
        "FARTER",
        "FARTY",
        "FATSO",
        "FIDDLE",
        "FIZZ",
        "FREDDY",
        "FROGGY",
        "FUZZY",
        "FUZZLE",
        "GABE",
        "GACK",
        "GAGGLE",
        "GARLIC",
        "GASSY",
        "GERKIN",
        "GIGGLE",
        "GLING",
        "GLOCK",
        "GLOP",
        "GLUB",
        "GO GO",
        "GOOB",
        "GOOBUS",
        "GOOFY",
        "GRUB",
        "GUS",
        "HAHA",
        "HAMMY",
        "HANKY",
        "HAPPY",
        "HARLEY",
        "HECKLE",
        "HEEHEE",
        "HOGS",
        "HOHO",
        "HONKER",
        "HOOHA",
        "HOOHOO",
        "HUMPTY",
        "HUNKER",
        "JACK",
        "JAMJAM",
        "JANGLE",
        "JAX",
        "JEAN",
        "JELLY",
        "JIGGLE",
        "JIMBO",
        "JINGLE",
        "JO",
        "JOEY",
        "JOJO",
        "JOKES",
        "JOLLY",
        "JULY",
        "JUMBO",
        "JUNE",
        "JUNIOR",
        "KATTY",
        "KEVEN",
        "KIKOO",
        "KISSY",
        "KOOTY",
        "KOOKY",
        "KNOTS",
        "LAWN",
        "LOLLY",
        "LOONIE",
        "LOOPY",
        "LUNA",
        "MARIO",
        "MILK",
        "MOONY",
        "MR.DOG",
        "MR.ODD",
        "MR.PEE",
        "NAPALM",
        "NAUGHT",
        "NED",
        "NEENER",
        "NOG",
        "NOODLE",
        "NUDES",
        "NYOOM",
        "OATS",
        "ORANGE",
        "PABLO",
        "PANDA",
        "PANKY",
        "PASTA",
        "PATCHY",
        "PEAHEN",
        "PEEPEE",
        "PEEPS",
        "PET",
        "PETE",
        "PHOTOS",
        "PICKLE",
        "PIERRE",
        "PILLOW",
        "PINKY",
        "PIPO",
        "PIZZA",
        "POGO",
        "POKEY",
        "POMPOM",
        "POOKY",
        "POOPOO",
        "POPO",
        "POPPY",
        "PUNCHY",
        "PURPLE",
        "QUINCY",
        "ROBERT",
        "ROCKY",
        "RONALD",
        "ROSES",
        "SCOOT",
        "SHAGGY",
        "SHAWTY",
        "SHAWN",
        "SHINY",
        "SHOES",
        "SHUSHU",
        "SILLY",
        "SLUG",
        "SMOG",
        "SMOOCH",
        "SNICKS",
        "SNORT",
        "SOLEIL",
        "SPANKS",
        "SPOON",
        "SQUASH",
        "SQUEAK",
        "SQUISH",
        "STAG",
        "STAN",
        "SUCKER",
        "SUNNY",
        "TEEHEE",
        "TICKLE",
        "TITTER",
        "TOASTY",
        "TOILET",
        "TOOTS",
        "TOOTSY",
        "TOP",
        "TOPPY",
        "TRAIN",
        "TULIP",
        "TUMBLE",
        "TUMMY",
        "TURD",
        "VINNY",
        "WAHWAH",
        "WASPS",
        "WEEWEE",
        "WHACKY",
        "WHIMSY",
        "WHIPPY",
        "WIGGLE",
        "WINKLE",
        "WOBBLE",
        "XAVIER",
        "YANNY",
        "YELLOW",
        "YIPPEE",
        "YOOHOO",
        "ZAZA",
        "ZANY",
        "ZOOMER",
        "ZOOMY"
    };

    private string[] menuScenesNames =
    {
        "MainMenu", "LobbyScene", "RoundScene", "ScoreBoard", "EndScene"
    };

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetComponent<PlayerInputManager>().onPlayerJoined += RegisterPlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GetComponent<PlayerInputManager>().onPlayerJoined -= RegisterPlayer;
    }


    // Called when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby) // Check if the loaded scene is the Lobby scene
        {
            // Enable player joining
            GetComponent<PlayerInputManager>().EnableJoining();
        }
        else
        {
            // Disable player joining in any other scene
            GetComponent<PlayerInputManager>().DisableJoining();
        }

        HidePlayersSpriteInMenuScenes();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Registers a new player when they join the game.
    /// - Retrieves the PlayerController from the PlayerInput.
    /// - Adds the player to the players list and persists their GameObject across scenes.
    /// - Randomizes the player's customization.
    /// - If in the LobbyScene, refreshes the lobby UI to reflect the new player.
    /// </summary>
    public void RegisterPlayer(PlayerInput input)
    {

        if (input.GetDevice<Mouse>() != null)
        {
            Destroy(input.gameObject);
        }
        var controller = input.GetComponent<PlayerController>();

        //controller.SetVisible(false);

        if (controller == null)
        {
            Debug.LogError("PlayerInput missing PlayerController!");
            return;
        }
        string name = names[Random.Range(0, names.Count - 1)];
        names.Remove(name);
        int index = getIndex();
        players.Add(controller);
        DontDestroyOnLoad(controller.gameObject);

        controller.InitializePlayer(index, name);
        controller.GetComponent<PlayerCustomization>().Randomize();

        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby
            || GameManager.Instance.CurrentGameState == GameManager.GameStates.MainMenu)
            controller.Setup(input);
        
        HidePlayersSpriteInMenuScenes();

        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.Refresh();
            }
            else
            {
                Debug.LogWarning("LobbyUIManager not found in the scene. Cannot refresh lobby UI.");
            }
        }
    }

    private int getIndex()
    {
        int lowestIndex = 0;

        for(int i = 0; i < players.Count; ++i)
        {
            if (players[i].PlayerIndex == lowestIndex)
                lowestIndex++;
            else
                break;
        }

        return lowestIndex;
    }


    public void RemovePlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
            Destroy(player.gameObject);
            LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            if (lobbyUIManager != null)
            {
                lobbyUIManager.Refresh();

                foreach (PlayerController p in players)
                {
                    p.setNumbIcon();
                }
            }
            else
            {
                Debug.LogWarning("LobbyUIManager not found in the scene. Cannot refresh lobby UI.");
            }
        }
        else
        {
            Debug.LogWarning("Player not found in the player list.");
        }
    }

    public void ClearPlayers()
    {
        foreach (var player in players.ToList()) //copy list
        {
            Destroy(player.gameObject);
        }

        players.Clear();
    }

    public void SwitchActionMaps()
    {
        foreach (var player in players)
        {
            player.switchActionMap();
        }
    }

    public void unFreezePlayers()
    {
        foreach (var player in players)
        {
            player.activateInputs();
        }
    }

    public void UpdateScoreOrder()
    {
        if (players.Count <= 1) return; // No need to sort if there's only one player

        players.Sort((a, b) =>
        {
            // First compare scores (descending)
            int scoreComparison = b.Score.CompareTo(a.Score);

            if (scoreComparison != 0)
                return scoreComparison;

            // If scores are equal, sort by PlayerIndex (ascending)
            return a.PlayerIndex.CompareTo(b.PlayerIndex);
        });
    }

    public int GetPlayerCount() => players.Count;
    
    private void HidePlayersSpriteInMenuScenes()
    {
        if (players.Count <= 0) return;
        // Disable the player sprite renderer to prevent it from showing in the lobby and main menu scenes
        foreach (string sceneName in menuScenesNames)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                foreach (PlayerController playerController in players)
                {
                    playerController.SetVisible(false);
                    playerController.resetDeath(); //just in case of player death at the exact moment when the scene changes
                }
                return;
            }
        }
    }

    public void checkGameStateAndPlayers() //checks if the state is race
    {
        if(GameManager.Instance.CurrentGameState == GameManager.GameStates.RaceLevel)
        {
            foreach (PlayerController player in players)
            {
                player.checkAngry();
            }
        }
        else if(GameManager.Instance.CurrentGameState == GameManager.GameStates.CoinLevel
            || GameManager.Instance.CurrentGameState == GameManager.GameStates.SurviveLevel)
        {
            foreach (PlayerController player in players)
            {
                player.cheer();
            }
        }

    }
}


