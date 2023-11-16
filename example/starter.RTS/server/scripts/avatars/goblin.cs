datablock RTSUnitData(goblinBlock : UnitBaseBlock)
{
   shapeFile = "~/data/shapes/RTSCreaturePack/Goblin/goblin.dts";
   RTSUnitTypeName = "goblin";
   baseDamage = 45; //60;
   attackDelay = 35;
   damagePlus = 3;   
   armor = 1;   
   moveSpeed = 4.5;   
   range = 26;   
   maxDamage = 250; 
   vision = 90;   
   isMelee = false;   
   boundingBox = "2.0 2.0 2.0";
};

function goblinBlock::onAttack(%this, %attacker, %target)
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
   %target.damage(%attacker,0,%damage,"MAGIC");
}
