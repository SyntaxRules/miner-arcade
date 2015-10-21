using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System; 		//Tells Random to use the Unity Engine random number generator.


public class GroundManager : MonoBehaviour {

    public static GroundManager instance = null; 

    protected Transform groundHolder;
    public GameObject[] groundTiles;

    /* Gems
     * 
     * Amethyst
     * Topaz
     * Sapphire
     * Emerald
     * Ruby
     * Diamond
     * Amber
     * Quartz
     */

    /* Ores
     * 
     * Terarria
     * 
     * Copper
     * Tin
     * Lead
     * Iron
     * Silver
     * Tungsten
     * Gold
     * Platinum
     * Demonite
     * Crimatane
     * Meteorite
     * Obsidian
     * Hellstone
     * 
     * Minecraft
     * 
     * Coal
     * Iron
     * Lapis Lazuli
     * Gold
     * Diamond
     * Redstone
     * Emerald
     * Nether Quartz
     * 
     * Motherload
     * 
     * Iron (30)
     * Bronze (60)
     * Silver (100)
     * Gold (250)
     * platinium (750)
     * Einsteninium (2,000)
     * Emerald (5,000)
     * Ruby (20,000)
     * Diamond (100,000)
     * Amazonite (500,000)
     */

    public static char DESTROYED = 'd';

    public float groundSize = 1;
    public float groundDepth;
    public float groundWidth;
    public char[,] ground;

    void Awake()
    {
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
        
        //Call the InitGame function to initialize the first level 
    }

    public static void destroyGroundTile(Transform t) 
    {
        int xPos = (int)(t.position.x / instance.groundSize);
        int yPos = (int)-(t.position.y / instance.groundSize);

        instance.ground[xPos, yPos] = DESTROYED;

    }

    public void initGround(char[,] pGround)
    {
        Debug.Log(ground);
        
        if (ground != null)
        {
            //There is already existing ground, destroy it then recreate.
            Destroy(groundHolder.gameObject);
            groundHolder = new GameObject("Ground").transform;
        }

        ground = pGround;

        initializeGroundObjects();
    }

	// Use this for initialization
    public void initGround()
    {

        //Initialize groung holder
        groundHolder = new GameObject("Ground").transform;

        createNewBoardArray();

        initializeGroundObjects();
 
        //3. Learn how to only create game objects on screen, then as the screen moves, create more.


    }

    void createNewBoardArray()
    {
        Debug.Log("Creating board");
        Debug.Log(groundTiles);

        //1. Order the ground tiles by probablility (least probability first)
        Array.Sort(groundTiles, new groundComparer());

        Debug.Log(groundTiles);

        //2. create ground array
        int xMax = (int)(groundWidth / groundSize) + 1;
        int yMax = (int)(groundDepth / groundSize);

        ground = new char[xMax, yMax];

        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                foreach (var groundTile in groundTiles)
                {
                    var gc = groundTile.GetComponent<Ground>();
                    if (y > gc.foundBelowLayer &&
                        y < gc.foundAboveLayer &&
                        Random.value <= gc.probablity)
                    {
                        ground[x, y] = gc.id;
                        break;
                    }   
                }
            }
        }
    }

    void initializeGroundObjects()
    {
        Array.Sort(groundTiles, new groundComparer());

        int xMax = (int)(groundWidth / groundSize) + 1;
        int yMax = (int)(groundDepth / groundSize);

        //2.From ground array create game objects
        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                foreach (var groundTile in groundTiles)
                {
                    var gc = groundTile.GetComponent<Ground>();
                    if (ground[x, y] == gc.id)
                    {
                        GameObject temp = Instantiate(groundTile, new Vector2(x * groundSize, -y * groundSize), Quaternion.identity) as GameObject;
                        temp.transform.SetParent(groundHolder);
                        break;
                    }
                }
            }
        }
    } 
}

public class groundComparer : IComparer
{

    // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((GameObject)x).GetComponent<Ground>().probablity.CompareTo(((GameObject)y).GetComponent<Ground>().probablity);
    }

}
