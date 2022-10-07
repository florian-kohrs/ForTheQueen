using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroDesigner : MonoBehaviourPun
{

    protected HeroCustomization HeroCustomization => hero.customization;

    protected Hero hero;

    public void StartDesigner(Hero hero)
    {
        this.hero = hero;
        gameObject.SetActive(true);

        classSelection.AddDesignChangeListener(OnClassChanges);
        foreach (var item in Selections)
        {
            item.gameObject.SetActive(true);
            item.AddDesignChangeListener(DesignChanged);
            item.SetHero(hero);
        }
    }

    public IEnumerable<ListSelection> Selections
    {
        get
        {
            yield return classSelection;
            yield return raceSelection;
            //yield return beardSelection;
            //yield return otherSelection;
        }
    }

    public ListSelection classSelection;
    public ListSelection raceSelection;
    public ListSelection beardSelection;
    public ListSelection otherSelection;


    private void Start()
    {
        foreach (var item in Selections)
        {
            item.gameObject.SetActive(false);
        }
    }

    protected void OnClassChanges()
    {

    }

    [PunRPC]
    protected void UpdateHeroCustomziation(object[] listIndices)
    {
        int[] newIndices = listIndices.OfType<int>().ToArray();
        int i = 0;
        foreach (var item in Selections)
        {
            item.UpdateSelection(newIndices[i]);
            i++;
        }
        ApplySelectionToHero();
    }

    protected void ApplySelectionToHero()
    {
        HeroCustomization.classId = classSelection.CurrentSelectedIndex;
        HeroCustomization.raceId = raceSelection.CurrentSelectedIndex;
        //hero.customization.beardId = beardSelection.CurrentSelectedIndex;
    }

    public void HideDesigners()
    {
        gameObject.SetActive(false);
    }

    public void ShowDesigner()
    {
        gameObject.SetActive(true);
    }

    protected object GetRpcReadyIndices()
    {
        return Selections.Select(s => (object)s.CurrentSelectedIndex).ToArray();
    }

    protected void DesignChanged()
    {
        if (!hero.IsMine)
            return;

        ApplySelectionToHero();
        Broadcast.SafeRPC(photonView, nameof(UpdateHeroCustomziation), RpcTarget.Others, null,
            GetCurrentSelectedIndices());
    }

    protected object[] GetCurrentSelectedIndices()
    {
        object[] indices = new object[Selections.Count()];
        int i = 0;
        foreach (var item in Selections)
        {
            indices[i] = item.CurrentSelectedIndex;
            i++;
        }
        return indices;
    }

}
