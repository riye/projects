using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Reference values (temporary, hacky)
public static class PawWarsRef
{
    public static bool IsGameRunning = true;

    public static Dictionary<WarriorType, Object> BasicCatPrefabs;
    public static Dictionary<WarriorType, Properties> PropertiesList;


    public static float GroundSpawnHeight = -4.18f;
    public static float AerialSpawnHeight = 2.87f;

    public static float PlayerSpawnPoint = -9.7f;
    public static float EnemySpawnPoint = 9.7f;

    public static float OffScreenBelowHeight = -6f;
    
    public static List<Warrior> GetRandomCatSpawnList()
    {
        // Create randomized spawn times of the different cat types
        int a = (int)Random.Range(0f, 5f);
        var playerWarriors = new List<Warrior>();
        for (int k = a; k < 100; k = k + 5)
        {
            playerWarriors.Add(new Warrior(k, WarriorType.BasicWarrior));
        }
        a = (int)Random.Range(0f, 5f);
        //for (int j = a; j < 100; j = j + 20)
        //{
        //    playerWarriors.Add(new Warrior(j, WarriorType.DiveBomber));
        //}
        //a = (int)Random.Range(0f, 5f);
        //for (int j = a; j < 100; j = j + 10)
        //{
        //    playerWarriors.Add(new Warrior(j, WarriorType.HeavyWarrior));
        //}
        playerWarriors = playerWarriors.OrderBy(warrior => warrior.spawnTime).ToList();
        return playerWarriors;
    }
}

public class Warrior
{
    public int spawnTime;
    public WarriorType type;

    public Warrior()
    {
        spawnTime = 0;
        type = WarriorType.BasicWarrior;
    }

    public Warrior(int spawn, WarriorType warriorType)
    {
        spawnTime = spawn;
        type = warriorType;
    }
}

public enum WarriorType
{
    BasicWarrior = 0,
    HeavyWarrior = 1,
    DiveBomber = 2
}

//properties set of each cat
public class Properties
{
    public float health;
    public float attack;
    public float defense;
    public float mass;
    public float speed;
    public string textureName;
    public int startTime;
    public int endTime;
    public int count;
    public bool isAerial;
    public bool hasOwnScript;

    public Properties()
    {
        health = 0f;
        attack = 0f;
        defense = 0f;
        mass = 0f;
        speed = 0f;
        textureName = "";
        startTime = 0;
        endTime = 0;
        count = 0;
        isAerial = false;
        hasOwnScript = false;
    }

    public Properties(int health, int attack, int defense, int mass, int speed, string texture, int start, int end, int count, bool aerial, bool ownScript)
    {
        this.health = health;
        this.attack = attack;
        this.defense = defense;
        this.mass = mass;
        this.speed = speed;
        this.textureName = texture;
        this.startTime = start;
        this.endTime = end;
        this.count = count;
        this.isAerial = aerial;
        this.hasOwnScript = ownScript;
    }
}

