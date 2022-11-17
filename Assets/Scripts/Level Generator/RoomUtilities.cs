using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gate {
    NONE = 0, UP = 1, RIGHT = 2, DOWN = 4, LEFT = 8
}

public struct Room {
    public int4 gates;
}

[System.Serializable]
public class int4 {
    bool _debug = false;
    private BitArray value = new BitArray(4);

    public uint Value => value.ConvertUInt();

    #region Constructors

    public int4(int4 copy) {
        value = (BitArray)copy.value.Clone();
        if (_debug) Debug.Log("New int4 (copy) " + ToString());
    }

    public int4(uint integer) {
        value = new BitArray(4, false);
        Add(integer);
        if (_debug) Debug.Log("New int4 (uint) : " + ToString());
    }

    public int4(BitArray value) {
        this.value = new BitArray(4, false);
        for (int i = 0; i < 4; i++) {
            this.value[i] = value[i];
        }
        if (_debug) Debug.Log("New int4 (bitarray) : " + ToString());
    }

    public int4(Gate gate) : this((uint)gate) {
        if (_debug) Debug.Log("New int4 (gate) : " + ToString());
    }

    #endregion

    public bool Contains(Gate gate) {
        BitArray array = ((int)gate).ConvertBitArray();
        array.Length = 4;
        array = array.And(value);

        for (int i = 0; i < array.Length; i++) {
            if (array[0])
                return true;
            array = array.RightShift(1);
        }

        return false;
    }

    #region Add

    private int4 Add(Gate gate) {
        return Add((uint)gate);
    }

    private int4 Add(uint integer) {
        return Add(integer.ConvertBitArray());
    }

    private int4 Add(int4 other) {
        return Add(other.value);
    }

    private int4 Add(BitArray other) {
        value.Add(other);
        return this;
    }

    #endregion

    #region Remove

    private int4 Remove(Gate gate) {
        return Remove((uint)gate);
    }

    private int4 Remove(uint integer) {
        return Remove(integer.ConvertBitArray());
    }

    private int4 Remove(int4 other) {
        return Remove(other.value);
    }

    private int4 Remove(BitArray other) {
        value.Remove(other);
        return this;
    }

    #endregion

    #region Bit Operation

    private int4 RightShift(int shift) {
        value.RightShift(shift);
        return this;
    }

    private int4 LeftShift(int shift) {
        value.LeftShift(shift);
        return this;
    }

    private int4 Not() {
        value.Not();
        return this;
    }

    #region And

    private int4 And(Gate gate) {
        return And((uint)gate);
    }

    private int4 And(int4 other) {
        return And(other.value);
    }

    private int4 And(uint integer) {
        return And(integer.ConvertBitArray());
    }

    private int4 And(BitArray array) {
        if (array.Length != value.Length) { array.Length = value.Length; }
        value.And(array);
        return this;
    }

    #endregion

    #region Or

    private int4 Or(Gate gate) {
        return Or((uint)gate);
    }

    private int4 Or(int4 other) {
        return Or(other.value);
    }

    private int4 Or(uint integer) {
        return Or(integer.ConvertBitArray());
    }

    private int4 Or(BitArray array) {
        if (array.Length != value.Length) { array.Length = value.Length; }
        value.Or(array);
        return this;
    }

    #endregion

    #region Xor

    private int4 Xor(Gate gate) {
        return Xor((uint)gate);
    }

    private int4 Xor(int4 other) {
        return Xor(other.value);
    }

    private int4 Xor(uint integer) {
        return Xor(integer.ConvertBitArray());
    }

    private int4 Xor(BitArray array) {
        if (array.Length != value.Length) { array.Length = value.Length; }
        value.Xor(array);
        return this;
    }

    #endregion

    #endregion

    #region Operators

    #region Gate

    public static int4 operator ^(int4 us, Gate them) => new int4(us).Xor(them);
    public static int4 operator |(int4 us, Gate them) => new int4(us).Or(them);
    public static int4 operator &(int4 us, Gate them) => new int4(us).And(them);
    public static int4 operator +(int4 us, Gate them) => new int4(us).Add(them);
    public static int4 operator -(int4 us, Gate them) => new int4(us).Remove(them);

    #endregion

    #region int4

    public static int4 operator &(int4 us, int4 them) => new int4(us).And(them);
    public static int4 operator +(int4 us, int4 them) => new int4(us).Add(them);
    public static int4 operator -(int4 us, int4 them) => new int4(us).Remove(them);

    #endregion

    #region uint

    public static int4 operator &(int4 us, uint them) => new int4(us).And(them);
    public static int4 operator +(int4 us, uint them) => new int4(us).Add(them);
    public static int4 operator -(int4 us, uint them) => new int4(us).Remove(them);

    #endregion

    public static int4 operator <<(int4 us, int value) => new int4(us).LeftShift(value);
    public static int4 operator >>(int4 us, int value) => new int4(us).RightShift(value);
    public static int4 operator !(int4 us) => new int4(us).Not();

    public static implicit operator uint(int4 us) => us.Value;
    public static implicit operator int4(uint them) => new int4(them);
    public static implicit operator int4(BitArray them) => new int4(them);

    #endregion

    public override string ToString() {
        return ((uint)this).ToString() + " (" + value.Print() + ")";
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

    public static BitArray ConvertBitArray(this uint integer) {
        return new BitArray(new int[] { (int)integer } );
    }

    public static BitArray ConvertBitArray(this int integer) {
        return new BitArray(new int[] { integer });
    }
}