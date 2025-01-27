namespace CVB.NET.DataAccess.Tests
{
    using System;
    using DataAccess.MetaData.Attributes;
    using DataAccess.MetaData.Views;
    using Reflection.Caching;

    public static class TestOrmTypeDefinitions
    {
        public static OrmTypeMetaDataInfoViewBase ObjectType
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (object));

        public static OrmTypeMetaDataInfoViewBase SomeTypeWithAttributes
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (SomeType));

        public static OrmTypeMetaDataInfoViewBase TestModelType
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (TestModel));

        public static OrmTypeMetaDataInfoViewBase RootType
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (MyModelRootBase));

        public static OrmTypeMetaDataInfoViewBase OtherRootType
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (MyOtherRootModelBase));

        public static OrmTypeMetaDataInfoViewBase Level1Type
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (MyModelTypeLevel1));

        public static OrmTypeMetaDataInfoViewBase Level2Type
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (MyModelTypeLevel2));

        public static OrmTypeMetaDataInfoViewBase Level3Type
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (MyModelTypeLevel3));

        [ModelBase]
        public class MyModelRootBase
        {
            [Identifier]
            public Guid Guid { get; set; }

            [AllowNull]
            public string RootString { get; set; }
        }

        [ModelBase]
        public class MyOtherRootModelBase
        {
            [Identifier]
            public string Key { get; set; }

            [AllowNull]
            public string Value { get; set; }
        }

        public class MyModelTypeLevel1 : MyModelRootBase
        {
            [AllowNull]
            public string Level1String { get; set; }
        }

        public class TestModel : MyModelRootBase
        {
            public string TestString { get; set; }
        }

        public class MyModelTypeLevel2 : MyModelTypeLevel1
        {
            [AllowNull]
            public string Level2String { get; set; }

            public SomeType ThisShouldNotBeRecognizedAsDependency { get; }

            public MyOtherRootModelBase TestReferenceToAnotherBaseModel { get; set; }

            public TestModel ForeignReferenceTest { get; set; }
        }

        public class MyModelTypeLevel3 : MyModelTypeLevel2
        {
            [AllowNull]
            public string Level3String { get; set; }
        }

        public class SomeType
        {
            [Identifier]
            public Guid Guid { get; set; }

            [AllowNull]
            public string RootString { get; set; }
        }
    }
}