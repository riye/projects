using UnityEngine;
using System.Collections;

public class BirdTransporter : MonoBehaviour
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

    private bool hasChild = true;

    // Use this for initialization
    void Start()
    {
        //initialize certain fields based on the cat type
        // health
        health = 10;
        // mass
        // y start position? since some are in the air
        // velocity? some force scaling?
        // attack damage amount
        atkDmg = GetComponent<Rigidbody2D>().mass * 10;

        if (tag == "Ally")
        {
            transform.position = new Vector2(-9.57f, 2.87f);
            direction = 1;
            endX = 9.7f;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.position = new Vector2(9.7f, 2.87f);
            direction = -1;
            endX = -9.57f;
        }
        collided = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(direction, 0);

        //transfer properties to the cat
        SetCatChildProperties();
    }

    void SetCatChildProperties()
    {
        transform.GetChild(0).GetComponent<CatFlying>().tag = tag;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x + (direction) * 0.1f, transform.position.y, transform.position.z);
        if (Mathf.Abs(transform.position.x - endX) <= 0.1f)
        {
            Destroy(gameObject);
        }
        if (Mathf.Abs(transform.position.x - endX) <= 4f && hasChild)
        {
            print("dropping cat");
            transform.GetChild(0).GetComponent<CatFlying>().DropCat();
            hasChild = false;
        }
    }

    //void OnCollisionEnter2D(Collision2D coll)
    //{
    //    print("collided");
    //    if (coll.gameObject.tag != gameObject.tag)
    //    {
    //        coll.gameObject.SendMessage("TakeDamage", atkDmg);
    //        //coll.gameObject.GetComponent<CatWarrior>().TakeDamage(atkDmg);
    //        //randomize this within another range for different knockback amounts
    //        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * -30f, 0), ForceMode2D.Force);
    //    }
    //    collided = true;
    //}

    public void TakeDamage(float amount)
    {
        print("took damage");
        // Decrement the player's health by amount.
        health -= amount;

        if (health <= 0f)
        {
            //goodbye :'(
            DropCat();
            BeginFallingAnimation();
        }
        else
        {
            //play damage animation + sound
        }
    }

    public void DropCat()
    {
        transform.GetChild(0).GetComponent<CatFlying>().DropCat();
        hasChild = false;
    }

    public void BeginFallingAnimation()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * -30f, 60f), ForceMode2D.Force);
        GetComponent<Rigidbody2D>().AddTorque(direction * 5f, ForceMode2D.Force);
        GetComponent<Collider2D>().enabled = false;
        tag = "NotActive";
    }
}
