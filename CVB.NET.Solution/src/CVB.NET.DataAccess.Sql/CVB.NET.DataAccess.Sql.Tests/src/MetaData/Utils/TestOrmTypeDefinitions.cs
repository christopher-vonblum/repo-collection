namespace CVB.NET.DataAccess.Sql.Tests.MetaData.Utils
{
    using DataAccess.MetaData.Views;
    using Reflection.Caching;
    using TreeModelRepository.Schema;

    public static class TestOrmTypeDefinitions
    {
        public static OrmTypeMetaDataInfoViewBase TreeModelBase
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (TreeModelBase));

        public static OrmTypeMetaDataInfoViewBase ModelBase
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (ModelBase));

        public static OrmTypeMetaDataInfoViewBase ModelType
            = ReflectionCache.Get<OrmTypeMetaDataInfoViewBase>(typeof (ModelType));
    }
}