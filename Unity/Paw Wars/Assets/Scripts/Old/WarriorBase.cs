//using UnityEngine;
//using System.Collections;

////all land warriors
//public class WarriorBase : MonoBehaviour
//{
//    public float health;
//    public float attack;
//    public float defense;
//    public float mass;
//    public float speed;
//    public int startTime;
//    public int endTime;
//    public int count;
//    public bool isAerial;
//    public bool hasOwnScript;

//    private int direction;
//    private float endX;
//    public float positionX;
//    private bool collided;
//    private bool firstCollided;
//    private bool launched; //track if launched, cannot launch while in air
//    private bool hastened; //track if hastened, cannot hasten while running

//    public WarriorBase()
//    {
//    }

//    public void SetProperties(Properties properties)
//    {
//        health = properties.health;
//        attack = properties.attack;
//        defense = properties.defense;
//        mass = properties.mass;
//        speed = properties.speed;
//        startTime = properties.startTime;
//        endTime = properties.endTime;
//        count = properties.count;
//        isAerial = properties.isAerial;
//        hasOwnScript = properties.hasOwnScript;
//    }

//    void Start()
//    {
//        if (!hasOwnScript)
//        {
//            var renderer = (SpriteRenderer)gameObject.GetComponent("SpriteRenderer");
//            float halfWidth = renderer.sprite.bounds.size.x / 2;
//            float halfHeight = renderer.sprite.bounds.size.y / 2;
//            if (tag == "Ally")
//            {
//                transform.position = new Vector2(PawWarsRef.PlayerSpawnPoint - halfWidth, PawWarsRef.GroundSpawnHeight + halfHeight);
//                direction = 1;
//                endX = PawWarsRef.EnemySpawnPoint + halfWidth;
//            }
//            else
//            {
//                GetComponent<SpriteRenderer>().flipX = true;
//                transform.position = new Vector2(PawWarsRef.EnemySpawnPoint + halfWidth, PawWarsRef.GroundSpawnHeight + halfHeight);
//                direction = -1;
//                endX = PawWarsRef.PlayerSpawnPoint - halfWidth;
//            }
//            collided = false;
//            GetComponent<BoxCollider2D>().size = renderer.sprite.bounds.size;
//            GetComponent<Rigidbody2D>().velocity = new Vector2(direction, 0);
//        }
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        if (!hasOwnScript)
//        {
//            // destroy object when out of bounds
//            //transform.position = new Vector3(transform.position.x + (direction) * 0.1f, transform.position.y, transform.position.z);
//            if (Mathf.Abs(transform.position.x - endX) <= 0.1f)
//            {
//                Destroy(gameObject);
//            }
//            if (transform.position.y <= PawWarsRef.OffScreenBelowHeight)
//            {
//                print("below ground, destroying cat");
//                Destroy(gameObject);
//            }


//            //if we just had a collision and the object hasn't rebounded back to its target speed, add force to it to push it forward
//            if (firstCollided)
//            {
//                GetComponent<Rigidbody2D>().velocity = new Vector2(-direction, 0);
//                //GetComponent<Rigidbody2D>().AddForce(transform.forward * -20f, ForceMode2D.Impulse);
//                firstCollided = false;
//                collided = true;
//            }
//            else if (collided && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x - direction) > 0.001f)
//            {
//                // randomize this within a range
//                print("adding force to bring back to speed: x: " + GetComponent<Rigidbody2D>().velocity.x + "vs: " + direction);
//                GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, 0), ForceMode2D.Force);
//            }
//            // once we've reached target speed, let it gooooooo
//            else if (collided)
//            {
//                print("target speed reached, recovery complete");
//                collided = false;
//            }
//            // no collisions? keep movin'
//            else
//            {
//                //GetComponent<Rigidbody2D>().velocity = new Vector2(direction, 0);
//            }
//        }

//        GetComponent<Animator>().SetBool("Attacking", collided);
//        GetComponent<Animator>().SetBool("Launched", launched);
//        GetComponent<Animator>().SetBool("Hastened", hastened);
//    }

//    void OnCollisionEnter2D(Collision2D coll)
//    {
//        if (!hasOwnScript)
//        {
//            if (tag == "Ally")
//            {
//                if (coll.gameObject.tag == "Enemy")
//                {
//                    //print(tag + " collided and giving " + attack + "damage");
//                    //coll.gameObject.SendMessage("EnemyCollided", attack);
//                    //gameObject.SendMessage("TakeDamage", coll.gameObject.GetComponent<WarriorBase>().attack);
//                    ////randomize this within another range for different knockback amounts
//                    GetComponent<Rigidbody2D>().AddForce(transform.forward * -20f, ForceMode2D.Impulse);
//                    firstCollided = true;
//                    collided = false;
//                    hastened = false;
//                }
//            }
//            if (coll.gameObject.tag == "Ground" && launched)
//            {
//                launched = false;
//            }
//        }
//    }

//    public void EnemyCollided(float attack)
//    {
//        if (!hasOwnScript)
//        {
//            //GetComponent<Rigidbody2D>().AddForce(transform.forward * -20f, ForceMode2D.Impulse);
//            firstCollided = true;
//            collided = false;
//            TakeDamage(attack);
//        }
//    }
//    public void Launch()
//    {
//        // Only launch if they are not in the air
//        if (!launched)
//        {
//            print("cat launched");
//            GetComponent<Rigidbody2D>().velocity = new Vector2(4f * direction, 5f);
//            //GetComponent<Rigidbody2D>().AddForce(transform.up * 250f + transform.right * 100f * direction, ForceMode2D.Force);
//            //transform.rotation = new Quaternion()
//            launched = true;
//            GetComponent<Animator>().SetBool("Launched", launched);
//        }
//    }

//    public void Hasten()
//    {
//        // Only launch if they are not in the air
//        if (!hastened)
//        {
//            print("cat hastened");
//            GetComponent<Rigidbody2D>().velocity = new Vector2(3f * direction, 0f);
//            //GetComponent<Rigidbody2D>().AddForce(transform.up * 250f + transform.right * 100f * direction, ForceMode2D.Force);
//            hastened = true;
//            GetComponent<Animator>().SetBool("Hastened", hastened);
//        }
//    }

//    public void TakeDamage(float amount)
//    {
//        if (!hasOwnScript)
//        {
//            // Decrement the player's health by amount.
//            health -= amount;
//            print(tag + " took damage to " + health + " health");
//            if (health <= 0f)
//            {
//                //goodbye :'(
//                BeginFallingAnimation();
//            }
//            else
//            {
//                //play damage animation + sound
//            }
//        }
//    }

//    // goodbye cat :'(
//    public void BeginFallingAnimation()
//    {
//        GetComponent<Animator>().SetTrigger("Fainted");
//        if (!hasOwnScript)
//        {
//            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
//            GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * -36f, 36f), ForceMode2D.Force);
//            GetComponent<Rigidbody2D>().AddTorque(direction, ForceMode2D.Impulse);
//            GetComponent<Collider2D>().enabled = false;
//            tag = "NotActive";
//        }
//    }
//}
