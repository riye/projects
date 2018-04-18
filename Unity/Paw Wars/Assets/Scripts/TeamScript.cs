using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// The game itself, including its actual cat spawn details list
// In charge of creating the cats at the right time

    // this same script is shared by the Player team and the enemy team (later: split out the logic into two different files)
public class TeamScript : MonoBehaviour
{
    public bool Player;

    private int frameIndex = 0;
    int listIndex = 0;
    int timeIndex = 0;
    PlayerMoraleGauge moraleGauge;

    List<Warrior> playerWarriors;

    // Use this for initialization
    void Start()
    {
        playerWarriors = PawWarsRef.GetRandomCatSpawnList();
        moraleGauge = GameObject.Find("PlayerGauge").GetComponent<PlayerMoraleGauge>();
    }

    // Responsible for spawning cats
    // If we need to spawn a cat at this time, spawn it
    void Update()
    {
        if (frameIndex % 100 == 1)
        {
            while (playerWarriors[listIndex].spawnTime == timeIndex && listIndex < playerWarriors.Count - 1)
            {
                // PLAYER TEAM
                if (Player)
                {
                    if (moraleGauge.gauge.value > 0)
                    {
                        var type = playerWarriors[listIndex].type;
                        //spawn the warrior type
                        var cat = Instantiate(PawWarsRef.BasicCatPrefabs[type]);
                        // should set the type of the cat within the cat game object WarriorBase script, and then make every single darn cat become a WarriorBase
                        // then in WarriorBase, define the behaviour per cat in there but they share the same 1 script !!!!!
                        var catObject = (GameObject)cat;

                        print("Initialized cat for player at (x, y) = (" + catObject.transform.position.x + ", " + catObject.transform.position.y + ")");

                        if (type == WarriorType.BasicWarrior || type == WarriorType.HeavyWarrior)
                        {
                            var component = catObject.GetComponent<CharacterBase>();
                            component.SetProperties(PawWarsRef.PropertiesList[type]);
                        }
                        catObject.tag = "Ally";
                        catObject.layer = 10; //playerteam 10 and enemyteam 9
                    }
                }

                // ENEMY TEAM
                else if (!Player)
                {
                    var type = playerWarriors[listIndex].type;
                    //spawn the warrior type
                    var cat = Instantiate(PawWarsRef.BasicCatPrefabs[type]);

                    var catObject = (GameObject)cat;

                    if (type == WarriorType.BasicWarrior || type == WarriorType.HeavyWarrior)
                    {
                        var component = catObject.GetComponent<CharacterBase>();
                        component.SetProperties(PawWarsRef.PropertiesList[type]);
                    }
                    catObject.tag = "Enemy";
                    catObject.layer = 9; //playerteam 10 and enemyteam 9
                }

                listIndex++;
            }
            timeIndex++;
        }
        frameIndex++;

    }
}
