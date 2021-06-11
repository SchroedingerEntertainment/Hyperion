// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SE.Hyperion.Desktop
{
    /// <summary>
    /// The raw pixel data stored in memory
    /// </summary>
    public class PixelBuffer : FinalizerObject, IDisposable
    {
        IntPtr memoryHandle;

        IntPtr pixelData;
        /// <summary>
        /// A pointer to the data in memory
        /// </summary>
        public IntPtr Data
        {
            get { return pixelData; }
        }

        /// <summary>
        /// The buffer size in memory
        /// </summary>
        public int Length
        {
            get
            {
                if (memoryHandle != IntPtr.Zero) return (GCHandle.FromIntPtr(memoryHandle).Target as Array).Length;
                else return 0;
            }
        }

        /// <summary>
        /// Creates an empty buffer instance
        /// </summary>
        public PixelBuffer()
        { }
        /// <summary>
        /// Creates a new buffer instance in memory
        /// </summary>
        /// <param name="length">The size in bytes used by the buffer</param>
        public PixelBuffer(int length)
        {
            Resize(length);
        }
        protected override void Dispose(bool disposing)
        {
            if (memoryHandle != IntPtr.Zero)
            {
                GCHandle handle = GCHandle.FromIntPtr(memoryHandle);
                handle.Free();

                memoryHandle = IntPtr.Zero;
                pixelData = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Resizes the buffer
        /// </summary>
        /// <param name="length">The new size in bytes</param>
        public void Resize(int length)
        {
            Dispose(false);

            GCHandle handle = GCHandle.Alloc(new byte[length], GCHandleType.Pinned);
            pixelData = Marshal.UnsafeAddrOfPinnedArrayElement(handle.Target as Array, 0);
            memoryHandle = GCHandle.ToIntPtr(handle);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static implicit operator bool(PixelBuffer buffer)
        {
            return (buffer.pixelData != IntPtr.Zero);
        }

        [MethodImpl(OptimizationExtensions.ForceInline)]
        public static implicit operator IntPtr(PixelBuffer buffer)
        {
            return buffer.pixelData;
        }

        /// <summary>
        /// Copies the underlaying memory region into the provided buffer object
        /// </summary>
        public void TransferData(PixelBuffer target)
        {
            if (target.Length != Length)
                throw new OverflowException();

            byte[] array = ToArray();
            Marshal.Copy(array, 0, target.pixelData, array.Length);
        }

        /// <summary>
        /// Gets a managed array representation of the pixel data
        /// </summary>
        /// <returns>A managed binary array</returns>
        public byte[] ToArray()
        {
            return GCHandle.FromIntPtr(memoryHandle).Target as byte[];
        }
    }
}
