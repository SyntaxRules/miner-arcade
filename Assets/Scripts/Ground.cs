using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ground : MonoBehaviour
{
    public float health = 3f;
    public float probablity = 1f;
    public char id = GroundManager.DESTROYED;
    public int foundBelowLayer = 0;
    public int foundAboveLayer = int.MaxValue;

    public bool causeDmg(float dmg)
    {
        //TODO: Change sprite based on amount of damage taken.

        health -= dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            //TODO: Destroy animation
            GroundManager.destroyGroundTile(this.transform);
            return true;
  
        }
        return false;
    }
}