using System.Threading;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using QiskitPlugin.Utility;
using QiskitPlugin.Input;

namespace QiskitPlugin.UI
{
    public class QiskitGUI : MonoBehaviour
    {
        Vector2 pointer = Vector2.zero;
        [SerializeField] SerializableDictionary<Gates, Sprite> gateTexs;
        [SerializeField] Sprite controllTex;
        [SerializeField] Image imagePref;
        [SerializeField] InputBar barPref;
        [SerializeField] Transform barMother;
        [SerializeField] Cursor cursor;
        //初期化に使うQiskitRegisterの数
        [SerializeField] int defaultRegister = 3;
        [SerializeField] GUIConfig config;


        Vector2Int cPos = Vector2Int.zero;
        Vector2 gap { get { return config.gap; } }
        int nowRegister { get; set; }
        Image[,] imageComplex;

        void Start()
        {
            nowRegister = defaultRegister;
            imageComplex = new Image[config.orderNum, nowRegister];

            for (int i = 0; i < config.orderNum; i++)
            {
                for (int x = 0; x < nowRegister; x++)
                {
                    var img = Instantiate(imagePref, transform);
                    img.transform.position = GetPosition(i, x);
                    imageComplex[i, x] = img;
                    img.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < nowRegister; i++)
            {
                var p = barMother.transform.position;
                p.y -= gap.y * i;
                var bar = Instantiate(barPref, barMother, false);

                bar.transform.position = p;
            }

            var register = nowRegister;

            var image = cursor.image;
            var pos = barMother.position;
            image.transform.position = pos;

            cursor.transform.SetAsLastSibling();
        }

        List<InputID> receved = new List<InputID>();
        List<KeyBind> keyConfig { get { return config.keyConfig; } }
        void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                receved.Clear();

                foreach (var bind in keyConfig)
                {
                    if (UnityEngine.Input.GetKeyDown(bind.key))
                    {
                        receved.Add(bind.id);
                        InputAction(bind.id);
                    }
                }
            }

        }

        CircuitBuilder circuit { get { return CircuitProvider.instance.circuits[0]; } }
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
                /*
                *ここまでにGate以外の処理を書く
                *
                */
                default:
                    var gate = Config.GateSetting.InputToGate(id);
                    if (circuit.CheckGate(cPos.x, cPos.y) == gate)
                    {
                        circuit.RemoveAt(cPos.x, cPos.y);
                        controllBuffer = null;
                    }
                    else if (circuit.IsControllGate(gate))
                    {
                        if (controllBuffer == null)
                        {
                            controllBuffer = new Vector2Int(cPos.x, cPos.y);
                        }
                        else if (controllBuffer.Value.x == cPos.x && (controllBuffer.Value.y - 1 == cPos.y || controllBuffer.Value.y + 1 == cPos.y))
                        {
                            circuit.AppendMultiCGate(gate, cPos.x, cPos.y, controllBuffer.Value.y);
                            controllBuffer = null;
                        }
                    }
                    else
                    {
                        circuit.AppendAt(cPos.x, cPos.y, gate);
                        controllBuffer = null;
                    }
                    UpdateGate(cPos);

                    break;
            }
        }

        Vector2Int? controllBuffer = null;

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

            if (cPos.y >= nowRegister)
            {
                cPos.y = 0;
            }
            else if (cPos.y < 0)
            {
                cPos.y = nowRegister - 1;
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

        protected virtual void AfterUpdate() { }

        protected void UpdateGate(Vector2Int position)
        {

            //縦方向に更新。横のつながりはないので
            for (int i = 0; i < circuit.register; i++)
            {
                var img = imageComplex[i, position.y];
                var gate = circuit.CheckGate(i, position.y);
                if (gate == Gates.none)
                {
                    img.gameObject.SetActive(false);
                }
                else
                {
                    if (circuit.IsControllGate(gate))
                    {
                        if (!circuit.IsTargetQubit(i, position.y))
                        {
                            img.sprite = controllTex;
                        }
                    }
                    else
                    {
                        img.sprite = gateTexs[gate];
                    }

                    img.gameObject.SetActive(true);
                }
            }

            AfterUpdate();
        }
    }
}