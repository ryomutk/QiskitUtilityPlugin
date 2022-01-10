using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class QPInputUtility : MonoBehaviour

{
    Vector2 pointer = Vector2.zero;
    [SerializeField] Sprite XTex;
    [SerializeField] Sprite HTex;
    [SerializeField] Image imagePref;
    [SerializeField] InputBar barPref;
    [SerializeField] Transform barMother;
    [SerializeField] Cursor cursor;
    [SerializeField] GateIndicator indicator;
    Vector2Int cPos = Vector2Int.zero;
    Vector2 gap { get { return QPongConfig.instance.gap; } }
    QPongConfig config { get { return QPongConfig.instance; } }
    Image[,] imageComplex;

    void Start()
    {
        imageComplex = new Image[config.orderNum, config.register];

        for (int i = 0; i < config.orderNum; i++)
        {
            for (int x = 0; x < config.register; x++)
            {
                var img = Instantiate(imagePref, transform);
                img.transform.position = GetPosition(i, x);
                imageComplex[i, x] = img;
                img.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < config.register; i++)
        {
            var p = barMother.transform.position;
            p.y -= gap.y * i;
            var bar = Instantiate(barPref, barMother, false);

            bar.transform.position = p;
        }

        var register = QPongConfig.instance.register;

        var image = cursor.image;
        var pos = barMother.position;
        image.transform.position = pos;
    }

    List<InputID> receved = new List<InputID>();
    List<KeyBind> keyConfig { get { return QPongConfig.instance.keyConfig; } }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            receved.Clear();

            foreach (var bind in keyConfig)
            {
                if (Input.GetKeyDown(bind.key))
                {
                    receved.Add(bind.id);
                    InputAction(bind.id);
                }
            }
        }

    }

    CircuitManager circuit { get { return DataProvider.instance.circuit; } }
    void InputAction(InputID id)
    {
        switch (id)
        {
            case InputID.up:
                cPos.y -= 1;
                UpdateCursor();
                break;
            case InputID.left:
                cPos.x -= 1;
                UpdateCursor();
                break;
            case InputID.right:
                cPos.x += 1;
                UpdateCursor();
                break;
            case InputID.down:
                cPos.y += 1;
                UpdateCursor();
                break;
            case InputID.hGate:
                if (circuit.CheckGate(cPos.y, cPos.x) == Gates.H)
                {
                    circuit.RemoveAt(cPos.y, cPos.x);
                    UpdateGate(cPos, Gates.none);
                }
                else
                {
                    circuit.AppendAt(cPos.y, cPos.x, Gates.H);
                    UpdateGate(cPos, Gates.H);
                }
                break;
            case InputID.xGate:
                if (circuit.CheckGate(cPos.y, cPos.x) == Gates.X)
                {
                    circuit.RemoveAt(cPos.y, cPos.x);
                    UpdateGate(cPos, Gates.none);
                }
                else
                {
                    circuit.AppendAt(cPos.y, cPos.x, Gates.X);
                    UpdateGate(cPos, Gates.X);
                }
                break;
        }
    }

    void UpdateCursor()
    {
        if (cPos.x >= config.orderNum)
        {
            cPos.x = 0;
        }
        else if (cPos.x < 0)
        {
            cPos.x = config.orderNum - 1;
        }

        if (cPos.y >= config.register)
        {
            cPos.y = 0;
        }
        else if (cPos.y < 0)
        {
            cPos.y = config.register - 1;
        }

        var coords = cPos;
        cursor.transform.position = GetPosition(coords.x, coords.y);
    }

    Vector2 GetPosition(int x, int y)
    {
        var pos = barMother.transform.position;
        pos.x += gap.x * x;
        pos.y -= gap.y * y;


        return pos;
    }


    void UpdateGate(Vector2Int position, Gates gate)
    {

        var img = imageComplex[position.x, position.y];
        switch (gate)
        {
            case Gates.X:
                img.gameObject.SetActive(true);
                img.sprite = XTex;
                break;
            case Gates.H:
                img.gameObject.SetActive(true);
                img.sprite = HTex;
                break;
            case Gates.none:
                img.gameObject.SetActive(false);
                break;
        }

        indicator.UpdateState();
    }
}