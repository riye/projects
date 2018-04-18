//using UnityEngine;
//using System.Collections;

//public class CatWarrior : MonoBehaviour {

//    public float positionX;
//    private int direction;
//    private float endX;
//    private bool collided;

//    private float health;
//    private float mass;
//    private float startY;
//    private float normalSpeed;
//    private float atkDmg;

//	// Use this for initialization
//	void Start () {
//        //initialize certain fields based on the cat type
//        // health
//        health = 10;
//        // mass
//        // y start position? since some are in the air
//        // velocity? some force scaling?
//        // attack damage amount
//        atkDmg = GetComponent<Rigidbody2D>().mass * 10;

//	    if (tag == "Ally")
//        {
//            transform.position = new Vector2(PawWarsRef.PlayerSpawnPoint, PawWarsRef.GroundSpawnHeight);
//            direction = 1;
//            endX = PawWarsRef.EnemySpawnPoint;
//        }
//        else
//        {
//            GetComponent<SpriteRenderer>().flipX = true;
//            transform.position = new Vector2(PawWarsRef.EnemySpawnPoint, PawWarsRef.GroundSpawnHeight);
//            direction = -1;
//            endX = PawWarsRef.PlayerSpawnPoint;
//        }
//        collided = false;
//        GetComponent<Rigidbody2D>().velocity = new Vector2(direction, 0);

//    }
//	// Update is called once per frame
//	void Update () {
//        //transform.position = new Vector3(transform.position.x + (direction) * 0.1f, transform.position.y, transform.position.z);
//        if (Mathf.Abs(transform.position.x - endX) <= 0.1f)
//        {
//            Destroy(gameObject);
//        }
//        if (transform.position.y <= -6f)
//        {
//            print("below ground, destroying aerial cat");
//            Destroy(gameObject);
//        }
//        //if we just had a collision and the object hasn't rebounded back to its target speed, add force to it to push it forward
//        if (collided && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x - direction) > 0.001f)
//        {
//            // randomize this within a range
//            GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, 0), ForceMode2D.Force);
//        }
//        // once we've reached target speed, let it gooooooo
//        else if (collided)
//        {
//            collided = !collided;
//        }
//	}

//    void OnCollisionEnter2D(Collision2D coll)
//    {
//        print("collided");
//        if ((coll.gameObject.tag == "Ally" && gameObject.tag == "Enemy") || 
//                (coll.gameObject.tag == "Enemy" && gameObject.tag == "Ally"))
//        {
//            coll.gameObject.SendMessage("TakeDamage", atkDmg);
//            gameObject.SendMessage("TakeDamage", coll.gameObject.GetComponent<CatWarrior>().atkDmg);
//            //coll.gameObject.GetComponent<CatWarrior>().TakeDamage(atkDmg);
//            //randomize this within another range for different knockback amounts
//            GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * -30f, 0), ForceMode2D.Force);
//        }
//        collided = true;

//    }

//    public void TakeDamage(float amount)
//    {
//        print("took damage");
//        // Decrement the player's health by amount.
//        health -= amount;

//        if (health <= 0f)
//        {
//            //goodbye :'(
//            BeginFallingAnimation();
//        }
//        else
//        {
//            //play damage animation + sound
//        }
//    }

//    public void BeginFallingAnimation()
//    {
//        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
//        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * -30f, 60f), ForceMode2D.Force);
//        GetComponent<Rigidbody2D>().AddTorque(direction * 5f, ForceMode2D.Force);
//        GetComponent<Collider2D>().enabled = false;
//        tag = "NotActive";
//    }
//}
