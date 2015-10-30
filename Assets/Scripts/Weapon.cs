using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public float fireRate = 0.5f;
    public float attackDamage = 1f;
    private float nextFire = 0.0f;

    // The Weapon causes the Dmg canvas to be applied to other ground objects
    // This way the ground objects don't ahve to store extra  dmg display infomraiton and they load faster
    public GameObject dmgCanvas;
    public float dmgShowDurration = 0.5f;

    public bool canFire()
    {
        return Time.time > nextFire;
    }

    public void fire()
    {
        nextFire = Time.time + fireRate;
        this.GetComponent<AudioSource>().Play();
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        handleCollision(coll);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        handleCollision(coll);
    }

    void handleCollision(Collision2D coll)
    {
        bool mineStuff = false;
#if MOBILE_INPUT
        mineStuff = CrossPlatformInputManager.GetButton("Mine");
#else
        mineStuff = Input.GetButton("Fire/Mine");
#endif
        if (coll.gameObject.tag == "Ground" && mineStuff)
        {
            var ground = coll.gameObject.GetComponent<Ground>();
            //set next moment that the weapon can hit
            if (this.canFire())
            {
                this.fire();

                //player is attacking
                ground.causeDmg(this.attackDamage);
                showDamage(this.attackDamage, coll.gameObject);

            }

        }
    }

    void showDamage(float attackDmg, GameObject hitItem)
    {
        string displayString;
        //format the dmg,
        if (attackDmg < 1f)
        {
            displayString = string.Format("{0:.##}", attackDmg);
        }
        else //if (attackDmg < 100)
        {
            displayString = attackDmg.ToString();
        }

        GameObject temp = Instantiate(dmgCanvas) as GameObject;
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        tempRect.position = hitItem.transform.position;
        //temp.transform.SetParent(hitItem.transform);
        //Debug.Log(hitItem.transform.position);
        //temp.transform.Translate(hitItem.transform.position);//hitItem.transform.position;//Camera.main.WorldToScreenPoint(hitItem.transform.position);// new Vector3(0f, 0f, 0f);
        //temp.transform.position = hitItem.transform.position;

        temp.GetComponentInChildren<UnityEngine.UI.Text>().text = '-' + displayString;
        temp.GetComponent<Animator>().SetTrigger("Hit");

        Destroy(temp.gameObject, 1f);
    }
}
