using UnityEngine;
using System.Collections;

public enum CatState
{
    Walk = 1,
    Run = 2,
    Attack0 = 3,
    Attack1 = 4,
    Attack2 = 5,
    Attack3 = 6,
    Attack4 = 7,
    Attack5 = 8,
    Attack6 = 9,
    Launch = 10,
    AirAttack0 = 11,
    AirAttack1 = 12,
    Special0 = 13,
    Special1 = 14,
    Hurt = 15,
    Faint = 16,
    GetUp = 17,

    Idle = 18, //for level complete
    Win = 19, //winning cheer animation
    Lose = 20, //collapsing lose animation
}

//all land warriors
public class CharacterBase : GameObjectBase
{
    private Animator animator;
    private SpriteRenderer renderer;
    private Rigidbody2D rigidBody;

    public float health;
    public float attack;
    public float defense;
    public float mass;
    public float speed;
    public int startTime;
    public int endTime;
    public int count;
    public bool isAerial;

    private int direction;
    private float endX;
    public float positionX;

    private CatState prevState;
    private CatState state = CatState.Walk;
    protected virtual CatState State
    {
        get { return state; }
        set
        {
            prevState = state;
            state = value;
            GetComponent<Animator>().SetInteger("State", (int)state);
            if (state == CatState.Walk)
                Walk();
            else if (state == CatState.Run)
                Run();
            else if (state == CatState.Attack0)
                Attack0();
            else if (state == CatState.Attack1)
                Attack1();
            else if (state == CatState.Attack2)
                Attack2();
            else if (state == CatState.Attack3)
                Attack3();
            else if (state == CatState.Attack4)
                Attack4();
            else if (state == CatState.Attack5)
                Attack5();
            else if (state == CatState.Attack6)
                Attack6();
            else if (state == CatState.Launch)
                Launch();
            else if (state == CatState.AirAttack0)
                AirAttack0();
            else if (state == CatState.AirAttack1)
                AirAttack1();
            else if (state == CatState.Special0)
                Special0();
            else if (state == CatState.Special1)
                Special1();
            //else if (state == CatState.Hurt) // the Hurt function is already called in TakeDamage()
                //Hurt();
            else if (state == CatState.Faint)
                Faint();
            else if (state == CatState.GetUp)
                GetUp();
            else if (state == CatState.Idle)
                Idle();
            else if (state == CatState.Win)
                Win();
            else if (state == CatState.Lose)
                Lose();
        }
    }

    public CharacterBase()
    {
    }

    public void SetProperties(Properties properties)
    {
        health = properties.health;
        attack = properties.attack;
        defense = properties.defense;
        mass = properties.mass;
        speed = properties.speed;
        startTime = properties.startTime;
        endTime = properties.endTime;
        count = properties.count;
        isAerial = properties.isAerial;
    }

    private GameObject BossObject;

    public virtual void Start()
    {
        BossObject = GameObject.Find("EnemyBoss");

        animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();

        float halfWidth = renderer.sprite.bounds.size.x / 2;
        float halfHeight = renderer.sprite.bounds.size.y / 2;
        if (tag == "Ally")
        {
            transform.position = new Vector2(PawWarsRef.PlayerSpawnPoint - halfWidth, PawWarsRef.GroundSpawnHeight + halfHeight);
            direction = 1;
            endX = PawWarsRef.EnemySpawnPoint + halfWidth;
        }
        else
        {
            renderer.flipX = true;
            transform.position = new Vector2(PawWarsRef.EnemySpawnPoint + halfWidth, PawWarsRef.GroundSpawnHeight + halfHeight);
            direction = -1;
            endX = PawWarsRef.PlayerSpawnPoint - halfWidth;
        }
        GetComponent<BoxCollider2D>().size = renderer.sprite.bounds.size;

        SetState(CatState.Walk);
        // explicitly call walk to initialize
        Walk();
    }
    // Update is called once per frame
    public override void RunUpdate()
    {
        // destroy object when out of bounds
        //transform.position = new Vector3(transform.position.x + (direction) * 0.1f, transform.position.y, transform.position.z);
        if (Mathf.Abs(transform.position.x - endX) <= 0.1f)
        {
            print("destroying cat: outside horizontal bounds");
            Destroy(gameObject);
        }
        if (transform.position.y <= PawWarsRef.OffScreenBelowHeight)
        {
            print("destroying cat: outside vertical bounds");
            Destroy(gameObject);
        }

        //// Animation Transitions, keep them consistent with each other (after transitions)
        //if (!animator.IsInTransition(0))
        //{
        //    //animator.GetNextAnimatorStateInfo(0).shortNameHash == Animator.StringToHash()
        //    var currentState = animator.GetCurrentAnimatorStateInfo(0);
        //    foreach (CatState catState in CatState.GetValues(typeof(CatState)))
        //    {
        //        if (currentState.IsName("Cat-" + catState.ToString()) && State != catState)
        //        {
        //            SetState(catState);
        //        }
        //    }
        //}

        if (state == CatState.Launch)
        {
            SetLaunchAngle();
        }

        #region Bounceback collision
        ////if we just had a collision and the object hasn't rebounded back to its target speed, add force to it to push it forward
        //if (firstCollided)
        //{
        //    GetComponent<Rigidbody2D>().velocity = new Vector2(-direction, 0);
        //    //GetComponent<Rigidbody2D>().AddForce(transform.forward * -20f, ForceMode2D.Impulse);
        //    firstCollided = false;
        //    collided = true;
        //}
        //else if (collided && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x - direction) > 0.001f)
        //{
        //    // randomize this within a range
        //    print("adding force to bring back to speed: x: " + GetComponent<Rigidbody2D>().velocity.x + "vs: " + direction);
        //    GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, 0), ForceMode2D.Force);
        //}
        //// once we've reached target speed, let it gooooooo
        //else if (collided)
        //{
        //    print("target speed reached, recovery complete");
        //    collided = false;
        //}
        //// no collisions? keep movin'
        //else
        //{
        //    //GetComponent<Rigidbody2D>().velocity = new Vector2(direction, 0);
        //}
        #endregion
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (tag == "Ally")
        {
            if (coll.gameObject.tag == "Enemy")
            {
                //print(tag + " collided and giving " + attack + "damage");
                //coll.gameObject.SendMessage("EnemyCollided", attack);
                //gameObject.SendMessage("TakeDamage", coll.gameObject.GetComponent<WarriorBase>().attack);
                ////randomize this within another range for different knockback amounts
                GetComponent<Rigidbody2D>().AddForce(transform.forward * -20f, ForceMode2D.Impulse);
                SetState(CatState.Attack0);
            }
        }
        if (coll.gameObject.tag == "Ground" && state == CatState.Launch)
        {
            SetState(CatState.AirAttack0);
        }
        if (coll.gameObject.tag == "Ground" && state == CatState.AirAttack0)
        {
            SetState(CatState.Run);
        }
    }

    // logic to determine which state to move to
    // it's also safeguarding against unwanted state transitions
    public virtual void SetState(CatState newState)
    {
        if (newState == State)
        {
            return;
        }
        switch (newState)
        {
            case CatState.Walk:
                break;
            case CatState.Run:
                if (!(state == CatState.Walk || state == CatState.AirAttack0))
                    return;
                break;
            case CatState.Launch:
                if (!(state == CatState.Walk || state == CatState.Run))
                    return;
                break;
            case CatState.Attack0: //generally means to attack
                if (state == CatState.Walk || state == CatState.Run)
                    break;
                else if (state == CatState.Launch)
                {
                    ResetRotation();
                    newState = CatState.AirAttack0;
                }
                else
                    return;
                break;
            case CatState.AirAttack0:
                if (!(state == CatState.Launch))
                    return;
                else
                    ResetRotation();
                break;
            case CatState.Special0:
                break;
            case CatState.Hurt:
                ResetRotation();
                break;
            case CatState.GetUp:
                if (state != CatState.Hurt)
                    return;
                break;
            case CatState.Faint:
                ResetRotation();
                break;
        }
        if (newState != State)
        {
            State = newState;
        }
    }

    protected void ResetRotation()
    {
        if (transform.rotation.w != 0)
        {
            transform.rotation = new Quaternion();
        }
    }

    protected virtual void Walk()
    {
        rigidBody.velocity = new Vector2(direction, 0);
    }

    protected virtual void Run()
    {
        print("cat hastened");
        rigidBody.velocity = new Vector2(3f * direction, 0f);
        //GetComponent<Rigidbody2D>().AddForce(transform.up * 250f + transform.right * 100f * direction, ForceMode2D.Force);
    }

    protected virtual void Attack0()
    {

    }

    protected virtual void Attack1()
    {

    }

    protected virtual void Attack2()
    {

    }

    protected virtual void Attack3()
    {

    }

    protected virtual void Attack4()
    {

    }

    protected virtual void Attack5()
    {

    }

    protected virtual void Attack6()
    {

    }

    float launchXRoot;
    float launchXRange;
    protected virtual void Launch()
    {
        // calculate force needed to launch cat to hit upper left hand corner of boss at 3/4 of its trajectory
        var d = BossObject.transform.position.x - transform.position.x;
        print("boss distance is " + d);
        var vx = 4f * direction;
        var vy = vx * d * (4 / 3) / 8;
        //rigidBody.velocity = new Vector2(4f * direction, 5f);
        rigidBody.velocity = new Vector2(vx, vy);

        launchXRoot = transform.position.x;
        launchXRange = (d * 4 / 3);

        //GetComponent<Rigidbody2D>().AddForce(transform.up * 250f + transform.right * 100f * direction, ForceMode2D.Force);
        //transform.rotation = new Quaternion()
    }

    private void SetLaunchAngle()
    {
        // set the angle of the cat to match the velocity ratio
        //double angle = ((rigidBody.velocity.x / rigidBody.velocity.y) - 0.5) / 2;
        double angle = (rigidBody.position.x - launchXRoot) / launchXRange * 14 - 10;
        transform.rotation = new Quaternion(0, 0, 1, (float)angle);
        print("Angle: velocity x: " + rigidBody.velocity.x + " y: " + rigidBody.velocity.y + " angle: " + angle);
    }

    protected virtual void AirAttack0()
    {
        print("cat air attacking - bouncing backward");
        rigidBody.velocity = new Vector2(-1f * direction, 1f);
    }

    protected virtual void AirAttack1()
    {

    }

    protected virtual void Special0()
    {

    }

    protected virtual void Special1()
    {

    }

    protected virtual void Hurt(int damageDealt)
    {
        // Decrement the player's health by amount.
        health -= damageDealt;
        print(tag + " took " + damageDealt + " damage to " + health + " health");
        if (health <= 0f)
        {
            SetState(CatState.Faint);
        }
        else
        {
            //knockback
            rigidBody.velocity = new Vector2(-direction, 0);
        }
    }

    public void TakeDamage(float amount)
    {
        Hurt((int)amount);
        SetState(CatState.Hurt);
    }

    protected virtual void GetUp()
    {
        rigidBody.velocity = new Vector2();
    }

    protected virtual void Faint()
    {
        rigidBody.constraints = RigidbodyConstraints2D.None;
        rigidBody.AddForce(new Vector2(direction * -36f, 36f), ForceMode2D.Force);
        rigidBody.AddTorque(direction, ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;
        tag = "NotActive";
    }


    // these need to be set by the game, when a level is determined to have ended.
    protected virtual void Idle()
    {

    }

    protected virtual void Win()
    {

    }

    protected virtual void Lose()
    {

    }
}
