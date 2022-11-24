using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gate {
    NONE = 0, UP = 1, RIGHT = 2, DOWN = 4, LEFT = 8
}

[System.Serializable]
public class Gates {
    [SerializeField] bint4 value = new bint4();

    public bool this[Gate gate] { get => value.Contains(gate); set => Modify((uint)gate, value); }
    public bool this[int index] { get => value.Contains((uint)index); set => Modify((uint)index, value); }
    public bint4 Value { get => value; set => this.value = value; }
    public int Count => Length();

    public Gates() {
        value = new bint4();
    }

    public Gates(uint integer) {
        value = new bint4(integer);
    }

    public Gates(Gates gate) {
        value = new bint4(gate.value);
    }

    private int Length() {
        int count = 0;
        for (int i = 0; i < bint4.Length; i++) {
            if (value[i]) ++count;
        }
        return count;
    }

    private void Modify(uint integer, bool state) {
        bool[] array = integer.ConvertBit();

        for (int i = 0; i < bint4.Length; i++) {
            if (state) {
                value[i] |= array[i];
            } else {
                array[i] = !array[i];
                value[i] &= array[i];
            }
        }

    }

    public Gate Random(bool state) {
        int count = 0;
        for (int i = 0; i < bint4.Length; i++)
            if (value[i] == state)
                ++count;

        int random = UnityEngine.Random.Range(0, count);
        count = 0;
        for (int i = 0; i < bint4.Length; i++) {
            if (value[i] == state) {
                if (random == count) {
                    return Tools.ToGate(i);
                }
                ++count;
            }
        }
        return Gate.NONE;
    }

    public override string ToString() {
        return value.ToString();
    }
}

//[System.Serializable]
//public class int4 {
//    [SerializeField] BitArray value = new BitArray(4);
//    bool _debug = false;

//    public static int Length => 4;
//    public uint Value => value.ConvertUInt();
//    public bool this[int index] => value[index];

//    #region Constructors

//    public int4() {
//        value = new BitArray(4, false);
//    }

//    public int4(int4 copy) {
//        value = (BitArray)copy.value.Clone();
//        if (_debug) Debug.Log("New int4 (copy) " + ToString());
//    }

//    public int4(uint integer) {
//        value = new BitArray(4, false);
//        Add(integer);
//        if (_debug) Debug.Log("New int4 (uint) : " + ToString());
//    }

//    public int4(BitArray value) {
//        this.value = new BitArray(4, false);
//        for (int i = 0; i < 4; i++) {
//            this.value[i] = value[i];
//        }
//        if (_debug) Debug.Log("New int4 (bitarray) : " + ToString());
//    }

//    public int4(Gate gate) : this((uint)gate) {
//        if (_debug) Debug.Log("New int4 (gate) : " + ToString());
//    }

//    #endregion

//    public bool Contains(Gate gate) {
//        return Contains((uint)gate);
//    }

//    public bool Contains(uint integer) {
//        BitArray array = ((int)integer).ConvertBitArray();
//        array.Length = 4;

//        for (int i = 0; i < value.Length; ++i) {
//            if (array[i] != value[i]) {
//                return false;
//            }
//        }

//        return true;
//    }

//    #region Add

//    private int4 Add(Gate gate) {
//        return Add((uint)gate);
//    }

//    private int4 Add(uint integer) {
//        return Add(integer.ConvertBitArray());
//    }

//    private int4 Add(int4 other) {
//        return Add(other.value);
//    }

//    private int4 Add(BitArray other) {
//        value.Add(other);
//        return this;
//    }

//    #endregion

//    #region Remove

//    private int4 Remove(Gate gate) {
//        return Remove((uint)gate);
//    }

//    private int4 Remove(uint integer) {
//        return Remove(integer.ConvertBitArray());
//    }

//    private int4 Remove(int4 other) {
//        return Remove(other.value);
//    }

//    private int4 Remove(BitArray other) {
//        value.Remove(other);
//        return this;
//    }

//    #endregion

//    #region Bit Operation

//    private int4 RightShift(int shift) {
//        value.RightShift(shift);
//        return this;
//    }

//    private int4 LeftShift(int shift) {
//        value.LeftShift(shift);
//        return this;
//    }

//    private int4 Not() {
//        value.Not();
//        return this;
//    }

//    #region And

//    private int4 And(Gate gate) {
//        return And((uint)gate);
//    }

//    private int4 And(int4 other) {
//        return And(other.value);
//    }

//    private int4 And(uint integer) {
//        return And(integer.ConvertBitArray());
//    }

//    private int4 And(BitArray array) {
//        if (array.Length != value.Length) { array.Length = value.Length; }
//        value.And(array);
//        return this;
//    }

//    #endregion

//    #region Or

//    private int4 Or(Gate gate) {
//        return Or((uint)gate);
//    }

//    private int4 Or(int4 other) {
//        return Or(other.value);
//    }

//    private int4 Or(uint integer) {
//        return Or(integer.ConvertBitArray());
//    }

//    private int4 Or(BitArray array) {
//        if (array.Length != value.Length) { array.Length = value.Length; }
//        value.Or(array);
//        return this;
//    }

//    #endregion

//    #region Xor

//    private int4 Xor(Gate gate) {
//        return Xor((uint)gate);
//    }

//    private int4 Xor(int4 other) {
//        return Xor(other.value);
//    }

//    private int4 Xor(uint integer) {
//        return Xor(integer.ConvertBitArray());
//    }

//    private int4 Xor(BitArray array) {
//        if (array.Length != value.Length) { array.Length = value.Length; }
//        value.Xor(array);
//        return this;
//    }

//    #endregion

//    #endregion

//    #region Operators

//    #region Gate

//    public static int4 operator ^(int4 us, Gate them) => new int4(us).Xor(them);
//    public static int4 operator |(int4 us, Gate them) => new int4(us).Or(them);
//    public static int4 operator &(int4 us, Gate them) => new int4(us).And(them);
//    public static int4 operator +(int4 us, Gate them) => new int4(us).Add(them);
//    public static int4 operator -(int4 us, Gate them) => new int4(us).Remove(them);

//    #endregion

//    #region int4

//    public static int4 operator &(int4 us, int4 them) => new int4(us).And(them);
//    public static int4 operator +(int4 us, int4 them) => new int4(us).Add(them);
//    public static int4 operator -(int4 us, int4 them) => new int4(us).Remove(them);

//    #endregion

//    #region uint

//    public static int4 operator &(int4 us, uint them) => new int4(us).And(them);
//    public static int4 operator +(int4 us, uint them) => new int4(us).Add(them);
//    public static int4 operator -(int4 us, uint them) => new int4(us).Remove(them);

//    #endregion

//    public static int4 operator <<(int4 us, int value) => new int4(us).LeftShift(value);
//    public static int4 operator >>(int4 us, int value) => new int4(us).RightShift(value);
//    public static int4 operator !(int4 us) => new int4(us).Not();

//    public static implicit operator uint(int4 us) => us.Value;
//    public static implicit operator int4(uint them) => new int4(them);
//    public static implicit operator int4(BitArray them) => new int4(them);

//    #endregion

//    public override string ToString() {
//        return ((uint)this).ToString() + " (" + value.Print() + ")";
//    }
//}

[System.Serializable]
public class bint4 {
    [SerializeField] bool[] value = new bool[4];
    bool _debug = false;

    public static int Length => 4;
    public uint Value => value.ConvertUInt();
    public bool this[int index] { get => value[index]; set => this.value[index] = value; }

    #region Constructors

    public bint4() {
        value = new bool[4];
    }

    public bint4(bint4 copy) {
        value = (bool[])copy.value.Clone();
        if (_debug) Debug.Log("New bint4 (copy) " + ToString());
    }

    public bint4(uint integer) {
        value = new bool[4];
        Add(integer);
        if (_debug) Debug.Log("New bint4 (uint) : " + ToString());
    }

    public bint4(bool[] other) {
        value = new bool[4];
        for (int i = 0; i < 4; i++) {
            this.value[i] = value[i];
        }
        if (_debug) Debug.Log("New bint4 (bint4) : " + ToString());
    }

    public bint4(Gate gate) : this((uint)gate) {
        if (_debug) Debug.Log("New bint4 (gate) : " + ToString());
    }

    public bint4(BitArray array) {
        value = new bool[4];
        for (int i = 0; i < value.Length; i++) {
            value[i] = array[i];
        }
        if (_debug) Debug.Log("New bint4 (bitarray) : " + ToString());
    }

    #endregion

    public bool Contains(Gate gate) {
        return Contains((uint)gate);
    }

    public bool Contains(uint integer) {
        bool[] array = integer.ConvertBit(4);
        //Debug.Log(value.Print() + " .. " + array.Print());

        for (int i = 0; i < value.Length; ++i) {
            if (array[i] & value[i] != array[i]) {
                return false;
            }
        }

        return true;
    }

    #region Add

    private bint4 Add(Gate gate) {
        return Add((uint)gate);
    }

    private bint4 Add(uint integer) {
        return Add(integer.ConvertBit());
    }

    private bint4 Add(bint4 other) {
        return Add(other.value);
    }

    private bint4 Add(bool[] other) {
        bool retain = false;
        for (int i = 0; i < value.Length; i++) {
            value[i] ^= other[i] ^ retain;
            retain = (!value[i] & (other[i] | retain)) | (value[i] & (other[i] & retain));
        }

        if (retain) {
            Debug.Log("Too much btw");
            for (int i = 0; i < value.Length; i++)
                value[i] = true;
        }

        return this;
    }

    #endregion

    #region Remove

    private bint4 Remove(Gate gate) {
        return Remove((uint)gate);
    }

    private bint4 Remove(uint integer) {
        return Remove(integer.ConvertBit());
    }

    private bint4 Remove(bint4 other) {
        return Remove(other.value);
    }

    private bint4 Remove(bool[] other) {
        bool retain = false;
        for (int i = 0; i < value.Length; i++) {
            value[i] = value[i] ^ other[i] ^ retain;
            retain = (value[i] & (other[i] ^ retain)) | (value[i] & other[i] & retain);
        }

        if (retain) {
            Debug.Log("Too much less");
            for (int i = 0; i < value.Length; i++)
                value[i] = false;
        }

        return this;
    }

    #endregion

    public bint4 Not() {
        for (int i = 0; i < value.Length; i++) {
            value[i] = !value[i];
        }
        return this;
    }

    #region Operators

    #region Gate

    public static bint4 operator +(bint4 us, Gate them) => new bint4(us).Add(them);
    public static bint4 operator -(bint4 us, Gate them) => new bint4(us).Remove(them);

    #endregion

    #region bint4

    public static bint4 operator !(bint4 us) => new bint4(us).Not();
    public static bint4 operator +(bint4 us, bint4 them) => new bint4(us).Add(them);
    public static bint4 operator -(bint4 us, bint4 them) => new bint4(us).Remove(them);

    #endregion

    #region uint

    public static bint4 operator +(bint4 us, uint them) => new bint4(us).Add(them);
    public static bint4 operator -(bint4 us, uint them) => new bint4(us).Remove(them);

    #endregion

    public static implicit operator uint(bint4 us) => us.Value;
    public static implicit operator bint4(uint them) => new bint4(them);
    //public static implicit operator bint4(BitArray them) => new bint4(them);

    #endregion

    public override string ToString() {
        string name = ((uint)this).ToString() + " (";
        for (int i = value.Length - 1; i > -1; --i) {
            name += value[i] ? 1 : 0;
        }
        name += ")";
        return name;
    }
}

public static class Tools {
    public static string Print<T>(this T[] t) {
        string output = "";
        for (int i = 0; i < t.Length; i++) {
            output += t[i];
        }
        return output;
    }

    public static string Print(this BitArray t) {
        string output = "";
        for (int i = t.Length - 1; i > -1; --i) {
            output += t[i] ? 1 : 0;
        }
        return output;
    }

    public static string Print(this bool[] t) {
        string output = "";
        for (int i = 0; i < t.Length; i++) {
            output += t[i] ? 1 : 0;
        }
        return output;
    }

    public static BitArray Add(this BitArray value, BitArray other) {
        bool retain = false;
        for (int i = 0; i < value.Length; i++) {
            value[i] ^= other[i] ^ retain;
            retain = (!value[i] & (other[i] | retain)) | (value[i] & (other[i] & retain));
        }

        if (retain) {
            Debug.Log("Too much btw");
            value.SetAll(true);
        }

        return value;
    }

    public static BitArray Remove(this BitArray value, BitArray other) {
        bool retain = false;
        for (int i = 0; i < value.Length; i++) {
            value[i] = value[i] ^ other[i] ^ retain;
            retain = (value[i] & (other[i] ^ retain)) | (value[i] & other[i] & retain);
        }

        if (retain) {
            Debug.Log("Too much less");
            value.SetAll(false);
        }

        return value;
    }

    public static uint ConvertUInt(this BitArray bitArray) {
        uint[] output = new uint[(bitArray.Length + 31) / 32];
        new BitArray(bitArray).CopyTo(output, 0);
        return output[0];
    }

    public static uint ConvertUInt(this bool[] bitArray) {
        uint[] output = new uint[(bitArray.Length + 31) / 32];
        new BitArray(bitArray).CopyTo(output, 0);
        return output[0];
    }

    public static BitArray ConvertBitArray(this uint integer) {
        return new BitArray(new int[] { (int)integer });
    }

    public static bool[] ConvertBit(this uint integer, int length = -1) {
        BitArray array = new BitArray(new int[] { (int)integer });
        bool[] output = new bool[length != -1 ? length : array.Length];
        for (int i = 0; i < output.Length; i++) {
            if (i < array.Length) {
                output[i] = array[i];
            } else {
                output[i] = false;
            }
        }
        return output;
    }

    public static BitArray ConvertBitArray(this int integer) {
        return new BitArray(new int[] { integer });
    }

    public static Vector2Int ToDirection(int direction) {
        switch (direction) {
            default:
                return Vector2Int.zero;
            case 0:
                return new Vector2Int(0, 1);
            case 1:
                return new Vector2Int(1, 0);
            case 2:
                return new Vector2Int(0, -1);
            case 3:
                return new Vector2Int(-1, 0);
        }
    }

    public static Vector2Int ToDirection(Gate gate) {
        switch (gate) {
            case Gate.NONE:
            default:
                return Vector2Int.zero;
            case Gate.UP:
                return new Vector2Int(0, 1);
            case Gate.RIGHT:
                return new Vector2Int(1, 0);
            case Gate.DOWN:
                return new Vector2Int(0, -1);
            case Gate.LEFT:
                return new Vector2Int(-1, 0);
        }
    }

    public static Gate ToGate(int direction) {
        switch (direction) {
            default:
                return Gate.NONE;
            case 0:
                return Gate.UP;
            case 1:
                return Gate.RIGHT;
            case 2:
                return Gate.DOWN;
            case 3:
                return Gate.LEFT;
        }
    }

    public static Gate Inverse(this Gate gate) {
        //uint integer = ((uint)gate << 2) | ((uint)gate >> 30);
        uint integer = (((uint)gate * 17) >> 2) % 16;
        return (Gate)integer;
    }

    public static int Ponder(List<float> list) {
        return Ponder(list.ToArray());
    }

    public static int Ponder(params float[] weight) {
        float totWeight = 0;

        for (int i = 0; i < weight.Length; i++) {
            totWeight += weight[i];
        }

        if (totWeight < 1f) {
            totWeight = 1f;
        }

        float random = UnityEngine.Random.Range(0, totWeight);

        for (int i = 0; i < weight.Length; i++) {
            if (random < weight[i]) {
                return i;
            }
            random -= weight[i];
        }

        return -1;
    }
}