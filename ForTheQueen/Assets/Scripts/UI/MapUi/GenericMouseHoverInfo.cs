using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericMouseHoverInfo : AdaptableInterfaceMask<TileOccupationInteractableScritableObject>
{

    public Image backGroundImage;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;
    
    protected override void AdaptUITo(TileOccupationInteractableScritableObject value, Vector3 pos)
    {
        RectTransform rect = GetComponent<RectTransform>();
        backGroundImage.color = value.hoverBackgroundSpriteColor;
        titleText.text = value.occupationName;
        descriptionText.text = value.description;
        Vector3 newPos = pos;
        newPos.y += 0.5f;
        newPos.z += rect.sizeDelta.y * rect.localScale.z + HexagonWorld.HEX_DIAMETER / 2;
        newPos.x -= rect.sizeDelta.x / 2 * rect.localScale.x;
        transform.position = newPos;
        //transform.LookAt(Camera.main.transform);
    }
}
