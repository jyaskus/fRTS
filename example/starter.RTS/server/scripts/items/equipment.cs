//-----------------------------------------------------------------------------
// All the items that can be equipped
//-----------------------------------------------------------------------------

// - - - - - - - -  --  - --  -- -------
datablock ParticleData(GreenExplosionParticles)
{
   textureName          = "~/data/shapes/particles/bubble";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = 0.1;
   lifetimeMS           = 2200;
   lifetimeVarianceMS   = 800;
   useInvAlpha =  false;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "0.4 0.6 0 0.6";
   colors[1]     = "0.2 0.8 0 0.4";
   colors[2]     = "0.0 1.0 0 0.2";

   sizes[0]      = 1.5;
   sizes[1]      = 0.9;
   sizes[2]      = 0.5;

   times[0]      = 0.0;
   times[1]      = 0.5;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(GreenExplosionEmitter)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.8;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "GreenExplosionParticles";
};

// JY - my custom projectiles

datablock ProjectileData(ArrowProjectile)
{
   projectileShapeName = "~/data/shapes/crossbow/projectile.dts";
   directDamage        = 30;
   radiusDamage        = 30;
   damageRadius        = 1.5;
//   explosion           = CrossbowExplosion;
//   particleEmitter     = CrossbowBoltEmitter;

   muzzleVelocity      = 100;
   velInheritFactor    = 0.3;

   armingDelay         = 0;
   lifetime            = 5000;
   fadeDelay           = 5000;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 0.80;

   hasLight    = true;
   lightRadius = 4;
   lightColor  = "0.5 0.5 0.25";
};
datablock ProjectileData(FireballProjectile)
{
   projectileShapeName = "~/data/shapes/crossbow/debris.dts";
   directDamage        = 30;
   radiusDamage        = 60;
   damageRadius        = 1.5;
   explosion           = CrossbowExplosion;
   particleEmitter     = CrossbowBoltEmitter;

   muzzleVelocity      = 100;
   velInheritFactor    = 0.3;

   armingDelay         = 0;
   lifetime            = 6000;
   fadeDelay           = 5000;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 0.80;

   hasLight    = true;
   lightRadius = 6;
   lightColor  = "1 0.25 0.125";
};
datablock ProjectileData(GreenProjectile)
{
   projectileShapeName = "~/data/shapes/crossbow/debris.dts";
   directDamage        = 34;
   radiusDamage        = 54;
   damageRadius        = 2.5;
   explosion           = CrossbowExplosion;
   particleEmitter     = GreenExplosionEmitter; // CrossbowBoltEmitter;

   muzzleVelocity      = 100;
   velInheritFactor    = 0.3;

   armingDelay         = 0;
   lifetime            = 6000;
   fadeDelay           = 5000;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 0.80;

   hasLight    = true;
   lightRadius = 8;
   lightColor  = "0.1 1 0.25";
};
// ---------------- AXE --------------------------
datablock ItemData(Axe)
{
   // Basic Item properties
   category = "Weapon";
   className = "Weapon";   
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

	// Dynamic properties defined by the scripts
   shapeFile = "~/data/shapes/items/axe01.dts";	
	pickUpName = "an axe";
	image = AxeImage;	
};
datablock ShapeBaseImageData(AxeImage)
{
   // AttackManager properties...
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   
   // Basic Item properties
   shapeFile = "~/data/shapes/items/axe01.dts";
   item = Axe;
      
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
};
function AxeImage::onFire(%this, %obj, %slot)
{
   // JY - do nothing its just for visual effects
   return %p;
}

// ---------------------- BOW ---------------------
datablock ItemData(Bow)
{
   // Basic Item properties
   category = "Weapon";
   className = "Weapon";   
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

	// Dynamic properties defined by the scripts
   shapeFile = "~/data/shapes/items/bow01.dts";	
	pickUpName = "a Bow";
	image = BowImage;	
};
datablock ShapeBaseImageData(BowImage)
{
   // AttackManager properties...
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   
   // Basic Item properties
   shapeFile = "~/data/shapes/items/bow01.dts";
   item = Bow;
      
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.8"; // JY  default was "0.1 0.4 -0.6"; 
   correctMuzzleVector = false;
   className = "WeaponImage";
   
   ammo = CrossbowAmmo;
   projectile = ArrowProjectile;
   projectileType = Projectile;
};
function BowImage::onFire(%this, %obj, %slot)
{
   %projectile = %this.projectile;

   // Decrement inventory ammo. The image's ammo state is update
   // automatically by the ammo inventory hooks.
//   %obj.decInventory(%this.ammo,1);

   // Determin initial projectile velocity based on the 
   // gun's muzzle point and the object's current velocity
   %muzzleVector = %obj.getMuzzleVector(%slot);
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(
   VectorScale(%muzzleVector, %projectile.muzzleVelocity),
   VectorScale(%objectVelocity, %projectile.velInheritFactor));

   // Create the projectile object
   %p = new (%this.projectileType)() {
      dataBlock        = %projectile;
      initialVelocity  = %muzzleVelocity;
      initialPosition  = %obj.getMuzzlePoint(%slot);
      sourceObject     = %obj;
      sourceSlot       = %slot;
      client           = %obj.client;
   };
   MissionCleanup.add(%p);
   return %p;
}

// ------------------ SPEAR ---------------
datablock ItemData(Spear)
{
   // Basic Item properties
   category = "Weapon";
   className = "Weapon";   
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;

	// Dynamic properties defined by the scripts
   shapeFile = "~/data/shapes/items/jspear.dts";	
	pickUpName = "a jspear";
	image = SpearImage;	
};
datablock ShapeBaseImageData(SpearImage)
{
   // AttackManager properties...
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   
   // Basic Item properties
   shapeFile = "~/data/shapes/items/jspear.dts";
   item = Spear;
      
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
};
function SpearImage::onFire(%this, %obj, %slot)
{
   // JY - do nothing its just for visual effects
   return %p;
}

// ------------------ HAMMER ------------------
datablock ItemData(Hammer)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "~/data/shapes/items/hammer01.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   pickUpName = "a hammer";
   image = HammerImage;
};

datablock ShapeBaseImageData(HammerImage)
{
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   shapeFile = "~/data/shapes/items/hammer01.dts";
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = Hammer;
};


function AxeImage::onFire(%this, %obj, %slot)
{
   // JY - do nothing the axe is just for visual effects
   return %p;
}

// ------------------ SWORD ------------------
datablock ItemData(Sword)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "~/data/shapes/items/sword_01.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   pickUpName = "a sword";
   image = SwordImage;
};

datablock ShapeBaseImageData(SwordImage)
{
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   shapeFile = "~/data/shapes/items/sword_01.dts";
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = Hammer;
};


function SwordImage::onFire(%this, %obj, %slot)
{
   // JY - do nothing the axe is just for visual effects
   return %p;
}

// --- GOBLIN STICK
datablock ItemData(Stick)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "~/data/shapes/items/stick.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   pickUpName = "a stick";
   image = StickImage;
};

datablock ShapeBaseImageData(StickImage)
{
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   shapeFile = "~/data/shapes/items/stick.dts";
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = Stick;
};
// --- GOBLIN HELMET
datablock ItemData(gHelmet)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "~/data/shapes/items/helmet.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   pickUpName = "a gHelmet";
   image = gHelmetImage;
};

datablock ShapeBaseImageData(gHelmetImage)
{
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   shapeFile = "~/data/shapes/items/helmet.dts";
   emap = true;
   mountPoint = 1;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = gHelmet;
};

// --- GOBLIN SHIELD
datablock ItemData(gShield)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "~/data/shapes/items/shield.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   pickUpName = "a gShield";
   image = gShieldImage;
};

datablock ShapeBaseImageData(gShieldImage)
{
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   shapeFile = "~/data/shapes/items/shield.dts";
   emap = true;
   mountPoint = 2;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = gShield;
};
// --- GOBLIN LANCE
datablock ItemData(gLance)
{
   category = "Weapon";
   className = "Weapon";

   shapeFile = "~/data/shapes/items/lance.dts";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;
   emap = true;
   pickUpName = "a lance";
   image = gLanceImage;
};

datablock ShapeBaseImageData(gLanceImage)
{
   minDamage = 20;
   maxDamage = 40;
   doFire = false;
   shapeFile = "~/data/shapes/items/lance.dts";
   emap = true;
   mountPoint = 0;
   eyeOffset = "0.1 0.4 -0.6";
   correctMuzzleVector = false;
   className = "WeaponImage";
   item = gLance;
};
