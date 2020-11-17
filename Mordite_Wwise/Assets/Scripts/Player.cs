using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject completion_particles;

    [Header("Display Data")]
    [SerializeField] SpriteRenderer input_display;
    [SerializeField] InputDisplay input_data;
    [SerializeField] Sprite[] input_sprites;
    [SerializeField] KeyCode[] possible_keys;

    [Header("Cheat Data")]
    [SerializeField] SpriteRenderer[] cheat_displays;
    [SerializeField] Sprite[] cheat_sprites;
    [SerializeField] KeyCode cheat_1_input;
    [SerializeField] KeyCode cheat_2_input;
    [SerializeField] TextMesh cheat_risk_display;

    [Header("Deathstone Data")]
    [SerializeField] Vector3 deathstone_speed;
    [SerializeField] float finishing_touch_distance;
    
    public DeathStone deathStone; //Set in GameManager
    public Player enemy; //Set in GameManager
    private KeyCode TARGET_KEY;
    private CHEAT[] cheats;
    public int MIN_KEY_INDEX = 0; //Minimum index of the key this player can find in the manager array
    public int MAX_KEY_INDEX = 3; //maximum index of keys that can be found in the manager
    private float cheat_risk;

    //TIMERS
    private float CHEAT_TIME;
    private float cheat_timer;
    private float BUFF_TIME;
    private float buff_timer;
    private float DEBUFF_TIME;
    private float debuff_timer;

    private float deathStone_speed_mod = 1;
    private float deathStone_debuff_mod = 0;
    private bool homeFieldActive;
    private bool desperateActive;

    // Start is called before the first frame update
    void Start()
    {
        cheats = new CHEAT[2];
        SetTargetKey();
        GetNewCheats();
    }

    // Update is called once per frame
    void Update()
    {
        //Once the player hits the target key, 
        //tell the manager that a new target is needed
        if (Input.GetKeyDown(TARGET_KEY))
        {
            SetTargetKey();
            Instantiate(completion_particles, input_display.transform);
            deathStone.AddVelocity(deathstone_speed * (deathStone_speed_mod  - deathStone_debuff_mod));
            input_data.StartWave();
        }
        else if (Input.anyKeyDown)
        {
            for (int i = 0; i < possible_keys.Length; i++)
            {
                if (possible_keys[i] == TARGET_KEY) continue;
                if (Input.GetKeyDown(possible_keys[i]))
                {
                    input_data.StartShake();
                    break;
                }
            }
        }

        //Check to see if either of these two buffs are active
        if (homeFieldActive)
        {
            //If in the given distance, activate the debuff, otherwise
            //make sure the enemy has the correct debuff mod
            if (Mathf.Abs(deathStone.transform.position.x - transform.position.x) <= 5.5f)
            {
                enemy.HomeFieldAdvantage();
            }
            else
            {
                enemy.ResetDebuff();
            }
        }
        if (desperateActive)
        {
            //Make sure the deathstone is with the given parameters,
            //if not reset the buff mod
            if (Mathf.Abs(deathStone.transform.position.x - transform.position.x) <= 2 || 
                Mathf.Abs(deathStone.transform.position.x - enemy.transform.position.x) <= 2)
            {
                Debug.Log("Here");
                deathStone_speed_mod = 1.5f;
            }
            else
            {
                deathStone_speed_mod = 1;
            }
        }

        //Check to see if a player has activated one of their cheats
        if (cheat_timer > 0) return;
        if (Input.GetKeyDown(cheat_1_input))
        {
            ActivateCheat(cheats[0]);
        }
        else if (Input.GetKeyDown(cheat_2_input))
        {
            ActivateCheat(cheats[1]);
        }
    }

    //Update the timer on fixed update so that
    //it doesnt screw with the build
    private void FixedUpdate()
    {
        //Update the cheat timer if necessary
        //and get new cheats after its done
        if (cheat_timer > 0)
        {
            cheat_timer += Time.deltaTime;
            if (cheat_timer >= CHEAT_TIME) GetNewCheats();
        }

        //Update the debuff timer if necessary
        //and reset variables after its done
        if (debuff_timer > 0)
        {
            debuff_timer += Time.deltaTime;
            if (debuff_timer >= DEBUFF_TIME)
            {
                debuff_timer = 0;
                deathStone_debuff_mod = 0;
                MIN_KEY_INDEX = 0;
                MAX_KEY_INDEX = 3;
            }
        }

        //Update the buff timer if necessary
        //and reset variables afterwards
        if (buff_timer > 0)
        {
            buff_timer += Time.deltaTime;
            if (buff_timer >= BUFF_TIME)
            {
                buff_timer = 0;
                deathStone_speed_mod = 1;
                homeFieldActive = false;
                desperateActive = false;
            }
        }

        //Subtract from cheat risk gradually
        if (cheat_risk > 0)
        {
            cheat_risk -= Time.deltaTime;
            cheat_risk_display.text = "" + (int)cheat_risk;
        }
    }

    //Set this player's target key
    public void SetTargetKey()
    {
        int index = Random.Range(MIN_KEY_INDEX, MAX_KEY_INDEX);
        TARGET_KEY = possible_keys[index];
        input_display.sprite = input_sprites[index];
    }

    //Get new cheats to use after the timer
    //has counted down
    private void GetNewCheats()
    {
        //Reset variables
        cheat_timer = 0;
        MAX_KEY_INDEX = 3;
        enemy.ResetDebuff();
        CHEAT c1 = cheats[0];
        CHEAT c2 = cheats[1];
        cheat_displays[0].enabled = true;
        cheat_displays[1].enabled = true;
        int index =  0;
        
        //Acquire new random cheats
        while (c1 == cheats[0])
        {
            index = Random.Range(0, 7);
            cheats[0] = GameManager.cheat_list[index];
            cheat_displays[0].sprite = cheat_sprites[index];
        }
        while (c2 == cheats[1] || cheats[1] == cheats[0])
        {
            index = Random.Range(0, 7);
            cheats[1] = GameManager.cheat_list[index];
            cheat_displays[1].sprite = cheat_sprites[index];
        }
    }

    //Figure out what to do based on the
    //cheat that needs to be activated
    private void ActivateCheat(CHEAT cheat)
    {
        switch (cheat)
        {
            case CHEAT.HEIGHTENED_FOCUS: //Increase effectiveness of pushes
                deathStone_speed_mod = 1.2f;
                buff_timer += 0.01f;
                BUFF_TIME = 1;
                cheat_risk += 30;
                break;
            case CHEAT.SELECTIVE_NULLIFICATION: //Decrease enemy push effectiveness
                enemy.SelectiveNullification();
                cheat_risk += 30;
                break;
            case CHEAT.FINISHING_TOUCH: //Push the deathstone a set distance
                deathStone.FinishingTouch(finishing_touch_distance);
                cheat_risk += 60;
                break;
            case CHEAT.HOME_FIELD_ADVANTAGE: //Make pushes less effective on player field side
                homeFieldActive = true;
                buff_timer += 0.01f;
                BUFF_TIME = 1;
                cheat_risk += 30;
                break;
            case CHEAT.DESPERATE_MEASURES: //Make pushes more effective when very close to either player
                desperateActive = true;
                buff_timer += 0.01f;
                BUFF_TIME = 1;
                cheat_risk += 60;
                break;
            case CHEAT.ADVANCED_FORMULAE: //Make inputs more difficult for the other player
                enemy.AdvancedFormulae();
                cheat_risk += 15;
                break;
            case CHEAT.EXPERT_FORMULAE: //Make inputs even more difficult for the other player
                enemy.ExpertFormulae();
                cheat_risk += 15;
                break;
            case CHEAT.ARCANE_LOCK: //Give yourself a single input, but a debuff to push distance
                MAX_KEY_INDEX = 0;
                deathStone_speed_mod = 0.6f;
                buff_timer += 0.01f;
                BUFF_TIME = 1;
                cheat_risk += 15;
                break;
        }
        //Start the timer and disable the cheat
        //ui elements
        CHEAT_TIME = 2;
        cheat_timer += 0.01f;
        cheat_displays[0].enabled = false;
        cheat_displays[1].enabled = false;
    }

    //Start up the selective nullification
    //debuff
    public void SelectiveNullification()
    {
        deathStone_debuff_mod = 0.2f;
        debuff_timer += 0.01f;
        DEBUFF_TIME = 1;
    }

    //Startup the advanced formulae debuff
    public void AdvancedFormulae()
    {
        MAX_KEY_INDEX = 6;
        debuff_timer += 0.01f;
        DEBUFF_TIME = 1;
    }

    //Startup the expert formulae debuff
    public void ExpertFormulae()
    {
        MIN_KEY_INDEX = 4;
        MAX_KEY_INDEX = 9;
        debuff_timer += 0.01f;
        DEBUFF_TIME = 1;
    }

    //Apply the homefield advantage debuff
    public void HomeFieldAdvantage()
    {
        deathStone_debuff_mod = 0.2f;
    }

    //reset any debuffs currently 
    //applied to this character in case
    //they are conditional
    public void ResetDebuff()
    {
        deathStone_debuff_mod = 0;
    }
}
