using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Math;

public class Level0 : CMSEntity
{
    public Level0()
    {
        Define<TagExecuteScript>().toExecute = Script;
    }

    IEnumerator Script()
    {
        G.main.HideHud();
        
        yield return G.main.Say("A tiny pack of tiny creatures was lost in the woods...");
        yield return G.main.SmartWait(5f);
        yield return G.main.Say("The food was scarce...");
        yield return G.main.SmartWait(3f);
        yield return G.main.Say("They had to choose who will be left behind...");
        yield return G.main.SmartWait(3f);
        yield return G.main.Unsay();

        G.main.AdjustSay(-1.2f);
        yield return G.main.Say("Pick 2, choose wisely.");
        
        G.ui.EnableInput();
        yield return G.main.SetupPicker(new List<DiceRarity>() { DiceRarity.COMMON, DiceRarity.UNCOMMON }, 2, dontClear: true);
        G.ui.DisableInput();
        
        yield return G.main.Say($"Poor <b>{G.main.picker.objects[0].GetNme()}</b> was <b>left behind</b>.");

        yield return G.main.SmartWait(3f);
        
        yield return G.main.picker.Clear();
    }
}

public class TagExecuteScript : EntityComponentDefinition
{
    public Func<IEnumerator> toExecute;
}