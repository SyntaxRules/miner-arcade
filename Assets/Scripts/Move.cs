using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Move : MonoBehaviour {
	public float speed = 1.0f;
    public GameObject weapon; //leads the player.
    public float weaponOffset = .16f;
    public float jumpForce = 4.0f;
    public static float weaponDistance = .25f;
    private bool isJumping = false;
    private bool isFalling = false;

	// Use this for initialization
	void Start () {
        //Physics.IgnoreCollision(this.GetComponent<Collider>(), weapon.GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(Screen.dpi);
        //Debug.Log(Screen.width);
        //Debug.Log(Camera.current.aspect);
        //Debug.Log(Camera.current.orthographicSize);
        //Debug.Log(Camera.current.pixelWidth);
        //Debug.Log(Camera.current.WorldToScreenPoint(new Vector3(0, 0, 0)));

        //Bounding movements are based in the level size

        MoveCharacter();
    }

    protected void MoveCharacter()
    {
        //a-d left and right
        //space to mine
        //w to jump
        //mouse to move mineing self
        Vector3 move;
        bool jump = false;
        float mouseAngle;
#if MOBILE_INPUT
        move = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0, 0);
        jump = CrossPlatformInputManager.GetButton("Jump");
        Debug.Log(CrossPlatformInputManager.GetAxis("Horizontal") + ", " + CrossPlatformInputManager.GetAxis("Vertical"));
        mouseAngle = Mathf.Atan2(CrossPlatformInputManager.GetAxis("Vertical"), CrossPlatformInputManager.GetAxis("Horizontal"));
#else
        move = new Vector3(Input.GetAxis("Horizontal"), 0/*Input.GetAxis("Vertical")*/, 0);
        jump = Input.GetButton("Jump");

        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(this.gameObject.transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        mouseAngle = Mathf.Atan2(offset.y, offset.x);// *Mathf.Rad2Deg;

#endif
        //if (transform.position.x + move.x >= 0 && 
        //    transform.position.x + move.x <= GroundManager.instance.groundWidth )
        {
            //only move if it will be inside our world box
            transform.position += move * speed * Time.deltaTime;
        }
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0f, transform.position.y);
        }
        else if (transform.position.x > GroundManager.instance.groundWidth)
        {
            transform.position = new Vector3(GroundManager.instance.groundWidth, transform.position.y);
        }


        if (!float.IsNaN(mouseAngle))
            weapon.transform.localPosition = new Vector3(Mathf.Cos(mouseAngle) * weaponDistance, Mathf.Sin(mouseAngle) * weaponDistance, 0);

        if (jump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (this.GetComponent<Rigidbody2D>().velocity.y == 0 && !isJumping) {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
        //Debug.Log(this.GetComponent<Rigidbody2D>().velocity.y);
        //transform.position += Vector3.up * jumpForce * Time.deltaTime;
    }
}
