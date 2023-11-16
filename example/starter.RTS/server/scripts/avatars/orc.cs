datablock RTSUnitData(orcBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/orc/orc.dts"; 
   RTSUnitTypeName = "villager";

   baseDamage = 19; //26;
   attackDelay = 35;
   damagePlus = 3;
   
   armor = 2;   
   moveSpeed = 5;    
   range = 4;
   
   maxDamage = 180; //maxDamage = health
   vision = 75;
   
   boundingBox = "2.0 2.0 2.0";
   
   isMelee = true; 
};

function orcBlock::onAttack(%this, %attacker, %target)
{
   %damage = %attacker.getDataBlock().baseDamage;
   if(%attacker.getNetModifier().baseDamage)
      %damage *= %attacker.getNetModifier().baseDamage;

   %armor  = %target.getDataBlock().armor;
   if(%target.getNetModifier().armor)
      %armor *= %target.getNetModifier().armor;
      
   if(%damage > %armor)
      %damage -= %armor;
   else
      %damage = 0;

    %target.damage(%attacker,0,%damage,"MELEE");
}

function orcBlock::onAdd(%this,%obj)
{
      %obj.mountImage(HammerImage,0);
}

// archer
datablock RTSUnitData(orcArcherBlock : orcBlock)
{
   RTSUnitTypeName = "orcArcher"; 

   baseDamage = 35;
   attackDelay = 35;
        
   range = 20; // map units - that can be far
   
   maxDamage = 200; //maxDamage = health
   vision = 80;
    
   isMelee = false; // has a crossbow (for now)
};

function orcArcherBlock::onAdd(%this,%obj)
{
      %obj.mountImage(CrossbowImage,0);
}
function orcArcherBlock::onAttack(%this, %attacker, %target)
{
   %damage = %attacker.getDataBlock().baseDamage;
   if(%attacker.getNetModifier().baseDamage)
      %damage *= %attacker.getNetModifier().baseDamage;

   %armor  = %target.getDataBlock().armor;
   if(%target.getNetModifier().armor)
      %armor *= %target.getNetModifier().armor;
      
   if(%damage > %armor)
      %damage -= %armor;
   else
      %damage = 0;

    %target.damage(%attacker,0,%damage,"RANGED");
}
// warrior
datablock RTSUnitData(orcWarriorBlock : orcBlock)
{
   RTSUnitTypeName = "orcWarrior"; 
   baseDamage = 40;
   attackDelay = 40;        
   range=4; // map units - that can be far
   moveSpeed=4.5;  
   maxDamage = 200; //maxDamage = health
   vision = 80;
};

function orcWarriorBlock::onAdd(%this,%obj)
{
    %obj.mountImage(SpearImage,0);
    echo("RTSUnitData::onAdd -- mounting Spear to " SPC %obj.getName());    
}
