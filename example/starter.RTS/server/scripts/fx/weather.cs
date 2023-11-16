//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
// Weather GameManager mod by Bil Simser (bsimser@shaw.ca)
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Configurable values you can tweak about how your weather system works
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Number of seconds (real-time) for a storm to roll in (or roll out)
// Default:  $time_to_storm = 60 seconds
//           $time_to_storm_init = 30 seconds
// Notes:
// This variable is used in the weather_change() method
// - time_to_storm_init only used when initializing weather at level start.
//-----------------------------------------------------------------------------
$time_to_storm = 60;
$time_to_storm_init = 30;

//-----------------------------------------------------------------------------
// Number of seconds (real-time) it takes rain to start/stop.
// Default: 20 seconds
// Notes:
// This is actually the "fade-time" rain takes to go from
// 10% to 99% or vice-versa.
//-----------------------------------------------------------------------------
$time_to_full_rain = 20;
$time_to_stop_rain = 20;


//-----------------------------------------------------------------------------
// Okay, stop tweaking...
//-----------------------------------------------------------------------------

// Sky conditions for weather_data
$sky_cloudless = 0;
$sky_cloudy = 1;
$sky_raining = 2;
$sky_lightning = 3;

//-----------------------------------------------------------------------------
// Gets the current weather situation and displays it to the client
// Bind this to a key so the player can ask what the weather is like. You 
// could also add code to only show the weather to someone who was outside.
//-----------------------------------------------------------------------------
function serverCmdDoWeather(%client)
{
    // Keep these in line with the $sky_xxx variables above
    %sky_look[$sky_cloudless] = "cloudless";
    %sky_look[$sky_cloudy] = "cloudy";
    %sky_look[$sky_raining] = "rainy";
    %sky_look[$sky_lightning] = "lit by flashes of lightning";
    
    if(isObject($weather_info))
    {    
        %weatherMsg = "The sky is " @ %sky_look[$weather_info.sky] @ " and ";
	if($weather_info.change >= 0)
	    %weatherMsg = %weatherMsg @ "you feel a warm wind from the south.";
	else
	    %weatherMsg = %weatherMsg @ "your foot tells you bad weather is due.";
    }
    else
    {
	%weatherMsg = "You have no feeling about the weather.";
    }

    messageClient(%client, 'weather', %weatherMsg);
}

//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
function WeatherManager::init(%this)
{
    // Initialize the weather system
    if(!isObject($weather_info))
    {
	$weather_info = new ScriptObject(weather_data);
	MissionCleanup.add(weather_data);

	$weather_info.pressure = 960;
	if(($time_info.month >= 7) && ($time_info.month <= 12))
	    $weather_info.pressure += dice(1, 50);
	else
	    $weather_info.pressure += dice(1, 80);

	$weather_info.change = 0;

	if($weather_info.pressure <= 980)
	{
	    $weather_info.sky = $sky_lightning;
            Sky.stormClouds(1, $time_to_storm_init); // added to start lightning, rain and clouds
            Sky.stormFog(1, $time_to_storm_init);
            %this.startRain();
            %this.startLightning();
        }
	else if($weather_info.pressure <= 1000)
	{
	    $weather_info.sky = $sky_raining;
	     Sky.stormClouds(1, $time_to_storm_init); // added to start rain and clouds
             Sky.stormFog(1, $time_to_storm_init);
             %this.startRain();
        }
	else if($weather_info.pressure <= 1020)
	{
	    $weather_info.sky = $sky_cloudy;
	     Sky.stormClouds(1, $time_to_storm_init); // added to start clouds
             Sky.stormFog(1, $time_to_storm_init);
        }
	else
	{
	    $weather_info.sky = $sky_cloudless;
	    Sky.stormClouds(0, $time_to_storm_init); // added to start clouds
	    Sky.stormFog(0, $time_to_storm_init);
        }
    }
}

//-----------------------------------------------------------------------------
// This updates the weather changes based on time, pressure
// and a random value. It also tells the engine to create the
// effects for the weather changes (rain, lightning, fog, etc.)
//-----------------------------------------------------------------------------
function WeatherManager::update(%this)
{
    %diff = 0;
    
    // Later months in year don't need as much pressure to create a change
    // (or you can change this to suit your environment)
    if(($time_info.month >= 9) && ($time_info.month <= 12))
	%diff = ($weather_info.pressure > 985 ? -2 : 2);
    else
	%diff = ($weather_info.pressure > 1015 ? -2 : 2);

    $weather_info.change += (dice(1, 4) * %diff + dice(2, 6) - dice(2, 6));

    // Cap the changes so weather isn't too drastic
    $weather_info.change = min($weather_info.change, 12);
    $weather_info.change = max($weather_info.change, -12);	
    
    $weather_info.pressure += $weather_info.change;
    
    // Cap the pressure values so we don't go crazy with weather changes
    $weather_info.pressure = min($weather_info.pressure, 1040);
    $weather_info.pressure = max($weather_info.pressure, 960);
	
    %change = 0;

    // Based on the current sky conditions and the change
    // value, determine what the new weather should be    
    switch($weather_info.sky)
    {
	case $sky_cloudless:
	    if($weather_info.pressure < 990)
		%change = 1;
	    else if($weather_info.pressure < 1010)
		if(dice(1, 4) == 1)
		    %change = 1;
		    
	case $sky_cloudy:
	    if($weather_info.pressure < 970)
	    {
		%change = 2;
	    }
	    else if($weather_info.pressure < 990)
	    {
		if(dice(1, 4) == 1)
		    %change = 2;
		else
		    %change = 0;
	    } 
	    else if($weather_info.pressure > 1030)
	    {
		if(dice(1, 4) == 1)
		    %change = 3;
	    }
		    
	case $sky_raining:
	    if($weather_info.pressure < 970)
	    {
		if(dice(1, 4) == 1)
		    %change = 4;
		else
		    %change = 0;
	    }
	    else if($weather_info.pressure > 1030)
		%change = 5;
	    else if($weather_info.pressure > 1010)
		if(dice(1, 4) == 1)
		    %change = 5;
		    
	case $sky_lightning:
	    if($weather_info.pressure > 1010)
		%change = 6;
	    else if(weather_info.pressure > 990)
		if(dice(1, 4) == 1)
		    %change = 6;
		    
	default:
	    $weather_info.sky = $sky_cloudless;
	    %change = 0;
    }
    
    // Implement the weather change now    
    switch(%change)
    {
	case 1:
	    messageAll("weather", "The sky starts to get cloudy.");
	    Sky.stormClouds(1, $time_to_storm);
	    Sky.stormFog(1, $time_to_storm);
	    $weather_info.sky = $sky_cloudy;
	    
	case 2:
	    messageAll("weather", "It starts to rain");
	    %this.startRain();
	    $weather_info.sky = $sky_raining;
	    
	case 3:
	    messageAll("weather", "The clouds disappear");
	    Sky.stormClouds(0, $time_to_storm);
	    Sky.stormFog(0, $time_to_storm);
	    $weather_info.sky = $sky_cloudless;
	    
	case 4:
	    messageAll("weather", "Lightning starts to show in the sky");
	    %this.startLightning();
	    $weather_info.sky = $sky_lightning;
    
	case 5:
	    messageAll("weather", "The rain stops");
	    %this.stopRain();
	    $weather_info.sky = $sky_cloudy;
	    
	case 6:
	    messageAll("weather", "The lightning stops");
	    %this.stopLightning();
	    $weather_info.sky = $sky_raining;
    }
}

//-----------------------------------------------------------------------------
// Support methods below to start and stop the rain and lightning effects
//-----------------------------------------------------------------------------

function WeatherManager::startRain(%this)
{
    if(!isObject($Rain))
    {
         // Randomly determin intensity of storm.
         // First determin wind speed
         // Then determin rain intensity (drops per min.):
         //      - Min. 2000 drops per min.
         //      - additional 500 drops per min for every point of wind speed.
         //      - Max. 2000 + (8 * 500) = 6000 drops.

         $windX = (dice(1,2) - 1 ? dice(1,5) - 1 : -(dice(1,5) -1 ));
         $windY = (dice(1,2) - 1 ? dice(1,5) - 1 : -(dice(1,5) -1 ));

         Sky.setWindVelocity($windX, $windY, 0);
         $intensity = ((abs($windX) + abs($windY)) * 500) + 2000;


	$Rain = new Precipitation()
	{
	    datablock = "HeavyRain";
	    minSpeed = 2.5;
	    maxSpeed = 3.0;
	    numDrops = $intensity;
	    boxWidth = 200;
	    boxHeight = 100;
	    minMass = 1.0;
	    maxMass = 2.0;
	    rotateWithCamVel = true;
	    doCollision = true;
	    useTurbulence = false;
	};
	$Rain.setPercentage(0.1);
        $Rain.modifyStorm(0.99, $time_to_full_rain);
    }
}

function WeatherManager::stopRain(%this)
{
    if(isObject($Rain))
    {
      $Rain.modifyStorm(0.1, $time_to_stop_rain);
      $Rain.schedule($time_to_stop_rain*1000 + 500, delete);
    }
    $Rain = "";
}

function WeatherManager::startLightning()
{
      //position the lightning in the middle of all human players
      %pos = "0 0 0";
      %numHumans = 0;

      %count = ClientGroup.getCount();
      for (%i = 0; %i < %count; %i++)
      {
        %client = ClientGroup.getObject(%i);
        if( !%client.isAIControlled() )
        {
              if(isObject(%client.player))
              {
                 %numHumans++;
                 %pos = VectorAdd(%pos, %client.player.position);
              }
        }
      }

    %pos = VectorScale(%pos, 1.0 / (%numHumans ));

    if (!isObject($Lightning))
    {
       $Lightning = new Lightning()
       {
         position = %pos;
         scale = "250 400 500";
         dataBlock = "LightningStorm";
         strikesPerMinute = "15";
         strikeWidth = "0.15";
         chanceToHitTarget = "0.02";
         strikeRadius = "500";
         boltStartRadius = "20";
         color = "1.000000 1.000000 1.000000 1.000000";
         fadeColor = "0.800000 0.700000 0.8000000 1.000000";
         useFog = "1";
         locked = "false";
      };
    }
}

function WeatherManager::stopLightning()
{
    if(isObject($Lightning))
	$Lightning.delete();
    $Lightning = "";
}
