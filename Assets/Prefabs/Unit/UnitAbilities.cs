using UnityEngine;
using System.Collections;

public enum Abilities
{
    Combat,
    Research,
    Mining,
    Hunting,
    Crafting
}

public enum UnitType
{
    Rookie,
    Soldier,
    Scientist,
    Miner,
    Hunter,
    Engineer
}

[System.Serializable]
public class UnitAbilities {

    public string name;

    public int combat=1;
    public int research=1;
    public int mining=1;
    public int hunting=1;
    public int crafting=1;

    public UnitType type = UnitType.Rookie;

    public int[] ability { get; private set; }
    public int[] XP { get; private set; }

    public int enemiesKilled { get; private set; }
    public int crittersHunted { get; private set; }

    private int hostUID;


    public UnitAbilities()
    {
        ability =  new int[] { combat, research, mining, hunting, crafting };
        XP = new int[] { combat, research, mining, hunting, crafting };
    }

    public UnitAbilities(int uid)
    {
        hostUID = uid;
        ability = new int[] { combat, research, mining, hunting, crafting };
        XP = new int[] { combat, research, mining, hunting, crafting };
    }

    public UnitType generateUnitType()
    {
        UnitType t = UnitType.Rookie;

        Abilities ab=Abilities.Combat;
        int abMax = 0;
        bool isEqual = true;
        for(int i=0; i<ability.Length;i++)
        {
            isEqual = abMax == ability[i] && isEqual;
            if (ability[i] > abMax)
            {
                ab = (Abilities)i;
                abMax = ability[i];
            }
        }
        if (isEqual)
        {
            abMax = 0;
            for (int i = 0; i < XP.Length; i++)
            {
                isEqual = abMax == XP[i] && isEqual;
                if (XP[i] > abMax)
                {
                    ab = (Abilities)i;
                    abMax = XP[i];
                }
            }
        }
        if (ab == Abilities.Combat) t = UnitType.Soldier;
        if (ab == Abilities.Research) t = UnitType.Scientist;
        if (ab == Abilities.Mining) t = UnitType.Miner;
        if (ab == Abilities.Hunting) t = UnitType.Hunter;
        if (ab == Abilities.Crafting) t = UnitType.Engineer;

        return t;
    }

    public void levelUpAbilities()
    {
        for(Abilities a = Abilities.Combat; a <= Abilities.Crafting; a++)
        {
            ability[(int)a] += XP[(int)a] / 10;
        }
    }

    public int AddXP(Abilities a, int val)
    {
        XP[(int)a] += val;
        return XP[(int)a];
    }

    public void killedEnemy(int xpBoost)
    {
        enemiesKilled++;
        AddXP(Abilities.Combat, xpBoost);
    }

    public void huntedCritter(int xpBoost)
    {
        crittersHunted++;
        AddXP(Abilities.Hunting, xpBoost);
    }

}
