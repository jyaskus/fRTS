//-----------------------------------------------------------------------------
// GameConnection Methods
// These methods are extensions to the GameConnection class. Extending
// GameConnection make is easier to deal with some of this functionality,
// but these could also be implemented as stand-alone functions.
//-----------------------------------------------------------------------------
// JY v3.3 
//-----------------------------------------------------------------------------

function RTSConnection::getClientIndex(%this)
{
   // Find our index in the client group...
   for  (%i=0; %i<ClientGroup.getCount(); %i++)
      if  (ClientGroup.getObject(%i) == %this)
         return %i;
         
   return -1;
}

function RTSConnection::onClientEnterGame(%this)
{
   commandToClient(%this, 'SyncClock', $Sim::Time - $Game::StartTime);
      
   // -----------------------------------------------------   
      
   // Add to team
   %clientIndex = %this.getClientIndex();
   %this.setTeam(%clientIndex);
   
   $Player::id = %this.getId();
   
   echo("RTSConnection::onClientEnterGame - team assigned =" @ %clientIndex);
  
   // Create simset to track all units
   %this.units = new SimGroup();
    
   // JY - start locations for players (up to 4 including AI)
   // try to read it from the mission file
   switch(%clientIndex)
   {
      case 0:
         if (! (MissionInfo.spawn1 $= ""))
            {
               %startX = getWord(MissionInfo.spawn1, 0);
               %startY = getWord(MissionInfo.spawn1, 1);
            }
         else
            {
                 %startX = -30;
                 %startY =  30;      
            }
            
      case 1:
         if (! (MissionInfo.spawn2 $= ""))
            {
               %startX = getWord(MissionInfo.spawn2, 0);
               %startY = getWord(MissionInfo.spawn2, 1);
            }
         else
            {
                 %startX = -185;
                 %startY = -250;
            }
            
      case 2:
         if (! (MissionInfo.spawn3 $= ""))
            {
               %startX = getWord(MissionInfo.spawn3, 0);
               %startY = getWord(MissionInfo.spawn3, 1);
            }
         else
            {
                 %startX = -185;
                 %startY =  420;
            } 
            
      case 3:
         if (! (MissionInfo.spawn4 $= ""))
            {
               %startX = getWord(MissionInfo.spawn4, 0);
               %startY = getWord(MissionInfo.spawn4, 1);
            }
         else
            {
              %startX =  240;
              %startY = -200;
            }           
   }

      echo("starting location is " @ %startX SPC %startY);                                 
      // -----------------------------------------------------
      // JY - SPAWN UNITS
      
      %offsetX = 30;
      %offsetY = 30;   
      
      // JY - initialize the array holding number of units of each type to build
      for(%i=o; %i < $Game::NumberOfUnits; %i++)
         %units_to_build[%i] = 0;
         
         /*
         0  warrior
         1  knightress    
         2  archer      
         3  villager
         4  beast
         5  goblin
         6  stoneman
         7  guardian
         8  shaman
         9  rat
         */
      
      if (%clientIndex == 0)
         {
            // player
            if ($Game::BUILD_UNITS $= true)
            {
                  //         JY - DEBUG option so NO UNITS ARE BUILT
               %units_to_build[0] = 2;  // warrior
               %units_to_build[1] = 2;  // knightress
               %units_to_build[2] = 4;  // archer
               %units_to_build[3] = 3;  // villager
               %units_to_build[4] = 0;  // beast
               %units_to_build[5] = 0;  // goblin
               %units_to_build[6] = 0;  // stoneman
               %units_to_build[7] = 1;  // guardian
               %units_to_build[8] = 1;  // shaman
               %units_to_build[9] = 1;  // rat
            }
         }
      else
         {
            if ($Game::BUILD_UNITS $= true)
            {
               // AI "orc"          
               %units_to_build[0] = 0;  // warrior
               %units_to_build[1] = 0;  // knightress
               %units_to_build[2] = 0;  // archer
               %units_to_build[3] = 0;  // villager
               %units_to_build[4] = 4;  // beast
               %units_to_build[5] = 2;  // goblin
               %units_to_build[6] = 2;  // stoneman
               %units_to_build[7] = 1;  // guardian
               %units_to_build[8] = 1;  // shaman
               %units_to_build[9] = 1;  // rat
            }
         }
         
      // BUILD out the units      
      for(%i=0; %i < $Game::NumberOfUnits; %i++)
         {
            if (%units_to_build[%i] > 0)
            {
               for (%loop=0; %loop < %units_to_build[%i] ; %loop++)
               {
                  %location = (%startX + %offsetX) SPC (%startY + %offsetY) SPC "250" SPC "0 0" SPC %clientIndex;
                  %this.createPlayer(%location, %i);
                  echo("RTSConnection::onClientEnterGame - creating unit #" SPC %i SPC "at location" SPC (%startX +  %offsetX) SPC (%startY +  %offsetY) SPC "250 for team #" SPC %this.getTeam());
                  %offsetX = %offsetX + 5;
               }
            }
            if (%offsetX > 50)
            {
               %offsetX = 30;
               %offsetY = %offsetY + 5;
            }
         }   
   // -------------------
      
   // Create simset to track selection
   %this.selection = new SimSet();
   
   // Create simset to track buildings
   %this.buildings = new SimGroup();

   // Create an observer cam for fly mode.
   // Create a new camera object.
   %this.observerCam = new Camera() 
   {
      dataBlock = Observer;
   };
   MissionCleanup.add( %this.observerCam );
   
   // Create a new RTS camera object.
   %this.rtsCam = new RTSCamera()
   {
      dataBlock = RTSObserver;
   };
   MissionCleanup.add( %this.rtsCam );

   // Do scoping
   %this.observerCam.scopeToClient(%this);
   %this.rtsCam.scopeToClient(%this);
   // Scope all the static resources.
   // sure we need to do this?
   if(isObject(ResourceSet))
     {
        for(%d = 0; %d < ResourceSet.getCount(); %d++)
        {
          %tr = ResourceSet.getObject(%d);
//          echo("Scoping resource (" @ %tr @ ")");
          %tr.scopeToClient(%this);
        }
   }
   // Start out with the rts cam.
   %this.setControlObject(%this.rtsCam);

//   echo("RTSConnection::onClientEnterGame--setting up resource store");   
   %this.resourceStore = resourceStore::Ctor();
   commandToClient(%this, 'AcceptSetupStores', "LOCAL"); 
   
   // ----------------------------------------
   // JY initialize if first time through -
  
   echo("-----------------------------------------------------");   
   echo("       Welcome to the Fantasy RTS game demo          ");
   echo("-----------------------------------------------------");   
   
   if ($Game::BUILD_BASE $= true)
   {   
      serverCmdPlantBuilding(%this,(%startX) SPC (%startY) SPC "250" SPC "0 0 1 0.8", "townCenterBlock"); // facing northeast
      serverCmdPlantBuilding(%this,(%startX - 35) SPC (%startY + 25) SPC "250" SPC "0 0 1 2.8", "farmBlock"); // facing ?   
      serverCmdPlantBuilding(%this,(%startX + 30) SPC (%startY + 15) SPC "250" SPC "0 0 1 0.5", "barracksBlock"); // facing ?      }  
   }
   
   // LOAD LEVEL AI
   if (( $Game::AI_ON ) && ($Game::AI_LOADED == false))
   {      
      $Game::AI_LOADED = true;
      // this is the ENEMY player ... would could do more,but just this one until all bugs worked out.
      
      if ( $Game::AI_NUM > 0)
      {
         // spawn enemy "AI"
        $EnemyAI=addAI("goblins1");
        $EnemyAI.name = "goblins1";        
        
        // spawn enemy units and base ...
        $EnemyAI.schedule(30000, "onAIClientEnterGame"); 
         // 0, 30s, 60s, 90s delays ...
      }      
      if ( $Game::AI_NUM > 1)
      {
         // spawn enemy "AI"
        $EnemyAI2=addAI("goblins2");
        $EnemyAI2.name = "goblins2";
        
        // spawn enemy units and base ...
        $EnemyAI2.schedule(60000, "onAIClientEnterGame"); 
         // 0, 30s, 60s, 90s delays ...
      }
      if ( $Game::AI_NUM > 2)
      {
         // spawn enemy "AI"
        $EnemyAI3=addAI("goblins3");
        $EnemyAI3.name = "goblins3";
        
        // spawn enemy units and base ...
        $EnemyAI3.schedule(90000, "onAIClientEnterGame"); 
         // 0, 30s, 60s, 90s delays ...
      }            
   }
}
// --------------------------------------------------
// For AI - so it dont stomp on players
function RTSConnection::onAIClientEnterGame(%this)
{     
   echo("\c2 RTSConnection::onAIClientEnterGame -----------------");
   
   // Add to team
   %clientIndex = %this.getClientIndex();
   %this.setTeam(%clientIndex);
   echo("\c2 RTSConnection::onAIClientEnterGame - team assigned =" @ %clientIndex);
  
   // Create simset to track all this players units
   %this.units = new SimGroup();
    
   // JY - start locations for players (up to 4 including AI)   
   
   // default
   %startX =  240;
   %startY = -200;
   
   // try to read it from the mission file
   switch(%clientIndex)
   {
      case 1:
         if (! (MissionInfo.spawn2 $= ""))
            {
               %startX = getWord(MissionInfo.spawn2, 0);
               %startY = getWord(MissionInfo.spawn2, 1);
            }
         else
            {
                 %startX = -185;
                 %startY = -250;
            }            
      case 2:
         if (! (MissionInfo.spawn3 $= ""))
            {
               %startX = getWord(MissionInfo.spawn3, 0);
               %startY = getWord(MissionInfo.spawn3, 1);
            }
         else
            {
                 %startX = -185;
                 %startY =  420;
            } 
      case 3:      
         if (! (MissionInfo.spawn4 $= ""))
            {
               %startX = getWord(MissionInfo.spawn4, 0);
               %startY = getWord(MissionInfo.spawn4, 1);
            }
         else
            {
              %startX =  240;
              %startY = -200;
            }           
   }

      echo("\c2 AI starting location is " @ %startX SPC %startY);                                 
      // -----------------------------------------------------
//      MapHud.createPingEvent("-80 0", "1 0 0"); // red
//      MapHud.createPingEvent("-80 0", "0 1 0"); // green
//      MapHud.createPingEvent("-80 0", "0 0 1"); // blue
      MapHud.createPingEvent(%startX SPC %startY, "1 0 0"); // red 
          
      // JY - initialize the array holding number of units of each type to build
      for(%i=o; %i < $Game::NumberOfUnits; %i++)
         %units_to_build[%i] = 0;
               
      if ($Game::BUILD_UNITS $= true)
      {
          // AI "orc"          
          %units_to_build[0] = 0;  // warrior
          %units_to_build[1] = 0;  // knightress
          %units_to_build[2] = 0;  // archer
          %units_to_build[3] = 0;  // villager
          %units_to_build[4] = 3;  // beast
          %units_to_build[5] = 1;  // goblin
          %units_to_build[6] = 1;  // stoneman
          %units_to_build[7] = 1;  // guardian
          %units_to_build[8] = 1;  // shaman     
          %units_to_build[9] = 1;  // rat     
      }
         
      
      // set the initial offset from the starting point
      %offsetX = 30;
      %offsetY = 30; 
      
      // JY - SPAWN UNITS
      for(%i=0; %i < $Game::NumberOfUnits; %i++)
         {
            if (%units_to_build[%i] > 0)
            {
               for (%loop=0; %loop < %units_to_build[%i] ; %loop++)
               {
                  %location = (%startX + %offsetX) SPC (%startY + %offsetY) SPC "250" SPC "0 0" SPC %clientIndex;
                  %this.createPlayer(%location, %i);
//                  %this.schedule(5000,%this.createPlayer,%location,%i);
                  echo("\c2 RTSConnection::onAIClientEnterGame - creating unit #" SPC %i SPC "at location" SPC (%startX +  %offsetX) SPC (%startY +  %offsetY) SPC "250 for team #" SPC %this.getTeam());
                  %offsetX = %offsetX + 5;
               }
            }
            if (%offsetX > 50)
            {
               %offsetX = 30;
               %offsetY = %offsetY + 5;
            }
         }   
   // -------------------
      
   // Create simset to track selection
   %this.selection = new SimSet();
   
   // Create simset to track buildings
   %this.buildings = new SimGroup();

   // Create an observer cam for fly mode.
   // Create a new camera object.
   %this.observerCam = new Camera() 
   {
      dataBlock = Observer;
   };
   MissionCleanup.add( %this.observerCam );
   
   // Create a new RTS camera object.
   %this.rtsCam = new RTSCamera()
   {
      dataBlock = RTSObserver;
   };
   MissionCleanup.add( %this.rtsCam );

   // Do scoping
   %this.observerCam.scopeToClient(%this);
   %this.rtsCam.scopeToClient(%this);
   
   // Scope all the static resources.
   // sure we need to do this?
   if(isObject(ResourceSet))
     {
        for(%d = 0; %d < ResourceSet.getCount(); %d++)
        {
          %tr = ResourceSet.getObject(%d);
//          echo("Scoping resource (" @ %tr @ ")");
          %tr.scopeToClient(%this);
        }

   }
   // Start out with the rts cam.
   %this.setControlObject(%this.rtsCam);

//   echo("RTSConnection::onClientEnterGame--setting up resource store");   
/*
   %this.resourceStore = resourceStore::Ctor();
   commandToClient(%this, 'AcceptSetupStores', "LOCAL"); 
*/   
   // ----------------------------------------
   // JY initialize if first time through -
  
   if ($Game::BUILD_BASE $= true)
   {   
      serverCmdPlaceAiBuilding(%this,(%startX) SPC (%startY) SPC "250" SPC "0 0 1 0.8", "orcTownCenterBlock"); // facing northeast
      serverCmdPlaceAiBuilding(%this,(%startX - 35) SPC (%startY + 25) SPC "250" SPC "0 0 1 0.1", "orcFarmBlock"); // facing ?   
      serverCmdPlaceAiBuilding(%this,(%startX + 35) SPC (%startY + 15) SPC "250" SPC "0 0 1 0.4", "orcBarracksBlock"); // facing ?         \
      // JY TODO -- .schedule functions to spawn more units until the buildings destroyed
   }  
}
// --------------------------------------------------
function RTSConnection::onClientLeaveGame(%this)
{
   if (isObject(%this.observerCam))
      %this.observerCam.delete();
   if (isObject(%this.rtsCam))
      %this.rtsCam.delete();
   if (isObject(%this.selection))
      %this.selection.delete();
      
   // exit if object already doesnt exist
   if (!isObject(%this.units))
      return;
   // Delete all the player buildings and units? Sure why not.
   // You could also turn them over to allies or something here
   
   for( %i = 0; %i < %this.units.getCount(); %i++ )
   {
      %unit = %this.units.getObject(%i);

      // Kind of hacky here
      RTSUnitData::onDisabled( 0, %unit, 0 );
      // SRZ: this looks like a mem leak--shouldn't we delete it?
      echo( "Client has disconnected, removing unit " @ %unit );
      // Let them die naturally - BJG  
      //%unit.delete();
   }


   for ( %i = 0; %i < %this.buildings.getCount(); %i++ )
   {
   	%unit = %this.buildings.getObject(%i);
   	RTSUnitData::onDisabled( 0, %unit, 0 );
   }
   
// SRZ: Tier2 Imp needed: We want to come up with a mechanism to handle buildings/units
// with no controlling connection, so we can leave them around and active without a controlling %client

   %this.buildings.delete();
      
   if ( isObject( %this.resourceStore ) )
   {
   	  for ( %i = 0; %i < %this.resourceStore.getCount(); %i++ )
   	  {
   	  	%resource = %this.resourceStore.getObject(%i);
   	  	%resource.delete();
   	  }
   
   	%this.resourceStore.delete();
   }
}

//-----------------------------------------------------------------------------

function RTSConnection::createPlayer(%this, %spawnPoint, %index)
{
   echo("\c2 RTSConnection::createPlayer()-------------------------------------------");
   echo ("\c2 this (client?) is (" @ %this @ ") unit type is (" @ %index @ ")");
   %scale = "1 1 1";
   switch(%index)
   {
      case 0:
         %data = warriorBlock;
         %scale = "1.45 1.45 1.4";
      case 1:
         %data = knightressBlock;
         %scale = "1.2 1.2 1.25";
      case 2:
         %data = archerBlock;
         %scale = "0.8 0.8 0.8";         
      case 3:
         %data = villagerBlock;
      case 4:
         %data = beastBlock;
         %scale = "1.2 1.2 1.1";
      case 5:
         %data = goblinBlock;
         %scale = "0.9 0.9 0.9";
      case 6:
         %data = stonemanBlock;
         %scale = "1.6 1.6 1.5";
      case 7:
         %data = guardianBlock;
         %scale = "0.65 0.65 0.65";         
      case 8:
         %data = shamanBlock;
         %scale = "0.55 0.55 0.55";                  
      case 9:
         %data = ratBlock;
         %scale = "0.55 0.55 0.55";                           
   }
      
   %player = new RTSUnit() 
   {
      dataBlock = %data;
      scale = %scale;
//      shapeFile = "~/game/data/players/player/player.dts";
      path = "";
   };
   echo("\c2 Created new %player" SPC %player SPC "for Connection (" @ %this @") dataBlock =" SPC %player.getDataBlock().getName() @ "Team=" @ %this.getTeam() );
   
   %player.setControllingConnection(%this);
   %player.setSkinName( getWord($Pref::Server::TeamInfo[%this.getTeam()], 3) );
   %player.setTeam(%this.getTeam());
   %player.setTransform(%spawnPoint);
   %player.client = %this;
   %player.maxAmt = 25;                    
   %player.stats["Kills"] = 0;            //(getRandom() * 5) % 5;      // ? how to add these in game ? JY
   %player.stats["Damage Delt"] = 0;      //(getRandom() * 1000) % 1000;
//   echo("\c2 stats: Kills and Damage Delt for unit set to (0)");
   
   //keep track of units built per client
   %this.stats["Units Built"]++;
   
   // Add the unit to the group of units
   %this.units.add(%player);
   
   // only for AI controlled players --- set the scan    
   %maxRange = %player.getDatablock().mVision / 25;
   if (%maxRange < 1)
      %maxRange = 1;       // min distance for scans  ... 25 vision  = 1 WU range
   if (%maxRange > 10)                           //   ... 100 vision = 4 WU range
      %maxRange = 10;      // max safe                ... 250 vision = 10 WU range
//   echo("\c2  Player on create - scan range set to " @ %maxRange);
   
   // JY - so ignore PEONS -- they will stand around and do thier own thing
   if (%index > 3)
      {
         echo("\c2 scheduling this unit to scan - range=" @ %maxRange);
         %delay= (30 * %index) * 1000; // 120s/150s/180s delay to keep it all random .. 1-3 minutes, so they dont attack in waves
         %player.schedule(%delay,"startscan",%maxRange); 
      }

   echo("\c2 RTSConnection::createPlayer ------------------------------------------");
}

//-----------------------------------------------------------------------------

function RTSConnection::onLeaveMissionArea(%this)
{
   // The control objects invoked this method when they
   // move out of the mission area.
}

function RTSConnection::onEnterMissionArea(%this)
{
   // The control objects invoked this method when they
   // move back into the mission area.
}

// JY - from starter.fps
function RTSConnection::onDeath(%this, %sourceObject, %sourceClient, %damageType, %damLoc)
{
   echo (" RTSConnection::onDeath - updating source client stats and score");
   
   // JY - increment kill stats for client
   %sourceClient.stats["Kills"]++; // this correct? or credit the wrong client ?

   // Doll out points and display an appropriate message
   if (%damageType $= "Suicide" || %sourceClient == %this) 
   {
      %this.incScore(-1);
      //messageAll('MsgClientKilled','%1 takes his own life!',%this.name);
   }
   else 
   { 
      %sourceClient.incScore(1);      
      if (%sourceClient.score >= $Game::EndGameScore)
         {
            messageAll('MsgClientKilled','%1 gets nailed by %2!',%this.name,%sourceClient.name);
            //cycleGame();
            endGame();
         }
   }   
}