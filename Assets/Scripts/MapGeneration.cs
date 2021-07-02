using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject startRoom;

    public List<GameObject> basicRoom;
    public List<GameObject> rareRoom;
    public List<GameObject> challengeRoom;
    public List<GameObject> endRoom;

    public List<GameObject> ColumnCulDeSac;
    public List<GameObject> RowRightCulDeSac;

    public enum Zone // your custom enumeration
    {
        Foret,
        Vallée,
        Grotte,
        Coeur
    }

    public Zone zone;

    [Tooltip("De 0 à 3 (0 = Introduction, 1 = boss de début, 2 = merchand, 3 = boss)")]
    public int niveau;

    //*-----------------------------------------------------------------------------------------------------------------

    private int largeurGaucheMax = 2;
    private int largeurDroiteMax = 2;
    private int profondeurMax = 5;

    private int nbrTuileRandom = 0;
    private int nbrTuile = 0;

    private enum TypeRoom // your custom enumeration
    {
        Basic,
        Rare,
        Challenge,
        End,
        Start,
        Null
    }

    TypeRoom[,] dispositionMap;

    // Start is called before the first frame update
    void Start()
    {
        MiseAVideGrille();

        if (zone.ToString() == "Foret")
        {
            GenerationForet();
        }
    }

    void MiseAVideGrille()
    {
        int i = 0;
        int j = 0;

        while (i < 11)
        {
            while (j < 11)
            {
                dispositionMap = new TypeRoom[i, j];
                j++;
            }
            i++;
        }
    }

    void GenerationForet()
    {
        if (niveau == 1)
        {
            nbrTuileRandom = Random.Range(1, 4);
            nbrTuile = nbrTuileRandom + 6;
        }

        dispositionMap[5, 0] = TypeRoom.Start;


        
    }
}
