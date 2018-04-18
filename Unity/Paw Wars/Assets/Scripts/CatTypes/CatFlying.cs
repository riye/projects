using UnityEngine;
using System.Collections;

public class CatFlying : MonoBehaviour
{

    public float positionX;
    private int direction;
    private float endX;
    private bool collided;

    private float health;
    private float mass;
    private float startY;
    private float normalSpeed;
    private float atkDmg;

    // Use this for initialization
    void Start()
    {
        health = 10;
        atkDmg = GetComponent<Rigidbody2D>().mass * 10;

        if (tag == "Ally")
        {
            GetComponent<SpriteRenderer>().flipY = false;
            direction = 1;
            endX = 9.7f;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipY = true;
            //transform.position = new Vector2(9.7f, -1.78f);
            direction = -1;
            endX = -9.57f;
        }
        collided = false;

        GetComponent<Rigidbody2D>().velocity = new Vector2(direction, 0);

        Physics2D.IgnoreLayerCollision(11, 11);

        SetTriggerBoxChildProperties();
    }

    void SetTriggerBoxChildProperties()
    {
        transform.GetChild(0).GetComponent<TriggerBox>().tag = tag;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -6f)
        {
            print("below ground, destroying aerial cat");
            Destroy(gameObject);
        }
        //if (Mathf.Abs(transform.position.x - endX) <= 0.1f)
        //{
        //    print("flying cat reached off-screen, destroying");
        //    Destroy(gameObject);
        //}
    }

    public void DropCat()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().freezeRotation = false;
        transform.parent = null;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("aerial cat collided");
        if (tag != "NotActive" && coll.gameObject.tag == "Ground" && coll.gameObject.layer != 11)
        {
            BeginFallingAnimation();
        }
    }

    public void TakeDamage(float amount)
    {
        print("took damage");
        // Decrement the player's health by amount.
        health -= amount;

        if (health <= 0f)
        {
            //goodbye :'(
            //only start this once
            BeginFallingAnimation();
        }
        else
        {
            //play damage animation + sound
        }
    }

    public void BeginFallingAnimation()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * -30f, 10f), ForceMode2D.Force);
        GetComponent<Rigidbody2D>().AddTorque(direction * 5f, ForceMode2D.Force);
        GetComponent<Collider2D>().enabled = false;
        tag = "NotActive";
        transform.GetChild(0).tag = "NotActive";
        // bounce the target out
        // unlock the rotation and y position
        //set the face, dying animation sound, etc.

        //in the Update fn, once this object is below a certain y value, then destroy it
    }
}
