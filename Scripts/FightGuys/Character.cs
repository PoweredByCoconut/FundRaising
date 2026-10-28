using System;
// Character class represents a fighter in the game. The point of this class is to
// Manage the characters so that they all have a fair fight.
namespace CharacterNS{
  public class Character
  {
    private int health;
    private int atkPwr;
    private int defense;
    private Boolean isAlive;

    // constructor
    public Character ()
    {
      new Character(30);
    }
    public Character (int startingHealth)
    {
      this.health = startingHealth;
      this.atkPwr = 3;
      this.defense = 1;
      this.isAlive = true;
    }

    // Adds to defense rating
    // no ARGS
    public void defend()
    {
      if (isAlive) {
        this.defense += 5;
        this.health += 1;
        this.atkPwr -= 2;
      }
    }

    // Attacks another character
    // Character defending - The character that is getting attacked
    public Boolean attack(Character defending)
    {
      if (isAlive) {
        // deal damage to the defender
        Boolean attacked = defending.recieveAtk(this.atkPwr);
        this.atkPwr = (int) Math.Ceiling(this.atkPwr * 1.5);
        this.defense -= 2;
        return attacked;
      } else {
        return false;
      }
    }

    // Heal the character
    // no ARGS
    public void heal()
    {
      if (isAlive) {
        this.health += 3;
        this.atkPwr += 1;
        this.defense -= 1;
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

    public int getHealth() => health;
    public int getAtkPwr() => atkPwr;
    public int getDefense() => defense;
    public Boolean getIsAlive() => isAlive;
  }

}