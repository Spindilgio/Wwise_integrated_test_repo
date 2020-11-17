using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CHEAT
{
    HEIGHTENED_FOCUS,
    SELECTIVE_NULLIFICATION,
    FINISHING_TOUCH,
    HOME_FIELD_ADVANTAGE,
    DESPERATE_MEASURES,
    ADVANCED_FORMULAE,
    EXPERT_FORMULAE,
    ARCANE_LOCK
}

public class GameManager : MonoBehaviour
{
    [SerializeField] float deathstoneSpeed;

    public Player p1;
    public Player p2;
    public DeathStone deathStone;
    public static CHEAT[] cheat_list = new CHEAT[8] { CHEAT.HEIGHTENED_FOCUS,
        CHEAT.SELECTIVE_NULLIFICATION,
        CHEAT.FINISHING_TOUCH,
        CHEAT.HOME_FIELD_ADVANTAGE,
        CHEAT.DESPERATE_MEASURES,
        CHEAT.ADVANCED_FORMULAE,
        CHEAT.EXPERT_FORMULAE,
        CHEAT.ARCANE_LOCK };

    private float STONE_GOAL;

    // Start is called before the first frame update
    void Start()
    {
        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        STONE_GOAL = width / 2;
        p1.deathStone = deathStone;
        p2.deathStone = deathStone;
        p1.enemy = p2;
        p2.enemy = p1;
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if a player has won
        //Player 1 win
        if (deathStone.transform.position.x > STONE_GOAL)
        {
            SceneManager.LoadScene("Player1Win");
        }
        //Player 2 win
        if (deathStone.transform.position.x < -STONE_GOAL)
        {
            SceneManager.LoadScene("Player2Win");
        }
    }
}
