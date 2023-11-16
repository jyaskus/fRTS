//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//  Functions that implement game-play
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function onServerCreated()
{
   // Server::GameType is sent to the master server.
   // This variable should uniquely identify your game and/or mod.
   $Server::GameType = "RTS Starter Kit";

   // Server::MissionType sent to the master server.  Clients can
   // filter servers based on mission type.
   $Server::MissionType = "FFA";

   // GameStartTime is the sim time the game started. Used to calculated
   // game elapsed time.
   $Game::StartTime = 0;

   // Load up all datablocks, objects etc.  This function is called when
   // a server is constructed.
   exec("~/server/scripts/audio/audioProfiles.cs");
   exec("~/server/scripts/fx/environment.cs");     // JY -- seems like this is needed for precipitation to work 
   exec("~/server/scripts/fx/chimneyfire.cs");     // JY -- for smoke effects  
   exec("~/server/scripts/audio/player.cs");
   exec("~/server/scripts/fx/player.cs");
   exec("~/server/scripts/avatars/base.cs");
   exec("~/server/scripts/avatars/pathManager.cs");
   exec("~/server/scripts/avatars/player.cs");
   exec("~/server/scripts/avatars/villager.cs");   // JY - merging in world dom mod
   exec("~/server/scripts/avatars/warrior.cs");    // new units - JY
   exec("~/server/scripts/avatars/guardian.cs");   // new units - JY   dungeon guardian
   exec("~/server/scripts/avatars/rat.cs");        // RAT!  
   exec("~/server/scripts/avatars/archer.cs");     // JY    
   exec("~/server/scripts/avatars/knightress.cs"); // new units - JY  
   exec("~/server/scripts/avatars/beast.cs");      // new units - JY  
   exec("~/server/scripts/avatars/goblin.cs");     // JY Archer
   exec("~/server/scripts/avatars/stoneman.cs");   // JY Archer   
   exec("./centerPrint.cs");
   exec("./commands.cs");
   exec("./game.cs");
   exec("./centerPrint.cs");
   exec("./commands.cs");
   exec("./gameConnection.cs");
   exec("./item.cs");
   exec("./radiusDamage.cs");
   exec("./shapeBase.cs");
   exec("./teams.cs");
   exec("./triggers.cs");
   exec("./weapon.cs");
   exec("./buffs.cs");
   exec("~/server/scripts/globals.cs");
   exec("~/server/scripts/items/camera.cs");
   exec("~/server/scripts/items/crossbow.cs");
   exec("~/server/scripts/items/equipment.cs"); // JY
   exec("~/server/scripts/items/staticShape.cs");
   exec("~/server/scripts/items/building.cs");
   exec("common/server/lightingSystem.cs");
   exec("~/server/scripts/resources/resourceGeneration.cs"); // JY - world dom mod

   exec("~/server/scripts/fx/gameMgr.cs");     // JY -- game manager 
   
   // Keep track of when the game started
   $Game::StartTime = $Sim::Time;
}

function onServerDestroyed()
{
   // This function is called as part of a server shutdown.
}


//-----------------------------------------------------------------------------

function onMissionLoaded()
{
   // Called by loadMission() once the mission is finished loading.
   
   if(!isObject(ResourceSet))
   {
     new SimSet(ResourceSet);
     seedResource(Tree,80); // resource type,   number to create
     seedResource(Stone,60);
     seedResource(Gold,40);
   }

   startGame();
}

function onMissionEnded()
{
   // Called by endMission(), right before the mission is destroyed

   // Normally the game should be ended first before the next
   // mission is loaded, this is here in case loadMission has been
   // called directly.  The mission will be ended if the server
   // is destroyed, so we only need to cleanup here.
   cancel($Game::Schedule);
   $Game::Running = false;
   $Game::Cycling = false;
}


//-----------------------------------------------------------------------------

function startGame()
{
   if ($Game::Running) 
   {
      error("startGame: End the game first!");
      return;
   }

   // Inform the client we're starting up
   for( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ ) {
      %cl = ClientGroup.getObject( %clientIndex );
      commandToClient(%cl, 'GameStart');

      // Other client specific setup..
      %cl.score = 0;
   }

   // Start the game timer
   if ($Game::Duration)
      $Game::Schedule = schedule($Game::Duration * 1000, 0, "onGameDurationEnd" );
   $Game::Running = true;
   
   
    // Start the GameManager
    new ScriptObject(GameManager) {};
    MissionCleanup.add(GameManager);
    GameManager.init();
   
   // see if the mission file has music defined
   //for winter storm ambient sounds
   //new AudioProfile(wasteAmbient)
   %music = MissionInfo.music;
   if (!(%music $= ""))
   {
      // setup mission music      
      // JY - if we want to stop the music ... otherwise it blends with the ambient sounds
      alxStopAll();
      //alxStop($MusicAudioType); // channel 4
      echo("startGame:: playing music found in mission file:" SPC %music);
      alxPlay(%music);      
   }

}

function endGame()
{
   if (!$Game::Running)  {
      error("endGame: No game running!");
      return;
   }

   // Stop any game timers
   cancel($Game::Schedule);

   // Inform the client the game is over
   for( %clientIndex = 0; %clientIndex < ClientGroup.getCount(); %clientIndex++ ) {
      %cl = ClientGroup.getObject( %clientIndex );
      commandToClient(%cl, 'GameEnd');
   }

   // cleanup game manager
   GameManager.delete();
   
   // Delete all the temporary mission objects
//   resetMission();
   $Game::Running = false;
}

function onGameDurationEnd()
{
   // This "redirect" is here so that we can abort the game cycle if
   // the $Game::Duration variable has been cleared, without having
   // to have a function to cancel the schedule.
   if ($Game::Duration && !isObject(EditorGui))
      cycleGame();
}


//-----------------------------------------------------------------------------

function cycleGame()
{
   // This is setup as a schedule so that this function can be called
   // directly from object callbacks.  Object callbacks have to be
   // carefull about invoking server functions that could cause
   // their object to be deleted.
   if (!$Game::Cycling) 
   {
      $Game::Cycling = true;
      $Game::Schedule = schedule(0, 0, "onCycleExec");
   }
}

function onCycleExec()
{
   // End the current game and start another one, we'll pause for a little
   // so the end game victory screen can be examined by the clients.
   endGame();
   $Game::Schedule = schedule($Game::EndGamePause * 1000, 0, "onCyclePauseEnd");
}

function onCyclePauseEnd()
{
   $Game::Cycling = false;

   // Just cycle through the missions for now.
   %search = $Server::MissionFileSpec;
   for (%file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search)) {
      if (%file $= $Server::MissionFile) {
         // Get the next one, back to the first if there
         // is no next.
         %file = findNextFile(%search);
         if (%file $= "")
           %file = findFirstFile(%search);
         break;
      }
   }
   loadMission(%file);
}

//JY - Setup AI for Attack  
function AISetToAttack(%obj,%sourceObject)
{   
    echo( "AISetToAttack (" SPC %obj.getClassName() @ "," @ %obj.getRTSUnitTypeName() @ ":" @ %sourceObject.getClassName() @ "," @ %obj.getRTSUnitTypeName() SPC ")" );
    %obj.stopAll();  
    %obj.setAimObject(%sourceObject);  
    %obj.getDatablock().onAttack(%obj,%sourceObject);  
}  