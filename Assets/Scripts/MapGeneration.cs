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

    public List<GameObject> delimiteurHorizontal;
    public List<GameObject> delimiteurVertical;

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

    private int largeurMax = 2;
    private int profondeurMax = 5;

    private int nbrTuileRandom = 0;
    private int nbrTuile = 0;
    private int nbrChallengeRoom = 0;
    private int nbrEndRoom = 1;

    private int columnRandom = 0;
    private int rowRandom = 0;
    private int directionRoom = 0; // 1 : A gauche, 2 : En haut, 3 : A droite
    private int rareRoomChance = 10; // comme tel, une chance sur 10 d'avoir une salle rare.
    private int roomTypeRandom = 0;
    private int positionChallenge = 0;

    private GameObject roomParent;
    private GameObject delimiteurParent;

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
        roomParent = GameObject.Find("Rooms");
        delimiteurParent = GameObject.Find("Delimiteurs");

        MiseAVideGrille();

        if (zone.ToString() == "Foret")
        {
            GenerationForet();
        }
    }

    void MiseAVideGrille()
    {
        dispositionMap = new TypeRoom[11, 11];
        int i = 0;
        int j = 0;

        while (i < 11)
        {
            j = 0;
            while (j < 11)
            {
                dispositionMap[i, j] = TypeRoom.Null;
                j++;
            }
            i++;
        }
    }

    void GenerationForet()
    {
        if (niveau == 1)
        {
            nbrTuileRandom = Random.Range(1, 5);
            nbrTuile = nbrTuileRandom + 6;
            Debug.Log("Nombre tuile1 : " + nbrTuile);
            nbrChallengeRoom = 1;
        }

        GenerationMap();
    }

    void GenerationMap()
    {
        dispositionMap[5, 0] = TypeRoom.Start;
        Debug.Log("Nombre tuile2 : " + nbrTuile);

        dispositionMap[5, 0] = TypeRoom.Start;

        GenerateBasicRoom();

        GenerateChallengeRoom();

        PlacementRoom();

        PlacementDelimiteur();

        //DebugFloor();
    }

    void GenerateBasicRoom()
    {
        Debug.Log("Nombre tuile3 : " + nbrTuile);

        while (nbrTuile > 0)
        {
            columnRandom = Random.Range(5 - largeurMax, 5 + largeurMax);
            rowRandom = Random.Range(0, profondeurMax + 1);

            bool emplacementOK = false;

            if (dispositionMap[columnRandom, rowRandom] != TypeRoom.Null)
            {
                emplacementOK = false;
                directionRoom = Random.Range(1, 4);
                
                if (directionRoom == 1)
                {
                    if (columnRandom - 5 + largeurMax > 0)
                    {
                        columnRandom--;
                        emplacementOK = true;
                    }
                }
                else if (directionRoom == 2)
                {
                    //Debug.Log("rowRandom : " + rowRandom + " / profondeur max : " + profondeurMax);
                    if (rowRandom < profondeurMax)
                    {
                        rowRandom++;
                        emplacementOK = true;
                    }
                }
                else if (directionRoom == 3)
                {
                    //Debug.Log("columnRandom - 5 - largeurMax : " + (columnRandom - 5 - largeurMax));
                    if (columnRandom - 5 - largeurMax < 0)
                    {
                        columnRandom++;
                        emplacementOK = true;
                    }
                }

                if (emplacementOK && dispositionMap[columnRandom,rowRandom] == TypeRoom.Null)
                {
                    roomTypeRandom = Random.Range(1, rareRoomChance + 1 );
                    //Debug.Log("Estimation du placement d'une salle en : " + columnRandom + "/" + rowRandom);

                    if (roomTypeRandom == rareRoomChance)
                    {
                        dispositionMap[columnRandom, rowRandom] = TypeRoom.Rare;
                    }
                    else
                    {
                        dispositionMap[columnRandom, rowRandom] = TypeRoom.Basic;
                    }
                    nbrTuile--;
                }
            }
        }
    }

    void GenerateChallengeRoom()
    {
        int x = 0;
        while ((nbrChallengeRoom > 0 || nbrEndRoom > 0) && x < 200)
        {
            int i = 5 - largeurMax;
            int j = profondeurMax;
            TypeRoom salleAPoser;
            directionRoom = Random.Range(1, 3);
            positionChallenge = Random.Range(1, profondeurMax + 1);

            while (i <= 5 + largeurMax && (nbrChallengeRoom > 0 || nbrEndRoom > 0) & x < 200)
            {
                j = profondeurMax;
                while (j >= 0 && (nbrChallengeRoom > 0 || nbrEndRoom > 0) && x < 200)
                {
                    int randomTypeSalle = Random.Range(1, nbrChallengeRoom + nbrEndRoom + 1);

                    if (randomTypeSalle == 1)
                    {
                        if (nbrEndRoom > 0)
                        {
                            salleAPoser = TypeRoom.End;
                        }
                        else
                        {
                            salleAPoser = TypeRoom.Challenge;
                        }
                    }
                    else
                    {
                        if (nbrChallengeRoom > 0)
                        {
                            salleAPoser = TypeRoom.Challenge;
                        }
                        else
                        {
                            salleAPoser = TypeRoom.End;
                        }
                    }

                    if (dispositionMap[i, j] != TypeRoom.Null)
                    {
                        int probaEnd = Random.Range(1, 8);
                        //if (probaEnd < 4)
                        
                        if (probaEnd < 4 )
                        {
                                // On regarde que la zone à gauche est nul et qu'il est possible d'y poser quelque chose, puis celle du haut puis la droite.
                                if (dispositionMap[i - 1, j] == TypeRoom.Null && j <= profondeurMax && i - 1 >= 5 - largeurMax && i - 1 <= 5 + largeurMax)
                                {
                                    if (dispositionMap[i - 2, j] == TypeRoom.Start || dispositionMap[i - 1, j + 1] == TypeRoom.Start || dispositionMap[i, j] == TypeRoom.Start ||
                                        dispositionMap[i - 2, j] == TypeRoom.Challenge || dispositionMap[i - 1, j + 1] == TypeRoom.Challenge || dispositionMap[i, j] == TypeRoom.Challenge ||
                                        dispositionMap[i - 2, j] == TypeRoom.End || dispositionMap[i - 1, j + 1] == TypeRoom.End || dispositionMap[i, j] == TypeRoom.End) { /* On ne fait rien */ } 
                                    else {
                                        Debug.Log("TypeRoom à gauche : " + dispositionMap[i - 1, j]);
                                        dispositionMap[i - 1, j] = salleAPoser;
                                        if (salleAPoser == TypeRoom.Challenge) { nbrChallengeRoom--; } else { nbrEndRoom--; }
                                        Debug.LogWarning("Salle à poser dans condition 1 : " + salleAPoser + " ici : " + (i - 1) + "/" + j);
                                    }
                                }
                                else if (dispositionMap[i, j + 1] == TypeRoom.Null && j + 1 <= profondeurMax && i >= 5 - largeurMax && i <= 5 + largeurMax)
                                {
                                    if (dispositionMap[i - 1, j + 1] == TypeRoom.Start || dispositionMap[i, j + 2] == TypeRoom.Start || dispositionMap[i + 1, j + 1] == TypeRoom.Start || dispositionMap[i, j] == TypeRoom.Start ||
                                        dispositionMap[i - 1, j+1] == TypeRoom.Challenge || dispositionMap[i, j + 2] == TypeRoom.Challenge || dispositionMap[i + 1, j + 1] == TypeRoom.Challenge || dispositionMap[i, j] == TypeRoom.Challenge ||
                                        dispositionMap[i - 1, j+1] == TypeRoom.End || dispositionMap[i, j + 2] == TypeRoom.End || dispositionMap[i + 1, j +1] == TypeRoom.End || dispositionMap[i, j] == TypeRoom.End) { /* On ne fait rien */ }
                                    else
                                    {
                                        Debug.Log("TypeRoom en haut : " + dispositionMap[i, j + 1]);
                                        dispositionMap[i, j + 1] = salleAPoser;
                                        if (salleAPoser == TypeRoom.Challenge) { nbrChallengeRoom--; } else { nbrEndRoom--; }
                                        Debug.LogWarning("Salle à poser dans condition 2 : " + salleAPoser + " ici : " + i + "/" + (j + 1) );
                                    }
                                }
                                else if (dispositionMap[i + 1, j] == TypeRoom.Null && j <= profondeurMax && i + 1 >= 5 - largeurMax && i + 1 <= 5 + largeurMax)
                                {
                                    if (dispositionMap[i + 1, j + 1] == TypeRoom.Start || dispositionMap[i + 2, j] == TypeRoom.Start || dispositionMap[i, j] == TypeRoom.Start ||
                                        dispositionMap[i + 1, j + 1] == TypeRoom.Challenge || dispositionMap[i + 2, j] == TypeRoom.Challenge || dispositionMap[i, j] == TypeRoom.Challenge ||
                                        dispositionMap[i + 1, j + 1] == TypeRoom.End || dispositionMap[i + 2, j] == TypeRoom.End || dispositionMap[i, j] == TypeRoom.End) { /* On ne fait rien */ }
                                    else
                                    {
                                        Debug.Log("TypeRoom à droite : " + dispositionMap[i + 1, j]);
                                        dispositionMap[i + 1, j] = salleAPoser;
                                        if (salleAPoser == TypeRoom.Challenge) { nbrChallengeRoom--; } else { nbrEndRoom--; }
                                        Debug.LogWarning("Salle à poser dans condition 3 : " + salleAPoser + " ici : " + (i + 1) + "/" + j);
                                    }
                                }
                            
                        }
                    }
                    j--;
                    x++;
                }
                i++;
            }
        }
        if (x == 200)
        {
            Debug.LogError("On a pas pu mettre toutes les salles. :(");
        }
    }

    void PlacementRoom()
    {
        int i = 10;
        int j = 10;

        while (i >= 0)
        {
            j = 10;
            while (j >= 0)
            {
                Vector3 positionRoom = new Vector3(i *30 - 150 , j * 17);
                Quaternion rotationRoom = new Quaternion(0, 0, 0, 0);

                //Debug.Log("i/j : " + i + "/" + j);

                if (dispositionMap[i,j] == TypeRoom.Basic)
                {
                    int k = Random.Range(0, basicRoom.Count);
                    GameObject newRoom = Instantiate(basicRoom[k], positionRoom, rotationRoom);
                    newRoom.transform.parent = roomParent.transform;
                    Debug.Log("Placement d'une salle basic en : " + i + " / " + j);
                }
                else if (dispositionMap[i, j] == TypeRoom.Rare)
                {
                    int k = Random.Range(0, rareRoom.Count);
                    GameObject newRoom = Instantiate(rareRoom[k], positionRoom, rotationRoom);
                    newRoom.transform.parent = roomParent.transform;
                    Debug.Log("Placement d'une salle rare en : " + i + " / " + j);
                }
                else if (dispositionMap[i, j] == TypeRoom.Challenge)
                {
                    int k = Random.Range(0, challengeRoom.Count);
                    GameObject newRoom = Instantiate(challengeRoom[k], positionRoom, rotationRoom);
                    newRoom.transform.parent = roomParent.transform;
                    Debug.Log("Placement d'une salle challenge en : " + i + " / " + j);
                }
                else if (dispositionMap[i, j] == TypeRoom.End)
                {
                    int k = Random.Range(0, endRoom.Count);
                    GameObject newRoom = Instantiate(endRoom[k], positionRoom, rotationRoom);
                    newRoom.transform.parent = roomParent.transform;
                    Debug.Log("Placement d'une salle boss en : " + i + " / " + j);
                }

                j--;
            }
            i--;
        }
    }

    void PlacementDelimiteur()
    {
        // On place les délimiteurs en regardant de bas/gauche jusqu'à haut/droite
        int i = 0;
        
        while (i < 10)
        {
            int j = 0;

            while (j < 10)
            {
                Vector3 positionRoom = new Vector3(i * 30 - 150, j * 17);
                Quaternion rotationRoom = new Quaternion(0, 0, 0, 0);
                

                // On regarde à Gauche
                if (dispositionMap[i, j] != TypeRoom.Null)
                {
                    Vector3 positionDelimiteur = positionRoom;
                    if (i > 0)
                    {
                        if (dispositionMap[i - 1, j] == TypeRoom.Null)
                        {
                            int k = Random.Range(0, delimiteurVertical.Count);
                            positionDelimiteur.x -= 15f;
                            GameObject newDelimiteur = Instantiate(delimiteurVertical[k], positionDelimiteur, rotationRoom);
                            newDelimiteur.transform.parent = delimiteurParent.transform;
                        }
                    }
                    else
                    {
                        int k = Random.Range(0, delimiteurVertical.Count);
                        positionDelimiteur.x -= 15f;
                        GameObject newDelimiteur = Instantiate(delimiteurVertical[k], positionDelimiteur, rotationRoom);
                        newDelimiteur.transform.parent = delimiteurParent.transform;
                    }
                }

                // On regarde en Haut
                if (dispositionMap[i, j] != TypeRoom.Null)
                {
                    Vector3 positionDelimiteur = positionRoom;
                    if (j < 10)
                    {
                        if (dispositionMap[i, j + 1] == TypeRoom.Null)
                        {
                            int k = Random.Range(0, delimiteurHorizontal.Count);
                            //positionDelimiteur.y += 8.5f;
                            positionDelimiteur.y += 9f;
                            GameObject newDelimiteur = Instantiate(delimiteurHorizontal[k], positionDelimiteur, rotationRoom);
                            newDelimiteur.transform.parent = delimiteurParent.transform;
                        }
                    }
                    else
                    {
                        int k = Random.Range(0, delimiteurHorizontal.Count);
                        //positionDelimiteur.y += 8.5f;
                        positionDelimiteur.y += 9f;
                        GameObject newDelimiteur = Instantiate(delimiteurHorizontal[k], positionDelimiteur, rotationRoom);
                        newDelimiteur.transform.parent = delimiteurParent.transform;
                    }
                }

                // On regarde à Droite
                if (dispositionMap[i, j] != TypeRoom.Null)
                {
                    Vector3 positionDelimiteur = positionRoom;
                    if (i < 10)
                    {
                        if (dispositionMap[i + 1, j] == TypeRoom.Null)
                        {
                            int k = Random.Range(0, delimiteurVertical.Count);
                            positionDelimiteur.x += 15f;
                            GameObject newDelimiteur = Instantiate(delimiteurVertical[k], positionDelimiteur, rotationRoom);
                            newDelimiteur.transform.parent = delimiteurParent.transform;
                        }
                    }
                    else
                    {
                        int k = Random.Range(0, delimiteurVertical.Count);
                        positionDelimiteur.x += 15f;
                        GameObject newDelimiteur = Instantiate(delimiteurVertical[k], positionDelimiteur, rotationRoom);
                        newDelimiteur.transform.parent = delimiteurParent.transform;
                    }
                }

                // On regarde en Bas
                if (dispositionMap[i, j] != TypeRoom.Null)
                {
                    Vector3 positionDelimiteur = positionRoom;
                    if (j > 0)
                    {
                        if (dispositionMap[i, j - 1] == TypeRoom.Null)
                        {
                            int k = Random.Range(0, delimiteurHorizontal.Count);
                            //positionDelimiteur.y -= 8.5f;
                            positionDelimiteur.y -= 8f;
                            GameObject newDelimiteur = Instantiate(delimiteurHorizontal[k], positionDelimiteur, rotationRoom);
                            newDelimiteur.transform.parent = delimiteurParent.transform;
                        }
                    }
                    else
                    {
                        int k = Random.Range(0, delimiteurHorizontal.Count);
                        //positionDelimiteur.y -= 8.5f;
                        positionDelimiteur.y -= 8f;
                        GameObject newDelimiteur = Instantiate(delimiteurHorizontal[k], positionDelimiteur, rotationRoom);
                        newDelimiteur.transform.parent = delimiteurParent.transform;
                    }
                }

                j++;
            }

            i++;
        }
    }

    void DebugFloor()
    {
    }
}
