using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeroDesigner : MonoBehaviourPun
{

    protected HeroCustomization HeroCustomization => Hero.customization;

    public Hero Hero { get; protected set; }

    protected bool buttonsEnabled = true;

    protected bool IsMine => Hero.IsMine;

    public void SetButtonEnabledState(bool state)
    {
        buttonsEnabled = state && IsMine;
        foreach (var item in Selections)
        {
            item.SetButtonEnabledState(buttonsEnabled);
        }
    }

    public void StartDesigner(Hero hero)
    {
        this.Hero = hero;
        gameObject.SetActive(true);

        SetInteractableOfAllChildButtons(IsMine);


        classSelection.AddDesignChangeListener(OnClassChanges);
        foreach (var item in Selections)
        {
            item.gameObject.SetActive(true);
            item.SetButtonEnabledState(IsMine);
            item.AddDesignChangeListener(DesignChanged);
            item.SetHero(hero);
        }
    }

    protected void SetInteractableOfAllChildButtons(bool state)
    {
        GetComponentsInChildren<Button>().ToList().ForEach(b => b.interactable = state);
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
        if (!Hero.IsMine)
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
