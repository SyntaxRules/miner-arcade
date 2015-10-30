using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{

    public static string SAVE_LOCATION;
    public static Vector2 PLAYER_START_LOCATION = new Vector2(7, 1);
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    private GroundManager groundManager;                       //Store a reference to our BoardManager which will set up the level.
    public GameObject playerPrefab;
    private GameObject player;

    //Awake is always called before any Start functions
    void Awake()
    {
        SAVE_LOCATION = Application.persistentDataPath + "/gameInfo.dat";
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        groundManager = GetComponent<GroundManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        groundManager.initGround();

        player = Instantiate(playerPrefab, PLAYER_START_LOCATION, Quaternion.identity) as GameObject;

        loadGame();
    }

    void saveGame()
    {
        Debug.Log("Saving game at " + SAVE_LOCATION);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(SAVE_LOCATION);

        //Gather Save data!
        SaveData sd = new SaveData();
        sd.ground = groundManager.ground;

        bf.Serialize(file, sd);
        file.Close();
    }

    bool loadGame()
    {
        Debug.Log("Loading Game");
        if (File.Exists(SAVE_LOCATION))
        {
            Debug.Log("Game Exisits At " + SAVE_LOCATION);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(SAVE_LOCATION, FileMode.Open);
            SaveData sd = (SaveData)bf.Deserialize(file);
            file.Close();

            //reload the ground
            groundManager.initGround(sd.ground);
            
            //move the player back to start position
            player.transform.position = PLAYER_START_LOCATION;

            //TODO: Notification that we your game was successfully loaded
            //TODO: Some splash screen while loading

            return true;
        }
        return false;
    }

    //Update is called every frame.
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            saveGame();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            loadGame();
        }
    }
}

[Serializable ()]
public class SaveData : ISerializable 
{
    public char[,] ground;

    public SaveData() { }
    // This constructor is called automatically by the parent class, ISerializable
    // We get to custom-implement the serialization process here
    public SaveData(SerializationInfo info, StreamingContext ctxt)
    {
        // Get the values from info and assign them to the appropriate properties. Make sure to cast each variable.
        // Do this for each var defined in the Values section above
        ground = (char[,])info.GetValue("ground", typeof(char[,]));
    }
    // Required by the ISerializable class to be properly serialized. This is called automatically
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        // Repeat this for each var defined in the Values section
        info.AddValue("ground", ground);
    }

}
