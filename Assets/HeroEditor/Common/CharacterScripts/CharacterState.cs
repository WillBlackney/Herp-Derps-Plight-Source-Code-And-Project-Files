namespace Assets.HeroEditor.Common.CharacterScripts
{
    /// <summary>
    /// A list of possible character states for lower body animation (except Ready and Relax).
    /// </summary>
	public enum CharacterState
	{
		Ready		= -2,
        Relax		= -1,
        Idle 		= 0,
        Walk 		= 1,
        Run 		= 2,
        Jump 		= 3,
		Crouch 		= 4,
        Climb 		= 5,
        DeathB 		= 6,
        DeathF 		= 7
    }
}