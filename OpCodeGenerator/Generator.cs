﻿#region # using *.*
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable ConvertToConstant.Local
#endregion

namespace OpCodeGenerator
{
  /// <summary>
  /// Klasse mit den Methoden zum generieren der Opcodes
  /// </summary>
  public static partial class Generator
  {
    static IEnumerable<string> OpInternal(byte[] opCode, int pos, int instr)
    {
      // ADD: 00 - 03
      // OR:  08 - 0b

      // --- ADD/OR - mit Byte-Registern ---
      #region # // 00 00 - 00 3f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7) + ',' + R8(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              int r1 = opCode[pos] & 7;
              yield return StrB(opCode, pos, r1 == 5 ? 4 : 0) + Asm.Instructions[instr] + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rbp1ToC4 | AddrExt.Rsp2Skip) + ',' + R8(rr);
              opCode[pos]++;
            }
            pos--;
          } break;

          case 5:
          {
            yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, 21, 0, AddrExt.Rbp1ToC4) + ',' + R8(rr);
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 00 40 - 00 7f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C1 | AddrExt.Rsp2Skip) + ',' + R8(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C1 | AddrExt.Rsp2Skip) + ',' + R8(rr);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 00 80 - 00 bf
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C4 | AddrExt.Rsp2Skip) + ',' + R8(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C4 | AddrExt.Rsp2Skip) + ',' + R8(rr);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 00 c0 - 00 ff
      for (int y = 0; y < 64; y++)
      {
        yield return StrB(opCode, pos) + Asm.Instructions[instr] + R8(opCode[pos] & 7) + ',' + R8(opCode[pos] / 8 & 7);
        opCode[pos]++;
      }
      #endregion
      opCode[pos - 1]++;

      // --- ADD/OR - gleiche wie "00", jedoch mit 32-Bit Registern ---
      #region # // 01 00 - 01 3f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7) + ',' + R32(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              int r1 = opCode[pos] & 7;
              yield return StrB(opCode, pos, r1 == 5 ? 4 : 0) + Asm.Instructions[instr] + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rbp1ToC4 | AddrExt.Rsp2Skip) + ',' + R32(rr);
              opCode[pos]++;
            }
            pos--;
          } break;

          case 5:
          {
            yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, 21, 0, AddrExt.Rbp1ToC4) + ',' + R32(rr);
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 01 40 - 01 7f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C1 | AddrExt.Rsp2Skip) + ',' + R32(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C1 | AddrExt.Rsp2Skip) + ',' + R32(rr);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 01 80 - 01 bf
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C4 | AddrExt.Rsp2Skip) + ',' + R32(rr); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C4 | AddrExt.Rsp2Skip) + ',' + R32(rr);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 01 c0 - 01 ff
      for (int y = 0; y < 64; y++)
      {
        yield return StrB(opCode, pos) + Asm.Instructions[instr] + R32(opCode[pos] & 7) + ',' + R32(opCode[pos] / 8 & 7);
        opCode[pos]++;
      }
      #endregion
      opCode[pos - 1]++;

      // --- ADD/OR - gleiche wie "00", nur mit getauschten Operanden ---
      #region # // 02 00 - 02 3f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(opCode[pos] & 7); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              int r1 = opCode[pos] & 7;
              yield return StrB(opCode, pos, r1 == 5 ? 4 : 0) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rbp1ToC4 | AddrExt.Rsp2Skip);
              opCode[pos]++;
            }
            pos--;
          } break;

          case 5:
          {
            yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(opCode[pos] & 7, 21, 0, AddrExt.Rbp1ToC4);
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 02 40 - 02 7f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C1 | AddrExt.Rsp2Skip); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C1 | AddrExt.Rsp2Skip);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 02 80 - 02 bf
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C4 | AddrExt.Rsp2Skip); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R8(rr) + ',' + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C4 | AddrExt.Rsp2Skip);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 02 c0 - 02 ff
      for (int y = 0; y < 64; y++)
      {
        yield return StrB(opCode, pos) + Asm.Instructions[instr] + R8(opCode[pos] / 8 & 7) + ',' + R8(opCode[pos] & 7);
        opCode[pos]++;
      }
      #endregion
      opCode[pos - 1]++;

      // --- ADD/OR - gleiche wie "01", nur mit getauschten Operanden ---
      #region # // 03 00 - 02 3f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(opCode[pos] & 7); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              int r1 = opCode[pos] & 7;
              yield return StrB(opCode, pos, r1 == 5 ? 4 : 0) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rbp1ToC4 | AddrExt.Rsp2Skip);
              opCode[pos]++;
            }
            pos--;
          } break;

          case 5:
          {
            yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(opCode[pos] & 7, 21, 0, AddrExt.Rbp1ToC4);
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 03 40 - 02 7f
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C1 | AddrExt.Rsp2Skip); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 1) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C1 | AddrExt.Rsp2Skip);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 03 80 - 02 bf
      for (int y = 0; y < 64; y++)
      {
        int rr = opCode[pos] / 8 & 7;
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(opCode[pos] & 7, 4, 0, AddrExt.C4 | AddrExt.Rsp2Skip); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              yield return StrB(opCode, pos, 4) + Asm.Instructions[instr] + R32(rr) + ',' + R64Addr(opCode[pos] & 7, opCode[pos] / 8 & 7, opCode[pos] / 64 & 3, AddrExt.C4 | AddrExt.Rsp2Skip);
              opCode[pos]++;
            }
            pos--;
          } break;
        }
        opCode[pos]++;
      }
      #endregion
      #region # // 03 c0 - 02 ff
      for (int y = 0; y < 64; y++)
      {
        yield return StrB(opCode, pos) + Asm.Instructions[instr] + R32(opCode[pos] / 8 & 7) + ',' + R32(opCode[pos] & 7);
        opCode[pos]++;
      }
      #endregion
      opCode[pos - 1]++;
    }

    static IEnumerable<string> OpInternalF(byte[] opCode, int pos, int instr, int constBytes = 0)
    {
      for (int y = 0; y < 8; y++)
      {
        switch (opCode[pos] & 7)
        {
          default: yield return StrB(opCode, pos, constBytes) + Asm.InstructionsF[instr] + Asm.MemType[1] + R64Addr(opCode[pos] & 7, constBytes == 1 ? AddrExt.C1 : constBytes == 4 ? AddrExt.C4 : AddrExt.None); break;

          case 4:
          {
            opCode[++pos] = 0;
            for (int x = 0; x < 256; x++)
            {
              int r1 = opCode[pos] & 7;
              if (constBytes == 0)
              {
                yield return StrB(opCode, pos, r1 == 5 ? 4 : 0) + Asm.InstructionsF[instr] + Asm.MemType[1] + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rbp1ToC4 | AddrExt.Rsp2Skip);
              }
              else
              {
                yield return StrB(opCode, pos, constBytes) + Asm.InstructionsF[instr] + Asm.MemType[1] + R64Addr(r1, opCode[pos] / 8 & 7, opCode[pos] / 64, AddrExt.Rsp2Skip | (constBytes == 1 ? AddrExt.C1 : AddrExt.C4));
              }
              opCode[pos]++;
            }
            pos--;
          } break;

          case 5:
          {
            if (constBytes != 0) goto default;
            yield return StrB(opCode, pos, 4) + Asm.InstructionsF[instr] + Asm.MemType[1] + R64Addr(opCode[pos] & 7, 21, 0, AddrExt.Rbp1ToC4);
          } break;
        }

        opCode[pos]++;
      }
    }

    /// <summary>
    /// generiert alle OpCodes anhand bestimmter Regeln
    /// </summary>
    /// <returns>Enumerable der Zeilen, welche generiert wurden</returns>
    public static IEnumerable<string> GenerateOpCodes()
    {
      var opCode = new byte[15];
      int pos = 1;

      // --- 00 - 03:          ADD
      foreach (var line in OpInternal(opCode, pos, 0)) yield return line;
      // --- 04:               ADD al, const
      yield return StrB(opCode, pos - 1, 1) + Asm.Instructions[0] + R8(0) + ", x"; opCode[pos - 1]++;
      // --- 05:               ADD eax, const
      yield return StrB(opCode, pos - 1, 4) + Asm.Instructions[0] + R32(0) + ", x"; opCode[pos - 1]++;
      // --- 06:               ???
      yield return StrB(opCode, pos - 1) + "???"; opCode[pos - 1]++;
      // --- 07:               ???
      yield return StrB(opCode, pos - 1) + "???"; opCode[pos - 1]++;

      // --- 08 - 0b:          OR
      foreach (var line in OpInternal(opCode, pos, 1)) yield return line;
      // --- 0c:               OR al, const
      yield return StrB(opCode, pos - 1, 1) + Asm.Instructions[1] + R8(0) + ", x"; opCode[pos - 1]++;
      // --- 0d:               OR eax, const
      yield return StrB(opCode, pos - 1, 4) + Asm.Instructions[1] + R32(0) + ", x"; opCode[pos - 1]++;
      // --- 0e:               ???
      yield return StrB(opCode, pos - 1) + "???"; opCode[pos - 1]++;

      pos++;
      // --- 0f 00 00 - 0f 00 07  SLDT
      foreach (var line in OpInternalF(opCode, pos, 0)) yield return line;
      // --- 0f 00 08 - 0f 00 0f  STR
      foreach (var line in OpInternalF(opCode, pos, 1)) yield return line;
      // --- 0f 00 10 - 0f 00 17  LLDT
      foreach (var line in OpInternalF(opCode, pos, 2)) yield return line;
      // --- 0f 00 18 - 0f 00 1f  LTR
      foreach (var line in OpInternalF(opCode, pos, 3)) yield return line;
      // --- 0f 00 20 - 0f 00 27  VERR
      foreach (var line in OpInternalF(opCode, pos, 4)) yield return line;
      // --- 0f 00 28 - 0f 00 2f  VERW
      foreach (var line in OpInternalF(opCode, pos, 5)) yield return line;
      // --- 0f 00 30 - 0f 00 3f  ???
      for (int i = 0; i < 16; i++) { yield return StrB(opCode, pos) + "???"; opCode[pos]++; }

      // --- 0f 00 40 - 0f 00 47  SLDT
      foreach (var line in OpInternalF(opCode, pos, 0, 1)) yield return line;
      // --- 0f 00 48 - 0f 00 4f  STR
      foreach (var line in OpInternalF(opCode, pos, 1, 1)) yield return line;
      // --- 0f 00 50 - 0f 00 57  LLDT
      foreach (var line in OpInternalF(opCode, pos, 2, 1)) yield return line;
      // --- 0f 00 58 - 0f 00 5f  LTR
      foreach (var line in OpInternalF(opCode, pos, 3, 1)) yield return line;
      // --- 0f 00 60 - 0f 00 67  VERR
      foreach (var line in OpInternalF(opCode, pos, 4, 1)) yield return line;
      // --- 0f 00 68 - 0f 00 6f  VERW
      foreach (var line in OpInternalF(opCode, pos, 5, 1)) yield return line;
      // --- 0f 00 70 - 0f 00 7f  ???
      for (int i = 0; i < 16; i++) { yield return StrB(opCode, pos) + "???"; opCode[pos]++; }

      // --- 0f 00 80 - 0f 00 87  SLDT
      foreach (var line in OpInternalF(opCode, pos, 0, 4)) yield return line;
      // --- 0f 00 88 - 0f 00 8f  STR
      foreach (var line in OpInternalF(opCode, pos, 1, 4)) yield return line;
      // --- 0f 00 90 - 0f 00 97  LLDT
      foreach (var line in OpInternalF(opCode, pos, 2, 4)) yield return line;
      // --- 0f 00 98 - 0f 00 9f  LTR
      foreach (var line in OpInternalF(opCode, pos, 3, 4)) yield return line;
      // --- 0f 00 a0 - 0f 00 a7  VERR
      foreach (var line in OpInternalF(opCode, pos, 4, 4)) yield return line;
      // --- 0f 00 a8 - 0f 00 af  VERW
      foreach (var line in OpInternalF(opCode, pos, 5, 4)) yield return line;
      // --- 0f 00 b0 - 0f 00 bf  ???
      for (int i = 0; i < 16; i++) { yield return StrB(opCode, pos) + "???"; opCode[pos]++; }

      // --- 0f 00 c0 - 0f 00 cf  SLDT, STR
      for (int i = 0; i < 16; i++) { yield return StrB(opCode, pos) + Asm.InstructionsF[i / 8] + R32(i & 7); opCode[pos]++; }
      // --- 0f 00 c0 - 0f 00 ef  LLDT, LTR, VERR, VERW
      for (int i = 16; i < 48; i++) { yield return StrB(opCode, pos) + Asm.InstructionsF[i / 8] + R16(i & 7); opCode[pos]++; }
      // --- 0f 00 f0 - 0f 00 ff  ???
      for (int i = 0; i < 16; i++) { yield return StrB(opCode, pos) + "???"; opCode[pos]++; }
      pos--;

    }
  }
}
