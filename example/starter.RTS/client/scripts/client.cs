//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Server Admin Commands
//-----------------------------------------------------------------------------

function SAD(%password)
{
   if (%password !$= "")
      commandToServer('SAD', %password);
}

function SADSetPassword(%password)
{
   commandToServer('SADSetPassword', %password);
}


//----------------------------------------------------------------------------
// Misc server commands
//----------------------------------------------------------------------------

function clientCmdSyncClock(%time)
{
   // Time update from the server, this is only sent at the start of a mission
   // or when a client joins a game in progress.
}


// ----------------------------- //
// Pause functions
// ----------------------------- //
$gamePaused = 0;

function windowFocusChanged(%state)
{
   error("windowFocusChanged" SPC %state);
   if(%state == 0)
   {
      // other gui stuff goin on here.. like push-in pause gui message.
      echo ("PAUSE**");
      $timescale = 0;
 //     Clock.PauseTime(true);
   }
   else
   {
      // other gui stuff goin on here.. like pop-in pause gui message.
      echo ("PLAY**");
      $timescale = 1;
//      Clock.PauseTime(false);
   }
}

function pauseGame(%state)
{
   error("windowFocusChanged" SPC %state);
   if(%state == 0)
   {
      if ($gamePaused)
      {
         // other gui stuff goin on here.. like push-in pause gui message.
         echo ("UNPAUSE**");
         $timescale = 1;
 //        Clock.PauseTime(false);
	     $gamePaused = 0;
      }
      else
      {
         echo ("PAUSE**");
         $timescale = 0;
   //      Clock.PauseTime(true);
       	 $gamePaused = 1;
      }
   }
}