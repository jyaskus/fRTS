//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

// Variables used by server scripts & code.  The ones marked with (c)
// are accessed from code.  Variables preceeded by Pref:: are server
// preferences and stored automatically in the ServerPrefs.cs file
// in between server sessions.
//
//    (c) Server::ServerType              {SinglePlayer, MultiPlayer}
//    (c) Server::GameType                Unique game name
//    (c) Server::Dedicated               Bool
//    ( ) Server::MissionFile             Mission .mis file name
//    (c) Server::MissionName             DisplayName from .mis file
//    (c) Server::MissionType             Not used
//    (c) Server::PlayerCount             Current player count
//    (c) Server::GuidList                Player GUID (record list?)
//    (c) Server::Status                  Current server status
//
//    (c) Pref::Server::Name              Server Name
//    (c) Pref::Server::Password          Password for client connections
//    ( ) Pref::Server::AdminPassword     Password for client admins
//    (c) Pref::Server::Info              Server description
//    (c) Pref::Server::MaxPlayers        Max allowed players
//    (c) Pref::Server::RegionMask        Registers this mask with master server
//    ( ) Pref::Server::BanTime           Duration of a player ban
//    ( ) Pref::Server::KickBanTime       Duration of a player kick & ban
//    ( ) Pref::Server::MaxChatLen        Max chat message len
//    ( ) Pref::Server::FloodProtectionEnabled Bool

//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------

function initServer()
{

   echo("\n--------- Initializing MOD: RTS Starter Kit: Server ---------");

   // Server::Status is returned in the Game Info Query and represents the
   // current status of the server. This string sould be very short.
   $Server::Status = "Unknown";

   // Turn on testing/debug script functions
   $Server::TestCheats = false;

   // Specify where the mission files are.
   $Server::MissionFileSpec = "*/missions/*.mis";

   // The common module provides the basic server functionality
   initBaseServer();

   // Load up game server support scripts
   exec("./scripts/audio/audioProfiles.cs");
   exec("./scripts/audio/player.cs");
   exec("./scripts/fx/player.cs");
   exec("./scripts/fx/environment.cs");
   exec("./scripts/fx/gameMgr.cs");       // JY - game manager
   exec("./scripts/fx/chimneyfire.cs");
   exec("./scripts/avatars/base.cs");
   exec("./scripts/avatars/player.cs");
   exec("./scripts/avatars/warrior.cs");     // JY Warrior
   exec("./scripts/avatars/archer.cs");      // JY Archer
   exec("./scripts/avatars/knightress.cs");  // JY Knightress
   exec("./scripts/avatars/beast.cs");     // JY beast
   exec("./scripts/avatars/guardian.cs");     // JY goblin dungeon guardian   
   exec("./scripts/avatars/shaman.cs");     // JY goblin dungeon guardian   
   exec("./scripts/avatars/rat.cs");     // JY goblin dungeon guardian   
      
   exec("./scripts/avatars/goblin.cs");      // JY Archer
   exec("./scripts/avatars/stoneman.cs");      // JY Archer
   
   exec("./scripts/core/centerPrint.cs");
   exec("./scripts/core/commands.cs");
   exec("./scripts/core/gameConnection.cs");
   exec("./scripts/core/item.cs");
   exec("./scripts/core/radiusDamage.cs");
   exec("./scripts/core/shapeBase.cs");
   exec("./scripts/core/teams.cs");
   exec("./scripts/core/triggers.cs");
   exec("./scripts/core/weapon.cs");
   exec("./scripts/core/stats.cs");
   exec("./scripts/core/buffs.cs");
   exec("./scripts/globals.cs");

   exec("./scripts/items/camera.cs");
   exec("./scripts/items/crossbow.cs");
   exec("./scripts/items/equipment.cs");   // JY
   exec("./scripts/items/staticShape.cs");
   exec("./scripts/items/building.cs");
   
   exec("./scripts/resources/resources-server.cs");   // world dom mod - JY
   
   exec("./scripts/core/game.cs");
}


//-----------------------------------------------------------------------------

function initDedicated()
{
   enableWinConsole(true);
   echo("\n--------- Starting Dedicated Server ---------");

   // Make sure this variable reflects the correct state.
   $Server::Dedicated = true;

   // The server isn't started unless a mission has been specified.
   if ($missionArg !$= "") {
      createServer("MultiPlayer", $missionArg);
   }
   else
      echo("No mission specified (use -game filename)");
}

function debugTxt( %text )
{
   // only display in debug mode ... otherwise, dont  
   if (! (getBuildString() $= "Debug"))
      return;
      
   if ($DEBUG == false)
      return;
      
   // else
   echo( %text );   
}