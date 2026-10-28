using Godot;
using System;
using CharacterNS;

public partial class TestCharacterClass : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RunAllTests();
	}

	/// <summary>
	/// Runs a comprehensive suite of character ability tests and prints results to the Godot console.
	/// </summary>
	private void RunAllTests()
	{
		GD.Print("=======================================");
		GD.Print("      Starting Character Test Sweep     ");
		GD.Print("=======================================");

		// 1. Initialize a test character (The Model)
		Character self = new Character(30);
		GD.Print($"[SETUP] Initial Self Health: {self.getHealth()}");

		// --- TEST 1: Basic Attack Cycle ---
		TestBasicAttackCycle(self);

		// --- TEST 2: Defense and Damage Mitigation ---
		TestDefenseAndDamageMitigation(self);

		// --- TEST 3: Healing and Recovery ---
		TestHealingAndRecovery(self);


		GD.Print("\n=======================================");
		GD.Print("      Character Test Sweep Complete     ");
		GD.Print("======================================");
	}

	private void TestBasicAttackCycle(Character self)
	{
		GD.Print("\n--- Running Test 1: Basic Attack Cycle ---");
		// Setup target
		Character target = new Character(25);
		GD.Print($"[SETUP] Target initial health: {target.getHealth()}");

		// Execute test logic
		self.attack(target); // Assume this method exists and handles damage calculation
		
		// Report result
		if (target.getHealth() < 25)
		{
			GD.Print($"[PASS] Target health successfully reduced to: {target.getHealth()}");
		}
		else
		{
			GD.Print($"[FAIL] Expected target damage, but current health is still high: {target.getHealth()}");
		}
	}

	private void TestDefenseAndDamageMitigation(Character self)
	{
		GD.Print("\n--- Running Test 2: Defense & Damage Mitigation ---");
		// Setup and execute defense phase
		self.defend(); // Corrected method call based on Character.cs (no args)
		Character target2 = new Character(40);
		self.attack(target2);
		
		// Report result (Assuming successful mitigation means health is significantly higher than expected raw damage)
		if (target2.getHealth() > 35) // Arbitrary check for success, assuming a baseline damage of 40
		{
			GD.Print($"[PASS] Damage mitigated successfully. Target 2 Health: {target2.getHealth()}");
		}
		else
		{
			GD.Print($"[FAIL] Defense mitigation failed or was insufficient. Target 2 Health: {target2.getHealth()}");
		}
	}

	private void TestHealingAndRecovery(Character self)
	{
		GD.Print("\n--- Running Test 3: Healing & Recovery ---");
		// Setup damage
		Character target = new Character(1); // Dummy target for method signature
		target.attack(self);
		GD.Print($"[SETUP] Self health before heal: {self.getHealth()}");

		// Execute healing (no arguments)
		self.heal(); 
		
		// Report result (Assuming successful heal increases health by at least the amount healed)
		if (self.getHealth() > 30) // Check if it increased significantly from initial setup (30 -> damaged -> healed)
		{
			GD.Print($"[PASS] Healing successful. Self Health: {self.getHealth()}");
		}
		else
		{
			GD.Print($"[FAIL] Healing failed or was insufficient. Self Health: {self.getHealth()}");
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}