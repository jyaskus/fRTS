//-----------------------------------------------------------------------------

// Variables used by client scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are client
// preferences and stored automatically in the ~/client/prefs.cs file
// in between sessions.
//
//    (c) Client::MissionFile             Mission file name
//    ( ) Client::Password                Password for server join

//    (?) Pref::Player::CurrentFOV
//    (?) Pref::Player::DefaultFov
//    ( ) Pref::Input::KeyboardTurnSpeed

//    (c) pref::Master[n]                 List of master servers
//    (c) pref::Net::RegionMask     
//    (c) pref::Client::ServerFavoriteCount
//    (c) pref::Client::ServerFavorite[FavoriteCount]
//    .. Many more prefs... need to finish this off

// Moves, not finished with this either...
//    (c) firstPerson
//    $mv*Action...

//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function initClient()
{
   echo("\n--------- Initializing MOD: RTS Starter Kit: Client ---------");

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = false;

   // Game information used to query the master server
   $Client::GameTypeQuery = "RTS Starter Kit";
   $Client::MissionTypeQuery = "FFA";

   // The common module provides basic client functionality
   initBaseClient();

   // InitCanvas starts up the graphics system.
   // The canvas needs to be constructed before the gui scripts are
   // run because many of the controls assume the canvas exists at
   // load time.
   initCanvas("RTS");
   
   // override the base profiles if necessary
   exec("./ui/customProfiles.cs");

   // Load up the Game GUIs
   exec("./ui/defaultGameProfiles.cs");

   // for displaying stats -- JY -- with "F1" key
   exec("./ui/statsGui.gui"); 

   /// Load client-side Audio Profiles/Descriptions
   exec("./scripts/audioProfiles.cs");
   
   exec("./scripts/centerPrint.cs");
   exec("./scripts/chatHud.cs");
   exec("./scripts/rtsEvents.cs");
   exec("./scripts/client.cs");
   exec("./scripts/default.bind.cs");
   exec("./scripts/game.cs");
   exec("./scripts/loadingGui.cs");
   exec("./scripts/messageHud.cs");
   exec("./scripts/missionDownload.cs");
   exec("./scripts/optionsDlg.cs");
   exec("./scripts/playerList.cs");
   exec("./scripts/inputHandler.cs");
   exec("./scripts/selection.cs");
   exec("./scripts/playGui.cs");
   exec("./scripts/serverConnection.cs");
   exec("./scripts/groupManager.cs");
   exec("./scripts/stats.cs");
   exec("./scripts/mapHud.cs");
   
   // world dom mod
   exec("./scripts/resources-client.cs");
   exec("./scripts/buildings.cs");  

   exec("~/main.cs");

   exec("./ui/aboutDlg.gui");
   exec("./ui/chatHud.gui");
   exec("./ui/endGameGui.gui");
   exec("./ui/joinServerGui.gui");
   exec("./ui/loadingGui.gui");
   exec("./ui/LobbyGui.gui");
   exec("./ui/mainMenuGui.gui");
   exec("./ui/optionsDlg.gui");
   exec("./ui/playerList.gui");
   
   // JY - eliminating console errors
   if(!isObject(NormalTextProfile))       new GuiControlProfile(NormalTextProfile: GuiTextProfile)      { fontColor = "0 255 0"; }; 
   if(!isObject(GettingLowTextProfile))   new GuiControlProfile(GettingLowTextProfile: GuiTextProfile)  { fontColor = "128 128 0"; };
   exec("./ui/playGui.gui");
   exec("./ui/remapDlg.gui");
   exec("./ui/startMissionGui.gui");
   exec("./ui/StartupGui.gui");
   exec("./ui/DemoPlayGuiOverlay.gui");
   
   exec("./config.cs");

   // Really shouldn't be starting the networking unless we are
   // going to connect to a remote server, or host a multi-player
   // game.
   setNetPort(0);

   // Copy saved script prefs into C++ code.
   setShadowDetailLevel( $pref::shadows );
   setDefaultFov( $pref::Player::defaultFov );
   setZoomSpeed( $pref::Player::zoomSpeed );

   // Joystick magic
   $enableDirectInput = "1";
   activateDirectInput();
   enableJoystick();
   
   // Start up the main menu... this is separated out into a 
   // method for easier mod override.

   if ($JoinGameAddress !$= "") 
   {
      // If we are instantly connecting to an address, load the
      // main menu then attempt the connect.
      loadMainMenu();
      %conn = new RTSConnection(ServerConnection);
      %conn.setConnectArgs($pref::Player::Name);
      %conn.setJoinPassword($Client::Password);
      %conn.connect($JoinGameAddress);
   }
   else 
   {
      // Otherwise go to the splash screen.
      Canvas.setCursor("DefaultCursor");
      loadStartup();
   }
}


//-----------------------------------------------------------------------------


function loadMainMenu()
{
   // Startup the client with the Main menu...
   Canvas.setContent( MainMenuGui );


   // Make sure the audio initialized.
   if($Audio::initFailed) {
      MessageBoxOK("Audio Initialization Failed", 
         "The OpenAL audio system failed to initialize.  " @
         "You can get the most recent OpenAL drivers <a:www.garagegames.com/docs/torque/gstarted/openal.html>here</a>.");
   }

   Canvas.setCursor("DefaultCursor");
}


