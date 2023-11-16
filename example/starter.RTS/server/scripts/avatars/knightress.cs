datablock RTSUnitData(knightressBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/knightress/knightress.dts";  
   RTSUnitTypeName = "knightress";

   baseDamage = 42; //56;
   attackDelay = 32;
   damagePlus = 3;   
   armor = 12;   
   moveSpeed = 5;   
   range = 5;
   maxDamage = 420; //maxDamage = health
   vision = 90;   
   boundingBox = "2.0 2.0 2.0";   
   isMelee = true;
   
   rechargeRate=0.25;
   repairRate=0.25;   
};

function knightressBlock::onAttack(%this, %attacker, %target)
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

   // http://www.torquepowered.com/community/forums/viewthread/36438
   //   %target.applyDamage(%damage);
   %target.damage(%attacker,0,%damage,"MELEE");
}

