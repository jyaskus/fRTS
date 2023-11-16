datablock RTSUnitModifierData(SharpenedWeaponsModifier)
{
   damage = 1.5;
};

datablock RTSUnitModifierData(CrippleModifier)
{
//-----------------------Begin Bug Fix: http://garagegames.com/mg/forums/result.thread.php?qt=23339
   moveSpeed = -0.5;
//-----------------------End Bug Fix 
};

datablock RTSUnitModifierData(HardenedArmorModifier)
{
   armor = 1.5;
};

datablock RTSUnitModifierData(SwiftFeetModifier)
{
   moveSpeed = 1.5;
};

datablock RTSUnitModifierData(Bonus)
{
   baseDamage = 1.5;
   attackDelay = 0.75;
   armor = 1.5;
   moveSpeed = 1.5;
};

datablock RTSUnitModifierData(Handicap)
{
   baseDamage = 0.5;
   attackDelay = 1.25;
   armor = 0.5;
   moveSpeed = 0.5;
};

//-----------------------Begin Bug Fix: http://garagegames.com/mg/forums/result.thread.php?qt=23339
datablock RTSUnitModifierData(BaseStats)
{
	damage = 1.0;
	attackDelay = 1.0;
	armor = 1.0;
	moveSpeed = 1.0;
	range = 1.0;
	vision = 1.0;
};
//-----------------------End Bug Fix 