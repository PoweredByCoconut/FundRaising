using System;
using Godot;
// Character class represents a fighter in the game. The point of this class is to
// Manage the characters so that they all have a fair fight.
namespace CharacterNS
{
  public class Character
  {
    public const int MAXHEALTH = 20;
    private const int MAXATK = 6;
    private const int MAXDEF = 6;
    private int health;
    private int atkPwr;
    private int defense;
    private Boolean isAlive;

    // constructor
    public Character()
    {
      new Character(20);
    }
    public Character(int startingHealth)
    {
      this.health = (startingHealth <= MAXHEALTH) ? startingHealth : MAXHEALTH;
      this.atkPwr = 3;
      this.defense = 1;
      this.isAlive = true;
    }

    // Adds to defense rating
    // no ARGS
    public void defend()
    {
      if (isAlive)
      {
        this.defense = (this.defense + 3 > MAXDEF) ? MAXDEF : this.defense + 2; // increase by 5
        this.atkPwr = (this.atkPwr - 1 < 1) ? 1 : this.atkPwr - 1;
      }
    }

    // Attacks another character
    // Character defending - The character that is getting attacked
    public Boolean attack(Character defending)
    {
      if (isAlive)
      {
        // deal damage to the defender
        Boolean attacked = defending.recieveAtk(this.atkPwr);
        // int newAtk = (int)Math.Ceiling(this.atkPwr * 1.5);
        int newAtk = this.atkPwr + 1;
        this.atkPwr = (newAtk > MAXATK) ? MAXATK : newAtk;
        this.defense = (this.defense - 1 < 1) ? 1 : this.defense - 2;
        return attacked;
      }
      else
      {
        return false;
      }
    }

    // Heal the character
    // no ARGS
    public void heal()
    {
      if (isAlive)
      {
        this.health = (this.health + 7 >= MAXHEALTH) ? MAXHEALTH : this.health + 2;
        this.atkPwr = (this.atkPwr + 1 > MAXATK) ? MAXATK : this.atkPwr + 1;
        this.defense = (this.defense - 1 < 1) ? 1 : this.defense - 1;
      }
    }

    // Recieve damage to self
    // int dmg - amount of damage recieved
    private Boolean recieveAtk(int dmg)
    {
      int effectiveDamage = Math.Max(0, dmg - this.defense);
      int newHealth = this.health - effectiveDamage;
      if (newHealth <= 0)
      {
        this.isAlive = false;
        this.health = 0; // Ensure health is explicitly set to 0 upon death
        return false;
      }
      this.health = newHealth; // FIX: Update the character's actual health
      return true;
    }

    public int getHealth() => this.health;
    public int getAtkPwr() => this.atkPwr;
    public int getDefense() => this.defense;
    public Boolean getIsAlive() => this.isAlive;
  }

}
