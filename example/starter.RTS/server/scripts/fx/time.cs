//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
// Time GameManager mod by Bil Simser (bsimser@shaw.ca)
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Configurable values you can tweak about how your game time system works
//-----------------------------------------------------------------------------
$hours_in_day = 24;
$days_in_week = 7;
$days_in_month = 28;
$months_in_year = 16;

//-----------------------------------------------------------------------------
// What state the sun is in
//-----------------------------------------------------------------------------
$sun_dark = 0;
$sun_rise = 1;
$sun_light = 2;
$sun_set = 3;

//-----------------------------------------------------------------------------
// Variables of what hour during the clock the sun rises and sets
//-----------------------------------------------------------------------------
$hour_rise = 5;
$hour_light = 6;
$hour_set = 21;
$hour_dark = 22;

//-----------------------------------------------------------------------------
// Constants for flashy month names
// Remember to keep these in line with $months_in_year
//-----------------------------------------------------------------------------
$month_name[0] = "Month of Winter";
$month_name[1] = "Month of the Winter Wolf";
$month_name[2] = "Month of the Frost Giant";
$month_name[3] = "Month of the Old Forces";
$month_name[4] = "Month of the Grand Struggle";
$month_name[5] = "Month of the Spring";
$month_name[6] = "Month of Nature";
$month_name[7] = "Month of Futility";
$month_name[8] = "Month of the Dragon";
$month_name[9] = "Month of the Sun";
$month_name[10] = "Month of the Heat";
$month_name[11] = "Month of the Battle";
$month_name[12] = "Month of the Dark Shades";
$month_name[13] = "Month of the Shadows";
$month_name[14] = "Month of the Long Shadows";
$month_name[15] = "Month of the Ancient Darkness";
$month_name[16] = "Month of the Great Evil";

//-----------------------------------------------------------------------------
// Same as month names. Keep in line with $days_in_week value
//-----------------------------------------------------------------------------
$weekdays[0] = "the Day of the Moon";
$weekdays[1] = "the Day of the Bull";
$weekdays[2] = "the Day of the Deception";
$weekdays[3] = "the Day of Thunder";
$weekdays[4] = "the Day of Freedom";
$weekdays[5] = "the Day of the Great Gods";
$weekdays[6] = "the Day of the Sun";

//-----------------------------------------------------------------------------
// Okay, stop tweaking...
//-----------------------------------------------------------------------------


//-----------------------------------------------------------------------------
// Returns the current game time in a text message.
//-----------------------------------------------------------------------------
function serverCmdDoTime(%client)
{
    %gameTimeStr = "";
    
    if(isObject($time_info))
    {
	// day in [1..$days_in_month]
	%day = $time_info.day + 1;
	
	// Figures out what day of the week it is based on our time scale	
	%weekday = (($days_in_month * $time_info.month) + %day) % $days_in_week;

         // Get the hour of the day value from a 24 hour clock
         %hdiv2 = $hours_in_day / 2;
         if($time_info.hours % %hdiv2 == 0)
            %hour_of_day = %hdiv2;
         else
            %hour_of_day = $time_info.hours % %hdiv2;
	    
	// Determine if it's am or pm
	if($time_info.hours >= 12)
	    %hourStr = "pm";
	else
	    %hourStr = "am";
	
	%gameTimeStr = "It is " @ %hour_of_day @ " o'clock " @ %hourStr 
	    @ ", on " @ $weekdays[%weekday] @ ".";
	    
	%suf = "th";
	
	if(((%day % 100) / 10) != 1)
	{
	    switch(%day % 10)
	    {
		case 1:
		    %suf = "st";
		    
		case 2:
		    %suf = "nd";
		
		case 3:
		    %suf = "rd";
	    }
	}
	
	%gameTimeStr = %gameTimeStr @ "\n" @
	    "It is the " @ %day @ %suf @ " Day of the " @
	    $month_name[$time_info.month] @ ", Year " @
	    $time_info.year @ ".";
    }

    messageClient(%client, 'time', %gameTimeStr);
}

//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
function TimeManager::init(%this)
{
    // Initialize the time system
    if(!isObject($time_info))
    {
	$time_info = new ScriptObject(time_data);
	MissionCleanup.add(time_data);
	
	// Initialize our time variables. These could
	// be read from a file to keep the world persistant
	$time_info.hours = 0;
	$time_info.day = 0;
	$time_info.month = 0;
	$time_info.year = 0;
	
        if($time_info.hours <= $hour_rise - 1)
	    $time_info.sunlight = $sun_dark;
	else if($time_info.hours == $hour_light - 1)
	    $time_info.sunlight = $sun_rise;
	else if($time_info.hours <= $hour_set - 1)
	    $time_info.sunlight = $sun_set;
        else
            $time_info.sunlight = $sun_dark;	
    }
}

//-----------------------------------------------------------------------------
// This updates the game clock to pass another hour of game time
// in the real world. It will handle the time of day (see above for
// what the definitions are for this world) and can be hooked up to
// a day/night cycle system to really move the sun object.
//-----------------------------------------------------------------------------
function TimeManager::update(%this)
{
    $time_info.hours++;

    // Update the sun tracker so we can deal with weather
    // changes later. This also sends a message out to everyone
    // in the game about what's going on. You can use the 
    // time_info.sunlight variable somewhere else (for example
    // only allow certain types of creatures to roam at night or
    // to open and close shops or something).
    switch($time_info.hours)
    {
	case $hour_rise:
	    $time_info.sunlight = $sun_rise;
	    messageAll("weather", "The sun rises in the east");
       
	case $hour_light:
	    $time_info.sunlight = $sun_light;
	    messageAll("weather", "The day has begun");
       
	case $hour_set:
	    $time_info.sunlight = $sun_set;
	    messageAll("weather", "The sun slowly disappears in the west");
       
	case $hour_dark:
	    $time_info.sunlight = $sun_dark;
	    messageAll("weather", "The night has begun");
    }

    // Update the time_info values and increment the
    // day, month, year info   
    if($time_info.hours > $hours_in_day) {
	$time_info.hours = 0;
	$time_info.day++;
       
	if($time_info.day > $days_in_month) {
	    $time_info.day = 0;
	    $time_info.month++;
	   
	    if($time_info.month > $months_in_year) {
		$time_info.month = 0;
		$time_info.year++;
	    }
	}
    }
}

