namespace CVB.NET.Rewriting.Utils
{
    using System;
    using System.Runtime.CompilerServices;
    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public static class PropertyGenerationUtils
    {
        public static PropertyDefinition CreateAutoProperty(
            TypeDefinition declaringType,
            TypeReference propertyType,
            string propertyName,
            PropertyAttributes propertyAttributes = PropertyAttributes.None,
            bool isStatic = false,
            bool markCompilerGenerated = true)
        {
            FieldDefinition backingField = CreatePropertyBackingField(declaringType, propertyType, propertyName, isStatic, markCompilerGenerated);

            declaringType.Fields.Add(backingField);

            PropertyDefinition autoPropertyDefinition = new PropertyDefinition(propertyName, propertyAttributes, propertyType)
                                                        {
                                                            GetMethod = CreatePropertyAutoGetter(declaringType, propertyType, backingField, propertyName, isStatic, markCompilerGenerated),
                                                            SetMethod = CreatePropertyAutoSetter(declaringType, propertyType, backingField, propertyName, isStatic, markCompilerGenerated)
                                                        };

            autoPropertyDefinition.HasThis = !isStatic;

            declaringType.Methods.Add(autoPropertyDefinition.GetMethod);
            declaringType.Methods.Add(autoPropertyDefinition.SetMethod);

            declaringType.Properties.Add(autoPropertyDefinition);

            return autoPropertyDefinition;
        }

        public static FieldDefinition CreatePropertyBackingField(TypeDefinition declaringType, TypeReference propertyType, string propertyName, bool isStatic = false, bool markCompilerGenerated = true)
        {
            FieldDefinition fieldDefinition;

            if (markCompilerGenerated)
            {
                fieldDefinition = new FieldDefinition($"<{propertyName}>k__BackingField", FieldAttributes.Private, propertyType);

                fieldDefinition.CustomAttributes.Add(new CustomAttribute(declaringType.Module.Import(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes))));
            }
            else
            {
                string fieldName = propertyName;

                if (fieldName.Length >= 1)
                {
                    char first = fieldName[0];

                    fieldName = char.ToLowerInvariant(first) + fieldName.Substring(1);
                }

                fieldDefinition = new FieldDefinition(fieldName, FieldAttributes.Private, propertyType);
            }

            fieldDefinition.IsStatic = isStatic;

            return fieldDefinition;
        }

        public static MethodDefinition CreatePropertyAutoGetter(TypeDefinition declaringType, TypeReference propertyType, FieldDefinition backingField, string propertyName, bool isStatic = false, bool markCompilerGenerated = true)
        {
            MethodDefinition getMethod = CreatePropertyGetter(declaringType, propertyType, propertyName, isStatic);

            if (markCompilerGenerated)
            {
                getMethod.CustomAttributes.Add(new CustomAttribute(declaringType.Module.Import(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes))));
            }

            ILProcessor gm = getMethod.Body.GetILProcessor();

            if (isStatic)
            {
                // get static field
                gm.Emit(OpCodes.Ldsfld, backingField);
            }
            else
            {
                // current instance
                gm.Emit(OpCodes.Ldarg_0);
                // get instance field
                gm.Emit(OpCodes.Ldfld, backingField);
            }

            gm.Emit(OpCodes.Ret);

            return getMethod;
        }

        public static MethodDefinition CreatePropertyGetter(TypeDefinition declaringType, TypeReference propertyType, string propertyName, bool isStatic = false)
        {
            MethodDefinition getMethod = new MethodDefinition(
                "get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType);

            getMethod.IsStatic = isStatic;
            getMethod.HasThis = !isStatic;

            getMethod.SemanticsAttributes = MethodSemanticsAttributes.Getter;

            return getMethod;
        }

        public static MethodDefinition CreatePropertyAutoSetter(TypeDefinition declaringType, TypeReference propertyType, FieldDefinition backingField, string propertyName, bool isStatic = false, bool markCompilerGenerated = true)
        {
            MethodDefinition setMethod = CreatePropertySetter(declaringType, propertyType, propertyName, isStatic);

            if (markCompilerGenerated)
            {
                setMethod.CustomAttributes.Add(new CustomAttribute(declaringType.Module.Import(typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes))));
            }

            ILProcessor sm = setMethod.Body.GetILProcessor();

            sm.Emit(OpCodes.Ldarg_0);

            if (isStatic)
            {
                // set static field
                sm.Emit(OpCodes.Stsfld, backingField);
            }
            else
            {
                // new field value
                sm.Emit(OpCodes.Ldarg_1);
                // set field
                sm.Emit(OpCodes.Stfld, backingField);
            }

            sm.Emit(OpCodes.Ret);

            return setMethod;
        }

        public static MethodDefinition CreatePropertySetter(TypeDefinition declaringType, TypeReference propertyType, string propertyName, bool isStatic = false)
        {
            MethodDefinition setMethod = new MethodDefinition(
                "set_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                declaringType.Module.TypeSystem.Void);

            setMethod.IsStatic = isStatic;
            setMethod.HasThis = !isStatic;

            setMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, propertyType));

            setMethod.SemanticsAttributes = MethodSemanticsAttributes.Setter;

            return setMethod;
        }
    }
}