datablock RTSUnitData(shamanBlock : UnitBaseBlock)
{
   shapeFile="~/data/shapes/PACK/guardian/guardian.dts";  
   RTSUnitTypeName="shaman";
   baseDamage=42; ///60;
   attackDelay=32;
   damagePlus=3;   
   armor=5;   
   moveSpeed=5;   
   range=22;   
   maxDamage=320; //maxDamage = health
   vision=90;
   isMelee=false;
   
   rechargeRate=0.25;
   repairRate=0.25;
   
   boundingBox = "2.0 2.0 2.0";   
};

function shamanBlock::onAttack(%this, %attacker, %target)
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