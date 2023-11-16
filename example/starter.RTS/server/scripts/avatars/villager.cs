datablock RTSUnitData(villagerBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/bot/bot.dts";
   //peasant/player.dts";
   RTSUnitTypeName = "villager";
   baseDamage = 16;
   attackDelay = 30;
   damagePlus = 1;   
   armor = 1;   
   moveSpeed = 5;   
   range = 5;   
   maxDamage = 150; //maxDamage = health
   vision = 60;   
   isMelee = true;
   boundingBox = "2.0 2.0 2.0";
};

function villagerBlock::onAttack(%this, %attacker, %target)
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
// it's not nice to apply damage directly
//   %target.applyDamage(%damage);
   %target.damage(%attacker,0,%damage,"MELEE");
 }
