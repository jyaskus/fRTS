datablock RTSUnitData(stonemanBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/RTSCreaturePack/StoneMan/stoneman.dts";
   RTSUnitTypeName = "stoneman";
   baseDamage = 56; //74;
   attackDelay = 42;
   damagePlus = 3;  
   armor = 6;   
   moveSpeed = 3;   
   range = 4;   
   maxDamage = 480; //maxDamage = health
   vision = 50;   
   boundingBox = "2.0 2.0 2.0";   
   isMelee = true;
   rechargeRate=0.1;
   repairRate=0.1;   
};

function stonemanBlock::onAttack(%this, %attacker, %target)
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

//   %target.applyDamage(%damage);
   %target.damage(%attacker,0,%damage,"MELEE");   
}