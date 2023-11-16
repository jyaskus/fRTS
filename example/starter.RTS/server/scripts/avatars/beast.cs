datablock RTSUnitData(beastBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/beast/beast.dts";

   RTSUnitTypeName = "beast";
   baseDamage = 20; //30;
   attackDelay = 25;
   damagePlus = 3;   
   armor = 2;   
   moveSpeed = 9;   
   range = 2;   
   maxDamage = 250; //maxDamage = health
   vision = 100;   
   isMelee = true;
   
   boundingBox = "2.0 2.0 2.0";
   
   rechargeRate=0.25;
   repairRate=0.25;   
};

function beastBlock::onAttack(%this, %attacker, %target)
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
//   %target.applyDamage(%damage);
}
