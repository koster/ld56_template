using System;
using System.Collections.Generic;

public static class PriorityLayers
{
    public static int FIRST = -10000;
    public static int NORMAL = 0;
    public static int LAST = 10000;
    public static int LAST_SPECIAL = 10001;
}

public abstract class BaseInteraction
{
    public virtual int Priority()
    {
        return PriorityLayers.NORMAL;
    }
}

public class Interactor
{
    public List<BaseInteraction> all = new();

    public void Init()
    {
        var allTypes = ReflectionUtil.FindAllSubslasses<BaseInteraction>();
        foreach (var t in allTypes)
            all.Add(Activator.CreateInstance(t) as BaseInteraction);
    }

    public List<T> FindAll<T>()
    {
        return InteractionCache<T>.FindAll(this);
    }
}

public static class InteractionCache<T>
{
    static List<T> all;

    public static List<T> FindAll(Interactor interactor)
    {
        if (all != null)
            return all;

        all = new List<T>(64);
        foreach (var a in interactor.all)
            if (a is T ast)
                all.Add(ast);
        all.Sort((a, b) => (a as BaseInteraction).Priority() - (b as BaseInteraction).Priority());
        return all;
    }
}