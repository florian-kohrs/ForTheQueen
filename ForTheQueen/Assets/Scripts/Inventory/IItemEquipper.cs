using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemEquipper
{

    Transform HelmetParent { get; }

    void EquipHelmet(GameObject g);

    Transform WeaponParent { get; }

    void EquipWeapon(GameObject g);

}
