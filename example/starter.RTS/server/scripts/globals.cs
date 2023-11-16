// Game duration in secs, no limit if the duration is set to 0
$Game::Duration = 0; //20 * 60; // 20m time limit ?

// When a client score reaches this value, the game is ended.
$Game::EndGameScore = 8;

// Pause while looking over the end game screen (in secs)
$Game::EndGamePause = 30;

// area below were trees arent placed
$Game::TreeLine = 60; 

// area below were gold and stone arent placed
$Game::RockLine = 120; 

// JY -- horibble, like we need more places to change when adding units ... but works
$Game::NumberOfUnits = 10; // 0 to 7 + guardian + shaman + rat

// JY -- flag to spawn initial units 
$Game::BUILD_UNITS = true;

// JY - flag to build town center 
$Game::BUILD_BASE = true;
$Game::SPAWN_RESOURCES = false;
$Game::AI_ON = true; 
$Game::AI_NUM = 2;    // max 6 players at once
$Game::AI_LOADED = false; // loaded 

// turn on/off debug messages
$DEBUG = true;
//
$Game::CLOSE_ENOUGH = 5.0; // close enough -- unit arrives at destination.


// global ID to hold the value of the first players ID ... for scoring
$Player::id = 0; 

$AI::SpawnCount = 0;

// minimum damage to exceed for activating playDeathCry and playHurtAnimation
$GAME::minDmg = 10; 

$Game::Shadows = false;

$Projectile::Default = 0;
$Projectile::Arrow = 0;
$Projectile::Fireball = 0;
$Projectile::Green = 0;