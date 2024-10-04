using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Savefile
{
    public float volSfx = 0.8f;
    public float volMusic = 0.8f;
}

public class Savesystem : MonoBehaviour
{
    const string SlotId = "saveslot";
    public static Savefile slot;

    void Awake()
    {
        TryLoading();
        G.save = slot;

        StartCoroutine(SaveEverySoOften());
    }

    void TryLoading()
    {
        slot = JsonConvert.DeserializeObject<Savefile>(PlayerPrefs.GetString(SlotId, "{}"));
    }

    IEnumerator SaveEverySoOften()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            var str = JsonConvert.SerializeObject(slot);
            PlayerPrefs.SetString(SlotId, str);
        }
    }
}