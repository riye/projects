using UnityEngine;
using System.Collections;

public enum BossState
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

    Idle = 18,
    Entrance = 19,
    None = 20,
}

//all land warriors
public class BossBase : GameObjectBase
{
    protected Animator animator;
    protected SpriteRenderer renderer;
    protected Rigidbody2D rigidBody;

    public float health;
    public float attack;
    public float defense;
    public float mass;
    public float speed;
    public int startTime;
    public int endTime;
    public int count;
    public bool isAerial;

    protected int direction;
    protected float endX;
    public float positionX;

    protected float damageDealt; //how much damage the player takes

    private BossState prevState;
    private BossState state = BossState.None;
    protected virtual BossState State
    {
        get { return state; }
        set
        {
            prevState = state;
            state = value;
            GetComponent<Animator>().SetInteger("State", (int)state);
            if (state == BossState.Walk)
                Walk();
            else if (state == BossState.Run)
                Run();
            else if (state == BossState.Attack0)
                Attack0();
            else if (state == BossState.Attack1)
                Attack1();
            else if (state == BossState.Attack2)
                Attack2();
            else if (state == BossState.Attack3)
                Attack3();
            else if (state == BossState.Attack4)
                Attack4();
            else if (state == BossState.Attack5)
                Attack5();
            else if (state == BossState.Attack6)
                Attack6();
            else if (state == BossState.Launch)
                Launch();
            else if (state == BossState.AirAttack0)
                AirAttack0();
            else if (state == BossState.AirAttack1)
                AirAttack1();
            else if (state == BossState.Special0)
                Special0();
            else if (state == BossState.Special1)
                Special1();
            else if (state == BossState.Hurt)
                Hurt();
            else if (state == BossState.Faint)
                Faint();
            else if (state == BossState.GetUp)
                GetUp();
            else if (state == BossState.Idle)
                Idle();
            else if (state == BossState.None)
                None();
            else if (state == BossState.Entrance)
                Entrance();
        }
    }

    public BossBase()
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

    public virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();

        float halfWidth = renderer.sprite.bounds.size.x / 2;
        float halfHeight = renderer.sprite.bounds.size.y / 2;

        tag = "Enemy";
        renderer.flipX = true;
        transform.position = new Vector2(PawWarsRef.EnemySpawnPoint + halfWidth, PawWarsRef.GroundSpawnHeight + halfHeight);
        direction = -1;
        endX = PawWarsRef.PlayerSpawnPoint - halfWidth;

        GetComponent<BoxCollider2D>().size = renderer.sprite.bounds.size;

        SetState(BossState.None);
        None();
    }
    // Update is called once per frame
    public override void RunUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ally")
        {
            //print(tag + " collided and giving " + attack + "damage");
            //coll.gameObject.SendMessage("EnemyCollided", attack);
            //gameObject.SendMessage("TakeDamage", coll.gameObject.GetComponent<WarriorBase>().attack);
            ////randomize this within another range for different knockback amounts
            GetComponent<Rigidbody2D>().AddForce(transform.forward * -20f, ForceMode2D.Impulse);
            SetState(BossState.Attack0);
        }
        else if (coll.gameObject.tag == "Ground" && state == BossState.Launch)
        {
            SetState(BossState.Attack0);
        }
    }

    // begin the entrance animation of the boss when the level begins
    public virtual void Enter()
    {
        State = BossState.Entrance;
    }

    // logic to determine which state to move to
    public virtual void SetState(BossState newState)
    {
        if (newState == State)
        {
            return;
        }
        switch (newState)
        {
            case BossState.Walk:
                break;
            case BossState.Run:
                if (!(state == BossState.Walk))
                    return;
                break;
            case BossState.Launch:
                if (!(state == BossState.Walk || state == BossState.Run))
                    return;
                break;
            case BossState.Attack0: //generally means to attack
                if (state == BossState.Walk || state == BossState.Run)
                    break;
                else if (state == BossState.Launch)
                    newState = BossState.AirAttack0;
                else
                    return;
                break;
            case BossState.AirAttack0:
                if (!(state == BossState.Launch))
                    return;
                break;
            case BossState.Special0:
                break;
            case BossState.Hurt:
                break;
            case BossState.GetUp:
                if (state != BossState.Hurt)
                    return;
                break;
            case BossState.Faint:
                break;
        }
        if (newState != State)
        {
            State = newState;
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

    protected virtual void Launch()
    {
        // calculate force needed to launch cat to hit upper left hand corner of boss at 3/4 of its trajectory

        rigidBody.velocity = new Vector2(4f * direction, 5f);
        //GetComponent<Rigidbody2D>().AddForce(transform.up * 250f + transform.right * 100f * direction, ForceMode2D.Force);
        //transform.rotation = new Quaternion()
    }

    protected virtual void AirAttack0()
    {

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

    protected virtual void Hurt()
    {
        // Decrement the player's health by amount.
        health -= damageDealt;
        print(tag + " took " + damageDealt + " damage to " + health + " health");
        if (health <= 0f)
        {
            SetState(BossState.Faint);
        }
        else
        {
            //knockback
            rigidBody.velocity = new Vector2(-direction, 0);
        }
    }

    public void TakeDamage(float amount)
    {
        damageDealt = amount;
        SetState(BossState.Hurt);
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

    //idling in battle
    protected virtual void Idle()
    {

    }

    // entrance animation when level begins
    protected virtual void Entrance()
    {

    }

    //pre-entrance - no movement
    protected virtual void None()
    {

    }
}
