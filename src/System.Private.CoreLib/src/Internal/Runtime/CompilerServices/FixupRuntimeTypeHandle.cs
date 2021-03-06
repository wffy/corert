// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Internal.Runtime.CompilerServices
{
    public unsafe struct FixupRuntimeTypeHandle
    {
        private IntPtr _value;

        public FixupRuntimeTypeHandle(RuntimeTypeHandle runtimeTypeHandle)
        {
            _value = *(IntPtr*)&runtimeTypeHandle;
        }

        public RuntimeTypeHandle RuntimeTypeHandle
        {
            get
            {
                // Managed debugger uses this logic to figure out the interface's type
                // Update managed debugger too whenever this is changed.
                // See CordbObjectValue::WalkPtrAndTypeData in debug\dbi\values.cpp

                if (((_value.ToInt64()) & 0x1) != 0)
                {
                    return *(RuntimeTypeHandle*)(_value.ToInt64() - 0x1);
                }
                else
                {
                    RuntimeTypeHandle returnValue = default(RuntimeTypeHandle);
                    *(IntPtr*)&returnValue = _value;
                    return returnValue;
                }
            }
        }
    }
}
