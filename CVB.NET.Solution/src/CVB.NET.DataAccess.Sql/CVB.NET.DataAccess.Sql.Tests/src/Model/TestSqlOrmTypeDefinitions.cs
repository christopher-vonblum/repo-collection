namespace CVB.NET.DataAccess.Sql.Tests.Model
{
    using DataAccess.Tests;
    using Reflection.Caching;
    using T4Queries.Model.MetaData;

    public static class TestSqlOrmTypeDefinitions
    {
        public static OrmSqlTypeMetaDataInfoView ObjectType
            = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (object));

        public static OrmSqlTypeMetaDataInfoView SomeTypeWithAttributes
            = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (TestOrmTypeDefinitions.SomeType));

        public static OrmSqlTypeMetaDataInfoView RootType
            = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (TestOrmTypeDefinitions.MyModelRootBase));

        public static OrmSqlTypeMetaDataInfoView Level1Type
            = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (TestOrmTypeDefinitions.MyModelTypeLevel1));

        public static OrmSqlTypeMetaDataInfoView Level2Type
            = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (TestOrmTypeDefinitions.MyModelTypeLevel2));

        public static OrmSqlTypeMetaDataInfoView Level3Type
            = ReflectionCache.Get<OrmSqlTypeMetaDataInfoView>(typeof (TestOrmTypeDefinitions.MyModelTypeLevel3));
    }
}