

if (!isObject(DemoScreens))
{
   new SimSet(DemoScreens);

   %s = new ScriptObject()
   {
      title = "Welcome!";
      text = "Welcome to the RTS Starter Pack Demo!  Here you will learn the key features of this starter pack, so that you can make your very own RTS Game!";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);
   
   %s = new ScriptObject()
   {
      title = "RTS Camera";
      text = "The RTS Camera is a modifed torque camera tailored for an RTS.  It supports panning, zooming, and changing pitch.  Try moving your mouse to the edges of the screen to see this in action. You can zoom with the mouse wheel, and change pitch by holding control and using the mouse wheel.";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);
   
   %s = new ScriptObject()
   {
      title = "RTS Unit";
      text = "RTS Units are the little guys that run around and beat each other up.  Once you have at least one unit selected, try right-clicking on the terrain to make them move.";
      pushCommand = "demoBeginUnit();";
      popCommand = "";
   };
   DemoScreens.add(%s);
   
   %s = new ScriptObject()
   {
      title = "Selection";
      text = "You can select units by either clicking on them, or you can drag-select a group of them.  When you select units, they will appear in the selection at the bottom of your screen.  Try it out!";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);

   %s = new ScriptObject()
   {
      title = "Groups";
      text = "Now that you have some units selected, you can group them and bind the group to a number key.  Press Control+1, then select something else, then press 1.  Voila!  Your group is selected!  You can also select a group of units and press Control+Shift+# to add the selection to a group.";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);
   
   %s = new ScriptObject()
   {
      title = "Attacking";
      text = "To make one of your units attack another one, select a unit, then click on the 'Attack' button (the red cross) and then click on another one of your units.";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);

   %s = new ScriptObject()
   {
      title = "Networking";
      text = "The RTS Starter Kit has some awesome networking optimizations, designed with RTS-specific gameplay in mind. Unit data updates are very efficient, unit commands are batched for efficiency, and objects like projectiles aren't tracked over the network.";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);

   %s = new ScriptObject()
   {
      title = "Buildings";
      text = "It is possible to place buildings.  Click on the giant BUILD button.  A translucent version of the building (a marker) will appear attatched to the mouse, which will be replaced with the real building on a left click.  It is possible to change the marker color to show that a building can/cannot be placed in a particular location.";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);

   %s = new ScriptObject()
   {
      title = "Minimap";
      text = "The minimap shows a rendering of the terrain and displays strategic information like the location of known units and buildings.  You can control the camera by left-clicking the mini-map at the position you'd like to view.  Right-clicking the minimap issues a move order to the currently selected units.  The minimap also provides alerts, indicating areas of importance.";
      pushCommand = "startMapPinging();";
      popCommand = "stopMapPinging();";
   };
   DemoScreens.add(%s);
   
   %s = new ScriptObject()
   {
      title = "Thanks!";
      text = "Thanks for checking out this very brief feature overview!  Hit escape to exit.";
      pushCommand = "";
      popCommand = "";
   };
   DemoScreens.add(%s);
   
}

function demoBeginUnit()
{
   $RTSCamera.setOrbitDistance(10);
   $RTSCamera.setCameraPosition(-35, -32);
   $RTSCamera.setPitchAngle(0);
}

function makeGreenPing()
{
   MapHud.createPingEvent("-80 0", "0 1 0");
}

function makeBluePing()
{
   MapHud.createPingEvent("-40 40", "0 0 1");
}

function startMapPinging()
{
   $pingThread = schedule(2500, 0, startMapPinging);
   MapHud.createPingEvent("0 0", "1 0 0");
   schedule(750, 0, makeGreenPing);
   schedule(1500, 0, makeBluePing);
}

function stopMapPinging()
{
   if (isEventPending($pingThread))
      cancel($pingThread);
   $pingThread = 0;
}

function StartDemo()
{
   $CurrScreen = -1;
   PlayGui.add(DemoPlayGuiOverlay);
   displayNextScreen();
   $RunningDemo = true;
}

function StopDemo()
{
   PlayGui.remove(DemoPlayGuiOverlay);
   $RunningDemo = false;
}

function displayNextScreen()
{
   if ($CurrScreen == DemoScreens.getCount() - 1)
      return;

   if ($CurrScreen != -1)
   {
      %oldScreen = DemoScreens.getObject($CurrScreen);
      eval(%oldScreen.popCommand);
   }
   
   $CurrScreen++;
   
   %screen = DemoScreens.getObject($CurrScreen);
   DemoTitle.setText(%screen.title);
   DemoText.setText(%screen.text);
   eval(%screen.pushCommand);
}

function displayPreviousScreen()
{
   if ($CurrScreen == 0)
      return;

   %oldScreen = DemoScreens.getObject($CurrScreen);
   eval(%oldScreen.popCommand);
   
   $CurrScreen--;
   
   %screen = DemoScreens.getObject($CurrScreen);
   DemoTitle.setText(%screen.title);
   DemoText.setText(%screen.text);
   eval(%screen.pushCommand);
}

