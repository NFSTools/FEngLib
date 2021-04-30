using System;
using System.Collections.Generic;
using System.IO;

namespace FEngLib
{
    public static class BinaryExtensions
    {
        public static T[] ReadArray<T>(this BinaryReader binaryReader, Func<T> func, int length)
        {
            var array = new T[length];

            for (var i = 0; i < length; i++) array[i] = func();

            return array;
        }

        public static void WriteArray<T>(this BinaryWriter binaryWriter, IEnumerable<T> array, Action<T> writeAction)
        {
            foreach (var t in array) writeAction(t);
        }

        public static T ReadEnum<T>(this BinaryReader binaryReader) where T : IConvertible
        {
            var underlyingType = Enum.GetUnderlyingType(typeof(T));

            if (underlyingType == typeof(uint))
                return (T) Enum.ToObject(typeof(T), binaryReader.ReadUInt32());

            if (underlyingType == typeof(int))
                return (T) Enum.ToObject(typeof(T), binaryReader.ReadInt32());

            if (underlyingType == typeof(ushort))
                return (T) Enum.ToObject(typeof(T), binaryReader.ReadUInt16());

            if (underlyingType == typeof(short))
                return (T) Enum.ToObject(typeof(T), binaryReader.ReadInt16());

            throw new Exception();
        }

        public static void WriteEnum<T>(this BinaryWriter binaryWriter, T value) where T : IConvertible
        {
            switch ((IConvertible) Convert.ChangeType(value, value.GetTypeCode()))
            {
                case uint ui:
                    binaryWriter.Write(ui);
                    break;

                case int ui:
                    binaryWriter.Write(ui);
                    break;

                case ushort ui:
                    binaryWriter.Write(ui);
                    break;

                case short ui:
                    binaryWriter.Write(ui);
                    break;
                default:
                    throw new Exception();
            }
        }

        public static long WritePointer(this BinaryWriter bw)
        {
            bw.Write(0);
            return bw.BaseStream.Position - 4;
        }

        public static void AlignReader(this BinaryReader br, int boundary)
        {
            if (br.BaseStream.Position % boundary != 0)
                br.BaseStream.Position += boundary - br.BaseStream.Position % boundary;
        }

        public static void AlignReader(this BinaryReader br, uint boundary)
        {
            if (br.BaseStream.Position % boundary != 0)
                br.BaseStream.Position += boundary - br.BaseStream.Position % boundary;
        }

        public static void AlignWriter(this BinaryWriter bw, int boundary)
        {
            if (bw.BaseStream.Position % boundary != 0)
            {
                var bytesToWrite = boundary - bw.BaseStream.Position % boundary;
                bw.Write(new byte[bytesToWrite]);
            }
        }

        public static void AlignWriter(this BinaryWriter bw, uint boundary)
        {
            if (bw.BaseStream.Position % boundary != 0)
            {
                var bytesToWrite = boundary - bw.BaseStream.Position % boundary;
                bw.Write(new byte[bytesToWrite]);
            }
        }
    }
}