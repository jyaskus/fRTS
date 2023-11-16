//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

if ( isObject( moveMap ) )
   moveMap.delete();
new ActionMap(moveMap);


//------------------------------------------------------------------------------
// Non-remapable binds
//------------------------------------------------------------------------------

function escapeFromGame()
{
   if ( $Server::ServerType $= "SinglePlayer" )
      MessageBoxYesNo( "Quit Mission", "Exit from this Mission?", "disconnect();", "");
   else
      MessageBoxYesNo( "Disconnect", "Disconnect from the server?", "disconnect();", "");
}

moveMap.bindCmd(keyboard, "escape", "", "escapeFromGame();");

function showPlayerList(%val)
{
   if (%val)
      PlayerListGui.toggle();
}

moveMap.bind( keyboard, F2, showPlayerList );


//------------------------------------------------------------------------------
// Movement Keys
//------------------------------------------------------------------------------

$movementSpeed = 1; // m/s

function setSpeed(%speed)
{
   if(%speed)
      $movementSpeed = %speed;
}

function moveleft(%val)
{
   $mvLeftAction = %val * $movementSpeed;
}

function moveright(%val)
{
   $mvRightAction = %val * $movementSpeed;
}

function moveforward(%val)
{
   $mvForwardAction = %val * $movementSpeed;
}

function movebackward(%val)
{
   $mvBackwardAction = %val * $movementSpeed;
}

function moveup(%val)
{
   $mvUpAction = %val * $movementSpeed;
}

function movedown(%val)
{
   $mvDownAction = %val * $movementSpeed;
}

function turnLeft( %val )
{
   $mvYawRightSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function turnRight( %val )
{
   $mvYawLeftSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panUp( %val )
{
   $mvPitchDownSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panDown( %val )
{
   $mvPitchUpSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function getMouseAdjustAmount(%val)
{
   // based on a default camera fov of 90'
   return(%val * ($cameraFov / 90) * 0.01);
}

function yaw(%val)
{
   $mvYaw += getMouseAdjustAmount(%val);
}

function pitch(%val)
{
   $mvPitch += getMouseAdjustAmount(%val);
}

function jump(%val)
{
   $mvTriggerCount2++;
}

function joyPitch(%val)
{
   $mvPitchUpSpeed = %val;
}

function joyYaw(%val)
{
   $mvYawLeftSpeed = %val;
}

moveMap.bind( keyboard, left, moveleft );
moveMap.bind( keyboard, right, moveright );
moveMap.bind( keyboard, up, moveforward );
moveMap.bind( keyboard, down, movebackward );
//moveMap.bind( keyboard, space, jump );
moveMap.bind( mouse, xaxis, yaw);
moveMap.bind( mouse, yaxis, pitch );
moveMap.bind( mouse, zaxis, updateOrbitDistance );
moveMap.bind( mouse, "ctrl zaxis", updateViewPitch );
moveMap.bind( mouse, "alt zaxis",  updateViewYaw);
moveMap.bindCmd( keyboard, home, "updateViewPitch(120);", "");
moveMap.bindCmd( keyboard, end, "updateViewPitch(-120);", "");
moveMap.bind( joystick0, yaxis, "ds", "-0.005 0.005", "0.05", joyPitch );
moveMap.bind( joystick0, xaxis, "ds", "-0.005 0.005", "0.05", joyYaw);


//------------------------------------------------------------------------------
// Mouse Trigger
//------------------------------------------------------------------------------

function mouseFire(%val)
{
   $mvTriggerCount0++;
}

function altTrigger(%val)
{
   $mvTriggerCount1++;
}

//------------------------------------------------------------------------------
// Zoom and FOV functions
//------------------------------------------------------------------------------

if($Pref::player::CurrentFOV $= "")
   $Pref::player::CurrentFOV = 45;

function setZoomFOV(%val)
{
   if(%val)
      toggleZoomFOV();
}
      
function toggleZoom( %val )
{
   if ( %val )
   {
      $ZoomOn = true;
      setFov( $Pref::player::CurrentFOV );
   }
   else
   {
      $ZoomOn = false;
      setFov( $Pref::player::DefaultFov );
   }
}

moveMap.bind(keyboard, r, setZoomFOV);
moveMap.bind(keyboard, e, toggleZoom);


//------------------------------------------------------------------------------
// Camera & View functions
//------------------------------------------------------------------------------

function toggleFreeLook( %val )
{
   if ( %val )
      $mvFreeLook = true;
   else
      $mvFreeLook = false;
}

function toggleFirstPerson(%val)
{
   if (%val)
      $firstPerson = !$firstPerson;
}

function toggleCamera(%val)
{
   if (%val)
      commandToServer('ToggleCamera');
}

function updateOrbitDistance(%val)
{
  if( %val > 0 )
     $RTSCamera.decreaseOrbitDistance();
  else if( %val < 0 )
     $RTSCamera.increaseOrbitDistance();
}

function updateViewPitch(%val)
{
   if( %val > 0 )
      $RTSCamera.decreasePitchAngle();
   else if( %val < 0 )
      $RTSCamera.increasePitchAngle();
}

function updateViewYaw(%val)
{
   if( %val > 0 )
      $RTSCamera.decreaseYawAngle();
   else if( %val < 0 )
      $RTSCamera.increaseYawAngle();
}


function snapToLastEvent()
{
   if ($LastEventPos !$= "")
      $RTSCamera.setCameraPosition(getWord($LastEventPos, 0), getWord($LastEventPos, 1));
}

moveMap.bindCmd( keyboard, "space", "snapToLastEvent();", "" );
moveMap.bind( keyboard, z,            toggleFreeLook      );
moveMap.bind( keyboard, tab,          toggleFirstPerson   );
moveMap.bind( keyboard, "alt c",      toggleCamera        );

//------------------------------------------------------------------------------
// Group Management
//------------------------------------------------------------------------------

for (%i = 0; %i <= 9; %i++)
{
   eval("moveMap.bindCmd( keyboard, \"ctrl " @ %i @ "\", \"GroupManager.createGroup(" @ %i @ ");\", \"\");");
   eval("moveMap.bindCmd( keyboard, \"shift " @ %i @ "\", \"GroupManager.addGroupSelection(" @ %i @ ");\", \"\");");
   eval("moveMap.bindCmd( keyboard, \"ctrl-shift " @ %i @ "\", \"GroupManager.addGroupToGroup(" @ %i @ ");\", \"\");");
   eval("moveMap.bindCmd( keyboard, " @ %i @ ", \"GroupManager.selectGroupFromKeyboard(" @ %i @ ");\", \"\");");
}

//------------------------------------------------------------------------------
// Misc. Player stuff
//------------------------------------------------------------------------------

moveMap.bindCmd(keyboard, "ctrl w", "commandToServer('playCel',\"wave\");", "");
moveMap.bindCmd(keyboard, "ctrl s", "commandToServer('playCel',\"salute\");", "");
moveMap.bindCmd(keyboard, "ctrl k", "commandToServer('suicide');", "");

//------------------------------------------------------------------------------
// Message HUD functions
//------------------------------------------------------------------------------

function pageMessageHudUp( %val )
{
   if ( %val )
      pageUpMessageHud();
}

function pageMessageHudDown( %val )
{
   if ( %val )
      pageDownMessageHud();
}

function resizeMessageHud( %val )
{
   if ( %val )
      cycleMessageHudSize();
}

moveMap.bind(keyboard, u, toggleMessageHud );
moveMap.bind(keyboard, y, teamMessageHud );
moveMap.bind(keyboard, "pageUp", pageMessageHudUp );
moveMap.bind(keyboard, "pageDown", pageMessageHudDown );
moveMap.bind(keyboard, "p", resizeMessageHud );


//------------------------------------------------------------------------------
// Demo recording functions
//------------------------------------------------------------------------------

function startRecordingDemo( %val )
{
   if ( %val )
      startDemoRecord();
}

function stopRecordingDemo( %val )
{
   if ( %val )
      stopDemoRecord();
}

moveMap.bind( keyboard, F3, startRecordingDemo );
moveMap.bind( keyboard, F4, stopRecordingDemo );


//------------------------------------------------------------------------------
// Helper Functions
//------------------------------------------------------------------------------

function dropCameraAtPlayer(%val)
{
   if (%val)
      commandToServer('dropCameraAtPlayer');
}

function dropPlayerAtCamera(%val)
{
   if (%val)
      commandToServer('DropPlayerAtCamera');
}

//moveMap.bind(keyboard, "F8", dropCameraAtPlayer);
//moveMap.bind(keyboard, "F7", dropPlayerAtCamera);

function bringUpOptions(%val)
{
   if (%val)
      Canvas.pushDialog(OptionsDlg);
}

moveMap.bind(keyboard, "ctrl o", bringUpOptions);


//------------------------------------------------------------------------------
// Dubuging Functions
//------------------------------------------------------------------------------

$MFDebugRenderMode = 0;
function cycleDebugRenderMode(%val)
{
   if (!%val)
      return;

   if (getBuildString() $= "Debug")
   {
      if($MFDebugRenderMode == 0)
      {
         // Outline mode, including fonts so no stats
         $MFDebugRenderMode = 1;
         GLEnableOutline(true);
      }
      else if ($MFDebugRenderMode == 1)
      {
         // Interior debug mode
         $MFDebugRenderMode = 2;
         GLEnableOutline(false);
         setInteriorRenderMode(7);
         showInterior();
      }
      else if ($MFDebugRenderMode == 2)
      {
         // Back to normal
         $MFDebugRenderMode = 0;
         setInteriorRenderMode(0);
         GLEnableOutline(false);
         show();
      }
   }
   else
   {
      echo("Debug render modes only available when running a Debug build.");
   }
}

//GlobalActionMap.bind(keyboard, "F9", cycleDebugRenderMode);


//------------------------------------------------------------------------------
// Misc.
//------------------------------------------------------------------------------
//-----------------------Begin Bug Fix: http://www.garagegames.com/mg/forums/result.thread.php?qt=23290
function reDrawMiniMapOnToggleScreen(%keyState)
{
    if (%keyState != 0)
    {
        toggleFullScreen();
        schedule(10, MapHud.rebuildMap(), true);
	}
}
GlobalActionMap.bind(keyboard, "alt enter", reDrawMiniMapOnToggleScreen);
//-----------------------End Bug Fix 
GlobalActionMap.bind(keyboard, "tilde", toggleConsole);

GlobalActionMap.bindCmd(keyboard, "F1", "", "StatsGUI.toggle();");

//Allows us to view our stats
//moveMap.bind(keyboard,  s,   toggleStatsGUI );

//moveMap.bind( keyboard, F2, showPlayerList );
//------------------------------------------------------------------------------
// Selection
//------------------------------------------------------------------------------

if($Selection::Modifier $= "")
   $Selection::Modifier = 0;

function toggleSelectionModifier()
{
   if($Selection::Modifier)
      $Selection::Modifier = 0;
   else
      $Selection::Modifier = 1;
}

moveMap.bindCmd(keyboard, "lshift", "toggleSelectionModifier();", "toggleSelectionModifier();");


//-----------------------------------------------------------------------------
// This sets custom key bindings to display:
//      - The Time    (F6)
//      - The Weather (F9)
//-----------------------------------------------------------------------------
function displayTime(%val)
{
    if(%val)
        commandToServer('DoTime');
}
GlobalActionMap.bind(keyboard, "F6", displayTime);

function displayWeather(%val)
{
    if(%val)
        commandToServer('DoWeather');
}
GlobalActionMap.bind(keyboard, "F9", displayWeather);

// PAUSE function
moveMap.bind(keyboard, "pause", pauseGame);