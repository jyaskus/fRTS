//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
// GameManager mod by Bil Simser (bsimser@shaw.ca)
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// This mod contains a simple game manager class. This can be used for almost
// anything and is meant to manage any game attributes like time, weather,
// game events, etc.
//
// The samples here are for game time and weather changes to get you started.
//
// Everything is run through a single, scheduled method called update
// which is fired every second. Which in turn, calls up the update method
// on any additional managers you create. This is about as OO as you can get 
// with script.
//
// This manager was inspired by the CircleMUD (www.circlemud.org) system.
//
// Usage:
// To use this system do the following:
// 1. Place the files from this package in your game structure. A good place 
//    would be in the server/scripts folder
// 2. Add exec("./gameMgr.cs") in game.cs at the end of onServerCreated
// 3. Add the following code in game.cs to the end of the startGame function
//    // Start the GameManager
//    new ScriptObject(GameManager) {};
//    MissionCleanup.add(GameManager);
//    GameManager.init();
// 4. Add the following code in game.cs to endGame:
//    GameManager.delete();
// 5. Tweak the variables at the top of the files to suit your needs.
// 6. Enjoy, improve, share.
//
// Extending GameManager:
// The GameManager here is just a simple starting point (like AIManager)
// but you can extend it to do whatever you want. Handle bot activity,
// dynamically change your world, update health points, trigger events,
// whatever. Just be creative!
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Configurable values you can tweak
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Number of real seconds to pass for 1 hour in the game
//
// This is used to count the ticks passed and determine when a "game" hour
// has passed. You can use it to fire events based on a game hour value
// (e.g. a player needs 1 hour to recharge a spell)
//
// Default: 75 seconds = 1 hour game time: 30 min. game day
// Suggested: 37.5 seconds = 1 hour: 15 min. game day
// Notes:
// To be really mean you can set this to 3600 which would be real-time
//-----------------------------------------------------------------------------
$GameManagerPrefs::SecsPerGameHour = 75;

//-----------------------------------------------------------------------------
// Okay, stop tweaking...
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Initializes the GameManager class. Use this method to create your game
// managers and to handle starting up the system.
// You should call this once when you create the GameManager object in game.cs
//-----------------------------------------------------------------------------
function GameManager::init(%this)
{
    // Copy local variables for this class from our prefs
    // Why? Because it's as close to OO as we can get and we
    // don't have to refer to global variables, we can use %this.xxx
    %this.SecsPerGameHour = $GameManagerPrefs::SecsPerGameHour;

    // The pulse variable keeps track of our game clock ticks and
    // is used to determine when a game event fires. You can have as
    // many game events as you like (time, weather, hit point regen, etc.)
    // and have them fire at different periods. Pulse is updated once per
    // pass through the update method so you just set the frequency of how 
    // often your own event handler fires. Think of pulse as a counter.
    %this.Pulse = 0;

    // Load whatever managers you create here
    exec("./time.cs");
    exec("./weather.cs");

    // Initialize the managers
    new ScriptObject(TimeManager);
    TimeManager.init();

    new ScriptObject(WeatherManager);
    WeatherManager.init();

    // Schedule the update method to fire 1 second from now 
    // (it will reschedule itself when it fires)
    %this.schedule(1000, update);
}

//-----------------------------------------------------------------------------
// The GameManager update method. This is fired every second and
// handles updating any managers it knows about. It could be expanded to deal
// with npc activity, player point regain, spell recharges, etc. Just 
// create new managers and add them to the ManagerList[] array.
//-----------------------------------------------------------------------------
function GameManager::update(%this)
{
    // This just fires events when each hour passes in the game
    if((%this.Pulse % %this.SecsPerGameHour) == 0)
    {
	TimeManager.update();
	WeatherManager.update();
    }
	
    // Handle other events based on how often you want to update them
    // For example you could move the WeatherManager::update here to
    // have it fire more (or less) often than once a game hour.
      
    // Increment the counter and roll pulse over after 10 hours of real time
    // Why 10 hours? Seems like a good number to use. Don't want the counter
    // getting too big and blowing the system up.
    %this.Pulse++;
    if(%this.Pulse >= (10 * 60 * 60))
	%this.Pulse = 0;

    // Schedule this event to fire again
    %this.schedule(1000, update);
}

//-----------------------------------------------------------------------------
// Utility function to generate x rolls of a n sided die (e.g 3d5, 2d4, etc.)
//-----------------------------------------------------------------------------
function dice(%num, %size)
{
    %sum = 0;
    
    if((%size <= 0) || (%num <= 0))
	return 0;
	
    while(%num > 0)
    {
	%rnd = getRandom(1, %size);
	%sum += %rnd;
	%num--;
    }
	
    return %sum;
}

//-----------------------------------------------------------------------------
// Simulates the min, max function. Hey, every game needs this!
//-----------------------------------------------------------------------------
function min(%a, %b)
{
    return (%a < %b ? %a : %b);
}

function max(%a, %b)
{
    return (%a > %b ? %a : %b);
}

//-----------------------------------------------------------------------------
// Returns the absolute value of a number
//-----------------------------------------------------------------------------
function abs(%a)
{
    if (%a < 0)
       %a = %a * -1;
    return %a;
}