using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DragonBones;

public class MainUpdater : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip swipe;
    public AudioClip right_s;
    public AudioClip wrong;
    public AudioSource Bus_Noise;
    public GameObject high_score;
    public GameObject game_score;
    public GameObject left;
    public GameObject right;
    public GameObject lucky;
    public GameObject unlucky;
    public GameObject play;
    public GameObject money;
    public GameObject price;
    public GameObject set;
    public GameObject instructions;
    public GameObject ticket;
    public GameObject zero;
    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;
    public GameObject timer;
    public GameObject play_menu;
    public GameObject shop;
    public GameObject sound;

    public int Choosen_Skin_ID = 0;
    public int Try_Skin_ID = 0;

    public AudioClip[] Bus_Noise_arr;
    public Sprite[] high_score_arr;
    public Sprite[] game_score_arr;
    public Sprite[] left_arr;
    public Sprite[] right_arr;
    public Sprite[] play_arr;
    public Sprite[] pause_arr;
    public Sprite[] money_arr;
    public Sprite[] price_arr;
    public Sprite[] set_arr;
    public Sprite[] instructions_arr;
    public Sprite[] ticket_arr;
    public Sprite[] zero_arr;
    public Sprite[] one_arr;
    public Sprite[] two_arr;
    public Sprite[] three_arr;
    public Sprite[] four_arr;
    public Sprite[] five_arr;
    public Sprite[] six_arr;
    public Sprite[] seven_arr;
    public Sprite[] eight_arr;
    public Sprite[] nine_arr;
    public Sprite[] timer_arr;
    public Sprite[] play_menu_arr;
    public Sprite[] shop_arr;
    public Sprite[] soundon_arr;
    public Sprite[] soundoff_arr;

    private GameObject timerGO;
    private GameObject timer1;
    private GameObject timer2;

    private GameObject recordOne;
    private GameObject recordTwo;
    private GameObject recordThree;
    private GameObject scoreOne;
    private GameObject scoreTwo;
    private GameObject scoreThree;

    private GameObject ticketGO;
    private GameObject ticket11;
    private GameObject ticket22;
    private GameObject ticket33;
    private GameObject ticket44;
    private GameObject ticket55;
    private GameObject ticket66;

    private GameObject moneyGO;
    private GameObject money11;
    private GameObject money22;
    private GameObject money33;
    private GameObject money44;
    private GameObject money55;
    private GameObject money66;

    private GameObject priceGO;
    private GameObject price11;
    private GameObject price22;
    private GameObject price33;
    private GameObject price44;
    private GameObject price55;
    private GameObject price66;

    private GameObject leftGO;
    private GameObject rightGO;

    private GameObject luckyGO;
    private GameObject unluckyGO;

    private GameObject instructionsGO;

    private GameObject play_menuGO;
    private GameObject shopGO;
    private GameObject soundGO;

    private UnityArmatureComponent anim;
    private DragonBonesData animData;
    private UnityTextureAtlasData animAtlas;

    public int score = 0;
    public int record = 0;
    public float speed = 25f;
    private bool isPaused = true;
    private bool onResume = false;
    private bool isStart = true;
    private bool spawn = false;
    private bool despawn = false;
    private bool isSwipeAllowed = false;
    private bool isShopStarted = true;
    private bool isSkinPaid = true;
    private bool count = false;
    private bool nextSkinChoosed = true;
    private int ticks = 1;
    private int difficulty = 10;
    private bool isFirstTouch = true;
    private float fp;
    private float pp;
    private float lp;
    private float dragDistance;
    private Thread spawnTickets;
    int i = 0;
    private bool updateTimer;
    private bool startTimer;
    private bool endTimer;
    private int ticket1 = 0;
    private int ticket2 = 1;
    private int ticket3 = 2;
    private int ticket4 = 3;
    private int ticket5 = 4;
    private int ticket6 = 5;
    private int sum1 = 0;
    private int sum2 = 0;
    private int loseCount = 0;

    public int money_count = 0;

    private bool[] skins_availible = { true, false, false, false, false, false, false, false, false, false, false, false };

    private int[] skins_prices = {0, 50, 50, 50, 100, 100, 100, 100, 500, 500, 500, 500};

    private bool isMenu = true;

    void Start()
    {
        PlayerPrefs.SetString("Skin_0", "true");
        try
        {
            record = PlayerPrefs.GetInt("Record", 0);
            money_count = PlayerPrefs.GetInt("Money", 0);
            for(int i = 0; i < 12; i++)
            {
                try
                {
                    skins_availible[i] = Boolean.Parse(PlayerPrefs.GetString("Skin_" + i));
                }
                catch
                {

                }
            }
            Choosen_Skin_ID = PlayerPrefs.GetInt("Skin", 0);
        }catch
        {

        }
        updateSkin(Choosen_Skin_ID);
        setMenu();
        /*DragonBones.UnityFactory.factory.LoadDragonBonesData("forGame/skin_00_anim_01_ske");
        UnityFactory.factory.LoadTextureAtlasData("forGame/skin_00_anim_01_tex");
        anim = UnityFactory.factory.BuildArmatureComponent("Armature");
        anim.transform.localScale = new Vector3(2, 2, 1);*/
        updateRecord();
        updateScore();
        //audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetInt("Sounds", 1);
        Bus_Noise.volume = PlayerPrefs.GetInt("Sounds", 1);
        dragDistance = Screen.width * 12 / 100;
        }

    void Update()
    {
        if (!isPaused)
        {
            if (onResume)
            {
                anim.transform.position = new Vector3(0f, 0f, 0.05f);
                spawnTickets = new Thread(new ThreadStart(spawnTicket));
                spawnTickets.Start();
                onResume = false;
            }
            if (startTimer)
            {
                startTimer = false;
                timerGO = Instantiate(timer, new Vector3(0f, -3.5f, -0.1f), Quaternion.Euler(0, 0, 0));
            }
            if (updateTimer)
            {
                updateTimer = false;
                updateTime();
            }
            if (endTimer)
            {
                endTime();
                endTimer = false;
            }
            // TEST
            if (Input.GetMouseButtonDown(0) && isStart)
            {
                spawnTickets = new Thread(new ThreadStart(spawnTicket));
                spawnTickets.Start();
                Destroy(instructionsGO);
                isStart = false;
                isPaused = false;
                ClearGame();
            }
            if (!isStart)
            {
                //    TEST
                //if((Input.touches[0].rawPosition.x > -1.0f && Input.touches[0].rawPosition.x < 1.0f) && (Input.touches[0].rawPosition.y > 4.9f && Input.touches[0].rawPosition.x < 6.4f))
                try
                {
                    if (((Input.touches[0].position.x > (Screen.width * 0.36f) && Input.touches[0].position.x < (Screen.width * 0.64f)) && (Input.touches[0].position.y > (Screen.height * 0.88f) && Input.touches[0].position.y < Screen.height) && Input.touches[0].phase == TouchPhase.Began))
                    {
                        isPaused = true;
                        onResume = true;
                        isMenu = true;
                        ClearGame();
                        setMenu();
                    }
                }
                catch
                {

                }
                if (count)
                {
                    int isEqual = UnityEngine.Random.Range(0, 2);
                    ticket1 = UnityEngine.Random.Range(0, 10);
                    ticket2 = UnityEngine.Random.Range(0, 10);
                    ticket3 = UnityEngine.Random.Range(0, 10);
                    ticket4 = 0;
                    ticket5 = 0;
                    ticket6 = 0;
                    sum1 = ticket1 + ticket2 + ticket3;
                    if (isEqual == 0)
                    {
                        ticket4 = UnityEngine.Random.Range(0, 10);
                        ticket5 = UnityEngine.Random.Range(0, 10);
                        ticket6 = UnityEngine.Random.Range(0, 10);
                    }
                    else
                    {
                        simulateEquality(sum1);
                    }
                    sum2 = ticket4 + ticket5 + ticket6;
                    count = false;
                }
                if (spawn)
                {
                    createTicket();
                    spawn = false;
                    audio.PlayOneShot(swipe);
                }
                if (despawn)
                {
                    Destroy(ticketGO);
                    Destroy(luckyGO);
                    Destroy(unluckyGO);
                    endTime();
                    isSwipeAllowed = false;
                    audio.PlayOneShot(swipe);
                    restart();
                }
                if (ticketGO.transform.position.y > 1)
                {
                    ticketGO.transform.Translate(Vector3.down * Time.deltaTime * speed);
                }
                else
                {
                    isSwipeAllowed = true;
                }

                if (isSwipeAllowed)
                {
                    if ((Input.touches[0].phase == TouchPhase.Began && Input.touches[0].position.y < (Screen.height * 0.88f)))
                    {
                        fp = Input.touches[0].position.x;
                        pp = Input.touches[0].position.x;
                        lp = Input.touches[0].position.x;
                    }
                    if (Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].position.y < (Screen.height * 0.88f))
                    {
                        pp = lp;
                        lp = Input.touches[0].position.x;
                        if (lp - pp > 0)
                        {
                            if (Mathf.Abs(lp - fp) > dragDistance)
                            {
                                ticketGO.transform.Translate(Vector3.right * Time.deltaTime * speed * Mathf.Abs(lp - fp));
                                if (isFirstTouch)
                                {
                                    audio.PlayOneShot(swipe);
                                    isFirstTouch = false;
                                }
                            }
                        }
                        if (lp - pp < 0)
                        {
                            if (Mathf.Abs(lp - fp) > dragDistance)
                            {
                                ticketGO.transform.Translate(Vector3.left * Time.deltaTime * speed * Mathf.Abs(lp - fp));
                                if (isFirstTouch)
                                {
                                    audio.PlayOneShot(swipe);
                                    isFirstTouch = false;
                                }
                            }
                        }
                    }

                    if (ticketGO.transform.position.x < -3.0f)
                    {
                        if (sum1 != sum2)
                        {
                            score++;
                            if (score > record)
                            {
                                record = score;
                                PlayerPrefs.SetInt("Record", record);
                            }
                            ticks++;
                            Destroy(ticketGO);
                            Destroy(luckyGO);
                            Destroy(unluckyGO);
                            audio.PlayOneShot(right_s);
                            if (score % 5 == 0)
                            {
                                money_count += 10;
                                PlayerPrefs.SetInt("Money", money_count);
                            }
                            endTime();
                            updateScore();
                            updateRecord();
                            isSwipeAllowed = false;
                            isFirstTouch = true;
                            spawnTickets.Abort();
                            spawnTickets = new Thread(new ThreadStart(spawnTicket));
                            spawnTickets.Start();
                        }
                        else
                        {
                            Destroy(ticketGO);
                            Destroy(luckyGO);
                            Destroy(unluckyGO);
                            endTime();
                            restart();
                            isSwipeAllowed = false;
                            spawnTickets.Abort();
                            spawnTickets = new Thread(new ThreadStart(spawnTicket));
                            spawnTickets.Start();
                        }
                    }
                    if (ticketGO.transform.position.x > 3.0f)
                    {
                        if (sum1 == sum2)
                        {
                            score++;
                            if (score > record)
                            {
                                record = score;
                                PlayerPrefs.SetInt("Record", record);
                            }
                            ticks++;
                            Destroy(ticketGO);
                            Destroy(luckyGO);
                            Destroy(unluckyGO);
                            audio.PlayOneShot(right_s);
                            if (score % 5 == 0)
                            {
                                money_count += 10;
                                PlayerPrefs.SetInt("Money", money_count);
                            }
                            endTime();
                            updateScore();
                            updateRecord();
                            isSwipeAllowed = false;
                            isFirstTouch = true;
                            spawnTickets.Abort();
                            spawnTickets = new Thread(new ThreadStart(spawnTicket));
                            spawnTickets.Start();
                        }
                        else
                        {
                            Destroy(ticketGO);
                            Destroy(luckyGO);
                            Destroy(unluckyGO);
                            endTime();
                            restart();
                            isSwipeAllowed = false;
                            spawnTickets.Abort();
                            spawnTickets = new Thread(new ThreadStart(spawnTicket));
                            spawnTickets.Start();
                        }
                    }
                }
            }
        }
        else
        {
            if (isMenu)
            {
                //pause
                if ((Input.touches[0].position.x > (Screen.width * 0.36f) && Input.touches[0].position.x < (Screen.width * 0.64f)) && (Input.touches[0].position.y > (Screen.height * 0.88f) && Input.touches[0].position.y < (Screen.height)) && (Input.touches[0].phase == TouchPhase.Began))
                {
                    play.GetComponent<SpriteRenderer>().sprite = play_arr[Choosen_Skin_ID];
                    clearMenu();
                    isMenu = false;
                    isPaused = false;
                    if (isStart)
                    {
                        instructionsGO = Instantiate(instructions, new Vector3(0.0f, 0.0f, -0.1f), Quaternion.Euler(0, 0, 0));
                    }
                }
                //play menu
                if ((Input.touches[0].position.x > (Screen.width * 0.21f) && Input.touches[0].position.x < (Screen.width * 0.79f)) && (Input.touches[0].position.y > (Screen.height * 0.62f) && Input.touches[0].position.y < (Screen.height * 0.73f)) && (Input.touches[0].phase == TouchPhase.Began))
                {
                    play.GetComponent<SpriteRenderer>().sprite = play_arr[Choosen_Skin_ID];
                    clearMenu();
                    isMenu = false;
                    isPaused = false;
                    if (isStart)
                    {
                        instructionsGO = Instantiate(instructions, new Vector3(0.0f, 0.0f, -0.1f), Quaternion.Euler(0, 0, 0));
                    }
                }
                //shop
                if ((Input.touches[0].position.x > (Screen.width * 0.21f) && Input.touches[0].position.x < (Screen.width * 0.79f)) && (Input.touches[0].position.y > (Screen.height * 0.44f) && Input.touches[0].position.y < (Screen.height *0.56f)) && (Input.touches[0].phase == TouchPhase.Began))
                {
                    clearMenu();
                    isMenu = false;
                    isShopStarted = true;
                }
                //sound on off
                if ((Input.touches[0].position.x > (Screen.width * 0.21f) && Input.touches[0].position.x < (Screen.width * 0.79f)) && (Input.touches[0].position.y > (Screen.height * 0.27f) && Input.touches[0].position.y < (Screen.height * 0.38f)) && (Input.touches[0].phase == TouchPhase.Began))
                {
                    if (PlayerPrefs.GetInt("Sounds", 1) == 1)
                    {
                        PlayerPrefs.SetInt("Sounds", 0);
                        audio.volume = 0;
                        Bus_Noise.volume = 0;
                        soundGO.GetComponent<SpriteRenderer>().sprite = soundoff_arr[Choosen_Skin_ID];
                    }
                    else
                    {
                        PlayerPrefs.SetInt("Sounds", 1);
                        audio.volume = 1;
                        Bus_Noise.volume = 1;
                        soundGO.GetComponent<SpriteRenderer>().sprite = soundon_arr[Choosen_Skin_ID];
                    }
                }
            }
            else
            {
                if (isShopStarted)
                {
                    Try_Skin_ID = Choosen_Skin_ID;
                    updateSkin(Try_Skin_ID);
                    setShop();
                    isShopStarted = false;
                }

                // right
                // TEST
                //if ((Input.touches[0].rawPosition.x > 2.5f && Input.touches[0].rawPosition.x < 3.6f))
                if (((Input.touches[0].position.x > (Screen.width * 0.85) && Input.touches[0].position.x < (Screen.width)) && (Input.touches[0].phase == TouchPhase.Began) && nextSkinChoosed))
                {
                    if (Try_Skin_ID < 11)
                    {
                        Try_Skin_ID++;
                    }
                    else
                    {
                        Try_Skin_ID = 0;
                    }
                    ClearShop();
                    updateSkin(Try_Skin_ID);
                    setShop();
                    nextSkinChoosed = false;
                    new Thread(nextSkinCooldown).Start();
                }


                // left
                // TEST
                //if ((Input.touches[0].rawPosition.x > -3.6f && Input.touches[0].rawPosition.x < -2.5f))
                if (((Input.touches[0].position.x > 0 && Input.touches[0].position.x < (Screen.width * 0.15f)) && (Input.touches[0].phase == TouchPhase.Began) && nextSkinChoosed))
                {
                    if (Try_Skin_ID > 0)
                    {
                        Try_Skin_ID--;
                    }
                    else
                    {
                        Try_Skin_ID = 11;
                    }
                    ClearShop();
                    updateSkin(Try_Skin_ID);
                    setShop();
                    nextSkinChoosed = false;
                    new Thread(nextSkinCooldown).Start();
                }


                // set
                // TEST
                //if ((Input.touches[0].rawPosition.x > -2.0f && Input.touches[0].rawPosition.x < 2.0f) && (Input.touches[0].rawPosition.y > -4.0f && Input.touches[0].rawPosition.y < -3.0f))
                if ((Input.touches[0].position.x > (Screen.width * 0.22f) && Input.touches[0].position.x < (Screen.width * 0.78f)) && (Input.touches[0].position.y > (Screen.height * 0.19f) && Input.touches[0].position.y < (Screen.height * 0.26f)) && (Input.touches[0].phase == TouchPhase.Began))
                {
                    if (isSkinPaid)
                    {
                        Choosen_Skin_ID = Try_Skin_ID;
                        PlayerPrefs.SetInt("Skin", Choosen_Skin_ID);
                        isMenu = true;
                        isShopStarted = true;
                        ClearShop();
                        updateSkin(Choosen_Skin_ID);
                        setMenu();
                    }
                    else
                    {
                        if (skins_prices[Try_Skin_ID] <= money_count)
                        {
                            money_count -= skins_prices[Try_Skin_ID];
                            skins_availible[Try_Skin_ID] = true;
                            PlayerPrefs.SetInt("Money", money_count);
                            string skinName = "Skin_" + Try_Skin_ID;
                            PlayerPrefs.SetString(skinName, "true");
                            ClearShop();
                            updateSkin(Try_Skin_ID);
                            setShop();
                        }
                    }
                }
                // pause
                //    TEST
                //if((Input.touches[0].rawPosition.x > -1.0f && Input.touches[0].rawPosition.x < 1.0f) && (Input.touches[0].rawPosition.y > 4.9f && Input.touches[0].rawPosition.x < 6.4f))
                if ((Input.touches[0].position.x > (Screen.width * 0.36f) && Input.touches[0].position.x < (Screen.width *0.64f)) && (Input.touches[0].position.y > (Screen.height * 0.88f) && Input.touches[0].position.y < (Screen.height)) && (Input.touches[0].phase == TouchPhase.Began))
                {
                    isShopStarted = true;
                    isMenu = true;
                    ClearShop();
                    updateSkin(Choosen_Skin_ID);
                    setMenu();
                }
            }
        }
    }

    private void nextSkinCooldown()
    {
        nextSkinChoosed = false;
        Thread.Sleep(100);
        nextSkinChoosed = true;
    }

    private void setShop()
    {
        setMoney();
        leftGO = Instantiate(left, new Vector3(-3.0f, 0f, -0.1f), Quaternion.Euler(0, 0, 0));
        rightGO = Instantiate(right, new Vector3(3.0f, 0f, -0.1f), Quaternion.Euler(0, 0, 0));
        setPrice();
    }

    private void setMenu()
    {
        play.GetComponent<SpriteRenderer>().sprite = pause_arr[Choosen_Skin_ID];
        play_menuGO = Instantiate(play_menu, new Vector3(0.0f, 2.3f, -0.1f), Quaternion.Euler(0, 0, 0));
        shopGO = Instantiate(shop, new Vector3(0.0f, 0.0f, -0.1f), Quaternion.Euler(0, 0, 0));
        if (PlayerPrefs.GetInt("Sounds", 1) == 1)
        {
            sound.GetComponent<SpriteRenderer>().sprite = soundon_arr[Choosen_Skin_ID];
        }
        else
        {

            sound.GetComponent<SpriteRenderer>().sprite = soundoff_arr[Choosen_Skin_ID];
        }
        soundGO = Instantiate(sound, new Vector3(0.0f, -2.3f, -0.1f), Quaternion.Euler(0, 0, 0));
    }

    private void clearMenu()
    {
        Destroy(play_menuGO);
        Destroy(shopGO);
        Destroy(soundGO);
    }

    private void setPrice()
    {
        if (skins_availible[Try_Skin_ID])
        {
            priceGO = Instantiate(set, new Vector3(0.0f, -3.5f, -0.1f), Quaternion.Euler(0, 0, 0));
            isSkinPaid = true;
        }
        else
        {
            priceGO = Instantiate(price, new Vector3(0.0f, -3.5f, -0.1f), Quaternion.Euler(0, 0, 0));
            updatePrice();
            isSkinPaid = false;
        }
    }

    private void updatePrice()
    {
        numUpdate(ref price11, ((skins_prices[Try_Skin_ID] - skins_prices[Try_Skin_ID] % 100000) % 1000000) / 100000, -1.4f, -3.5f, -0.2f);
        numUpdate(ref price22, ((skins_prices[Try_Skin_ID] - skins_prices[Try_Skin_ID] % 10000) % 100000) / 10000, -0.84f, -3.5f, -0.2f);
        numUpdate(ref price33, ((skins_prices[Try_Skin_ID] - skins_prices[Try_Skin_ID] % 1000) % 10000) / 1000, -0.28f, -3.5f, -0.2f);
        numUpdate(ref price44, ((skins_prices[Try_Skin_ID] - skins_prices[Try_Skin_ID] % 100) % 1000) / 100, 0.28f, -3.5f, -0.2f);
        numUpdate(ref price55, ((skins_prices[Try_Skin_ID] - skins_prices[Try_Skin_ID] % 10) % 100) / 10, 0.84f, -3.5f, -0.2f);
        numUpdate(ref price66, skins_prices[Try_Skin_ID] % 10, 1.4f, -3.5f, -0.2f);
        price11.transform.SetParent(priceGO.transform);
        price22.transform.SetParent(priceGO.transform);
        price33.transform.SetParent(priceGO.transform);
        price44.transform.SetParent(priceGO.transform);
        price55.transform.SetParent(priceGO.transform);
        price66.transform.SetParent(priceGO.transform);
    }

    private void setMoney()
    {
        moneyGO = Instantiate(money, new Vector3(0.0f, 3.9f, -0.1f), Quaternion.Euler(0, 0, 0));
        updateMoney();
    }

    private void updateMoney()
    {
        numUpdate(ref money11, ((money_count - money_count % 100000) % 1000000) / 100000, -0.28f, 3.9f, -0.2f);
        numUpdate(ref money22, ((money_count - money_count % 10000) % 100000) / 10000, 0.28f, 3.9f, -0.2f);
        numUpdate(ref money33, ((money_count - money_count % 1000) % 10000) / 1000, 0.84f, 3.9f, -0.2f);
        numUpdate(ref money44, ((money_count - money_count % 100) % 1000) / 100, 1.4f, 3.9f, -0.2f);
        numUpdate(ref money55, ((money_count - money_count % 10) % 100) / 10, 1.9f, 3.9f, -0.2f);
        numUpdate(ref money66, money_count % 10, 2.4f, 3.9f, -0.2f);
        money11.transform.SetParent(moneyGO.transform);
        money22.transform.SetParent(moneyGO.transform);
        money33.transform.SetParent(moneyGO.transform);
        money44.transform.SetParent(moneyGO.transform);
        money55.transform.SetParent(moneyGO.transform);
        money66.transform.SetParent(moneyGO.transform);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPaused = pause;
            isMenu = pause;
        }
        if (isPaused && isMenu)
        {
            ClearGame();
        }
    }

    private void ClearGame()
    {
        Destroy(ticketGO);
        Destroy(luckyGO);
        Destroy(unluckyGO);
        endTime();
        int scoreBuffer = score;
        int ticksBuffer = ticks;
        int diffBaffer = difficulty;
        restart();
        score = scoreBuffer;
        ticks = ticksBuffer;
        difficulty = diffBaffer;
        updateScore();
        spawnTickets.Abort();
        onResume = true;
    }

    private void ClearShop()
    {
        Destroy(moneyGO);
        Destroy(priceGO);
        Destroy(leftGO);
        Destroy(rightGO);
    }

    private void endTime()
    {
        Destroy(timerGO);
        Destroy(timer1);
        Destroy(timer2);
    }

    private void restart()
    {
        PlayerPrefs.Save();
        try
        {
            record = PlayerPrefs.GetInt("Record", 0);
        }
        catch
        {

        }
        score = 0;
        spawn = false;
        despawn = false;
        count = false;
        isFirstTouch = true;
        isSwipeAllowed = false;
        startTimer = false;
        updateTimer = false;
        endTimer = false;
        sum1 = 0;
        sum2 = 0;
        ticks = 1;
        difficulty = 10;
        updateRecord();
        updateScore();
    }

    private void createTicket()
    {
        numUpdate(ref ticket11, ticket1, -1.4f, 10f, -0.2f);
        numUpdate(ref ticket22, ticket2, -0.84f, 10f, -0.2f);
        numUpdate(ref ticket33, ticket3, -0.28f, 10f, -0.2f);
        numUpdate(ref ticket44, ticket4, 0.28f, 10f, -0.2f);
        numUpdate(ref ticket55, ticket5, 0.84f, 10f, -0.2f);
        numUpdate(ref ticket66, ticket6, 1.4f, 10f, -0.2f);
        ticketGO = Instantiate(ticket, new Vector3(0f, 10f, -0.1f), Quaternion.Euler(0, 0, 0));
        ticket11.transform.SetParent(ticketGO.transform);
        ticket22.transform.SetParent(ticketGO.transform);
        ticket33.transform.SetParent(ticketGO.transform);
        ticket44.transform.SetParent(ticketGO.transform);
        ticket55.transform.SetParent(ticketGO.transform);
        ticket66.transform.SetParent(ticketGO.transform);
        luckyGO = Instantiate(lucky);
        unluckyGO = Instantiate(unlucky);
    }

    private void updateTime()
    {
        Destroy(timer1);
        Destroy(timer2);
        if (i > 9)
        {
            numUpdate(ref timer1, (i - (i % 10)) / 10, -0.24f, -3.53f, -0.2f);
            numUpdate(ref timer2, i % 10, 0.28f, -3.53f, -0.2f);
        }
        else
        {
            numUpdate(ref timer1, 0, -0.24f, -3.53f, -0.2f);
            numUpdate(ref timer2, i, 0.28f, -3.53f, -0.2f);
        }
    }

    private void updateRecord()
    {
        Destroy(recordOne);
        Destroy(recordTwo);
        Destroy(recordThree);
        if (record < 1000)
        {
            if(record < 100)
            {
                if (record < 10)
                {
                    recordOne = Instantiate(zero, new Vector3(-3f, 5.4f, -0.4f), Quaternion.Euler(0, 0, 0));
                    recordTwo = Instantiate(zero, new Vector3(-2.35f, 5.4f, -0.4f), Quaternion.Euler(0, 0, 0));
                    numUpdate(ref recordThree, record % 10, -1.7f, 5.4f, -0.4f);
                }
                else
                {
                    recordOne = Instantiate(zero, new Vector3(-3f, 5.4f, -0.4f), Quaternion.Euler(0, 0, 0));
                    numUpdate(ref recordTwo, ((record - record % 10) % 100) / 10, -2.35f, 5.4f, -0.4f);
                    numUpdate(ref recordThree, record % 10, -1.7f, 5.4f, -0.4f);
                }
            }
            else
            {
                numUpdate(ref recordOne, ((record - record % 100) % 1000) / 100, -3f, 5.4f, -0.4f);
                numUpdate(ref recordTwo, ((record - record % 10) % 100) / 10, -2.35f, 5.4f, -0.4f);
                numUpdate(ref recordThree, record % 10, -1.7f, 5.4f, -0.4f);
            }
        }
    }
    private void updateScore()
    {
        Destroy(scoreOne);
        Destroy(scoreTwo);
        Destroy(scoreThree);
        if (score < 1000)
        {
            if (score < 100)
            {
                if (score < 10)
                {
                    scoreOne = Instantiate(zero, new Vector3(1.7f, 5.4f, -0.4f), Quaternion.Euler(0, 0, 0));
                    scoreTwo = Instantiate(zero, new Vector3(2.35f, 5.4f, -0.4f), Quaternion.Euler(0, 0, 0));
                    numUpdate(ref scoreThree, score % 10, 3f, 5.4f, -0.4f);
                }
                else
                {
                    scoreOne = Instantiate(zero, new Vector3(1.7f, 5.4f, -0.4f), Quaternion.Euler(0, 0, 0));
                    numUpdate(ref scoreTwo, (score - score % 10) / 10, 2.35f, 5.4f, -0.4f);
                    numUpdate(ref scoreThree, score % 10, 3f, 5.4f, -0.4f);
                }
            }
            else
            {
                numUpdate(ref scoreOne, (score - score % 100) / 100, 1.7f, 5.4f, -0.4f);
                numUpdate(ref scoreTwo, (score - score % 10) / 10, 2.35f, 5.4f, -0.4f);
                numUpdate(ref scoreThree, score % 10, 3f, 5.4f, -0.4f);
            }
        }
    }

    private void numUpdate(ref GameObject save, int num, float x, float y, float z)
    {
        switch (num)
        {
            case 0:
                save = Instantiate(zero, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                save = Instantiate(one, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 2:
                save = Instantiate(two, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 3:
                save = Instantiate(three, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 4:
                save = Instantiate(four, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 5:
                save = Instantiate(five, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 6:
                save = Instantiate(six, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 7:
                save = Instantiate(seven, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 8:
                save = Instantiate(eight, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
            case 9:
                save = Instantiate(nine, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                break;
        }
    }

    private void spawnTicket()
    {
        while (true)
        {
            count = true;
            Thread.Sleep(1000);
            spawn = true;
            if (ticks % 10 == 0 && ticks > 1)
            {
                if (difficulty > 5)
                {
                    difficulty--;
                }
            }
            startTimer = true;
            for (i = difficulty; i > 0; i--)
            {
                updateTimer = true;
                Thread.Sleep(1000);
            }
            endTimer = true;
            despawn = true;
            Thread.Sleep(500);
        }
    }

    private void simulateEquality(int sum)
    {
        int summ = sum;
        for (int i = 0; i < summ; i++)
        {
            int numToAdd = UnityEngine.Random.Range(0, 3);
            switch (numToAdd)
            {
                case 0:
                    if (ticket4 < 9)
                    {
                        ++ticket4;
                        break;
                    }
                    else
                    {
                        if (ticket5 < 9)
                        {
                            ++ticket5;
                            break;
                        }
                        else
                        {
                            if (ticket6 < 9)
                            {
                                ++ticket6;
                                break;
                            }
                        }
                    }
                    break;
                case 1:
                    if (ticket5 < 9)
                    {
                        ++ticket5;
                        break;
                    }
                    else
                    {
                        if (ticket4 < 9)
                        {
                            ++ticket4;
                            break;
                        }
                        else
                        {
                            if (ticket6 < 9)
                            {
                                ++ticket6;
                                break;
                            }
                        }
                    }
                    break;
                case 2:
                    if (ticket6 < 9)
                    {
                        ++ticket6;
                        break;
                    }
                    else
                    {
                        if (ticket5 < 9)
                        {
                            ++ticket5;
                            break;
                        }
                        else
                        {
                            if (ticket4 < 9)
                            {
                                ++ticket4;
                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }

    private void updateSkin(int Skin_ID)
    {
        try{
            DragonBones.UnityFactory.factory.RemoveDragonBonesData(animData.name);
        }catch{}
        try{
            DragonBones.UnityFactory.factory.RemoveTextureAtlasData(animAtlas.name);
        }catch{}
        try{
            anim.Dispose();
        }catch{}
        animData = DragonBones.UnityFactory.factory.LoadDragonBonesData("Animations/anim_0" + Skin_ID.ToString() + "/skin_0" + Skin_ID.ToString() + "_anim_01_ske");
        animAtlas = UnityFactory.factory.LoadTextureAtlasData("Animations/anim_0" + Skin_ID.ToString() + "/skin_0" + Skin_ID.ToString() + "_anim_01_tex");
        anim = UnityFactory.factory.BuildArmatureComponent("Armature");
        anim.animation.Play("animtion0");
        Bus_Noise.clip = Bus_Noise_arr[Skin_ID];
        Bus_Noise.Play();
        high_score.GetComponent<SpriteRenderer>().sprite = high_score_arr[Skin_ID];
        game_score.GetComponent<SpriteRenderer>().sprite = game_score_arr[Skin_ID];
        left.GetComponent<SpriteRenderer>().sprite = left_arr[Skin_ID];
        right.GetComponent<SpriteRenderer>().sprite = right_arr[Skin_ID];
        if (!isPaused)
        {
            play.GetComponent<SpriteRenderer>().sprite = play_arr[Skin_ID];
        }
        else
        {
            play.GetComponent<SpriteRenderer>().sprite = pause_arr[Skin_ID];
        }
        money.GetComponent<SpriteRenderer>().sprite = money_arr[Skin_ID];
        price.GetComponent<SpriteRenderer>().sprite = price_arr[Skin_ID];
        set.GetComponent<SpriteRenderer>().sprite = set_arr[Skin_ID];
        instructions.GetComponent<SpriteRenderer>().sprite = instructions_arr[Skin_ID];
        ticket.GetComponent<SpriteRenderer>().sprite = ticket_arr[Skin_ID];
        zero.GetComponent<SpriteRenderer>().sprite = zero_arr[Skin_ID];
        one.GetComponent<SpriteRenderer>().sprite = one_arr[Skin_ID];
        two.GetComponent<SpriteRenderer>().sprite = two_arr[Skin_ID];
        three.GetComponent<SpriteRenderer>().sprite = three_arr[Skin_ID];
        four.GetComponent<SpriteRenderer>().sprite = four_arr[Skin_ID];
        five.GetComponent<SpriteRenderer>().sprite = five_arr[Skin_ID];
        six.GetComponent<SpriteRenderer>().sprite = six_arr[Skin_ID];
        seven.GetComponent<SpriteRenderer>().sprite = seven_arr[Skin_ID];
        eight.GetComponent<SpriteRenderer>().sprite = eight_arr[Skin_ID];
        nine.GetComponent<SpriteRenderer>().sprite = nine_arr[Skin_ID];
        timer.GetComponent<SpriteRenderer>().sprite = timer_arr[Skin_ID];
        if (PlayerPrefs.GetInt("Sounds", 1) == 1)
        {
            sound.GetComponent<SpriteRenderer>().sprite = soundon_arr[Skin_ID];
        }
        else
        {

            sound.GetComponent<SpriteRenderer>().sprite = soundoff_arr[Skin_ID];
        }
        play_menu.GetComponent<SpriteRenderer>().sprite = play_menu_arr[Skin_ID];
        shop.GetComponent<SpriteRenderer>().sprite = shop_arr[Skin_ID];

        try
        {
            leftGO.GetComponent<SpriteRenderer>().sprite = left_arr[Skin_ID];
        }
        catch
        {

        }
        try
        {
            rightGO.GetComponent<SpriteRenderer>().sprite = right_arr[Skin_ID];
        }
        catch
        {

        }
        try
        {
            moneyGO.GetComponent<SpriteRenderer>().sprite = money_arr[Skin_ID];
        }
        catch
        {

        }
        try
        {
            priceGO.GetComponent<SpriteRenderer>().sprite = price_arr[Skin_ID];
        }
        catch
        {

        }
        try
        {
            instructionsGO.GetComponent<SpriteRenderer>().sprite = instructions_arr[Skin_ID];
        }
        catch
        {

        }
        try
        {
            ticketGO.GetComponent<SpriteRenderer>().sprite = ticket_arr[Skin_ID];
        }
        catch
        {

        }

        try
        {
            if (isPaused)
            {
                if (!isShopStarted)
                {
                    updateMoney();
                    updatePrice();
                }
                updateRecord();
                updateScore();
            }
            else
            {
                updateRecord();
                updateScore();
            }
        }
        catch
        {

        }

    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
