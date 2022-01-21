using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Runtime.CompilerServices;
using Mirror;
using PixelCrushers.DialogueSystem;
using UnityStandardAssets.CrossPlatformInput;
using Mirror.Websocket;
using Rewired;


public class PreGame : MonoBehaviour {

    public bool showSkills = false;
    public string chosen1 = "Balder";
    public string chosen2 = "None";
    public string chosen3 = "None";
    public string chosen4 = "None";
    public string control1a = "null";
    public string control2 = "null";
    public string control3 = "null";
    public string control4 = "null";
    public string sceneTarget = "The Forest";

    public GameObject netManager;

    private string controlID = null;

    public List<string> models = new List<string>();
    private List<string> controllers = new List<string>();
    private List<string> sceneList = new List<string>();
    private List<int> controllersInt = new List<int>();
    private bool showConfig = false;
    private bool fpScene = false;
    private bool savedData = false;
  
   
    private bool ready1 = false;                 //Controls that everyone is ready before moving forward. 
    private bool ready2 = true;
    private bool ready3 = true;
    private bool ready4 = true;

    private bool mouseAsssigned = false;
    private bool serverButtonClick = false;


    private bool axisXback1 = true;                 //Axis of controllers must go back before using it again to change button
    private bool axisXback2 = true;
    private bool axisXback3 = true;
    private bool axisXback4 = true;

    private bool attackBack1 = true;
    private bool attackBack2 = true;
    private bool attackBack3 = true;
    private bool attackBack4 = true;

    private int labelWidth;
    private int labelHeight;
    private int portraitWidth;
    private int bigBoardWidth;
    private int bigBoard1X;
    private int bigBoard2X;
    private int bigBoard3X;
    private int bigBoard4X;

    private int yControl1;
    private int yControl2;
    private int yControl3;
    private int yControl4;

    private int nextPlayer = 2;
    private int cntScenes = 0;
    public int fontSizeLabel;


    private Vector2 labelPos1;
    private Vector2 labelPos2;
    private Vector2 labelPos3;
    private Vector2 labelPos4;
    private Vector2 nameVec1;
    private Vector2 nameVect2;
    private Vector2 nameVect3;
    private Vector2 nameVect4;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    private Camera cam;
    private PlayerPreGame ppGame1;
    private PlayerPreGame ppGame2;
    private PlayerPreGame ppGame3;
    private PlayerPreGame ppGame4;
    private Texture2D pic1;
    private Texture2D pic2;
    private Texture2D pic3;
    private Texture2D pic4;
    private string playerConfig;
    private string profile1 = "Player1";
    public string profile2 = "Player2";
    public string profile3 = "Player3";
    public string profile4 = "Player4";
    public string ip2 = null;
    public string ip3 = null;
    public string ip4 = null;
    public List<string> ipAddresses = new List<string>();
    private string language = "en";
    private string damage = "";
    private string damage2 = "";
    private string damage3 = "";
    private string damage4 = "";
    private string platform;

    private Texture2D bigboard;
    private Texture2D boxUnChecked;
    private Texture2D boxChecked;
    private Texture2D leftB;
    private Texture2D rightB;
    private Texture2D leftBpressed;
    private Texture2D rightBpressed;
    public Texture2D activeButton;
    private Texture2D activeButton1 = null;
    private Texture2D activeButton2 = null;
    private Texture2D activeButton3 = null;
    private Texture2D activeButton4 = null;
    private Texture2D inputTexture;

    private Rect active1;
    private Rect active2;
    private Rect active3;
    private Rect active4;
    private Rect button1R;
    private Rect button1L;
    private Rect button2R;
    private Rect button2L;
    private Rect button3R;
    private Rect button3L;
    private Rect button4R;
    private Rect button4L;
    private Rect configRect1;
    private Rect configRect2;
    private Rect configRect3;
    private Rect configRect4;
    private Rect readyRect1;
    private Rect readyRect2;
    private Rect readyRect3;
    private Rect readyRect4;
    private Rect backRect;
    private Rect tickBox1;
    private Rect tickBox2;
    private Rect tickBox3;
    private Rect tickBox4;

    private Rect ventureRect;
    private Rect serverRect;


    public GUIStyle myStyle;
    private GUIStyle otherStyle;        // Used for large buttons / labels with text. 
    public GUISkin otherGUISkin;       //For large buttons/labels with text. 
    public GUIStyle styleButton;
    public GUISkin mySkin;
    public GUIStyle newStyle;
    public GUIStyle middleLabels;       //middleCenterLabels
 
    //Positions and related
    private int mouseAndKeybordY;
    private int joystickY;

    //localisation
    private string ventureForth = "VentureForth";
    private string gatherParty = "You must gather your party before venturing forth";
    private string backButton = "BACK";
    private string pressButton = "Press controller button";
    private string playerString = "PLAYER";
    private string readyString = "READY";
    private string mouseKLabel = "Mouse&Keyboard";
    private string controllerLabel = "Press Button 1 in an unassigned connected controller, to assign to this character";
    private string selectPC = "You need to select a character not selected already by another player";
    private string selectCorrectPC = ": You need to select a character before venturing forth";

    //Rewired
    private Player player1R = null;          // The Rewired Player
    private Player player2R = null;
    private Player player3R = null;
    private Player player4R = null;
    private int totalControllers;

    private int controller1 = 20;
    private int controller2 = 15;
    private int controller3 = 10;
    private int controller4 = 10;


    public int playerId1 = 0;
    public int playerId2 = 1;
    public int playerId3 = 2;
    public int playerId4 = 3;

    private float internalCounter = 0;
    private float internalCounter2 = 0;
    private float internalCounter3 = 0;
    private float internalCounter4 = 0;

    //Courotine
    private bool alive = true;
    private enum State
    {
        Idle,
        Seq01
    }
    private State state;
    private NetworkManager manager;
    private string localIPAddress;

    private string TestGUI = "None";
    private string test2 = "";

    //DisplaySkills
    private DisplaySkills disSkills;
    private Loading loading;

    private void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
        foreach (Player p in ReInput.players.Players)
        {
                p.controllers.hasKeyboard = false;
                p.controllers.hasMouse = false;
   
        }
        //    Debug.Log(ReInput.controllers.Joysticks.Count);
        // Assign each Joystick to a Player initially

        /*
        foreach (Rewired.Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            AssignJoystickToNextOpenPlayer(j);
            Debug.Log(j.name );
        }*/
    }


    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }



    void AssignJoystickToNextOpenPlayer(Rewired.Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player

            Debug.Log(p.name + "/" + j.name);
            return;
        }

    }


    // Use this for initialization
    void Start ()
    {
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
        manager = GameObject.FindGameObjectWithTag("Network Manager").GetComponent<NetworkManager>();
        platform = Application.platform.ToString();
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
                     
        }
            
        cam = Camera.main;
        player1 = transform.Find("Player1").gameObject;
        player2 = transform.Find("Player2").gameObject;
    //    player3 = transform.Find("Player3").gameObject;
     //   player4 = transform.Find("Player4").gameObject;
        models.Add("Balder");
        models.Add("Nanna");
        models.Add("Oleg");
        models.Add("Fred");
        models.Add("Rose");
        models.Add("None");

        sceneList.Add("The Forest");
        sceneList.Add("The Cross");

        bigboard = (Texture2D)(Resources.Load("GUI/Bigboard"));
        boxUnChecked = (Texture2D)(Resources.Load("GUI/Empty"));
        boxChecked = (Texture2D)(Resources.Load("GUI/Checked"));
        activeButton = (Texture2D)(Resources.Load("GUI/ActiveButton"));
        inputTexture = (Texture2D)(Resources.Load("GUI/Input"));
        disSkills = GetComponent<DisplaySkills>();
        if (GetComponent<Loading>() != null)
        {
            loading = GetComponent<Loading>();
            Debug.Log("loading");
        }

     //   fontSizeLabel = myStyle.fontSize = (int)(Screen.height * 0.015f);

        styleButton = mySkin.GetStyle("button");
        styleButton.fontSize = (int)(Screen.height * 0.017f);
        styleButton.normal.textColor = Color.white;
        styleButton.alignment = TextAnchor.MiddleCenter;

        newStyle = new GUIStyle();
        newStyle.wordWrap = true;
        newStyle.fontSize = (int)(Screen.height * 0.017f);
        newStyle.normal.textColor = Color.white;

        otherStyle = otherGUISkin.GetStyle("button");
        otherStyle.fontSize = (int)(Screen.height * 0.017f);
        otherStyle.normal.textColor = Color.white;
        otherStyle.wordWrap = true;

        middleLabels = otherGUISkin.GetStyle("label");
        middleLabels.fontSize = (int)(Screen.height * 0.017f);
        middleLabels.normal.textColor = Color.white;
        middleLabels.wordWrap = true;
        middleLabels.alignment = TextAnchor.MiddleCenter;

        fontSizeLabel = myStyle.fontSize = (int)(Screen.height * 0.017f);
        myStyle = mySkin.GetStyle("label");
        myStyle.fontSize = fontSizeLabel;
        myStyle.normal.textColor = Color.white;
        myStyle.alignment = TextAnchor.UpperLeft;


        ppGame1 = transform.Find("Player1").gameObject.GetComponent<PlayerPreGame>();
    //    ppGame2 = transform.Find("Player2").gameObject.GetComponent<PlayerPreGame>();
   //     ppGame3 = transform.Find("Player3").gameObject.GetComponent<PlayerPreGame>();
    //    ppGame4 = transform.Find("Player4").gameObject.GetComponent<PlayerPreGame>();

        leftB = (Texture2D)(Resources.Load("GUI/Left"));
        rightB = (Texture2D)(Resources.Load("GUI/Right"));
        leftBpressed = (Texture2D)(Resources.Load("GUI/LeftPressed"));
        rightBpressed = (Texture2D)(Resources.Load("GUI/RightPressed"));
        pic1= (Texture2D)(Resources.Load("Portraits/Balder"));
        pic2 = (Texture2D)(Resources.Load("Portraits/emptyPic"));
        pic3 = (Texture2D)(Resources.Load("Portraits/emptyPic"));
        pic4 = (Texture2D)(Resources.Load("Portraits/emptyPic"));

        labelWidth = (int) (Screen.width * 0.16f);
        labelHeight = (int)(Screen.height * 0.03f);
        portraitWidth = (int)(Screen.width * 0.10f);

        bigBoardWidth = (int)(Screen.width * 0.20f);
        bigBoard1X = (int)(Screen.width * 0.22f);
        bigBoard2X = bigBoard1X + (int)(Screen.width * 0.19f);
        bigBoard3X = bigBoard2X + (int)(Screen.width * 0.19f);
        bigBoard4X = bigBoard3X + (int)(Screen.width * 0.19f);
         

        configRect1 = new Rect(Screen.width * 0.133f, Screen.height * 0.9f + labelHeight, Screen.width * 0.18f, Screen.height * 0.06f);
        configRect2 = new Rect(Screen.width * 0.316f, Screen.height * 0.9f + labelHeight, Screen.width * 0.18f, Screen.height * 0.06f);
        configRect3 = new Rect(Screen.width * 0.499f, Screen.height * 0.9f + labelHeight, Screen.width * 0.18f, Screen.height * 0.06f);
        configRect4 = new Rect(Screen.width * 0.69f, Screen.height * 0.9f + labelHeight, Screen.width * 0.18f, Screen.height * 0.06f);
        
        serverRect = new Rect(Screen.width * 0.88f, Screen.height * 0.18f, Screen.width * 0.12f, Screen.height * 0.22f);

        backRect = new Rect(0, Screen.height * 0.02f, Screen.width * 0.10f, Screen.height * 0.2f);
        ventureRect = new Rect(Screen.width * 0.35f, Screen.height * 0.39f, Screen.width * 0.30f, Screen.height * 0.05f);
  
        labelPos1 = new Vector2 (bigBoard1X + (int)(Screen.width * 0.025f), Screen.height * 0.07f);
        int tempAdjust = (int)(Screen.width * 0.19f);
        labelPos2 = new Vector2((int)(labelPos1.x) + tempAdjust, Screen.height * 0.07f);
        labelPos3 = new Vector2((int)(labelPos2.x) + tempAdjust, Screen.height * 0.07f);
        labelPos4 = new Vector2((int)(labelPos3.x) + tempAdjust, Screen.height * 0.07f);

        readyRect1 = new Rect(bigBoard1X + (int)(Screen.width * 0.01f), Screen.height * 0.32f, Screen.width * 0.13f, Screen.height * 0.06f);
        readyRect2 = new Rect(bigBoard2X + (int)(Screen.width * 0.01f), Screen.height * 0.32f, Screen.width * 0.13f, Screen.height * 0.06f);
        readyRect3 = new Rect(bigBoard3X + (int)(Screen.width * 0.01f), Screen.height * 0.32f, Screen.width * 0.13f, Screen.height * 0.06f);
        readyRect4 = new Rect(bigBoard4X + (int)(Screen.width * 0.01f), Screen.height * 0.32f, Screen.width * 0.13f, Screen.height * 0.06f);

        tickBox1 = new Rect(bigBoard1X + (int)(Screen.width * 0.01f) + (int)(Screen.width * 0.13f), Screen.height * 0.32f, Screen.width * 0.05f, Screen.height * 0.06f);
        tickBox2 = new Rect(bigBoard2X + (int)(Screen.width * 0.01f) + (int)(Screen.width * 0.13f), Screen.height * 0.32f, Screen.width * 0.05f, Screen.height * 0.06f);
        tickBox3 = new Rect(bigBoard3X + (int)(Screen.width * 0.01f) + (int)(Screen.width * 0.13f), Screen.height * 0.32f, Screen.width * 0.05f, Screen.height * 0.06f);
        tickBox4 = new Rect(bigBoard4X + (int)(Screen.width * 0.01f) + (int)(Screen.width * 0.13f), Screen.height * 0.32f, Screen.width * 0.05f, Screen.height * 0.06f);

        nameVec1 = new Vector2 (labelPos1.x , (int)(Screen.height * 0.02f));
        nameVect2 = new Vector2(labelPos2.x , (int)(Screen.height * 0.02f));
        nameVect3 = new Vector2(labelPos3.x, (int)(Screen.height * 0.02f));
        nameVect4 = new Vector2(labelPos4.x, (int)(Screen.height * 0.02f));

        int buttonHeightWeight = (int)(Screen.width * 0.06f);
        button1R = new Rect(bigBoard1X + buttonHeightWeight + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);
        button1L = new Rect(bigBoard1X + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);

        button2R = new Rect(bigBoard2X + buttonHeightWeight + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);
        button2L = new Rect(bigBoard2X + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);

        button3R = new Rect(bigBoard3X + buttonHeightWeight + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);
        button3L = new Rect(bigBoard3X + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);

        button4R = new Rect(bigBoard4X + buttonHeightWeight + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);
        button4L = new Rect(bigBoard4X + (0.5f * buttonHeightWeight), Screen.height * 0.81f, buttonHeightWeight, buttonHeightWeight);

        //End

        CalculateDamage();
   //     CallControllers();
        mouseAndKeybordY = (int)(Screen.height * 0.20f);
        joystickY = mouseAndKeybordY + labelHeight;
        AddjustPCPostions();
        Invoke ("AddjustPCPostions", 0.1f);
        localIPAddress = LocalIPAddress();
        StartCoroutine("FSM");        
    }

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Idle:
                    yield return new WaitForSeconds(0.1f);
                    Idle();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq01:
                    Seq01();
                    yield return new WaitForSeconds(0);
                    break;
            }

        }
        yield return null;
    }

    private void Idle()
    {
        language = DialogueLua.GetVariable("language").asString;
        DialogueManager.SetLanguage(language);
        profile1 = DialogueLua.GetVariable("profile").AsString;        
        string targetScene = DialogueLua.GetVariable("targetScene").asString;
   //     Debug.Log(DialogueLua.GetVariable("test").asString);
        if (DialogueLua.GetVariable("test").asString == "No" || DialogueLua.GetVariable("test").asString == "Yes")
        {

            StopCoroutine("FSM");
            if (language == "en" || language == "es" || language == "fr")
            {
                GetLanguage();
               
                
            }
            GetComponent<Scores>().CheckConnection();
            netManager = GameObject.FindGameObjectWithTag("Network Manager");
            if (DialogueLua.GetVariable("test").asString == "Yes")
            {
      //          Debug.Log(DialogueLua.GetVariable("arenaMode").asString);
   
                
                if (netManager.activeSelf)
                {
                    netManager.SetActive(false);
                }

            }
            else if (DialogueLua.GetVariable("test").asString == "No")
            {
                if (netManager.activeSelf == false)
                {
                    netManager.SetActive(true);
                }
            }

            //        Debug.Log(targetScene);
            string isSceneFP = DialogueLua.GetVariable("sceneFP").asString;

            if (isSceneFP == "Yes")
            {
                fpScene = true;
            }
        }
    }

    private void Seq01()
    {

    }

    private void Update()
    {
        myStyle.fontSize = fontSizeLabel;
        //     Debug.Log(chosen1);
        totalControllers = ReInput.controllers.Joysticks.Count;

        for (int cnt = 0; cnt < ReInput.players.Players.Count; cnt++ )
        {
            //       Debug.Log(ReInput.players.Players[cnt].name + "/" + ReInput.players.Players[cnt].controllers.joystickCount);


            //           Debug.Log(ReInput.players.Players[cnt].name + "/" + cnt);

            if (control1a != "null" && player1R != null)
            {
                Controller1();
            }
            else if (player1R == null && totalControllers >= 0 && ReInput.players.Players[cnt].GetButtonUp("Fire"))
            {
                player1R = ReInput.players.Players[cnt];
                int tempXX = cnt + 1;
                control1a = "joystick " + tempXX.ToString();
                activeButton1 = activeButton;
                controller1 = cnt;
                playerId1 = cnt;
                internalCounter = Time.realtimeSinceStartup + 1;
                active1 = button1R;
                activeButton1 = activeButton;
            }



            if (control2 != "null")
            {
                Controller2();
            }
            else if (cnt != playerId1 && totalControllers >= 1 && ReInput.players.Players[cnt] != player1R && player1R != null && internalCounter + 1 < Time.realtimeSinceStartup && ReInput.players.Players[cnt].GetButtonUp("Fire"))
            {
                player2R = ReInput.players.Players[cnt];
                ready2 = false;
                int tempXX = cnt + 1;
                control2 = "joystick " + tempXX.ToString();
                activeButton2 = activeButton;
                playerId2 = cnt;
                controller2 = cnt;
                Debug.Log(cnt);
                test2 = test2 + ReInput.players.Players[cnt].name;
                internalCounter2 = Time.realtimeSinceStartup + 1;
                nextPlayer = 3;
                activeButton2 = activeButton;
                active2 = button2R;
            }


            if (control3 != "null")
            {
                Controller3();
            }
            else if (cnt != playerId1 && cnt != playerId2 && totalControllers >= 2 && ReInput.players.Players[cnt] != player1R && ReInput.players.Players[cnt] != player2R && player1R != null && player2R != null && internalCounter2 + 1 < Time.realtimeSinceStartup && ReInput.players.Players[cnt].GetButtonUp("Fire"))
            {
                player3R = ReInput.players.Players[cnt];
                ready3 = false;
                int tempXX = cnt + 1;
                control3 = "joystick " + tempXX.ToString();
                activeButton3 = activeButton;
                Debug.Log(cnt);
                playerId3 = cnt;
                controller3 = cnt;
                internalCounter3 = Time.realtimeSinceStartup + 1;
                nextPlayer = 4;
                activeButton3 = activeButton;
                active3 = button3R;
            }



            if (control4 != "null")
            {
                Controller4();
            }
            else if (cnt != playerId1 && cnt != playerId2 && cnt != playerId3 && totalControllers >= 3 && ReInput.players.Players[cnt] != player1R && ReInput.players.Players[cnt] != player2R && ReInput.players.Players[cnt] != player3R && player1R != null && player2R != null && player3R != null && internalCounter3 + 1 < Time.realtimeSinceStartup && ReInput.players.Players[cnt].GetButtonUp("Fire"))
            {
                player4R = ReInput.players.Players[cnt];
                ready4 = false;
                int tempXX = cnt + 1;
                control4 = "joystick " + tempXX.ToString();
                activeButton4 = activeButton;
                playerId4 = cnt;
                controller4 = cnt;
                internalCounter4 = Time.realtimeSinceStartup + 1;
                nextPlayer = 5;
                activeButton4 = activeButton;
                active4 = button4R;

            }


        }

    }


    public void Controller1 ()
    {
   //     Debug.Log(player1R.GetAxis("Move Horizontal"));
        if (player1R.GetButtonUp("Fire") && internalCounter + 0.2f < Time.realtimeSinceStartup)
        {
            internalCounter = Time.realtimeSinceStartup + 0.2f;
            //       Debug.Log(control1a + "Fire");
            if (active1 == button1R)
            {
                if (ready1 == true)
                {
                    ready1 = false;

                }
                chosen1 = ChangeActive(true, chosen1, "Player1");
                pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
            }
            else if (active1 == readyRect1 )
            {
                Debug.Log("ready");
                ready1 = !ready1;
                
            }
            else if (active1 == button1L)
            {
                if (ready1 == true)
                {
                    ready1 = false;

                }
                chosen1 = ChangeActive(false, chosen1, "Player1");
                pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
            }
            else if (active1 == backRect)
            {
                BackToMainMenu();
            }
            else if (active1 == ventureRect)
            {
                VentureForth();
            }
            else if (active1 == serverRect)
            {
                serverButtonClick = true;

            }
        }

   


        if (player1R.GetAxis("Move Horizontal") > 0.9f)
        {

            if (axisXback1 == true)
            {
                axisXback1 = false;
                if (active1 == button1R)
                {
                    active1 = readyRect1;
                }
                else if (active1 == readyRect1)
                {
                    active1 = backRect;
                }

                else if (active1 == backRect && ready1 == true && ready2 == true && ready3 == true && ready4 == true)
                {
                    active1 = ventureRect;
                }
                else if (active1 == backRect)
                {
                    active1 = button1L;
                }

                else if (active1 == ventureRect)
                {
                    active1 = button1L;
                }
                else if (active1 == button1L)
                {
                    active1 = button1R;
                }

            }


        }

        else if (player1R.GetAxis("Move Horizontal") < 0.5f && player1R.GetAxis("Move Horizontal") > -0.5f)

        {
            axisXback1 = true;
        }

        else if (player1R.GetAxis("Move Horizontal") < -0.9f)
        {
            if (axisXback1 == true)
            {
                axisXback1 = false;
      //          Debug.Log(axisXback1);
                if (active1 == button1R)
                {
                    active1 = button1L;
                }
                else if (active1 == button1L && ready1 == true && ready2 == true && ready3 == true && ready4 == true)
                {
                    active1 = ventureRect;
                }

                else if (active1 == button1L)
                {
                    active1 = backRect;
                }
                else if (active1 == backRect)
                {
                    active1 =  readyRect1;
                }
                else if (active1 == readyRect1)
                {
                    active1 = button1R;
                }

            }

        }
        
    
    
    }


    public void Controller2 ()
    {
               
    //    Debug.Log(CrossPlatformInputManager.GetAxis(control2 + "_X") + "/" + axisXback2);
        if (player2R.GetAxis("Move Horizontal") > 0.9f)
        {

     //       Debug.Log(CrossPlatformInputManager.GetAxis(control2 + "_X") + "/" + axisXback2);
            if (axisXback2 == true)
            {
                axisXback2 = false;

                if (active2 == button2R)
                {
                    active2 = readyRect2;
                }
                else if (active2 == readyRect2)
                {
                    active2 = button2L;

                }
                else if (active2 == button2L)
                {
                    active2 = button2R;
                }
            }
        }

        else if (player2R.GetAxis("Move Horizontal") < 0.5f && player2R.GetAxis("Move Horizontal") > -0.5f)

        {
            axisXback2 = true;
   //         Debug.Log(axisXback2);
        }

        else if (player2R.GetAxis("Move Horizontal") < -0.9f)
        {
            if (axisXback2 == true)
            {
                axisXback2 = false;

                if (active2 == button2R)
                {
                    active2 = button2L;
                }
                else if (active2 == button2L)
                {
                    active2 = readyRect2;
                }
                else if (active2 == readyRect2)
                {
                    active2 = button2R;
                }
            }

        }

        if (player2R.GetButtonUp("Fire") && internalCounter2 + 0.2f < Time.realtimeSinceStartup)
        {
            internalCounter2 = Time.realtimeSinceStartup + 0.2f;
            //      Debug.Log("Fire2");
            if (active2 == button2R)
            {
    
                if (ready2 == true)
                {
                    ready2 = false;

                }
                chosen2 = ChangeActive(true, chosen2, "Player2");
                pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));
            }
            else if (active2 == readyRect2)
            {
                ready2 = !ready2;
            }
            else if (active2 == button2L)
            {
                if (ready2 == true)
                {
                    ready2 = false;

                }
                chosen2 = ChangeActive(false, chosen2, "Player2");
                pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));
            }
        }

    }

    public void Controller3()
    {
        if (player3R.GetAxis("Move Horizontal") > 0.9f)
        {
            if (axisXback3 == true)
            {
                axisXback3 = false;

                if (active3 == button3R)
                {
                    active3 = readyRect3;
                }
                else if (active3 == readyRect3)
                {
                    active3 = button3L;

                }
                else if (active3 == button3L)
                {
                    active3 = button3R;
                }
            }
        }

        else if (player3R.GetAxis("Move Horizontal") < 0.5f && player3R.GetAxis("Move Horizontal") > -0.5f)

        {
            axisXback3 = true;
            //         Debug.Log(axisXback2);
        }

        else if (player3R.GetAxis("Move Horizontal") < -0.9f)
        {
            if (axisXback3 == true)
            {
                axisXback3 = false;

                if (active3 == button3R)
                {
                    active3 = button3L;
                }
                else if (active3 == button3L)
                {
                    active3 = readyRect3;
                }
                else if (active3 == readyRect3)
                {
                    active3 = button3R;
                }
            }

        }

        if (player3R.GetButtonUp("Fire") && internalCounter3 + 0.2f < Time.realtimeSinceStartup)
        {
            internalCounter3 = Time.realtimeSinceStartup + 0.2f;
     //       Debug.Log("Fire3");
            if (active3 == button3R)
            {

                if (ready3 == true)
                {
                    ready3 = false;

                }
                chosen3 = ChangeActive(true, chosen3, "Player3");
                pic3 = (Texture2D)(Resources.Load("Portraits/" + chosen3));
            }
            else if (active3 == readyRect3)
            {
                ready3 = !ready3;
            }
            else if (active3 == button3L)
            {
                if (ready3 == true)
                {
                    ready3 = false;

                }
                chosen3 = ChangeActive(false, chosen3, "Player3");
                pic3 = (Texture2D)(Resources.Load("Portraits/" + chosen3));
            }
        }

    }

    public void Controller4()
    {
        if (player4R.GetAxis("Move Horizontal") > 0.9f)
        {
            if (axisXback4 == true)
            {
                axisXback4 = false;

                if (active4 == button4R)
                {
                    active4 = readyRect4;
                }
                else if (active4 == readyRect4)
                {
                    active4 = button4L;

                }
                else if (active4 == button4L)
                {
                    active4 = button4R;
                }
            }
        }
        else if (player4R.GetAxis("Move Horizontal") < 0.5f && player4R.GetAxis("Move Horizontal") > -0.5f)

        {
            axisXback4 = true;
        }

        else if (player4R.GetAxis("Move Horizontal") < -0.9f)
        {
            if (axisXback4 == true)
            {
                axisXback4 = false;

                if (active4 == button4R)
                {
                    active4 = button4L;
                }
                else if (active4 == button4L)
                {
                    active4 = readyRect4;
                }
                else if (active4 == readyRect4)
                {
                    active4 = button4R;
                }
            }

        }

        if (player4R.GetButtonUp("Fire") && internalCounter4 + 0.2f < Time.realtimeSinceStartup)
        {
            internalCounter4 = Time.realtimeSinceStartup + 0.2f;
            //      Debug.Log("Fire2");
            if (active4 == button4R)
            {

                if (ready4 == true)
                {
                    ready4 = false;

                }
                chosen4 = ChangeActive(true, chosen4, "Player4");
                pic4 = (Texture2D)(Resources.Load("Portraits/" + chosen4));
            }
            else if (active4 == readyRect4 )
            {
                ready4 = !ready4;
            }
            else if (active4 == button4L)
            {
                if (ready4 == true)
                {
                    ready4 = false;

                }
                chosen4 = ChangeActive(false, chosen4, "Player4");
                pic4 = (Texture2D)(Resources.Load("Portraits/" + chosen4));
            }
        }
    }


    private string CheckButtonPressed ()
    {
        string joystickUsed = null;
        if (Input.GetKeyUp("joystick 1 button 0"))
        {

            joystickUsed = "1";
        }
        else if (Input.GetKeyUp("joystick 2 button 0"))
        {
            joystickUsed = "2";
        }
        else if (Input.GetKeyUp("joystick 3 button 0"))
        {
            joystickUsed = "3";
        }
        else if (Input.GetKeyUp("joystick 4 button 0"))
        {
            joystickUsed = "4";
        }
        else if (Input.GetKeyUp("joystick 5 button 0"))
        {
            joystickUsed = "5";
        }

        return joystickUsed;
    }

    private void OnGUI ()
    {
        if (showSkills == false)
        {
            DisplayCharacters();
            DisplayOptions();
        }
        else
        {
            disSkills.ShowSkills();
        }
        
        if (savedData == true)
        {
            loading.ShowLoadingScreen();
        }

        /*
        if (!NetworkClient.isConnected && !NetworkServer.active && fpScene == false)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }
        StopButtons();*/
   //     GUI.Label(new Rect(Screen.width * 0.01f, Screen.height * 0.94f, Screen.width * 0.9f, Screen.height * 0.06f), TestGUI, styleButton);
        Cursor.visible = true;
    }

    private void DisplayOptions ()
    {
        if (fpScene == true)
        {
            //      GUI.DrawTexture (new Rect(0, Screen.height * 0.2f, Screen.width * 0.18f, Screen.height * 0.07f), )
            GUI.Label(new Rect(0, Screen.height * 0.25f, Screen.width * 0.18f, Screen.height * 0.05f), "SCENARIO:", myStyle);
            GUI.DrawTexture(new Rect(0, Screen.height * 0.29f, Screen.width * 0.18f, Screen.height * 0.06f), inputTexture);
            GUI.Label(new Rect(0, Screen.height * 0.29f, Screen.width * 0.18f, Screen.height * 0.06f), sceneList[cntScenes], middleLabels);

            if (GUI.Button(new Rect(0, Screen.height * 0.35f, Screen.width * 0.05f, Screen.height * 0.07f), new GUIContent("", leftB)))
            {
                if (cntScenes == 0)
                {
                    cntScenes = sceneList.Count - 1;
                }
                else
                {
                    cntScenes--;
                }
                sceneTarget = sceneList[cntScenes];
            }
            if (GUI.Button(new Rect(Screen.width * 0.18f - Screen.width * 0.05f, Screen.height * 0.35f, Screen.width * 0.05f, Screen.height * 0.07f), new GUIContent("", rightB)))
            {
                if (cntScenes == sceneList.Count - 1)
                {
                    cntScenes = 0;
                }
                else
                {
                    cntScenes++;
                }
                sceneTarget = sceneList[cntScenes];
            }

            GUI.Label(new Rect(0, Screen.height * 0.45f, Screen.width * 0.18f, Screen.height * 0.05f), "ENEMY FACTION:", myStyle);
            GUI.DrawTexture(new Rect(0, Screen.height * 0.49f, Screen.width * 0.18f, Screen.height * 0.06f), inputTexture);
            GUI.Label(new Rect(0, Screen.height * 0.49f, Screen.width * 0.18f, Screen.height * 0.06f), "Undead", middleLabels);

            if (GUI.Button(new Rect(0, Screen.height * 0.55f, Screen.width * 0.05f, Screen.height * 0.07f), new GUIContent("", leftB)))
            {
                DialogueManager.ShowAlert("Only one faction available. More to be added soon (Hint: Monsters)");
            }
            if (GUI.Button(new Rect(Screen.width * 0.18f - Screen.width * 0.05f, Screen.height * 0.55f, Screen.width * 0.05f, Screen.height * 0.07f), new GUIContent("", rightB)))
            {
                DialogueManager.ShowAlert("Only one faction available. More to be added soon (Hint: Monsters)");
            }
            //      GUI.Button(new Rect(0, Screen.height * 0.35f, Screen.width * 0.05f, Screen.height * 0.07f), "LEFT");
        }
    }

    private void DisplayCharacters()
    {

        if (GUI.Button(backRect, backButton, otherStyle))
        {
            BackToMainMenu();
        }

    //    Debug.Log(ready1 + "/" + ready2 + "/" + ready3 + "/" + ready4);

        if (ready1 == true && ready2 == true && ready3 == true && ready4 == true)
        {
            if (GUI.Button(ventureRect, ventureForth, styleButton))
            {
                VentureForth();
            }

        }
        else
        {
            GUI.Label(new Rect(Screen.width * 0.40f, Screen.height * 0.39f, Screen.width * 0.50f, Screen.height * 0.06f), gatherParty, myStyle);
            
        }

        GUI.DrawTexture(new Rect(bigBoard1X, 0, Screen.width * 0.20f, Screen.height * 0.39f), bigboard);
        GUI.Label(new Rect(nameVec1.x, nameVec1.y, Screen.width * 0.20f, Screen.height * 0.39f), profile1, myStyle);

        GUI.DrawTexture(new Rect(bigBoard2X, 0, Screen.width * 0.20f, Screen.height * 0.39f), bigboard);
        GUI.DrawTexture(new Rect(bigBoard3X, 0, Screen.width * 0.20f, Screen.height * 0.39f), bigboard);
        GUI.DrawTexture(new Rect(bigBoard4X, 0, Screen.width * 0.20f, Screen.height * 0.39f), bigboard);
        /*
        GUI.DrawTexture(new Rect(0, 0, portraitWidth, portraitWidth), pic1);
        GUI.DrawTexture(new Rect(Screen.width - portraitWidth, 0, portraitWidth, portraitWidth), pic2);
        GUI.DrawTexture(new Rect(0, Screen.height - portraitWidth, portraitWidth, portraitWidth), pic3);
        GUI.DrawTexture(new Rect(Screen.width - portraitWidth, Screen.height - portraitWidth, portraitWidth, portraitWidth), pic4);*/

        Character1();
        Character2();
        Character3();
        Character4();       
    }


    private void Character1 ()
    {



        //Player1/////////////////////////////////////////////////////////////////////////////
        Debug.Log(chosen1 + "/" + DialogueLua.GetActorField(chosen1, "health").asString);

        GUI.Label(new Rect(labelPos1.x, labelPos1.y, labelWidth, labelHeight), chosen1, myStyle);
        GUI.Label(new Rect(labelPos1.x, labelPos1.y + labelHeight, labelWidth, labelHeight), DialogueLua.GetActorField(chosen1, "fullName").asString, myStyle);
        GUI.Label(new Rect(labelPos1.x, labelPos1.y + (2 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "race " + language).asString + ": " + DialogueLua.GetActorField(chosen1, "race " + language).asString, myStyle);
        GUI.Label(new Rect(labelPos1.x, labelPos1.y + (3 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "class " + language).asString + ": " + DialogueLua.GetActorField(chosen1, "class " + language).asString, myStyle);
        GUI.Label(new Rect(labelPos1.x, labelPos1.y + (4 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "health " + language).asString + ": " + DialogueLua.GetActorField(chosen1, "health").asString, myStyle);
        GUI.Label(new Rect(labelPos1.x, labelPos1.y + (5 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "armor " + language).asString + ": " + DialogueLua.GetActorField(chosen1, "armor").asString, myStyle);
        GUI.Label(new Rect(labelPos1.x, labelPos1.y + (6 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "damage " + language).asString + ": " + damage, myStyle);



        if (GUI.Button(readyRect1, readyString, styleButton))
        {
            ready1 = !ready1;
        }

        if (ready1 == false)
        {
            GUI.DrawTexture(tickBox1, boxUnChecked);

        }
        else
        {
            GUI.DrawTexture(tickBox1, boxChecked);

        }

        GUI.skin = mySkin;
        if (GUI.Button(button1L, new GUIContent("", leftB)))
        {
            if (ready1 == true)
            {
                ready1 = false;
            }
            chosen1 = ChangeActive(false, chosen1, "Player1");
            if (chosen1 != "None")
            {
                pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
            }
            else
            {
                pic1 = (Texture2D)(Resources.Load("Portraits/emptyPic"));
            }
        }
        if (GUI.Button(button1R, new GUIContent("", rightB)))
        {
            if (ready1 == true)
            {
                ready1 = false;

            }
            chosen1 = ChangeActive(true, chosen1, "Player1");

            /*
            if (chosen1 != "None")
            {
                pic1 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
            }
            else
            {
                pic1 = (Texture2D)(Resources.Load("Portraits/emptyPic"));
            }*/
        }

    //    Debug.Log(control1a);
        if (control1a == "null")
        {
            if (GUI.Button(new Rect(bigBoard1X, Screen.height * 0.55f, labelWidth, labelHeight * 3), mouseKLabel, styleButton))
            {
                //               Debug.Log(ReInput.players.Players.Count);
                for (int cnt = 3; cnt > 0; cnt--)
                {
                    if (player1R == null)
                    {
                        //                    Debug.Log("Button");

                        
                        player1R = ReInput.players.Players[cnt];
                        ready1 = false;
                        control1a = "Mouse&Keyboard";
                        //         activeButton2 = activeButton;
                        playerId1 = cnt;
                        controller1 = cnt;
                        nextPlayer = 2;
                        pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen1));
                        mouseAsssigned = true;

                        //Unassign other potential joysticks to this controller.                               if (player2R.controllers.joystickCount > 0)  //just checking, not needed though
                        {
                            foreach (Rewired.Joystick j in player1R.controllers.Joysticks)
                            {
                                Debug.Log(j.name);
                            }
                        }
                        player1R.controllers.ClearAllControllers();

                        //Check that only this player has assigned mouse / keyboard
                        foreach (Player p in ReInput.players.Players)
                        {
                            if (p != player1R)
                            {
                                p.controllers.hasKeyboard = false;
                                p.controllers.hasMouse = false;
                            }
                        }
                        //Assign Mouse&Keyboard to this player
                        player1R.controllers.hasKeyboard = true;
                        player1R.controllers.hasMouse = true;
                    }
                }
            }
            GUI.Label(new Rect(labelPos1.x, Screen.height * 0.65f, labelWidth, labelHeight * 10), controllerLabel, newStyle);
        }

        GUI.Label(new Rect(labelPos1.x, Screen.height * 0.90f, labelWidth, labelHeight), control1a, myStyle);
        /*
         if (GUI.Button(configRect1, "CONFIG", styleButton))
         {
             playerConfig = "Player1";
             ActivateConfig();
             showConfig = true;
             GetControllersInfo();
         }*/

        //     Debug.Log(activeButton1 + "/" + active1 + button1R);
   //     Debug.Log(activeButton.name);
        if (activeButton1 != null)
        {
            
            GUI.DrawTexture(active1, activeButton);
        }

    }

    private void Character2 ()
    {


        //Player2/////////////////////////////////////////////////////////////////////////
        //Arrow buttons

        GUI.Label(new Rect(nameVect2.x, nameVect2.y, Screen.width * 0.20f, Screen.height * 0.39f), profile2, myStyle);


        if (control2 != "null")
        {


            GUI.Label(new Rect(labelPos2.x, labelPos2.y, labelWidth, labelHeight), chosen2, myStyle);
            GUI.Label(new Rect(labelPos2.x, labelPos2.y + labelHeight, labelWidth, labelHeight), DialogueLua.GetActorField(chosen2, "fullName").asString, myStyle);
            GUI.Label(new Rect(labelPos2.x, labelPos2.y + (2 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "race " + language).asString + ": " + DialogueLua.GetActorField(chosen2, "race " + language).asString, myStyle);
            GUI.Label(new Rect(labelPos2.x, labelPos2.y + (3 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "class " + language).asString + ": " + DialogueLua.GetActorField(chosen2, "class " + language).asString, myStyle);
            GUI.Label(new Rect(labelPos2.x, labelPos2.y + (4 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "health " + language).asString + ": " + DialogueLua.GetActorField(chosen2, "health").asString, myStyle);
            GUI.Label(new Rect(labelPos2.x, labelPos2.y + (5 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "armor " + language).asString + ": " + DialogueLua.GetActorField(chosen2, "armor").asString, myStyle);
            GUI.Label(new Rect(labelPos2.x, labelPos2.y + (6 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "damage " + language).asString + ": " + damage2, myStyle);

            GUI.Label(new Rect(labelPos2.x, Screen.height * 0.90f, labelWidth, labelHeight), control2, myStyle);
            
            /*
            if (GUI.Button(configRect2, "CONFIG", styleButton))
            {
                playerConfig = "Player2";
                ActivateConfig();
                showConfig = true;
                GetControllersInfo();
            }*/


            if (GUI.Button(readyRect2, readyString, styleButton))
            {
                bool goodToGo = true;

                if (chosen2 == chosen1 && ready1 == true)
                {
                    goodToGo = false;
                }

                if (chosen2 == chosen3 && ready3 == true)
                {
                    goodToGo = false;
                }

                if (chosen2 == chosen4 && ready4 == true)
                {
                    goodToGo = false;
                }

                if (ready2 == true)
                {
                    ready2 = false;
                }

                if (goodToGo == true)
                {
                    if (ready2 == false)
                    {
                        ready2 = true;
                    }

                    
                }
                else
                {
                    SendAlertUsedPC("Player2", chosen2);
                }


            }

            if (ready2 == false)
            {
                GUI.DrawTexture(tickBox2, boxUnChecked);

            }
            else
            {
                GUI.DrawTexture(tickBox2, boxChecked);

            }


            if (GUI.Button(button2L, new GUIContent("", leftB)))
            {
                if (ready2 == true)
                {
                    ready2 = false;
                }
                chosen2 = ChangeActive(false, chosen2, "Player2");
                if (chosen2 != "None")
                {
                    pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));
                }
                else
                {
                    pic2 = (Texture2D)(Resources.Load("Portraits/emptyPic"));
                }
                
            }
            if (GUI.Button(button2R, new GUIContent("", rightB)))
            {
                if (ready2 == true)
                {
                    ready2 = false;

                }
                chosen2 = ChangeActive(true, chosen2, "Player2");
                if (chosen2 != "None")
                {
                    pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));
                }
                else
                {
                    pic2 = (Texture2D)(Resources.Load("Portraits/emptyPic"));
                }
            }

            GUI.DrawTexture(active2, activeButton);

        }
        else
        {
            if (control1a != "null")
            {
                GUI.Label(new Rect(labelPos2.x, Screen.height * 0.65f, labelWidth, labelHeight * 10), controllerLabel, newStyle);
            }
           

            if (mouseAsssigned == false && nextPlayer == 2 && control1a != "null")
            {
                                
                if (control1a != "Mouse&Keyboard" && control3 != "Mouse&Keyboard" && control4 != "Mouse&Keyboard")
                {
                    if (GUI.Button(new Rect(bigBoard2X, Screen.height * 0.55f, labelWidth, labelHeight * 3), mouseKLabel, styleButton))
                    {
         //               Debug.Log(ReInput.players.Players.Count);
                        for (int cnt = 3; cnt > 0; cnt--)
                        {
           //                 Debug.Log(ReInput.players.Players[cnt].name + "/" + player1R.name);
                            if (ReInput.players.Players[cnt] != player1R)
                            {
                                if (player2R == null)
                                {
                //                    Debug.Log("Button");
                                    player2R = ReInput.players.Players[cnt];
                                    ready2 = false;
                                    control2 = "Mouse&Keyboard";
                                    //         activeButton2 = activeButton;
                                    playerId2 = cnt;
                                    controller2 = cnt;
                                     nextPlayer = 3;
                                    pic2 = (Texture2D)(Resources.Load("Portraits/" + chosen2));
                                    mouseAsssigned = true;

                                    //Unassign other potential joysticks to this controller.                               if (player2R.controllers.joystickCount > 0)  //just checking, not needed though
                                    {
                                        foreach (Rewired.Joystick j in player2R.controllers.Joysticks)
                                        {
                                            Debug.Log(j.name);
                                        }
                                    }
                                    player2R.controllers.ClearAllControllers();

                                    //Check that only this player has assigned mouse / keyboard
                                    foreach (Player p in ReInput.players.Players)
                                    {
                                        if (p != player2R)
                                        {
                                            p.controllers.hasKeyboard = false;
                                            p.controllers.hasMouse = false;
                                        }
                                    }
                                    //Assign Mouse&Keyboard to this player
                                    player2R.controllers.hasKeyboard = true;
                                    player2R.controllers.hasMouse = true;
                                }
                            }
                        }
                    }
            
                }
            }
        }
    }

    private void Character3 ()
    {

        //Player3/////////////////////////////////////////////////////////////////////////
        //Arrow buttons

        GUI.Label(new Rect(nameVect3.x, nameVect3.y, Screen.width * 0.20f, Screen.height * 0.39f), profile3, myStyle);

        if (control3 != "null")
        {
            GUI.Label(new Rect(labelPos3.x, labelPos3.y, labelWidth, labelHeight), chosen3, myStyle);
            GUI.Label(new Rect(labelPos3.x, labelPos3.y + labelHeight, labelWidth, labelHeight), DialogueLua.GetActorField(chosen3, "fullName").asString, myStyle);
            GUI.Label(new Rect(labelPos3.x, labelPos3.y + (2 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "race " + language).asString + ": " + DialogueLua.GetActorField(chosen3, "race " + language).asString, myStyle);
            GUI.Label(new Rect(labelPos3.x, labelPos3.y + (3 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "class " + language).asString + ": " + DialogueLua.GetActorField(chosen3, "class " + language).asString, myStyle);
            GUI.Label(new Rect(labelPos3.x, labelPos3.y + (4 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "health " + language).asString + ": " + DialogueLua.GetActorField(chosen3, "health").asString, myStyle);
            GUI.Label(new Rect(labelPos3.x, labelPos3.y + (5 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "armor " + language).asString + ": " + DialogueLua.GetActorField(chosen3, "armor").asString, myStyle);
            GUI.Label(new Rect(labelPos3.x, labelPos3.y + (6 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "damage " + language).asString + ": " + damage3, myStyle);

            GUI.Label(new Rect(labelPos3.x, Screen.height * 0.89f, labelWidth, labelHeight), control3, myStyle);
           
            /*
            if (GUI.Button(configRect3, "CONFIG", styleButton))
            {
                playerConfig = "Player3";
                ActivateConfig();
                showConfig = true;
                GetControllersInfo();
            }*/


            if (GUI.Button(readyRect3, readyString, styleButton))
            {
                bool goodToGo = true;

                if (chosen3 == chosen1 && ready1 == true)
                {
                    goodToGo = false;
                }

                if (chosen3 == chosen2 && ready2 == true)
                {
                    goodToGo = false;
                }

                if (chosen3 == chosen4 && ready4 == true)
                {
                    goodToGo = false;
                }


                if (goodToGo == true)
                {
                    ready3 = !ready3;
                }
                else
                {
                    SendAlertUsedPC("Player3", chosen3);
                }


            }

            if (ready3 == false)
            {
                GUI.DrawTexture(tickBox3, boxUnChecked);

            }
            else
            {
                GUI.DrawTexture(tickBox3, boxChecked);

            }


            if (GUI.Button(button3L, new GUIContent("", leftB)))
            {
                if (ready3 == true)
                {
                    ready3 = false;
                }
                chosen3 = ChangeActive(false, chosen3, "Player3");
                pic3 = (Texture2D)(Resources.Load("Portraits/" + chosen3));
            }
            if (GUI.Button(button3R, new GUIContent("", rightB)))
            {
                if (ready3 == true)
                {
                    ready3 = false;

                }
                chosen3 = ChangeActive(true, chosen3, "Player3");
                pic3 = (Texture2D)(Resources.Load("Portraits/" + chosen3));
            }

            GUI.DrawTexture(active3, activeButton);

        }
        else
        {
            if (control2 != "null")
            {
                GUI.Label(new Rect(labelPos3.x, Screen.height * 0.65f, labelWidth, labelHeight * 10), controllerLabel, newStyle);

            }

            if (mouseAsssigned == false && nextPlayer == 3)
            {
    //            Debug.Log(mouseAsssigned + "/" + nextPlayer);
                if (control1a != "Mouse&Keyboard" && control2 != "Mouse&Keyboard" && control4 != "Mouse&Keyboard")
                {
                    if (GUI.Button(new Rect(labelPos3.x, Screen.height * 0.55f, labelWidth, labelHeight * 3), mouseKLabel, styleButton))
                    {
                        for (int cnt = 3; cnt > 0; cnt--)
                        {
                            //                 Debug.Log(ReInput.players.Players[cnt].name + "/" + player1R.name);
                            if (ReInput.players.Players[cnt] != player1R)
                            {
                                if (player3R == null)
                                {
                                    //                    Debug.Log("Button");
                                    player3R = ReInput.players.Players[cnt];
                                    ready3 = false;
                                    control3 = "Mouse&Keyboard";
                                    //         activeButton2 = activeButton;
                                    playerId3 = cnt;
                                    controller3 = cnt;
                                    nextPlayer = 4;
                                    pic3 = (Texture2D)(Resources.Load("Portraits/" + chosen3));
                                    mouseAsssigned = true;

                                    //Unassign other potential joysticks to this controller.                               if (player2R.controllers.joystickCount > 0)  //just checking, not needed though
                                    {
                                        foreach (Rewired.Joystick j in player3R.controllers.Joysticks)
                                        {
                                            Debug.Log(j.name);
                                        }
                                    }
                                    player3R.controllers.ClearAllControllers();

                                    //Check that only this player has assigned mouse / keyboard
                                    foreach (Player p in ReInput.players.Players)
                                    {
                                        if (p != player3R)
                                        {
                                            p.controllers.hasKeyboard = false;
                                            p.controllers.hasMouse = false;
                                        }
                                    }
                                    //Assign Mouse&Keyboard to this player
                                    player3R.controllers.hasKeyboard = true;
                                    player3R.controllers.hasMouse = true;
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    private void Character4()
    {
        //Player4/////////////////////////////////////////////////////////////////////////
        //Arrow buttons

        GUI.Label(new Rect(nameVect4.x, nameVect4.y, Screen.width * 0.20f, Screen.height * 0.39f), profile4, myStyle);

        if (control4 != "null")
        {
            GUI.Label(new Rect(labelPos4.x, labelPos4.y, labelWidth, labelHeight), chosen4, myStyle);
            GUI.Label(new Rect(labelPos4.x, labelPos4.y + labelHeight, labelWidth, labelHeight), DialogueLua.GetActorField(chosen4, "fullName").asString, myStyle);
            GUI.Label(new Rect(labelPos4.x, labelPos4.y + (2 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "race " + language).asString + ": " + DialogueLua.GetActorField(chosen4, "race " + language).asString, myStyle);
            GUI.Label(new Rect(labelPos4.x, labelPos4.y + (3 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "class " + language).asString + ": " + DialogueLua.GetActorField(chosen4, "class " + language).asString, myStyle);
            GUI.Label(new Rect(labelPos4.x, labelPos4.y + (4 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "health " + language).asString + ": " + DialogueLua.GetActorField(chosen4, "health").asString, myStyle);
            GUI.Label(new Rect(labelPos4.x, labelPos4.y + (5 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "armor " + language).asString + ": " + DialogueLua.GetActorField(chosen4, "armor").asString, myStyle);
            GUI.Label(new Rect(labelPos4.x, labelPos4.y + (6 * labelHeight), labelWidth, labelHeight), DialogueLua.GetActorField("Dictionary", "damage " + language).asString + ": " + damage4, myStyle);

            GUI.Label(new Rect(labelPos4.x, Screen.height * 0.89f, labelWidth, labelHeight), control4, myStyle);
            
            /*
            if (GUI.Button(configRect4, "CONFIG", styleButton))
            {
                playerConfig = "Player4";
                ActivateConfig();
                showConfig = true;
                GetControllersInfo();
            }*/


            if (GUI.Button(readyRect4, readyString, styleButton))
            {
                bool goodToGo = true;

                if (chosen4 == chosen1 && ready1 == true)
                {
                    goodToGo = false;
                }

                if (chosen4 == chosen2 && ready2 == true)
                {
                    goodToGo = false;
                }

                if (chosen4 == chosen3 && ready4 == true)
                {
                    goodToGo = false;
                }


                if (goodToGo == true)
                {
                    ready4 = !ready4;
                }
                else
                {
                    SendAlertUsedPC("Player4", chosen4);
                }


            }

            if (ready4 == false)
            {
                GUI.DrawTexture(tickBox4, boxUnChecked);

            }
            else
            {
                GUI.DrawTexture(tickBox4, boxChecked);

            }


            if (GUI.Button(button4L, new GUIContent("", leftB)))
            {
                if (ready4 == true)
                {
                    ready4 = false;
                }
                chosen4 = ChangeActive(false, chosen4, "Player4");
                pic4 = (Texture2D)(Resources.Load("Portraits/" + chosen4));
            }
            if (GUI.Button(button4R, new GUIContent("", rightB)))
            {
                if (ready4 == true)
                {
                    ready4 = false;

                }
                chosen4 = ChangeActive(true, chosen4, "Player4");
                pic4 = (Texture2D)(Resources.Load("Portraits/" + chosen4));
            }

            GUI.DrawTexture(active4, activeButton);

        }
        else
        {
            if (control3 != "null")
            {
                GUI.Label(new Rect(Screen.width * 0.72f, Screen.height * 0.65f, labelWidth, labelHeight * 10), controllerLabel, newStyle);

            }

            if (mouseAsssigned == false && nextPlayer == 4)
            {
                if (control1a != "Mouse&Keyboard" && control2 != "Mouse&Keyboard" && control3 != "Mouse&Keyboard")
                {
                    if (GUI.Button(new Rect(Screen.width * 0.72f, Screen.height * 0.55f, labelWidth, labelHeight * 3), mouseKLabel, styleButton))
                    {
                        for (int cnt = 3; cnt > 0; cnt--)
                        {
                            //                 Debug.Log(ReInput.players.Players[cnt].name + "/" + player1R.name);
                            if (ReInput.players.Players[cnt] != player1R)
                            {
                                if (player4R == null)
                                {
                                    //                    Debug.Log("Button");
                                    player4R = ReInput.players.Players[cnt];
                                    ready4 = false;
                                    control4 = "Mouse&Keyboard";
                                    //         activeButton2 = activeButton;
                                    playerId4 = cnt;
                                    controller4 = cnt;
                                    nextPlayer = 5;
                                    pic4 = (Texture2D)(Resources.Load("Portraits/" + chosen4));
                                    mouseAsssigned = true;

                                    //Unassign other potential joysticks to this controller.                               if (player2R.controllers.joystickCount > 0)  //just checking, not needed though
                                    {
                                        foreach (Rewired.Joystick j in player4R.controllers.Joysticks)
                                        {
                                            Debug.Log(j.name);
                                        }
                                    }
                                    player4R.controllers.ClearAllControllers();

                                    //Check that only this player has assigned mouse / keyboard
                                    foreach (Player p in ReInput.players.Players)
                                    {
                                        if (p != player4R)
                                        {
                                            p.controllers.hasKeyboard = false;
                                            p.controllers.hasMouse = false;
                                        }
                                    }
                                    //Assign Mouse&Keyboard to this player
                                    player4R.controllers.hasKeyboard = true;
                                    player4R.controllers.hasMouse = true;
                                }
                            }
                        }
                    }

                }
            }
        }
    }


    

    private string ChangeActive (bool right, string active, string player)
    {
        string newActive = null;
        int tempCNT = 0;
        if (right == true)
        {
            for (int cnt = 0; cnt < models.Count; cnt++)
            {

                if (models[cnt] == active)
                {
                    tempCNT = cnt;
                }
            }

      //      Debug.Log(tempCNT);
            if (player == "Player1" & tempCNT == 4)
            {
                tempCNT = 0;
            }
            else if (tempCNT == 5 & player != "Player1")
            {
                tempCNT = 0;
            }
            else
            {
                tempCNT++;
            }

            newActive = models[tempCNT];
   //         Debug.Log(newActive);
            Transform tempPlayer = transform.Find(player + "/PCs");
            foreach (Transform ta in tempPlayer)
            {
                if (ta.name == newActive)
                {
                    ta.gameObject.SetActive(true);
                }
                else
                {
                    if (ta.gameObject.activeSelf)
                    {
                        ta.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {

            for (int cnt = 0; cnt < models.Count; cnt++)
            {

                if (models[cnt] == active)
                {
                    tempCNT = cnt;
                }
            }

            if (tempCNT == 0 & player == "Player1")
            {
                tempCNT = 4;

            }
            else if (tempCNT == 0 & player != "Player1")
            {
                tempCNT = 5;
            }
            else
            {
                tempCNT--;
            }
            newActive = models[tempCNT];
      //      Debug.Log(newActive);
            Transform tempPlayer = transform.Find(player + "/PCs");
            foreach (Transform ta in tempPlayer)
            {
                if (ta.name == newActive)
                {
                    ta.gameObject.SetActive(true);
                }
                else
                {
                    if (ta.gameObject.activeSelf)
                    {
                        ta.gameObject.SetActive(false);
                    }
                }
            }
        }

        if (player == "Player1")
        {
            chosen1 = newActive;
        }
        else if (player == "Player2")
        {
            chosen2 = newActive;
        }
        else if (player == "Player3")
        {
            chosen3 = newActive;
        }
        else if (player == "Player4")
        {
            chosen4 = newActive;
        }

        CalculateDamage();
  //      Debug.Log(newActive);
        return newActive;


    }

    private void CalculateDamage ()
    {
        damage = DialogueLua.GetActorField(chosen1, "minDam").asString + "D" + DialogueLua.GetActorField(chosen1, "maxDam").asString + "+" + DialogueLua.GetActorField(chosen1, "addDam").asString;

        if (chosen2 == "None")
        {
            damage2 = "";
        }
        else
        {
            damage2 = DialogueLua.GetActorField(chosen2, "minDam").asString + "D" + DialogueLua.GetActorField(chosen2, "maxDam").asString + "+" + DialogueLua.GetActorField(chosen2, "addDam").asString;

        }

        if (chosen3 == "None")
        {
            damage3 = "";
        }
        else
        {
            damage3 = DialogueLua.GetActorField(chosen3, "minDam").asString + "D" + DialogueLua.GetActorField(chosen3, "maxDam").asString + "+" + DialogueLua.GetActorField(chosen3, "addDam").asString;

        }

        if (chosen4 == "None")
        {
            damage4 = "";
        }
        else
        {
            damage4 = DialogueLua.GetActorField(chosen4, "minDam").asString + "D" + DialogueLua.GetActorField(chosen4, "maxDam").asString + "+" + DialogueLua.GetActorField(chosen4, "addDam").asString;

        }



    }


    public void SaveData ()
    {

        if (savedData == false)
        {
            string profile = DialogueLua.GetVariable("profile").asString;
            //    Debug.Log(profile);


            GetComponent<SaveGame>().saveAndExit = true;

            string targetScene = DialogueLua.GetVariable("targetScene").asString;
            //   Debug.Log(targetScene);
            if (targetScene == "ArcadeMode" || targetScene == "FPMode")
            {
                GetComponent<SaveGame>().sceneToExit = "ChooseLevel";
            }
            else
            {
                GetComponent<SaveGame>().sceneToExit = sceneTarget;
            }

            GetComponent<SaveGame>().SaveProfile("AutoSave", "ChooseLevel", true);
            savedData = true;
        }


    }

    private void SendAlertUsedPC (string player, string pc)
    {
        if (pc != "None")
        {
            GetComponent<DialogueSystemTrigger>().alertMessage = player + ": " + pc + selectPC;

        }
        else
        {
            GetComponent<DialogueSystemTrigger>().alertMessage = player + ": " + pc +  selectCorrectPC;

        }
        GetComponent<DialogueSystemTrigger>().OnUse();

    }

    private void ActivateConfig ()
    {
        transform.Find("Player1").gameObject.SetActive(false);
        transform.Find("Player2").gameObject.SetActive(false);
    }

    private void GetLanguage ()
    {
        if (language == "es")
        {
            gatherParty = "Debéis reunir a vuestro grupo antes de continuar";
            ventureForth = "¡Continuar!";
            backButton = "VOLVER";
            readyString = "LISTO";
            playerString = "JUGADOR";
            mouseKLabel = "Teclado/Ratón";
            controllerLabel = "Presiona botón 1 de un controlador conectado sin asignar, para asignarlo a este personaje";
            selectPC = ": Debeis seleccionar un personaje que no haya sido ya elegido por otro jugador";
            selectCorrectPC = ": Debeis seleccionar un personaje antes de continuar";
        }

        else if (language == "en")
        {
            gatherParty = "You must gather your party before venturing forth";
            ventureForth = "Venture Forth!";
            backButton = "BACK";
            readyString = "READY";
            playerString = "PLAYER";
            mouseKLabel = "Mouse&Keyboard";
            controllerLabel = "Press Button 1 in an unassigned connected controller, to assign to this character";
            selectPC = ": You need to select a character not selected already by another player";
            selectCorrectPC = ": You need to select a character before venturing forth";

        }
        else if (language == "fr")
        {
            gatherParty = "Vous devez rassembler votre groupe avant de vous aventurer";
            ventureForth = "Aventurez vous!";
            backButton = "QUITTER";
            readyString = "PRET";
            playerString = "JOUEUR";
            mouseKLabel = "Clavier/Souris";
            controllerLabel = "Presiona botón 1 de un controlador conectado sin asignar, para asignarlo a este personaje";
            selectPC = ": Vous devez choisir un personnage non sélectionné par un autre joueur, avant de vous aventurer";
            selectCorrectPC = ": You devez choisir un personnage avant de vous aventurer";

        }
    }

    private void AddjustPCPostions ()
    {


        float pc1Pos = bigBoard1X  * 0.85f;
        float distanceCam = Vector3.Distance(cam.transform.position, player1.transform.position);
        Vector3 pc1Point = new Vector3(pc1Pos, Screen.height * 0.81f, ((distanceCam / 2f)));
        Vector3 pc1Vector = cam.ScreenToWorldPoint(pc1Point);
        player1.transform.position = new Vector3(pc1Vector.x, player1.transform.position.y, player1.transform.position.z);

        float pc2Pos = bigBoard2X * 1.25f;
        float distanceCam2 = Vector3.Distance(cam.transform.position, player2.transform.position);
        Vector3 pc2Point = new Vector3(pc2Pos, Screen.height * 0.81f, ((distanceCam2 / 2f)));
        Vector3 pc2Vector = cam.ScreenToWorldPoint(pc2Point);
        player2.transform.position = new Vector3(pc2Vector.x, player2.transform.position.y, player2.transform.position.z);

        /*
        float pc1Pos = (bigBoard1X * 1.2f)/ Screen.width;
    //    Debug.Log(pc1Pos);
        float distanceCam = Vector3.Distance(cam.transform.position, player1.transform.position);
     //   Vector3 pc1Point = new Vector3(pc1Pos, Screen.height * 0.81f, (cam.nearClipPlane + (distanceCam/2.2f)));
        Vector3 pc1Point = new Vector3(pc1Pos, Screen.height * 0.81f, ( (distanceCam / 2f)));
        Vector3 pc1Vector = cam.ScreenToWorldPoint(pc1Point);
    //    Debug.Log(pc1Vector);
        player1.transform.position = new Vector3(pc1Vector.x, player1.transform.position.y, player1.transform.position.z);

        float pc2Pos = (bigBoard2X + (((float)(bigBoardWidth)) / 2)) / Screen.width;
        float distanceCam2 = Vector3.Distance(cam.transform.position, player2.transform.position);
        //   Vector3 pc1Point = new Vector3(pc1Pos, Screen.height * 0.81f, (cam.nearClipPlane + (distanceCam/2.2f)));
        Vector3 pc2Point = new Vector3(pc2Pos, Screen.height * 0.81f, ((distanceCam2 / 6f)));
        Vector3 pc2Vector = cam.ScreenToWorldPoint(pc2Point);
    //    Debug.Log(pc2Vector);
        player2.transform.position = new Vector3(pc2Vector.x, player2.transform.position.y, player2.transform.position.z);*/

    }

    private void BackToMainMenu ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void VentureForth()
    {

        DialogueLua.SetActorField("Player1", "chosen", chosen1);

        DialogueLua.SetActorField("Player2", "chosen", chosen2);

        DialogueLua.SetActorField("Player3", "chosen", chosen3);

        DialogueLua.SetActorField("Player4", "chosen", chosen4);

        DialogueLua.SetActorField("Player1", "control", control1a);

        DialogueLua.SetActorField("Player2", "control", control2);

        DialogueLua.SetActorField("Player3", "control", control3);

        DialogueLua.SetActorField("Player4", "control", control4);


        DialogueLua.SetActorField("Player1", "controller", playerId1);

        DialogueLua.SetActorField("Player2", "controller", playerId2);

        DialogueLua.SetActorField("Player3", "controller", playerId3);

        DialogueLua.SetActorField("Player4", "controller", playerId4);

        //      Debug.Log(control1 + "/" + control2 + "/" + control3 + "/" + control4);
   //     Debug.Log(chosen1 + "/" + playerId1 + "/" + playerId2 + "/" + playerId3 + "/" + playerId4);
  //      GetComponent<DisplaySkills>().enabled = true;
  //      showSkills = true;
       
        if (DialogueLua.GetVariable("arenaMode").asString == "Yes" ) 
        {
            GetComponent<DisplaySkills>().enabled = true;
            showSkills = true;
        }
        else
        {
            Invoke("SaveData", 0);
        }

       
    }

    //GUI for Server and WIfi players. 
    void StartButtons()
    {
        if (!NetworkClient.active)
        {
            if (serverButtonClick == true ||  GUI.Button(serverRect, "Start Server. Allow Wifi Players. Server IP: " + localIPAddress, otherStyle)) manager.StartServer(); serverButtonClick = false;



        }
        else
        {
            // Connecting
            GUILayout.Label("Connecting to " + manager.networkAddress + "..");
            if ( GUILayout.Button("Cancel Connection Attempt"))
            {
                
                manager.StopClient();
            }
        }
    }

    void StatusLabels()
    {
        // server / client status message
        if (NetworkServer.active)
        {
            GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
        }
        if (NetworkClient.isConnected)
        {
            GUILayout.Label("Client: address=" + manager.networkAddress);
        }
    }

    void StopButtons()
    {
   
        if (NetworkServer.active)
        {

            if (GUI.Button(serverRect, "Stop Server. Server IP: " + localIPAddress, otherStyle))
            {
                manager.StopServer();
            }
        }
    }

    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    public void MobileMessages (string IP, float x, float y, int attack, int block, int jump, int special, int left, int right, string message)
    {
        if (ipAddresses.Contains(IP))
        {
            /*
            if (IP == ip2)
            {
                Controller2("0", attack, x);
            }
            else if (IP == ip3)
            {
                Debug.Log(ip3);
                Controller3("0", attack, x);
            }
            else if (IP == ip4)
            {
                Debug.Log(ip4);
                Controller4("0", attack, x);
            }*/
        }
        else if (control2 == "null")
        {
            if (attack == 1)
            {
                ip2 = IP;
                DialogueLua.SetActorField("Player2", "ip", IP);
                ipAddresses.Add(IP);
                control2 = "mobile";
                DialogueLua.SetActorField("Player2", "control", "mobile");
                profile2 = "Player2/IP:" + ip2;
                active2 = button2R;
                ready2 = false;
                string[] tempControllers = Input.GetJoystickNames();
                
                if (control1a == "Mouse&Keyboard" && tempControllers.Length > 0)
                {
                    controlID = "joystick 1";
                }
                else if (tempControllers.Length > 1)
                {
                    
                    controlID = "joystick 2";
                }
                Invoke("WaitForPlayer3", 0);
                nextPlayer = 3;
            }
        }
        else if (control3 == "null")
        {
            if (attack == 1)
            {
                ip3 = IP;
                ipAddresses.Add(IP);
                DialogueLua.SetActorField("Player3", "ip", IP);
                control3 = "mobile";
                DialogueLua.SetActorField("Player3", "control", "mobile");
                profile3 = "Player3/IP:" + ip3;
                active3 = button3R;
                ready3 = false;
                string[] tempControllers = Input.GetJoystickNames();
                
                if (control1a == "joystick 1" && control2 == "joystick2" && tempControllers.Length > 2)
                {
                    controlID = "joystick 3";
                }
                else if (control1a == "joystick 1" && control2 != "joystick 2" && tempControllers.Length > 1)
                {
                    controlID = "joystick 2";
                }
                else if (control1a == "joystick 1" && control2 == "joystick 2" && tempControllers.Length == 2)
                {
                    controlID = "None";
                }
                else if (control1a == "joystick 1"  && tempControllers.Length == 1)
                {
                    controlID = "None";
                }
                else if (control1a == "Mouse&Keyboard" && control2 == "mobile" && tempControllers.Length > 0)
                {
                    controlID = "joystick 1";
                }
                
                Invoke("WaitForPlayer4", 0);
                nextPlayer = 4;
            }
        }
        else if (control4 == "null")
        {
            if (attack == 1)
            {
                ip4 = IP;
                ipAddresses.Add(IP);
                control4 = "mobile";
                DialogueLua.SetActorField("Player4", "control", "mobile");
                DialogueLua.SetActorField("Player4", "ip", IP);
                profile4 = "Player4/IP:" + ip4;
                active4 = button3R;
                ready4 = false;
                string[] tempControllers = Input.GetJoystickNames();
            }
        }
    }
}

