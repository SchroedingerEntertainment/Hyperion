// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using SE.Mixin;

namespace SE.Hyperion.Desktop.Win32
{
    /// <summary>
    /// A special IL generator used to generate WndProc proxies
    /// </summary>
    public struct WndProcGenerator : IGenerator
    {
        struct LabelItem : IHashEntry<WindowMessage>
        {
            private readonly int hash;
            public int Hash
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return hash; }
            }

            private readonly WindowMessage key;
            public WindowMessage Key
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return key; }
            }

            private readonly Label label;
            public Label Label
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return label; }
            }

            public bool IsEmpty
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return (hash == 0 || label == default(Label)); }
            }

            public LabelItem(int hash, WindowMessage key, Label label)
            {
                this.label = label;
                this.hash = hash;
                this.key = key;
            }
        }
        struct MethodItem : IHashEntry<Label>
        {
            private readonly int hash;
            public int Hash
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return hash; }
            }

            private readonly Label key;
            public Label Key
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return key; }
            }

            private readonly MethodInfo method;
            public MethodInfo Method
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return method; }
            }

            public bool IsEmpty
            {
                [MethodImpl(OptimizationExtensions.ForceInline)]
                get { return (hash == 0 || method == null); }
            }

            public MethodItem(int hash, Label key, MethodInfo method)
            {
                this.method = method;
                this.hash = hash;
                this.key = key;
            }
        }

        private readonly static Type MessageType;

        static WndProcGenerator()
        {
            MessageType = typeof(WindowMessage);
        }

        public void Emit(TypeBuilder builder, ILGenerator gen, MethodInfo method, IDictionary<Type, FieldBuilder> fields, IEnumerable<MethodInfo> components)
        {
            MethodInfo def = components.FirstOrDefault(x => !x.HasAttribute<WndProcAttribute>());
            if (def == null)
            {
                def = components.Last();
            }
            HashContainer<WindowMessage, LabelItem> jmpTable = new HashContainer<WindowMessage, LabelItem>();
            HashContainer<Label, MethodItem> ptrTable = new HashContainer<Label, MethodItem>();
            WndProcAttribute[] wndProcDeclare = null; foreach (MethodInfo component in components.Where(x => x != def && x.TryGetAttributes<WndProcAttribute>(out wndProcDeclare)))
            {
                Label marker = gen.DefineLabel();

                int hash;
                int index = ptrTable.Emplace(ref marker, out hash);
                ptrTable.Data[index] = new MethodItem(hash, marker, component);

                foreach (WindowMessage target in wndProcDeclare.Select(x => x.Message))
                {
                    WindowMessage msg = target;

                    index = jmpTable.Emplace(ref msg, out hash);
                    if (jmpTable.Data[index].IsEmpty)
                    {
                        jmpTable.Data[index] = new LabelItem(hash, msg, marker);
                    }
                }
            }

            ParameterInfo[] parameters = method.GetParameters();
            int paramIndex;
            
            parameters.TryGetIndex(x => x.ParameterType == MessageType, out paramIndex);
            foreach (LabelItem jmp in jmpTable.Where(x => !x.IsEmpty))
            {
                gen.Emit(OpCodes.Ldarg, paramIndex + 1);
                gen.Emit(OpCodes.Ldc_I4, (int)jmp.Key);
                gen.Emit(OpCodes.Beq, jmp.Label);
            }
            
            EmitCall(builder, gen, fields, def, parameters);
            foreach (MethodItem ptr in ptrTable.Where(x => !x.IsEmpty))
            {
                gen.MarkLabel(ptr.Key);
                EmitCall(builder, gen, fields, ptr.Method, parameters);
            }
        }

        public void Emit(TypeBuilder builder, ILGenerator gen, PropertyInfo property, IDictionary<Type, FieldBuilder> fields, IEnumerable<MemberInfo> components, bool store)
        {
            throw new NotImplementedException();
        }

        private static void EmitCall(TypeBuilder builder, ILGenerator gen, IDictionary<Type, FieldBuilder> fields, MethodInfo component, ParameterInfo[] parameters)
        {
            if (!component.IsStatic)
            {
                FieldBuilder propertyContainer; if (!fields.TryGetValue(component.DeclaringType, out propertyContainer))
                {
                    Type componentType = component.DeclaringType;

                    propertyContainer = builder.DefineField(componentType.FullName, componentType, FieldAttributes.Private);
                    fields.Add(componentType, propertyContainer);
                }

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldflda, propertyContainer);
            }

            ParameterInfo[] tmp = component.GetParameters();
            if (tmp.Length > 0)
            {
                ImplicitAttribute attrib = tmp[0].GetCustomAttribute<ImplicitAttribute>();

                if (attrib != null)
                {
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Isinst, tmp[0].ParameterType);
                }
            }

            if (parameters.Length > 0)
                gen.Emit(OpCodes.Ldarg_1);

            if (parameters.Length > 1)
                gen.Emit(OpCodes.Ldarg_2);

            if (parameters.Length > 2)
                gen.Emit(OpCodes.Ldarg_3);

            for (int i = 3; i < parameters.Length; i++)
                gen.Emit(OpCodes.Ldarg, i + 1);

            gen.Emit(OpCodes.Call, component);
            gen.Emit(OpCodes.Ret);
        }
    }
}
